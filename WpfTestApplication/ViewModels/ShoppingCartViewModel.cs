using System;
using System.Data;
using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Model;
using ProductsOverviewRow = WpfTestApplication.Model.ProductsDataSet.ProductsOverviewRow;
using ShoppingCartItemsRow = WpfTestApplication.Model.ProductsDataSet.ShoppingCartItemsRow;

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
            Items = ProductsModel.Instance.CartItems.DefaultView;
        }

        // TODO Should use a method of a model class.
        public void AddProduct(int productId)
        {
            string productQuery = string.Format("ProductID = {0}", productId);
            DataRow[] existingCartItems = ProductsModel.Instance.CartItems.Select(productQuery);

            ShoppingCartItemsRow cartItem;
            if (existingCartItems.Length == 1)
            {
                cartItem = existingCartItems[0] as ShoppingCartItemsRow;
                cartItem.Quantity += 1;
            }
            else
            {
                DateTime now = DateTime.Now;

                // Need acces to ProductsOverviewDataTable, or just a row.
                // In case of a row: this could be passed from ProductsViewModel, but not from ProductViewModel (it is still desirable to order there too).
                ProductsOverviewRow productRow = ProductsModel.Instance.Products.FindByProductID(productId);

                cartItem = ProductsModel.Instance.CartItems.AddShoppingCartItemsRow(ProductsModel.Instance.Cart, 1, productRow, now, now);
            }

            ProductsModel.Instance.CartItems.AcceptChanges();
            // TODO Needed?
            RaisePropertyChanged("Items");

            // TODO The binding seemed to work on Items.Count too.
            RaisePropertyChanged("ItemsCount");

            ProductItemCount = Convert.ToInt32(ProductsModel.Instance.CartItems.Compute("Sum(Quantity)", null));
            TotalValue = Convert.ToDouble(ProductsModel.Instance.CartItems.Compute("Sum(Value)", null));
        }

        public static readonly DependencyProperty ProductItemCountProperty =
            DependencyProperty.Register("ProductItemCount", typeof(int), typeof(ShoppingCartViewModel), new PropertyMetadata(0));

        public int ProductItemCount
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
