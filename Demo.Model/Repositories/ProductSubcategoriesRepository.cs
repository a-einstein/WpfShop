using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Data;
using System.Threading.Tasks;
using ProductSubcategoriesDataTable = Demo.Model.ProductsDataSet.ProductSubcategoriesDataTable;
using ProductSubcategoriesRow = Demo.Model.ProductsDataSet.ProductSubcategoriesRow;

namespace Demo.Model
{
    public class ProductSubcategoriesRepository : ProductsServiceConsumer
    {
        private ProductSubcategoriesRepository()
        { }

        private static volatile ProductSubcategoriesRepository instance;
        private static object syncRoot = new Object();

        public static ProductSubcategoriesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductSubcategoriesRepository();
                    }
                }

                return instance;
            }
        }

        private ProductSubcategoriesDataTable ListTable
        {
            get { return ShoppingWrapper.Instance.ProductsDataSet.ProductSubcategories; }
        }

        public void Clear()
        {
            ListTable.Clear();
            ListTable.AcceptChanges();
        }

        // Currently this only made for testpurposes and stores only locally.
        public ProductSubcategoriesRow CreateListElement(ProductSubcategoryRowDto rowDto)
        {
            var tableRow = Convert(rowDto);
            ListTable.Rows.Add(tableRow);
            ListTable.AcceptChanges();

            return tableRow;
        }

        public async Task<DataView> ReadList(bool addEmptyElement = true)
        {
            var table = ShoppingWrapper.Instance.ProductsDataSet.ProductSubcategories;

            var task = Task.Factory.StartNew(async () =>
            {
                // Note that currently data is cached and read only once.
                if (ListTable.Count == 0)
                {
                    var listDto = await ProductsServiceClient.GetProductSubcategoriesAsync();

                    if (addEmptyElement)
                    {
                        var tableRow = ListTable.NewRow(ShoppingWrapper.NoId, string.Empty, ShoppingWrapper.NoId);
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

        private ProductSubcategoriesRow Convert(ProductSubcategoryRowDto dtoRow)
        {
            var tableRow = ListTable.NewRow
            (
                dtoRow.ProductSubcategoryID,
                dtoRow.Name,
                dtoRow.ProductCategoryID
            );

            return tableRow;
        }
    }
}
