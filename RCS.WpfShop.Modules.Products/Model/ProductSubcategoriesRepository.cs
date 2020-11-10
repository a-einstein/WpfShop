using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductSubcategoriesRepository : Repository<ObservableCollection<ProductSubcategory>, ProductSubcategory>
    {
        #region CRUD
        public async Task<bool> ReadList(bool addEmptyElement = true)
        {
            Clear();

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
                List.Add(subcategory);
            }

            foreach (var subcategory in subcategories)
            {
                List.Add(subcategory);
            }

            return true;
        }
        #endregion
    }
}
