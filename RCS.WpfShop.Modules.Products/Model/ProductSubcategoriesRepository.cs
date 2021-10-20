using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductSubcategoriesRepository :
        Repository<List<ProductSubcategory>, ProductSubcategory>
    {
        #region Construction
        public ProductSubcategoriesRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        protected override async Task<bool> Read(bool addEmptyElement = true)
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

            if (addEmptyElement)
            {
                var subcategory = new ProductSubcategory();
                items.Add(subcategory);
            }

            foreach (var subcategory in subcategories)
            {
                items.Add(subcategory);
            }

            return true;
        }
        #endregion
    }
}
