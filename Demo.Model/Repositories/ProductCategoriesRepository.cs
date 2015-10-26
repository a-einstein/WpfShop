using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Data;
using System.Threading.Tasks;
using ProductCategoriesDataTable = Demo.Model.DataSet.ProductsDataSet.ProductCategoriesDataTable;
using ProductCategoriesRow = Demo.Model.DataSet.ProductsDataSet.ProductCategoriesRow;

namespace Demo.Model
{
    public class ProductCategoriesRepository : ProductsServiceConsumer
    {
        private ProductCategoriesRepository()
        { }

        private static volatile ProductCategoriesRepository instance;
        private static object syncRoot = new Object();

        public static ProductCategoriesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductCategoriesRepository();
                    }
                }

                return instance;
            }
        }

        private ProductCategoriesDataTable ListTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductCategories; }
        }

        public void Clear()
        {
            ListTable.Clear();
            ListTable.AcceptChanges();
        }

        // Currently this only made for testpurposes and stores only locally.
        public ProductCategoriesRow CreateListElement(ProductCategoryRowDto rowDto)
        {
            var tableRow = Convert(rowDto);
            ListTable.Rows.Add(tableRow);
            ListTable.AcceptChanges();

            return tableRow;
        }

        public async Task<DataView> ReadList(bool addEmptyElement = true)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                if (ListTable.Count == 0)
                {
                    var listDto = await ProductsServiceClient.GetProductCategoriesAsync();

                    if (addEmptyElement)
                    {
                        var tableRow = ListTable.NewRow(NoId, string.Empty);
                        ListTable.Rows.Add(tableRow);
                    }

                    foreach (var dtoRow in listDto)
                    {
                        var tableRow = Convert(dtoRow);
                        ListTable.Rows.Add(tableRow);
                    }

                    ListTable.AcceptChanges();
                }

                return ListTable.DefaultView;
            });

            // TODO ?!
            return await task.Result;
        }

        private ProductCategoriesRow Convert(ProductCategoryRowDto dtoRow)
        {
            var tableRow = ListTable.NewRow
            (
                dtoRow.ProductCategoryID,
                dtoRow.Name
            );

            return tableRow;
        }
    }
}
