using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
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

            if (addEmptyElement)
            {
                var category = new ProductCategory() { Name = string.Empty };
                items.Add(category);
            }

            foreach (var category in categories)
            {
                items.Add(category);
            }

            return true;
        }
        #endregion
    }
}
