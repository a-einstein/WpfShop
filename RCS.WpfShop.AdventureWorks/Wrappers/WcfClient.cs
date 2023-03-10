using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using RCS.WpfShop.AdventureWorks.Wcf;
using System.Threading.Tasks;

namespace RCS.WpfShop.AdventureWorks.Wrappers
{
    public class WcfClient : IProductsService
    {
        #region Construction
        // Intended to enable injection.
        // TODO Maybe the constructor with Binding could be applied instead with more options.
        public WcfClient(EndpointAndAddressConfiguration configuration)
        {
            ProductsServiceClient = new ProductsServiceClient(configuration.EndpointConfiguration, configuration.RemoteAddress);
        }

        // TODO Check if this needs to become more sophisticated.
        private ProductsServiceClient ProductsServiceClient { get; set; }
        #endregion

        #region Interface
        public Task<ProductCategoryList> GetProductCategoriesAsync()
        {
            return ProductsServiceClient.GetProductCategoriesAsync();
        }

        public Task<ProductSubcategoryList> GetProductSubcategoriesAsync()
        {
            return ProductsServiceClient.GetProductSubcategoriesAsync();
        }

        public Task<ProductsOverviewList> GetProductsOverviewByAsync(int? categoryId, int? subcategoryId, string searchString)
        {
            return ProductsServiceClient.GetProductsOverviewByAsync(categoryId, subcategoryId, searchString);
        }

        public Task<Product> GetProductDetailsAsync(int productId)
        {
            return ProductsServiceClient.GetProductDetailsAsync(productId);
        }
        #endregion
    }
}