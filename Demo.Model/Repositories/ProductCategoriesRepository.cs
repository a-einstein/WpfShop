using Common.DomainClasses;
using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Threading.Tasks;

namespace Demo.Model
{
    public class ProductCategoriesRepository : Repository<ProductCategory>
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

        public async Task ReadList(bool addEmptyElement = true)
        {
            Clear();

            var task = Task.Run(async () =>
            {
                var categories = await ProductsServiceClient.GetProductCategoriesAsync();

                if (addEmptyElement)
                {
                    var category = new ProductCategory() { Id = NoId, Name = string.Empty };
                    list.Add(category);
                }

                foreach (var category in categories)
                {
                    list.Add(category);
                }
            });

            await task;
        }
    }
}
