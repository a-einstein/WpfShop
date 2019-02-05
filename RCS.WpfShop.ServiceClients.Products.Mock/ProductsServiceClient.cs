using Moq;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System.Threading.Tasks;

namespace RCS.WpfShop.ServiceClients.Products.Mock
{
    // The concept and use of this class is based on:
    // http://www.fascinatedwithsoftware.com/blog/post/2011/06/01/How-to-Mock-a-WCF-Service-with-Unity-and-Moq.aspx

    public class ProductsServiceClient : IProductsService
    {
        #region Moq
        private static Mock<IProductsService> mock;

        static public Mock<IProductsService> Mock
        {
            get
            {
                if (mock == null)
                    Initialize();

                return mock;
            }
        }

        static public void Initialize()
        {
            // TODO Actually IProductsService could do without using Moq. Make better use of it.

            mock = new Mock<IProductsService>();

            const string categoryNameBase = "Category";

            mock.Setup(service => service.GetProductCategories())
                .Returns(new ProductCategoryList() {
                    new ProductCategory() { Id=1, Name= $"{categoryNameBase} 1" },
                    new ProductCategory() { Id=2, Name= $"{categoryNameBase} 2" }
                });

            const string subcategoryNameBase = "Subcategory";

            mock.Setup(service => service.GetProductSubcategories())
                .Returns(new ProductSubcategoryList() {
                    new ProductSubcategory() { Id=1, ProductCategoryId=1, Name=$"{subcategoryNameBase} 1.1" },
                    new ProductSubcategory() { Id=2, ProductCategoryId=1, Name=$"{subcategoryNameBase} 1.2" },
                    new ProductSubcategory() { Id=3, ProductCategoryId=2, Name=$"{subcategoryNameBase} 2.1" },
                    new ProductSubcategory() { Id=4, ProductCategoryId=2, Name=$"{subcategoryNameBase} 2.2" }
                });

            const int categoryIdExpected = 2;
            const int subcategoryIdExpected = 3;
            string searchStringExpected = $"{categoryIdExpected}.{subcategoryIdExpected}";
            string colorExpectedBase = $"Color {searchStringExpected}";

            mock.Setup(service => service.GetProductsOverviewBy(categoryIdExpected, subcategoryIdExpected, searchStringExpected))
                .Returns(new ProductsOverviewList() {
                    new ProductsOverviewObject() { Color = $"{colorExpectedBase}.1" },
                    new ProductsOverviewObject() { Color = $"{colorExpectedBase}.2" }
                });
        }
        #endregion

        #region IProductsService
        public ProductCategoryList GetProductCategories()
        {
            return Mock.Object.GetProductCategories();
        }

        public Task<ProductCategoryList> GetProductCategoriesAsync()
        {
            return Task.FromResult(GetProductCategories());
        }

        public Product GetProductDetails(int productId)
        {
            return Mock.Object.GetProductDetails(productId);
        }

        public Task<Product> GetProductDetailsAsync(int productId)
        {
            return Task.FromResult(GetProductDetails(productId));
        }

        public ProductsOverviewList GetProductsOverviewBy(int? productCategoryID, int? productSubcategoryID, string productNameString)
        {
            return Mock.Object.GetProductsOverviewBy(productCategoryID, productSubcategoryID, productNameString);
        }

        public Task<ProductsOverviewList> GetProductsOverviewByAsync(int? productCategoryID, int? productSubcategoryID, string productNameString)
        {
            return Task.FromResult(GetProductsOverviewBy(productCategoryID, productSubcategoryID, productNameString));
        }

        public ProductSubcategoryList GetProductSubcategories()
        {
            return Mock.Object.GetProductSubcategories();
        }

        public Task<ProductSubcategoryList> GetProductSubcategoriesAsync()
        {
            return Task.FromResult(GetProductSubcategories());
        }
        #endregion
    }
}
