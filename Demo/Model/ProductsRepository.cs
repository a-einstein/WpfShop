using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Data;
using System.Threading.Tasks;
using ProductCategoriesDataTable = Demo.Model.ProductsDataSet.ProductCategoriesDataTable;
using ProductDetailsDataTable = Demo.Model.ProductsDataSet.ProductDetailsDataTable;
using ProductDetailsRow = Demo.Model.ProductsDataSet.ProductDetailsRow;
using ProductsOverviewDataTable = Demo.Model.ProductsDataSet.ProductsOverviewDataTable;
using ProductsOverviewRow = Demo.Model.ProductsDataSet.ProductsOverviewRow;
using ProductSubcategoriesDataTable = Demo.Model.ProductsDataSet.ProductSubcategoriesDataTable;

namespace Demo.Model
{
    public class ProductsRepository
    {
        private ProductsRepository()
        { }

        private static volatile ProductsRepository instance;
        private static object syncRoot = new Object();

        public static ProductsRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductsRepository();
                    }
                }

                return instance;
            }
        }

        private ProductsServiceClient productsServiceClient;

        private ProductsServiceClient ProductsServiceClient
        {
            get
            {
                if (productsServiceClient == null)
                    productsServiceClient = new ProductsServiceClient();

                return productsServiceClient;
            }
        }

        // TODO Decide where to put this.
        public int NoId { get { return ShoppingWrapper.NoId; } }

        private ProductsOverviewDataTable ProductsOverviewDataTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductsOverview; }
        }

        public void Clear()
        {
            ProductsOverviewDataTable.Clear();
            ProductsOverviewDataTable.AcceptChanges();
        }

        // Currently this only made for testpurposes and stores only locally.
        public void CreateOverviewProduct(ProductsOverviewRowDto rowDto)
        {
            var tableRow = Convert(rowDto);
            ProductsOverviewDataTable.Rows.Add(tableRow);
            ProductsOverviewDataTable.AcceptChanges();
        }

        public async Task<DataView> ReadOverview()
        {
            var table = ShoppingWrapper.Instance.ProductsDataSet.ProductsOverview;

            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                // This also enables testing without a connection.
                if (table.Count == 0)
                {
                    // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
                    var productOverviewDto = await ProductsServiceClient.GetProductsOverviewAsync();

                    foreach (var dtoRow in productOverviewDto)
                    {
                        var tableRow = Convert(dtoRow);
                        table.Rows.Add(tableRow);
                    }
                }

                return table.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        private ProductsOverviewRow Convert(ProductsOverviewRowDto dtoRow)
        {
            var tableRow = ProductsOverviewDataTable.NewRow
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

            return tableRow;
        }

        public async Task<ProductDetailsRow> ReadDetails(int productID)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var productDetailsDto = await ProductsServiceClient.GetProductDetailsAsync(productID);

                // TODO This step may not be necessary in the future, when working directly with the Dto.
                var productDetailsRow = Convert(productDetailsDto);

                return productDetailsRow;
            });

            // TODO ?!
            return await task.Result;
        }

        private ProductDetailsDataTable ProductDetailsDataTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductDetails; }
        }

        private ProductDetailsRow Convert(ProductDetailsRowDto productDto)
        {
            var tableRow = ProductDetailsDataTable.NewRow
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

            return tableRow;
        }

        // TODO Put categories stuff into separate repository?
        public async Task<DataView> ReadProductCategories()
        {
            var table = ShoppingWrapper.Instance.ProductsDataSet.ProductCategories;

            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
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
        // TODO Refactor this like elsewhere.
        private static void ConvertInto(ProductCategoriesDataTable table, ProductCategoryListDto listDto)
        {
            foreach (var dtoRow in listDto)
            {
                var tableRow = table.AddProductCategoriesRow
                (
                    dtoRow.ProductCategoryID,
                    dtoRow.Name
                );
            }
        }

        public async Task<DataView> ReadProductSubcategories()
        {
            var table = ShoppingWrapper.Instance.ProductsDataSet.ProductSubcategories;

            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                if (table.Count == 0)
                {
                    var listDto = await ProductsServiceClient.GetProductSubcategoriesAsync();

                    // Add an empty element.
                    table.AddProductSubcategoriesRow(ShoppingWrapper.NoId, string.Empty, ShoppingWrapper.NoId);

                    ConvertInto(table, listDto);
                }

                return table.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        // Use the table as a reference, cannot assign to.
        // TODO Is this a reference?
        // TODO Refactor this like elsewhere.
        private static void ConvertInto(ProductSubcategoriesDataTable table, ProductSubcategoryListDto listDto)
        {
            foreach (var dtoRow in listDto)
            {
                var tableRow = table.AddProductSubcategoriesRow
                (
                    dtoRow.ProductSubcategoryID,
                    dtoRow.Name,
                    dtoRow.ProductCategoryID
                );
            }
        }
    }
}
