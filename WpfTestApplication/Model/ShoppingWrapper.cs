using System;
using System.ComponentModel;
using System.Data;
using WpfTestApplication.Model.ProductsDataSetTableAdapters;
using ProductDetailsDataTable = WpfTestApplication.Model.ProductsDataSet.ProductDetailsDataTable;
using ProductsOverviewRow = WpfTestApplication.Model.ProductsDataSet.ProductsOverviewRow;
using ShoppingCartItemsDataTable = WpfTestApplication.Model.ProductsDataSet.ShoppingCartItemsDataTable;
using ShoppingCartItemsRow = WpfTestApplication.Model.ProductsDataSet.ShoppingCartItemsRow;
using ShoppingCartsDataTable = WpfTestApplication.Model.ProductsDataSet.ShoppingCartsDataTable;
using ShoppingCartsRow = WpfTestApplication.Model.ProductsDataSet.ShoppingCartsRow;

namespace WpfTestApplication.Model
{
    // TODO Maybe put this functionality into the partial sub classes of ProductsDataSet. Problem: would not be able to have a singleton ProductsDataSet.
    // Other option: make properties here on wrapper sub classes which are instantiated with a single dataset. The constructor should be restricted...?
    class ShoppingWrapper
    {
        private ShoppingWrapper()
        {
            productsDataSet = new ProductsDataSet();
        }

        private static volatile ShoppingWrapper instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
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

        public void BeginGetProducts(RunWorkerCompletedEventHandler completer)
        {
            BeginGetData(new DoWorkEventHandler(FillProductsTable), completer);
        }

        private void FillProductsTable(object sender, DoWorkEventArgs e)
        {
            // Note this only retrieves the data once. whereas it would probably retrieve it every time in a realistic situation.
            if (productsDataSet.ProductsOverview.Count == 0)
            {
                ProductsOverviewTableAdapter productsTableAdapter = new ProductsOverviewTableAdapter();

                // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
                productsTableAdapter.Fill(productsDataSet.ProductsOverview);
            }

            e.Result = productsDataSet.ProductsOverview;
        }

        public void BeginGetProductDetails(object productID, RunWorkerCompletedEventHandler completer)
        {
            BeginGetData(new DoWorkEventHandler(GetProductDetails), completer, productID);
        }

        private void GetProductDetails(object sender, DoWorkEventArgs e)
        {
            int productID = (int)e.Argument;

            ProductDetailsTableAdapter productTableAdapter = new ProductDetailsTableAdapter();

            // Note this always tries to retrieve a record from the DB.
            ProductDetailsDataTable productDetailsTable = productTableAdapter.GetDataBy(productID);

            e.Result = productDetailsTable.FindByProductID(productID);
        }

        public void BeginGetProductCategories(RunWorkerCompletedEventHandler completer)
        {
            BeginGetData(new DoWorkEventHandler(FillProductCategoriesTable), completer);
        }

        private void FillProductCategoriesTable(object sender, DoWorkEventArgs e)
        {
            if (productsDataSet.ProductCategories.Count == 0)
            {
                ProductCategoriesTableAdapter categoriesTableAdapter = new ProductCategoriesTableAdapter();

                // Add an empty element.
                productsDataSet.ProductCategories.AddProductCategoriesRow(string.Empty);

                categoriesTableAdapter.ClearBeforeFill = false;
                // Note this only retrieves the data once, whereas it would probably retrieve it every time in a realistic situation.
                categoriesTableAdapter.Fill(productsDataSet.ProductCategories);
            }

            e.Result = productsDataSet.ProductCategories;
        }

        public void BeginGetProductSubcategories(RunWorkerCompletedEventHandler completer)
        {
            BeginGetData(new DoWorkEventHandler(FillProductSubcategoriesTable), completer);
        }

        private void FillProductSubcategoriesTable(object sender, DoWorkEventArgs e)
        {
            if (productsDataSet.ProductSubcategories.Count == 0)
            {
                ProductSubcategoriesTableAdapter subcategoriesTableAdapter = new ProductSubcategoriesTableAdapter();

                // Add an empty element.
                productsDataSet.ProductSubcategories.AddProductSubcategoriesRow(string.Empty, productsDataSet.ProductCategories.FindByProductCategoryID(-1));

                subcategoriesTableAdapter.ClearBeforeFill = false;
                // Note this only retrieves the data once, whereas it would probably retrieve it every time in a realistic situation.
                subcategoriesTableAdapter.Fill(productsDataSet.ProductSubcategories);
            }

            e.Result = productsDataSet.ProductSubcategories;
        }

        private void BeginGetData(DoWorkEventHandler worker, RunWorkerCompletedEventHandler completer, object argument = null)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += worker;
            backgroundWorker.RunWorkerCompleted += completer;

            backgroundWorker.RunWorkerAsync(argument);
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

        private ShoppingCartsRow Cart
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

        private const string cartItemsNumberExceptionMessage = "Unexpected number of found ShoppingCartItemsRows.";

        public void CartProduct(int productId)
        {
            string productQuery = string.Format("ProductID = {0}", productId);
            DataRow[] existingCartItems = CartItems.Select(productQuery);

            if (existingCartItems.Length == 0)
            {
                DateTime now = DateTime.Now;
                ProductsOverviewRow productRow = productsDataSet.ProductsOverview.FindByProductID(productId);

                CartItems.AddShoppingCartItemsRow(Cart, 1, productRow, now, now);
                CartItems.AcceptChanges();
            }
            else if (existingCartItems.Length == 1)
            {
                ShoppingCartItemsRow cartItem = existingCartItems[0] as ShoppingCartItemsRow;
                cartItem.Quantity += 1;
                cartItem.AcceptChanges();
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);
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
            return CartItems.Count > 0
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
