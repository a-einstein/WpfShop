using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.Interfaces;
using RCS.WpfShop.Common.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    public class ShoppingCartViewModel : 
        ItemsViewModel<CartItem>
    {
        #region Construction
        public ShoppingCartViewModel(IRepository<ObservableCollection<CartItem>, CartItem> cartItemsRepository)
        {
            CartItemsRepository = cartItemsRepository;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<CartItem>(Delete);
        }
        #endregion

        #region Services
        private IRepository<ObservableCollection<CartItem>, CartItem> CartItemsRepository { get; }
        #endregion

        #region Refresh
        // TODO This would be appropriate for an 'empty' button.
        protected override void Clear()
        {
            base.Clear();

            ClearAggregates();
        }

        private bool initialized;

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized && !initialized)
            {
                initialized = true;
            }

            return initialized;
        }
        #endregion

        #region CRUD
        public void CartProduct(IShoppingProduct productsOverviewObject)
        {
            CartItemsRepository.AddProduct(productsOverviewObject);
        }

        protected override async Task Read()
        {
            uiDispatcher.Invoke(delegate
            {
                foreach (var item in CartItemsRepository.List)
                {
                    Items.Add(item);
                }
            });

            UpdateAggregates();
        }

        public static readonly DependencyProperty DeleteCommandProperty =
             DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(ShoppingCartViewModel));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            private set => SetValue(DeleteCommandProperty, value);
        }

        private void Delete(CartItem cartItem)
        {
            CartItemsRepository.DeleteProduct(cartItem);
        }

        protected override void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.Items_CollectionChanged(sender, e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    (e.NewItems[0] as CartItem).PropertyChanged += CartItem_PropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    (e.OldItems[0] as CartItem).PropertyChanged -= CartItem_PropertyChanged;
                    break;
            }

            UpdateAggregates();
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity))
            {
                UpdateAggregates();
            }
        }
        #endregion

        #region Aggregates
        private void ClearAggregates()
        {
            ProductItemsCount = 0;
            TotalValue = 0;
        }

        private void UpdateAggregates()
        {
            ProductItemsCount = CartItemsRepository.ProductsCount();
            TotalValue = CartItemsRepository.CartValue();
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
