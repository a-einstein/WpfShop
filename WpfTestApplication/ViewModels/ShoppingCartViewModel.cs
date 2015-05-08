using System;
using System.Windows;
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
            Items = ShoppingWrapper.Instance.CartItems.DefaultView;
        }

        public void Add(int productId)
        {
            ShoppingWrapper.Instance.AddToCart(productId);

            RaisePropertyChanged("ItemsCount");

            ProductItemsCount = Convert.ToInt32(ShoppingWrapper.Instance.CartItems.Compute("Sum(Quantity)", null));
            TotalValue = Convert.ToDouble(ShoppingWrapper.Instance.CartItems.Compute("Sum(Value)", null));
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
