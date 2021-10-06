using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductCategoriesRepository : Repository<ObservableCollection<ProductCategory>, ProductCategory>
    {
        #region Construction
        public ProductCategoriesRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        public override async Task<bool> ReadList(bool addEmptyElement = true)
        {
            Clear();

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
