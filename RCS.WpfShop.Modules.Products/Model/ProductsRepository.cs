using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductsRepository : Repository<ProductsOverviewObject>
    {
        #region Construction
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
        #endregion

        #region CRUD
        // TODO This should get paged with an optional pagesize.
        public async Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            var productsOverview = new ProductsOverviewList();

            try
            {
                productsOverview = await ProductsServiceClient.GetProductsOverviewByAsync(
                    category?.Id,
                    subcategory?.Id,
                    namePart);
            }
            catch (Exception)
            {
                return null;
            }

            return productsOverview;
        }

        public async Task<Product> ReadDetails(int productID)
        {
            Product product = null;

            try
            {
                product = await ProductsServiceClient.GetProductDetailsAsync(productID);
            }
            catch (Exception)
            {
            }

            return product;
        }
        #endregion
    }
}