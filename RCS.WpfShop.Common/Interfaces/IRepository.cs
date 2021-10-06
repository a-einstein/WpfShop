using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Common.Interfaces
{
    public interface IRepository<TCollection, TElement>
        where TCollection : Collection<TElement>, new()
    {
        ReadOnlyCollection<TElement> Items { get; }

        // CRUD.
        Task Create(TElement element);
        Task Refresh(bool addEmptyElement = true);
        Task Update(TElement element);
        Task Delete(TElement element);

        #region Tmp
        TCollection List { get; }
        Task<bool> ReadList(bool addEmptyElement = true);
        #endregion
    }
}