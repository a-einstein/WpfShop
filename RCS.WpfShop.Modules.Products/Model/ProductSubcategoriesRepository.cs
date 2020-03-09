using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class ProductSubcategoriesRepository : Repository<ObservableCollection<ProductSubcategory>, ProductSubcategory>
    {
        #region Construction
        private ProductSubcategoriesRepository()
        { }

        private static volatile ProductSubcategoriesRepository instance;
        private static readonly object syncRoot = new object();

        public static ProductSubcategoriesRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductSubcategoriesRepository();
                    }
                }

                return instance;
            }
        }
        #endregion

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
