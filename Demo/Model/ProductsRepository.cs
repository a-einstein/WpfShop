using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Data;
using System.Threading.Tasks;
using ProductCategoriesDataTable = Demo.Model.ProductsDataSet.ProductCategoriesDataTable;
using ProductCategoriesRow = Demo.Model.ProductsDataSet.ProductCategoriesRow;
using ProductDetailsDataTable = Demo.Model.ProductsDataSet.ProductDetailsDataTable;
using ProductDetailsRow = Demo.Model.ProductsDataSet.ProductDetailsRow;
using ProductsOverviewDataTable = Demo.Model.ProductsDataSet.ProductsOverviewDataTable;
using ProductsOverviewRow = Demo.Model.ProductsDataSet.ProductsOverviewRow;
using ProductSubcategoriesDataTable = Demo.Model.ProductsDataSet.ProductSubcategoriesDataTable;
using ProductSubcategoriesRow = Demo.Model.ProductsDataSet.ProductSubcategoriesRow;

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
            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                // This also enables testing without a connection.
                if (ProductsOverviewDataTable.Count == 0)
                {
                    // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
                    var productOverviewDto = await ProductsServiceClient.GetProductsOverviewAsync();

                    foreach (var dtoRow in productOverviewDto)
                    {
                        var tableRow = Convert(dtoRow);
                        ProductsOverviewDataTable.Rows.Add(tableRow);
                    }
                }

                ProductsOverviewDataTable.AcceptChanges();

                return ProductsOverviewDataTable.DefaultView;
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

        private ProductCategoriesDataTable ProductCategoriesDataTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductCategories; }
        }

        // TODO Put categories stuff into separate repository?
        public async Task<DataView> ReadProductCategories(bool addEmptyElement = true)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                if (ProductCategoriesDataTable.Count == 0)
                {
                    var listDto = await ProductsServiceClient.GetProductCategoriesAsync();

                    if (addEmptyElement)
                    {
                    var tableRow = ProductCategoriesDataTable.NewRow(NoId, string.Empty);
                    ProductCategoriesDataTable.Rows.Add(tableRow);
                    }

                    foreach (var dtoRow in listDto)
                    {
                        var tableRow = Convert(dtoRow);
                        ProductCategoriesDataTable.Rows.Add(tableRow);
                    }

                    ProductCategoriesDataTable.AcceptChanges();
                }

                return ProductCategoriesDataTable.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        private ProductCategoriesRow Convert(ProductCategoryRowDto dtoRow)
        {
            var tableRow = ProductCategoriesDataTable.NewRow
            (
                dtoRow.ProductCategoryID,
                dtoRow.Name
            );

            return tableRow;
        }

        private ProductSubcategoriesDataTable ProductSubcategoriesDataTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductSubcategories; }
        }

        public async Task<DataView> ReadProductSubcategories(bool addEmptyElement = true)
        {
            var table = ShoppingWrapper.Instance.ProductsDataSet.ProductSubcategories;

            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                if (ProductSubcategoriesDataTable.Count == 0)
                {
                    var listDto = await ProductsServiceClient.GetProductSubcategoriesAsync();

                    if (addEmptyElement)
                    {
                        var tableRow = ProductSubcategoriesDataTable.NewRow(ShoppingWrapper.NoId, string.Empty, ShoppingWrapper.NoId);
                        ProductSubcategoriesDataTable.Rows.Add(tableRow);
                    }

                    foreach (var dtoRow in listDto)
                    {
                        var tableRow = Convert(dtoRow);
                        ProductSubcategoriesDataTable.Rows.Add(tableRow);
                    }

                    ProductSubcategoriesDataTable.AcceptChanges();
                }

                return ProductSubcategoriesDataTable.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        private ProductSubcategoriesRow Convert(ProductSubcategoryRowDto dtoRow)
        {
            var tableRow = ProductSubcategoriesDataTable.NewRow
            (
                dtoRow.ProductSubcategoryID,
                dtoRow.Name,
                dtoRow.ProductCategoryID
            );

            return tableRow;
        }
    }
}
