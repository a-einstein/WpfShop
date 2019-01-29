using System.Threading.Tasks;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.ServiceClients.Products.ProductsService;

namespace RCS.WpfShop.ServiceClients.Products.Mock
{
    class ProductsServiceClient : IProductsService
    {
        public ProductCategoryList GetProductCategories()
        {
            throw new System.NotImplementedException();
        }

        public Task<ProductCategoryList> GetProductCategoriesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Product GetProductDetails(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Product> GetProductDetailsAsync(int productId)
        {
            throw new System.NotImplementedException();
        }

        public ProductsOverviewList GetProductsOverviewBy(int? productCategoryID, int? productSubcategoryID, string productNameString)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProductsOverviewList> GetProductsOverviewByAsync(int? productCategoryID, int? productSubcategoryID, string productNameString)
        {
            throw new System.NotImplementedException();
        }

        public ProductSubcategoryList GetProductSubcategories()
        {
            throw new System.NotImplementedException();
        }

        public Task<ProductSubcategoryList> GetProductSubcategoriesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
