using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
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
        public ProductsRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        // TODO This should get paged with an optional pagesize.
        public async Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart)
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
                return null;
            }

            return productsOverview;
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

        #region Tmp
        public Task Refresh(ProductCategory category, ProductSubcategory subcategory, string searchString)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}