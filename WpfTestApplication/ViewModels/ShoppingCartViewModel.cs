using System;
using System.Data;
using WpfTestApplication.BaseClasses;
using ShoppingCartDataTable = WpfTestApplication.Data.ProductsDataSet.ShoppingCartDataTable;
using ShoppingCartItemDataTable = WpfTestApplication.Data.ProductsDataSet.ShoppingCartItemDataTable;
using ShoppingCartItemRow = WpfTestApplication.Data.ProductsDataSet.ShoppingCartItemRow;
using ShoppingCartRow = WpfTestApplication.Data.ProductsDataSet.ShoppingCartRow;

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

        private ShoppingCartDataTable cartTable;
        private ShoppingCartRow cartRow;
        private const string cartRowId = "1";

        private ShoppingCartItemDataTable cartItemTable;

        protected override void LoadData()
        {
            // This table might be removed alltogether, as only one cart is used.
            cartTable = new ShoppingCartDataTable();
            cartTable.AddShoppingCartRow(cartRowId);
            cartTable.AcceptChanges();

            cartRow = cartTable.Rows.Find(cartRowId) as ShoppingCartRow;

            cartItemTable = new ShoppingCartItemDataTable();

            Items = cartItemTable.DefaultView;
        }

        public void AddProduct(int productId)
        {
            string productQuery = string.Format("ProductID = {0}", productId);
            DataRow[] existingCartItems = cartItemTable.Select(productQuery);

            ShoppingCartItemRow cartItem;
            if (existingCartItems.Length == 1)
            {
                cartItem = existingCartItems[0] as ShoppingCartItemRow;
                cartItem.Quantity += 1;
            }
            else
            {
                DateTime now = DateTime.Now;
                cartItem = cartItemTable.AddShoppingCartItemRow(cartRow, 1, productId, now, now);
            }

            cartItemTable.AcceptChanges();
        }
    }
}
