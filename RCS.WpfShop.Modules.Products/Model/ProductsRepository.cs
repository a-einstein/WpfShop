using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductsRepository : Repository<ObservableCollection<ProductsOverviewObject>, ProductsOverviewObject>
    {
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

        public async Task<Product> ReadDetails(int productID)
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