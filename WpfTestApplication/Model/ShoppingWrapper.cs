using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
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
    // Other option: make properties here on wrapper sub classes which are instantiated with a single dataset. The constructor should be restricted...?
    class ShoppingWrapper : DependencyObject, INotifyPropertyChanged
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

        public ProductsOverviewDataTable Products
        {
            get
            {
                if (productsDataSet.ProductsOverview.Count == 0)
                {
                    // Asynchronous.
                    System.ComponentModel.BackgroundWorker backgroundWorker = new System.ComponentModel.BackgroundWorker();
                    backgroundWorker.DoWork += new DoWorkEventHandler(FillProductsTable);
                    backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GetProductsCompleted);
                    backgroundWorker.RunWorkerAsync();

                    // Synchronous.
                    //FillProdTable(this, new DoWorkEventArgs(null));
                }

                return productsDataSet.ProductsOverview;
            }
        }

        public static readonly DependencyProperty ProductCollectionProperty =
            DependencyProperty.Register("ProductCollection", typeof(ObservableCollection<ProductsOverviewRow>), typeof(ShoppingWrapper), new PropertyMetadata(new ObservableCollection<ProductsOverviewRow>()));

        // Alternative collection to signal CollectionChanged.
        public ObservableCollection<ProductsOverviewRow> ProductsCollection
        {
            get { return (ObservableCollection<ProductsOverviewRow>)GetValue(ProductCollectionProperty); }
            set { SetValue(ProductCollectionProperty, value); }
        }

        private void FillProductsTable(object sender, DoWorkEventArgs e)
        {
            ProductsOverviewTableAdapter productsTableAdapter = new ProductsOverviewTableAdapter();

            // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
            // Note this only retrieves the data once. whereas it would probably retrieve it every time in a realistic situation.
            productsTableAdapter.Fill(productsDataSet.ProductsOverview);

            // Should this be used? It does not seem assignable.
            e.Result = productsDataSet.ProductsOverview;
        }

        public ProductsOverviewDataTable ProductsHack;

        protected void GetProductsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception("Error retrieving data.");
            else
            {
                // This doe not seem assignable.
                //productsDataSet.ProductsOverview.DefaultView = (e.Result  as ProductsOverviewDataTable).DefaultView;

                //ProductsHack = e.Result as ProductsOverviewDataTable;

                ProductsCollection.Clear();

                // Convert.
                foreach (var row in productsDataSet.ProductsOverview)
                {
                    ProductsCollection.Add(row);
                }

                RaisePropertyChanged("Products");
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
