using Microsoft.Practices.Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Model;

namespace WpfTestApplication.ViewModels
{
    public class ShoppingCartViewModel : ItemsViewModel
    {
        private ShoppingCartViewModel() { }

        private static volatile ShoppingCartViewModel instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way of https://msdn.microsoft.com/en-us/library/ff650316.aspx
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

        protected override void LoadData()
        {
            // TODO Maybe make a separate property Items for this in a new wrapper class CartItems. And so forth. Check what this means for filtering.
            Items = ShoppingWrapper.Instance.CartItems.DefaultView;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<object>(Delete);
        }

        public void Increase(int productId)
        {
            ShoppingWrapper.Instance.CartItemQuantityIncrease(productId);
            UpdateTotals();
        }

        public ICommand DeleteCommand { get; set; }

        private void Delete(object parameter)
        {
            int cartItemID = (int)parameter;

            ShoppingWrapper.Instance.CartItemDelete(cartItemID);
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            RaisePropertyChanged("ItemsCount");

            ProductItemsCount = ShoppingWrapper.Instance.CartProductItemsCount();
            TotalValue = ShoppingWrapper.Instance.CartValue();
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
