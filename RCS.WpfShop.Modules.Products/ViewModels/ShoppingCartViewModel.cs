using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Modules.Products.Model;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    public class ShoppingCartViewModel : ItemsViewModel<CartItem>
    {
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
                Items = CartItemsRepository.Instance?.List;
                initialized = true;
            }

            return initialized;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<CartItem>(Delete);
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
            get => (ICommand)GetValue(DeleteCommandProperty);
            private set => SetValue(DeleteCommandProperty, value);
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
