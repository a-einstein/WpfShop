using RCS.AdventureWorks.Common.DomainClasses;
using RCS.AdventureWorks.Common.Dtos;
using System.Threading.Tasks;

namespace RCS.WpfShop.AdventureWorks.Wrappers
{
    public interface IProductsService
    {
        Task<ProductCategoryList> GetProductCategoriesAsync();
        Task<ProductSubcategoryList> GetProductSubcategoriesAsync();

        Task<ProductsOverviewList> GetProductsOverviewByAsync(int? categoryId, int? subcategoryId, string searchString);
        Task<Product> GetProductDetailsAsync(int productId);
    }
}
