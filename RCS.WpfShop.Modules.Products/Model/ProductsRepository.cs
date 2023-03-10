using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.WpfShop.AdventureWorks.Wcf;
using RCS.WpfShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductsRepository :
        Repository<List<ProductsOverviewObject>, ProductsOverviewObject>,
        IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int>
    {
        #region Construction
        // Need a parameterless constructor for tests.
        public ProductsRepository()
        { }

        public ProductsRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region Refresh
        public async Task Refresh(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            await Clear().ConfigureAwait(true);
            await Read(category, subcategory, namePart).ConfigureAwait(true);
        }
        #endregion

        #region CRUD
        // TODO This should get paged with an optional pagesize.
        private async Task Read(ProductCategory category, ProductSubcategory subcategory, string namePart)
        {
            ProductsOverviewList productsOverview;

            try
            {
                productsOverview = await ProductsServiceClient.GetProductsOverviewByAsync(
                    category?.Id,
                    subcategory?.Id,
                    namePart);
            }
            catch (Exception exception)
            {
                DisplayAlert(exception);
                return;
            }

            foreach (var product in productsOverview)
            {
                items.Add(product);
            }
        }

        public async Task<Product> Details(int productID)
        {
            Product product = null;

            try
            {
                product = await ProductsServiceClient.GetProductDetailsAsync(productID);
            }
            catch (Exception exception)
            {
                DisplayAlert(exception);
            }

            return product;
        }
        #endregion
    }
}