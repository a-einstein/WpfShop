using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.WpfShop.Common.Interfaces;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Modules.Products.GuiModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    public class ShoppingCartViewModel :
        ItemsViewModel<GuiCartItem>
    {
        #region Construction
        public ShoppingCartViewModel(IRepository<List<CartItem>, CartItem> cartItemsRepository)
        {
            CartItemsRepository = cartItemsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<GuiCartItem>(Delete);
        }
        #endregion

        #region Services
        private IRepository<List<CartItem>, CartItem> CartItemsRepository { get; }
        #endregion

        #region Refresh
        private bool collectionChanged;

        public override async Task Refresh()
        {
            await Initialize().ConfigureAwait(true);

            // Currently bluntly refresh.
            //if (collectionChanged)
            {
                await uiDispatcher.Invoke(async delegate
                {
                    // Note that the repository is leading. 
                    // Changes here are performed there, afterwhich it is reloaded.
                    ClearView();

                    await Read().ConfigureAwait(true);

                });

                collectionChanged = false;
            }

            UpdateAggregates();
        }

        protected override void ClearView()
        {
            base.ClearView();

            UpdateAggregates();
        }
        #endregion

        #region CRUD
        public async Task CartProduct(IShoppingProduct productsOverviewObject)
        {
            var existing = Items.FirstOrDefault(item => item.ProductId == productsOverviewObject.Id);

            if (existing == default)
            {
                await CartItemsRepository.Create(new CartItem(productsOverviewObject)).ConfigureAwait(true);
                collectionChanged = true;
            }
            else
            {
                existing.Quantity++;

                // TODO Use IShoppingProduct?
                // TODO Let GuiCartItem handle this too?
                await CartItemsRepository.Update(existing.CartItem);
            }

            await Refresh().ConfigureAwait(true);
        }

        protected override async Task Read()
        {
            uiDispatcher.Invoke(delegate
            {
                foreach (var item in CartItemsRepository.Items)
                {
                    Items.Add(new GuiCartItem(item));
                }
            });
        }

        public static readonly DependencyProperty DeleteCommandProperty =
             DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(ShoppingCartViewModel));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            private set => SetValue(DeleteCommandProperty, value);
        }

        private void Delete(GuiCartItem cartItem)
        {
            CartItemsRepository.Delete(cartItem.CartItem);
            collectionChanged = true;

            _ = Refresh();
        }

        protected override void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.Items_CollectionChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    (e.NewItems[0] as GuiCartItem).PropertyChanged += CartItem_PropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    (e.OldItems[0] as GuiCartItem).PropertyChanged -= CartItem_PropertyChanged;
                    break;
            }

            UpdateAggregates();
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GuiCartItem.Quantity))
            {
                CartItemsRepository.Update((sender as GuiCartItem).CartItem).ConfigureAwait(true);
                UpdateAggregates();
            }
        }
        #endregion

        #region Aggregates
        private void UpdateAggregates()
        {
            ProductItemsCount = SumQuantities();
            TotalValue = SumValues();
        }

        private int SumQuantities()
        {
            return Items.Count > 0
                ? Items.Sum(item => item.Quantity)
                : 0;
        }

        private decimal SumValues()
        {
            return Items.Count > 0
                ? Items.Sum(item => item.Value)
                : 0;
        }

        public static readonly DependencyProperty ProductItemCountProperty =
            DependencyProperty.Register(nameof(ProductItemsCount), typeof(int), typeof(ShoppingCartViewModel), new PropertyMetadata(0));

        public int ProductItemsCount
        {
            get => (int)GetValue(ProductItemCountProperty);
            set => SetValue(ProductItemCountProperty, value);
        }

        public static readonly DependencyProperty TotalValueProperty =
            DependencyProperty.Register(nameof(TotalValue), typeof(decimal), typeof(ShoppingCartViewModel), new PropertyMetadata((decimal)0));

        public decimal TotalValue
        {
            get => (decimal)GetValue(TotalValueProperty);
            set => SetValue(TotalValueProperty, value);
        }
        #endregion
    }
}
