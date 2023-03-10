using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.WpfShop.AdventureWorks.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductSubcategoriesRepository :
        Repository<List<ProductSubcategory>, ProductSubcategory>
    {
        #region Construction
        // Need a parameterless constructor for tests.
        public ProductSubcategoriesRepository()
        { }

        public ProductSubcategoriesRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        protected override async Task<bool> Read(bool addEmptyElement = true)
        {
            if (await base.Read(addEmptyElement))
            {
                ProductSubcategoryList subcategories;

                try
                {
                    subcategories = await ProductsServiceClient.GetProductSubcategoriesAsync();
                }
                catch (Exception exception)
                {
                    DisplayAlert(exception);
                    return false;
                }

                foreach (var subcategory in subcategories)
                {
                    items.Add(subcategory);
                }
            }

            return true;
        }
        #endregion
    }
}
