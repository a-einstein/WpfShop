using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductCategoriesRepository : Repository<ProductCategory>
    {
        #region Construction
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
        #endregion

        #region CRUD
        public async Task<bool> ReadList(bool addEmptyElement = true)
        {
            Clear();

            var categories = new ProductCategoryList();

            try
            {
                categories = await ProductsServiceClient.GetProductCategoriesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            if (addEmptyElement)
            {
                var category = new ProductCategory() { Name = string.Empty };
                List.Add(category);
            }

            foreach (var category in categories)
            {
                List.Add(category);
            }

            return true;
        }
        #endregion
    }
}
