using System;
using System.Data;
using WpfTestApplication.Model.ProductsDataSetTableAdapters;
using ProductCategoriesDataTable = WpfTestApplication.Model.ProductsDataSet.ProductCategoriesDataTable;
using ProductDetailsDataTable = WpfTestApplication.Model.ProductsDataSet.ProductDetailsDataTable;
using ProductDetailsRow = WpfTestApplication.Model.ProductsDataSet.ProductDetailsRow;
using ProductsOverviewDataTable = WpfTestApplication.Model.ProductsDataSet.ProductsOverviewDataTable;
using ProductsOverviewRow = WpfTestApplication.Model.ProductsDataSet.ProductsOverviewRow;
using ProductSubcategoriesDataTable = WpfTestApplication.Model.ProductsDataSet.ProductSubcategoriesDataTable;
using ShoppingCartItemsDataTable = WpfTestApplication.Model.ProductsDataSet.ShoppingCartItemsDataTable;
using ShoppingCartItemsRow = WpfTestApplication.Model.ProductsDataSet.ShoppingCartItemsRow;
using ShoppingCartsDataTable = WpfTestApplication.Model.ProductsDataSet.ShoppingCartsDataTable;
using ShoppingCartsRow = WpfTestApplication.Model.ProductsDataSet.ShoppingCartsRow;

namespace WpfTestApplication.Model
{
    // TODO Maybe put this functionality into the partial sub classes of ProductsDataSet. Problem: would not be able to have a singleton ProductsDataSet.
    class ShoppingWrapper
    {
        private ShoppingWrapper()
        {
            productsDataSet = new ProductsDataSet();
        }

        private static volatile ShoppingWrapper instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static ShoppingWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ShoppingWrapper();
                    }
                }

                return instance;
            }
        }

        private ProductsDataSet productsDataSet;

        public ProductsOverviewDataTable Products
        {
            get
            {
                if (productsDataSet.ProductsOverview.Count == 0)
                {
                    ProductsOverviewTableAdapter productsTableAdapter = new ProductsOverviewTableAdapter();

                    // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
                    // Note this only retrieves the data once. whereas it would probably retrieve it every time in a realistic situation.
                    productsTableAdapter.Fill(productsDataSet.ProductsOverview);
                }

                return productsDataSet.ProductsOverview;
            }
        }

        public ProductDetailsRow ProductDetails(int productID)
        {
            ProductDetailsTableAdapter productTableAdapter = new ProductDetailsTableAdapter();

            // Note this always tries to retrieve a record from the DB.
            ProductDetailsDataTable productDetailsTable = productTableAdapter.GetDataBy(productID);

            return productDetailsTable.FindByProductID(productID);
        }

        public ProductCategoriesDataTable ProductCategories
        {
            get
            {
                if (productsDataSet.ProductCategories.Count == 0)
                {
                    ProductCategoriesTableAdapter categoriesTableAdapter = new ProductCategoriesTableAdapter();

                    // Add an empty element.
                    productsDataSet.ProductCategories.AddProductCategoriesRow(string.Empty);

                    categoriesTableAdapter.ClearBeforeFill = false;
                    // Note this only retrieves the data once. whereas it would probably retrieve it every time in a realistic situation.
                    categoriesTableAdapter.Fill(productsDataSet.ProductCategories);
                }

                return productsDataSet.ProductCategories;
            }
        }

        public ProductSubcategoriesDataTable ProductSubcategories
        {
            get
            {
                if (productsDataSet.ProductSubcategories.Count == 0)
                {
                    ProductSubcategoriesTableAdapter subcategoriesTableAdapter = new ProductSubcategoriesTableAdapter();

                    // Add an empty element.
                    productsDataSet.ProductSubcategories.AddProductSubcategoriesRow(string.Empty, ProductCategories.FindByProductCategoryID(-1));

                    subcategoriesTableAdapter.ClearBeforeFill = false;
                    // Note this only retrieves the data once. whereas it would probably retrieve it every time in a realistic situation.
                    subcategoriesTableAdapter.Fill(productsDataSet.ProductSubcategories);
                }

                return productsDataSet.ProductSubcategories;
            }
        }

        // TODO This table might be removed alltogether, as only one cart is used.
        private ShoppingCartsDataTable Carts
        {
            get
            {
                return productsDataSet.ShoppingCarts;
            }
        }

        private ShoppingCartsRow cart;

        public ShoppingCartsRow Cart
        {
            get
            {
                if (cart == null)
                {
                    const string cartId = "1";

                    Carts.AddShoppingCartsRow(cartId);
                    Carts.AcceptChanges();

                    cart = Carts.Rows.Find(cartId) as ShoppingCartsRow;
                }

                return cart;
            }
        }

        public ShoppingCartItemsDataTable CartItems
        {
            get
            {
                // Note that simply the whole table is used, as all items belong to the user.
                // It is only kept in memory and not preserved. It is anticipated that only real orders are preserved.
                return productsDataSet.ShoppingCartItems;
            }
        }

        const string cartItemsNumberExceptionMessage = "Unexpected number of found ShoppingCartItemsRows.";

        public void CartItemQuantityIncrease(int productId)
        {
            string productQuery = string.Format("ProductID = {0}", productId);
            DataRow[] existingCartItems = CartItems.Select(productQuery);

            ShoppingCartItemsRow cartItem;

            if (existingCartItems.Length == 0)
            {
                DateTime now = DateTime.Now;

                // Need acces to ProductsOverviewDataTable, or just a row.
                // In case of a row: this could be passed from ProductsViewModel, but not from ProductViewModel (it is still desirable to order there too).
                ProductsOverviewRow productRow = Products.FindByProductID(productId);

                cartItem = CartItems.AddShoppingCartItemsRow(Cart, 1, productRow, now, now);
            }
            else if (existingCartItems.Length == 1)
            {
                cartItem = existingCartItems[0] as ShoppingCartItemsRow;
                cartItem.Quantity += 1;
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);

            CartItems.AcceptChanges();
        }

        public void CartItemDelete(int cartItemID)
        {
            ShoppingCartItemsRow cartItem = CartItems.FindByShoppingCartItemID(cartItemID);

            if (cartItem != null)
            {
                cartItem.Delete();
                CartItems.AcceptChanges();
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);
        }

        public int CartProductItemsCount()
        {
            return  CartItems.Count > 0
            ? Convert.ToInt32(ShoppingWrapper.Instance.CartItems.Compute("Sum(Quantity)", null))
            : 0;
        }

        public double CartValue()
        {
            return CartItems.Count > 0
            ? Convert.ToDouble(CartItems.Compute("Sum(Value)", null))
            : 0.0;
        }
    }
}
