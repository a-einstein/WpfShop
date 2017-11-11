using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Modules.Products.Model;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    [Export]
    public class ShoppingCartViewModel : ItemsViewModel<CartItem>, IPartImportsSatisfiedNotification
    {
        #region Construction
        private ShoppingCartViewModel() { }

        private static volatile ShoppingCartViewModel instance;
        private static object syncRoot = new Object();

        [Export("WidgetViewModel", typeof(ViewModel))]
        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        // TODO This might no longer be necessary if properly shared on import.
        public static ShoppingCartViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ShoppingCartViewModel();
                        }
                    }
                }

                return instance;
            }
        }

        public async void OnImportsSatisfied()
        {
            await Refresh();
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<CartItem>(Delete);
        }
        #endregion

        #region Refresh
        // TODO This would be appropriate for an 'empty' button.
        protected override void Clear()
        {
            base.Clear();

            ClearAggregates();
        }

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized)
            {
                Items = CartItemsRepository.Instance.List;
            }

            return (baseInitialized);
        }
        #endregion

        #region CRUD
        public void CartProduct(IShoppingProduct productsOverviewObject)
        {
            CartItemsRepository.Instance.AddProduct(productsOverviewObject);
        }

        public static readonly DependencyProperty DeleteCommandProperty =
             DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(ShoppingCartViewModel));

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            private set { SetValue(DeleteCommandProperty, value); }
        }

        private void Delete(CartItem cartItem)
        {
            CartItemsRepository.Instance.DeleteProduct(cartItem);
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
            ProductItemsCount = CartItemsRepository.Instance.ProductsCount();
            TotalValue = CartItemsRepository.Instance.CartValue();
        }

        public static readonly DependencyProperty ProductItemCountProperty =
            DependencyProperty.Register(nameof(ProductItemsCount), typeof(int), typeof(ShoppingCartViewModel), new PropertyMetadata(0));

        public int ProductItemsCount
        {
            get { return (int)GetValue(ProductItemCountProperty); }
            set { SetValue(ProductItemCountProperty, value); }
        }

        public static readonly DependencyProperty TotalValueProperty =
            DependencyProperty.Register(nameof(TotalValue), typeof(Decimal), typeof(ShoppingCartViewModel), new PropertyMetadata((Decimal)0));

        public Decimal TotalValue
        {
            get { return (Decimal)GetValue(TotalValueProperty); }
            set { SetValue(TotalValueProperty, value); }
        }
        #endregion
    }
}
