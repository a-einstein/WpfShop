using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Interfaces;
using RCS.WpfShop.Common.Interfaces;
using RCS.WpfShop.Common.ViewModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    /// <summary>
    /// Collection level Viewmodel on CartItems.
    /// </summary>
    public class CartViewModel :
        ItemsViewModel<CartItemViewModel>
    {
        #region Construction
        public CartViewModel(IRepository<List<CartItem>, CartItem> cartItemsRepository)
        {
            CartItemsRepository = cartItemsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<CartItemViewModel>(async (guiCartItem) => await Delete(guiCartItem));
        }
        #endregion

        #region Services
        private IRepository<List<CartItem>, CartItem> CartItemsRepository { get; }
        #endregion

        #region Refresh
        // Note that the repository is leading. Changes to the collection are performed there.
        // After which a new view is created by reloading.

        protected override void ClearView()
        {
            UpdateAggregates();

            base.ClearView();
        }
        #endregion

        #region CRUD
        public async Task CartProduct(IShoppingProduct shoppingProduct)
        {
            var existing = Items.FirstOrDefault(item => item.ProductId == shoppingProduct.Id);

            if (existing == default)
            {
                await CartItemsRepository.Create(new CartItem(shoppingProduct)).ConfigureAwait(true);

                await RefreshView().ConfigureAwait(true);
            }
            else
            {
                existing.Quantity++;

                UpdateAggregates();
            }
        }

        protected override async Task Read()
        {
            await uiDispatcher.InvokeAsync(() =>
            {
                // TODO Perhaps hide Repository.Items.
                // Use an asynchronous Read.

                foreach (var item in CartItemsRepository.Items)
                {
                    Items.Add(new CartItemViewModel(item));
                }
            });

            UpdateAggregates();

            await base.Read();
        }

        public static readonly DependencyProperty DeleteCommandProperty =
             DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(CartViewModel));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            private set => SetValue(DeleteCommandProperty, value);
        }

        private async Task Delete(CartItemViewModel cartItem)
        {
            await CartItemsRepository.Delete(cartItem.CartItem);

            await RefreshView();
        }

        protected override void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.Items_CollectionChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    (e.NewItems[0] as CartItemViewModel).PropertyChanged += CartItem_PropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    (e.OldItems[0] as CartItemViewModel).PropertyChanged -= CartItem_PropertyChanged;
                    break;
            }
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItemViewModel.Quantity))
            {
                // Aggregate from the single to the collection level.
                UpdateAggregates();
            }
        }
        #endregion

        #region Aggregates
        private void UpdateAggregates()
        {
            uiDispatcher.Invoke(() =>
            {
                ProductItemsCount = SumQuantities();
                TotalValue = SumValues();
            });
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
            DependencyProperty.Register(nameof(ProductItemsCount), typeof(int), typeof(CartViewModel), new PropertyMetadata(0));

        public int ProductItemsCount
        {
            get => (int)GetValue(ProductItemCountProperty);
            set => SetValue(ProductItemCountProperty, value);
        }

        public static readonly DependencyProperty TotalValueProperty =
            DependencyProperty.Register(nameof(TotalValue), typeof(decimal), typeof(CartViewModel), new PropertyMetadata((decimal)0));

        public decimal TotalValue
        {
            get => (decimal)GetValue(TotalValueProperty);
            set => SetValue(TotalValueProperty, value);
        }
        #endregion
    }
}
