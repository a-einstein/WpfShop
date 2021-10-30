using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Common.Interfaces
{
    public interface IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        /// <summary>
        /// Expose the contents of the repository.
        /// TODO Possibly instead just use Read functions returning an element or collection.
        /// </summary>
        ReadOnlyCollection<TElement> Items { get; }

        // CRUD.
        // TODO Make generally Task<bool>.
        // TODO Add regions, distinguishing between proper CRUD and others.

        /// <summary>
        /// Add element to the repository.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        Task Create(TElement element);

        /// <summary>
        /// Read all current elements from possible underlying storage.
        /// </summary>
        /// <param name="addEmptyElement">Add empty first element if desired.</param>
        /// <returns></returns>
        Task<bool> Refresh(bool addEmptyElement = true);

        /// <summary>
        /// Replace contents of element (identified by some property) by new values.
        /// Currently possibly void for some repositories, as not needed.
        /// If standardly implemented, it would make sense to add a Read function returning an the single element, at least for testing.
        /// TODO Consider this.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        Task Update(TElement element);

        /// <summary>
        /// Remove element from the repository.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        Task Delete(TElement element);
    }
}