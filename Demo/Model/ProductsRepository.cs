using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Data;
using System.Threading.Tasks;
using ProductDetailsDataTable = Demo.Model.ProductsDataSet.ProductDetailsDataTable;
using ProductDetailsRow = Demo.Model.ProductsDataSet.ProductDetailsRow;
using ProductsOverviewDataTable = Demo.Model.ProductsDataSet.ProductsOverviewDataTable;
using ProductsOverviewRow = Demo.Model.ProductsDataSet.ProductsOverviewRow;

namespace Demo.Model
{
    // TODO At lot of elements of this and similar classes could be made generic.
    public class ProductsRepository : ProductsServiceConsumer
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

        private ProductsOverviewDataTable ListTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductsOverview; }
        }

        public void Clear()
        {
            ListTable.Clear();
            ListTable.AcceptChanges();
        }

        // Currently this only made for testpurposes and stores only locally.
        public ProductsOverviewRow CreateListElement(ProductsOverviewRowDto rowDto)
        {
            var tableRow = Convert(rowDto);
            ListTable.Rows.Add(tableRow);
            ListTable.AcceptChanges();

            return tableRow;
        }

        public async Task<DataView> ReadList()
        {
            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                // This also enables testing without a connection.
                if (ListTable.Count == 0)
                {
                    // Note this currently takes in all of the table data. Of course this should be prefiltered and/or paged in a realistic situation. 
                    var productOverviewDto = await ProductsServiceClient.GetProductsOverviewAsync();

                    foreach (var dtoRow in productOverviewDto)
                    {
                        var tableRow = Convert(dtoRow);
                        ListTable.Rows.Add(tableRow);
                    }
                }

                ListTable.AcceptChanges();

                return ListTable.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        private ProductsOverviewRow Convert(ProductsOverviewRowDto dtoRow)
        {
            var tableRow = ListTable.NewRow
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

        private ProductDetailsDataTable DetailsTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductDetails; }
        }

        private ProductDetailsRow Convert(ProductDetailsRowDto productDto)
        {
            var tableRow = DetailsTable.NewRow
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
    }
}
