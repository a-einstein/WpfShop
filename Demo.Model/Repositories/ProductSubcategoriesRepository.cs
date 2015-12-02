using Common.DomainClasses;
using Demo.ServiceClients.Products.ServiceReference;
using System;
using System.Threading.Tasks;

namespace Demo.Model
{
    public class ProductSubcategoriesRepository : Repository<ProductSubcategory>
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

        public async Task ReadList(bool addEmptyElement = true)
        {
            Clear();

            var task = Task.Run(async () =>
            {
                var subcategories = await ProductsServiceClient.GetProductSubcategoriesAsync();

                if (addEmptyElement)
                {
                    var subcategory = new ProductSubcategory() { ProductCategoryID = NoId, Id=NoId, Name = string.Empty };
                    List.Add(subcategory);
                }

                foreach (var subcategory in subcategories)
                {
                    List.Add(subcategory);
                }
            });

            await task;
        }
    }
}
