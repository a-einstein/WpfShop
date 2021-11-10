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
                new() { Id=1, Name= $"{categoryNameBase} 1" },
                new() { Id=2, Name= $"{categoryNameBase} 2" }
            };

            mock.Setup(service => service.GetProductCategories())
                .Returns(categories);

            mock.Setup(service => service.GetProductCategoriesAsync().Result).
                Returns(categories);

            const string subcategoryNameBase = "Subcategory";

            // 2 subcategories per category.
            var subcategories = new ProductSubcategoryList() {
                new() { Id=1, ProductCategoryId=1, Name=$"{subcategoryNameBase} 1.1" },
                new() { Id=2, ProductCategoryId=1, Name=$"{subcategoryNameBase} 1.2" },
                new() { Id=3, ProductCategoryId=2, Name=$"{subcategoryNameBase} 2.1" },
                new() { Id=4, ProductCategoryId=2, Name=$"{subcategoryNameBase} 2.2" }
            };

            mock.Setup(service => service.GetProductSubcategories())
                .Returns(subcategories);

            mock.Setup(service => service.GetProductSubcategoriesAsync().Result)
                .Returns(subcategories);

            const int categoryIdExpected = 2;
            const int subcategoryIdExpected = 3;

            // Note the search string is not valid in the control.
            // TODO Make this more flexible.
            var searchStringExpected = $"{categoryIdExpected}.{subcategoryIdExpected}";
            var colorExpectedBase = $"Color {searchStringExpected}";

            // 2 colours for expected criteria 2, 3, "2.3".
            // The search string should be a substring of the color names.
            // Though not needed the category Ids are filled in too.
            var products = new ProductsOverviewList() {
                new() { ProductCategoryId = categoryIdExpected, ProductSubcategoryId = subcategoryIdExpected, Color = $"{colorExpectedBase}.1" },
                new() { ProductCategoryId = categoryIdExpected, ProductSubcategoryId = subcategoryIdExpected, Color = $"{colorExpectedBase}.2" }
            };

            // Note the parameter values have to fit.

            mock.Setup(service => service.GetProductsOverviewBy(categoryIdExpected, subcategoryIdExpected, searchStringExpected))
                .Returns(products);

            mock.Setup(service => service.GetProductsOverviewByAsync(categoryIdExpected, subcategoryIdExpected, searchStringExpected).Result)
                .Returns(products);
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
            => Mock.Object.GetProductsOverviewBy(productCategoryID, productSubcategoryID, productNameString);

        /// <summary>
        /// Expects 2, 3, "2.3".
        /// </summary>
        /// <param name="productCategoryID"></param>
        /// <param name="productSubcategoryID"></param>
        /// <param name="productNameString"></param>
        /// <returns>2 products with different colours.</returns>
        public async Task<ProductsOverviewList> GetProductsOverviewByAsync(int? productCategoryID, int? productSubcategoryID, string productNameString)
            => await Mock.Object.GetProductsOverviewByAsync(productCategoryID, productSubcategoryID, productNameString);

        /// <summary>
        /// </summary>
        /// <returns>4 subcategories.</returns>
        public ProductSubcategoryList GetProductSubcategories()
            => Mock.Object.GetProductSubcategories();

        /// <summary>
        /// </summary>
        /// <returns>4 subcategories.</returns>
        public async Task<ProductSubcategoryList> GetProductSubcategoriesAsync()
             => await Mock.Object.GetProductSubcategoriesAsync();
        #endregion
    }
}
