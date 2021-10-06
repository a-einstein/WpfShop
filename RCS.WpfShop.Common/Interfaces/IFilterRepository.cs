using RCS.AdventureWorks.Common.DomainClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Common.Interfaces
{
    public interface IFilterRepository<TCollection, TElement, TCategory, TSubcategory, TId> :
        IRepository<TCollection, TElement>
        where TCollection : Collection<TElement>, new()
    {
        Task Refresh(TCategory category, TSubcategory subcategory, string searchString);
        Task<Product> Details(TId elementId);

        #region Tmp
        Task<IList<ProductsOverviewObject>> ReadList(ProductCategory category, ProductSubcategory subcategory, string namePart);
        #endregion
    }
}
