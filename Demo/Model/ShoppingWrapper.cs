using ServiceClients.Products.ServiceReference;
using System;
using System.Data;
using System.Threading.Tasks;
using ProductCategoriesDataTable = Demo.Model.ProductsDataSet.ProductCategoriesDataTable;
using ProductDetailsDataTable = Demo.Model.ProductsDataSet.ProductDetailsDataTable;
using ProductDetailsRow = Demo.Model.ProductsDataSet.ProductDetailsRow;
using ProductsOverviewDataTable = Demo.Model.ProductsDataSet.ProductsOverviewDataTable;
using ProductSubcategoriesDataTable = Demo.Model.ProductsDataSet.ProductSubcategoriesDataTable;
using ShoppingCartItemsDataTable = Demo.Model.ProductsDataSet.ShoppingCartItemsDataTable;
using ShoppingCartItemsRow = Demo.Model.ProductsDataSet.ShoppingCartItemsRow;
using ShoppingCartsDataTable = Demo.Model.ProductsDataSet.ShoppingCartsDataTable;
using ShoppingCartsRow = Demo.Model.ProductsDataSet.ShoppingCartsRow;

namespace Demo.Model
{
    // TODO Maybe put this functionality into the partial sub classes of ProductsDataSet. But ProductsDataSet could not be a singleton .
    // Other option: make properties here on wrapper sub classes which are instantiated with a single dataset. The constructor should be restricted some way.

    // TODO After having moved data retrieval to a service, on both sides is still hung on to a DataSet, with conversions as a result. That may be optimized.

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

        // Choose for an int as this is the actual type of the Id.
        public int NoId { get { return -1; } }

        private ProductsDataSet productsDataSet;

        private ProductsServiceClient productsServiceClient;

        protected ProductsServiceClient ProductsServiceClient
        {
            get
            {
                // HACK Make shared some other way.
                if (productsServiceClient == null)
                    productsServiceClient = new ProductsServiceClient();

                return productsServiceClient;
            }
        }

        public async Task<DataView> GetProductsOverview()
        {
            var table = productsDataSet.ProductsOverview;

            var task = Task.Factory.StartNew(async () =>
            {
                if (table.Count == 0)
                {
                    // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
                    var productOverviewDto = await ProductsServiceClient.GetProductsOverviewAsync();

                    ConvertInto(table, productOverviewDto);
                }

                return table.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        // Use the table as a reference, cannot assign to.
        // TODO Is this a reference?
        private static void ConvertInto(ProductsOverviewDataTable overview, ProductsOverviewListDto listDto)
        {
            foreach (var dtoRow in listDto)
            {
                var overviewRow = overview.AddProductsOverviewRow
                (
                    dtoRow.ProductID,
                    dtoRow.Name,
                    dtoRow.Color,
                    dtoRow.ListPrice,
                    dtoRow.Size,
                    dtoRow.SizeUnitMeasureCode,
                    dtoRow.WeightUnitMeasureCode,
                    dtoRow.ThumbNailPhoto,
                    dtoRow.ProductCategoryID,
                    dtoRow.ProductCategory,
                    dtoRow.ProductSubcategoryID,
                    dtoRow.ProductSubcategory
                 );
            }
        }

        public async Task<ProductDetailsRow> GetProductDetails(int productID)
        {
            var table = productsDataSet.ProductDetails;

            var task = Task.Factory.StartNew(async () =>
            {
                var productDetailsDto = await ProductsServiceClient.GetProductDetailsAsync(productID);

                // TODO This step may not be necessary in the future, when working directly with the Dto.
                var productDetailsRow = Convert(table, productDetailsDto);

                return productDetailsRow;
            });

            // TODO ?!
            return await task.Result;
        }

        private static ProductDetailsRow Convert(ProductDetailsDataTable table, ProductDetailsRowDto productDto)
        {
            var productRow = table.NewRow
            (
                productDto.ProductID,
                productDto.Name,
                productDto.ListPrice,
                productDto.Color,
                productDto.Size,
                productDto.SizeUnitMeasureCode,
                productDto.Weight,
                productDto.WeightUnitMeasureCode,
                productDto.LargePhoto,
                productDto.ModelName,
                productDto.Description,
                productDto.ProductCategoryID,
                productDto.ProductCategory,
                productDto.ProductSubcategoryID,
                productDto.ProductSubcategory
            );

            return productRow;
        }

        public async Task<DataView> GetProductCategories()
        {
            var table = productsDataSet.ProductCategories;

            var task = Task.Factory.StartNew(async () =>
            {

                if (table.Count == 0)
                {
                    var listDto = await ProductsServiceClient.GetProductCategoriesAsync();

                    // Add an empty element.
                    var overviewRow = table.AddProductCategoriesRow(NoId, string.Empty);

                    ConvertInto(table, listDto);
                }

                return table.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        // Use the table as a reference, cannot assign to.
        // TODO Is this a reference?
        private static void ConvertInto(ProductCategoriesDataTable overview, ProductCategoryListDto listDto)
        {
            foreach (var dtoRow in listDto)
            {
                var overviewRow = overview.AddProductCategoriesRow
                (
                    dtoRow.ProductCategoryID,
                    dtoRow.Name
                );
            }
        }

        public async Task<DataView> GetProductSubcategories()
        {
            var table = productsDataSet.ProductSubcategories;

            var task = Task.Factory.StartNew(async () =>
            {
                if (table.Count == 0)
                {
                    var listDto = await ProductsServiceClient.GetProductSubcategoriesAsync();

                    // Add an empty element.
                    table.AddProductSubcategoriesRow(NoId, string.Empty, NoId);

                    ConvertInto(table, listDto);
                }

                return table.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        // Use the table as a reference, cannot assign to.
        // TODO Is this a reference?
        private static void ConvertInto(ProductSubcategoriesDataTable overview, ProductSubcategoryListDto listDto)
        {
            foreach (var dtoRow in listDto)
            {
                var overviewRow = overview.AddProductSubcategoriesRow
                (
                    dtoRow.ProductSubcategoryID,
                    dtoRow.Name,
                    dtoRow.ProductCategoryID
                );
            }
        }

        // TODO This table might be removed alltogether, as only one cart is used. Then the current relation and non nullable key should be remove, or made nullable.
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
            var productQuery = string.Format("ProductID = {0}", productId);
            var existingCartItems = CartItems.Select(productQuery);

            if (existingCartItems.Length == 0)
            {
                var now = DateTime.Now;
                var productRow = productsDataSet.ProductsOverview.FindByProductID((int)productId);

                // Note that ShoppingCartId currently is non nullable.
                CartItems.AddShoppingCartItemsRow(Cart, 1, productRow, now, now);
                CartItems.AcceptChanges();
            }
            else if (existingCartItems.Length == 1)
            {
                var cartItem = existingCartItems[0] as ShoppingCartItemsRow;
                cartItem.Quantity += 1;
                cartItem.AcceptChanges();
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);
        }

        public void CartItemDelete(int cartItemID)
        {
            var cartItem = CartItems.FindByShoppingCartItemID(cartItemID);

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
            ? System.Convert.ToInt32(ShoppingWrapper.Instance.CartItems.Compute("Sum(Quantity)", null))
            : 0;
        }

        public double CartValue()
        {
            return CartItems.Count > 0
            ? System.Convert.ToDouble(CartItems.Compute("Sum(Value)", null))
            : 0.0;
        }
    }
}
