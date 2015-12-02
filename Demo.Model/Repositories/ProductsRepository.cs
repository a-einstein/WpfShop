using Common.DomainClasses;
using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Threading.Tasks;

namespace Demo.Model
{
    public class ProductsRepository : Repository<ProductsOverviewObject>
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

        // TODO > Better return a Task.Result like in ReadDetails, maybe even the list straight from the service.
        // The List does need to hold items, except for testing currently.

        // TODO This should get paged with an optional pagesize.
        public async Task ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            Clear();

            var task = Task.Run(async () =>
            {
                var productOverview = await ProductsServiceClient.GetProductsOverviewByAsync(
                    category != null ? category.Id : NoId,
                    subcategory != null ? subcategory.Id : NoId,
                    namePart);

                foreach (var product in productOverview)
                {
                    List.Add(product);
                }
            });

            await task;
        }

        public async Task<Product> ReadDetails(int productID)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var product = await ProductsServiceClient.GetProductDetailsAsync(productID);

                return product;
            });

            return await task.Result;
        }
    }
}
