using Moq;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System.Threading.Tasks;

namespace RCS.WpfShop.AdventureWorks.Mock
{
    // The concept and use of this class is based on:
    // http://www.fascinatedwithsoftware.com/blog/post/2011/06/01/How-to-Mock-a-WCF-Service-with-Unity-and-Moq.aspx

    public class ProductsServiceClient : IProductsService
    {
        #region Moq
        private static Mock<IProductsService> mock;

        public static Mock<IProductsService> Mock
        {
            get
            {
                if (mock == null)
                    Initialize();

                return mock;
            }
        }

        public static void Initialize()
        {
            // Note this enables the use of he application for inspection of GUI element with with the same configuration
            // TODO Actually IProductsService could do without using Moq. Make better use of it.
            mock = new Mock<IProductsService>();

            const string categoryNameBase = "Category";

            // 2 categories.
            var categories = new ProductCategoryList() {
                new ProductCategory() { Id=1, Name= $"{categoryNameBase} 1" },
                new ProductCategory() { Id=2, Name= $"{categoryNameBase} 2" }
            };

            mock.Setup(service => service.GetProductCategories())
                .Returns(categories);

            mock.Setup(service => service.GetProductCategoriesAsync().Result).
                Returns(categories);

            const string subcategoryNameBase = "Subcategory";

            // Return 2 subcategories per category.
            mock.Setup(service => service.GetProductSubcategories())
                .Returns(new ProductSubcategoryList() {
                    new ProductSubcategory() { Id=1, ProductCategoryId=1, Name=$"{subcategoryNameBase} 1.1" },
                    new ProductSubcategory() { Id=2, ProductCategoryId=1, Name=$"{subcategoryNameBase} 1.2" },
                    new ProductSubcategory() { Id=3, ProductCategoryId=2, Name=$"{subcategoryNameBase} 2.1" },
                    new ProductSubcategory() { Id=4, ProductCategoryId=2, Name=$"{subcategoryNameBase} 2.2" }
                });

            const int categoryIdExpected = 2;
            const int subcategoryIdExpected = 3;
            var searchStringExpected = $"{categoryIdExpected}.{subcategoryIdExpected}";
            var colorExpectedBase = $"Color {searchStringExpected}";

            // Return 2 colours for expected criteria 2, 3, "2.3".
            // Note the string is not valid in the control.
            // TODO Make this more flexible.
            mock.Setup(service => service.GetProductsOverviewBy(categoryIdExpected, subcategoryIdExpected, searchStringExpected))
                .Returns(new ProductsOverviewList() {
                    new ProductsOverviewObject() { Color = $"{colorExpectedBase}.1" },
                    new ProductsOverviewObject() { Color = $"{colorExpectedBase}.2" }
                });
        }
        #endregion

        #region IProductsService
        // Note breaks do not work here.

        /// <summary>
        /// </summary>
        /// <returns>2 categories.</returns>
        public ProductCategoryList GetProductCategories()
            => Mock.Object.GetProductCategories();

        /// <summary>
        /// </summary>
        /// <returns>2 categories.</returns>
        public async Task<ProductCategoryList> GetProductCategoriesAsync()
            => await Mock.Object.GetProductCategoriesAsync();

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product GetProductDetails(int productId)
        {
            var result = Mock.Object.GetProductDetails(productId);
            return result;
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<Product> GetProductDetailsAsync(int productId)
        {
            return Task.FromResult(GetProductDetails(productId));
        }

        /// <summary>
        /// Expects 2, 3, "2.3".
        /// </summary>
        /// <param name="productCategoryID"></param>
        /// <param name="productSubcategoryID"></param>
        /// <param name="productNameString"></param>
        /// <returns>2 products with different colours.</returns>
        public ProductsOverviewList GetProductsOverviewBy(int? productCategoryID, int? productSubcategoryID, string productNameString)
        {
            var result = Mock.Object.GetProductsOverviewBy(productCategoryID, productSubcategoryID, productNameString);
            return result;
        }

        /// <summary>
        /// Expects 2, 3, "2.3".
        /// </summary>
        /// <param name="productCategoryID"></param>
        /// <param name="productSubcategoryID"></param>
        /// <param name="productNameString"></param>
        /// <returns>2 products with different colours.</returns>
        public Task<ProductsOverviewList> GetProductsOverviewByAsync(int? productCategoryID, int? productSubcategoryID, string productNameString)
        {
            return Task.FromResult(GetProductsOverviewBy(productCategoryID, productSubcategoryID, productNameString));
        }

        /// <summary>
        /// </summary>
        /// <returns>4 subcategories.</returns>
        public ProductSubcategoryList GetProductSubcategories()
        {
            var result = Mock.Object.GetProductSubcategories();
            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns>4 subcategories.</returns>
        public Task<ProductSubcategoryList> GetProductSubcategoriesAsync()
        {
            return Task.FromResult(GetProductSubcategories());
        }
        #endregion
    }
}
