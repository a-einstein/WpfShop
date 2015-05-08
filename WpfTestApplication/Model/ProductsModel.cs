using System;
using WpfTestApplication.Data;
using WpfTestApplication.Data.ProductsDataSetTableAdapters;
using ProductCategoriesDataTable = WpfTestApplication.Data.ProductsDataSet.ProductCategoriesDataTable;
using ProductDetailsDataTable = WpfTestApplication.Data.ProductsDataSet.ProductDetailsDataTable;
using ProductDetailsRow = WpfTestApplication.Data.ProductsDataSet.ProductDetailsRow;
using ProductsOverviewDataTable = WpfTestApplication.Data.ProductsDataSet.ProductsOverviewDataTable;
using ProductSubcategoriesDataTable = WpfTestApplication.Data.ProductsDataSet.ProductSubcategoriesDataTable;
using ShoppingCartItemsDataTable = WpfTestApplication.Data.ProductsDataSet.ShoppingCartItemsDataTable;
using ShoppingCartsDataTable = WpfTestApplication.Data.ProductsDataSet.ShoppingCartsDataTable;
using ShoppingCartsRow = WpfTestApplication.Data.ProductsDataSet.ShoppingCartsRow;

namespace WpfTestApplication.Model
{
    // TODO Probably break this up into separate classes.
    class ProductsModel
    {
        private ProductsModel()
        {
            productsDataSet = new ProductsDataSet();
        }

        private static volatile ProductsModel instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static ProductsModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductsModel();
                    }
                }

                return instance;
            }
        }

        private ProductsDataSet productsDataSet;

        private ProductsOverviewDataTable products;

        public ProductsOverviewDataTable Products
        {
            get
            {
                // TODO Test and use productsDataSet.ProductsOverview directly?
                if (products == null)
                {
                    ProductsOverviewTableAdapter productsTableAdapter = new ProductsOverviewTableAdapter();

                    // Note this currently takes in all the table data. Of course this should be prefiltered and/or paged in a realistic environment. 
                    products = productsDataSet.ProductsOverview;
                    productsTableAdapter.Fill(products);
                }

                return products;
            }
        }

        // TODO Check if this a sensible way.
        // TODO Rename?
        public ProductDetailsRow ProductDetails(int productID)
        {
            ProductDetailsTableAdapter productTableAdapter = new ProductDetailsTableAdapter();

            ProductDetailsDataTable productDetailsTable = productTableAdapter.GetDataBy(productID);

            return productDetailsTable.FindByProductID(productID);
        }

        private ProductCategoriesDataTable productCategories;

        public ProductCategoriesDataTable ProductCategories
        {
            get
            {
                if (productCategories == null)
                {
                    ProductCategoriesTableAdapter categoriesTableAdapter = new ProductCategoriesTableAdapter();

                    productCategories = productsDataSet.ProductCategories;
                    productCategories.AddProductCategoriesRow(string.Empty);
                    categoriesTableAdapter.ClearBeforeFill = false;
                    categoriesTableAdapter.Fill(productCategories);
                }

                return productCategories;
            }
        }

        private ProductSubcategoriesDataTable productSubcategories;

        public ProductSubcategoriesDataTable ProductSubcategories
        {
            get
            {
                if (productSubcategories == null)
                {
                    ProductSubcategoriesTableAdapter subcategoriesTableAdapter = new ProductSubcategoriesTableAdapter();

                    productSubcategories = productsDataSet.ProductSubcategories;
                    productSubcategories.AddProductSubcategoriesRow(string.Empty, ProductCategories.FindByProductCategoryID(-1));
                    subcategoriesTableAdapter.ClearBeforeFill = false;
                    subcategoriesTableAdapter.Fill(productSubcategories);
                }

                return productSubcategories;
            }
        }

        private ShoppingCartsDataTable carts;

        private ShoppingCartsDataTable Carts
        {
            get
            {
                if (carts == null)
                {

                    // This table might be removed alltogether, as only one cart is used.
                    carts = productsDataSet.ShoppingCarts;
                }

                return carts;
            }
        }

        private ShoppingCartsRow cart;
        private const string cartId = "1";

        public ShoppingCartsRow Cart
        {
            get
            {
                if (cart == null)
                {
                    Carts.AddShoppingCartsRow(cartId);
                    Carts.AcceptChanges();

                    cart = Carts.Rows.Find(cartId) as ShoppingCartsRow;
                }

                return cart;
            }
        }

        private ShoppingCartItemsDataTable cartItems;

        public ShoppingCartItemsDataTable CartItems
        {
            get
            {
                if (cartItems == null)
                {
                    cartItems = productsDataSet.ShoppingCartItems;
                }

                return cartItems;
            }
        }
    }
}
