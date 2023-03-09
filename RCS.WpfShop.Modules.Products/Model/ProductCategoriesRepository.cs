using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.Wcf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductCategoriesRepository :
        Repository<List<ProductCategory>, ProductCategory>
    {
        #region Construction
        // Need a parameterless constructor for tests.
        public ProductCategoriesRepository()
        { }

        public ProductCategoriesRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        protected override async Task<bool> Read(bool addEmptyElement = true)
        {
            if (await base.Read(addEmptyElement))
            {
                ProductCategoryList categories;

                try
                {
                    categories = await ProductsServiceClient.GetProductCategoriesAsync();
                }
                catch (Exception exception)
                {
                    DisplayAlert(exception);
                    return false;
                }

                foreach (var category in categories)
                {
                    items.Add(category);
                }
            }

            return true;
        }
        #endregion
    }
}
