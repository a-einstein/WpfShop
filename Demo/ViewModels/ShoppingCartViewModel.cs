using Demo.BaseClasses;
using Demo.Model;
using Microsoft.Practices.Prism.Commands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Demo.ViewModels
{
    public class ShoppingCartViewModel : ItemsViewModel
    {
        private ShoppingCartViewModel()
        {
            CartItemsRepository.Instance.Items.ListChanged += CartItems_ListChanged;

            Refresh();
        }

        private static volatile ShoppingCartViewModel instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static ShoppingCartViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ShoppingCartViewModel();
                    }
                }

                return instance;
            }
        }

        public override void Refresh()
        {
            Items = CartItemsRepository.Instance.Items;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<object>(Delete);
        }

        public override object NoId { get { return ShoppingWrapper.NoId; } }

        public void CartProduct(int productId)
        {
            CartItemsRepository.Instance.AddProduct(productId);
        }

        public ICommand DeleteCommand { get; set; }

        private void Delete(object parameter)
        {
            int cartItemID = (int)parameter;

            CartItemsRepository.Instance.DeleteProduct(cartItemID);
        }

        private void CartItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemChanged:
                    // Note that Quantity may become 0, which results in correct totals, but with an 'empty' CartItem still present, which can be removed by the delete button.
                    // This may either be considered handy as the user can correct a mistake, or be considered confusing.
                    // TODO Maybe remove CartItem on Quantity == 0, but that can not trivially be done here.
                    if (e.PropertyDescriptor != null && e.PropertyDescriptor.Name == "Quantity")
                        ProductItemsCount = CartItemsRepository.Instance.ProductsCount();

                    // Note that Value is calculated once AFTER setting of Quantity. So that is the trigger to calculate totals.
                    // Note that the price may be 0, so that Value does not change on changes of Quantity.
                    if (e.PropertyDescriptor != null && e.PropertyDescriptor.Name == "Value")
                        TotalValue = CartItemsRepository.Instance.CartValue();

                    break;

                // Note that this turned out a more reliable event than ItemAdded and ItemDeleted.
                case ListChangedType.Reset:
                    RaisePropertyChanged("ItemsCount");
                    ProductItemsCount = CartItemsRepository.Instance.ProductsCount();
                    TotalValue = CartItemsRepository.Instance.CartValue();
                    break;
            }
        }

        public static readonly DependencyProperty ProductItemCountProperty =
            DependencyProperty.Register("ProductItemsCount", typeof(int), typeof(ShoppingCartViewModel), new PropertyMetadata(0));

        public int ProductItemsCount
        {
            get { return (int)GetValue(ProductItemCountProperty); }
            set { SetValue(ProductItemCountProperty, value); }
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("TotalValue", typeof(Double), typeof(ShoppingCartViewModel), new PropertyMetadata(0.0));

        public Double TotalValue
        {
            get { return (Double)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }
    }
}
