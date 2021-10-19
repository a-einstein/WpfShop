using RCS.AdventureWorks.Common.DomainClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Common.Interfaces
{
    public interface IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        ReadOnlyCollection<TElement> Items { get; }

        // CRUD.
        Task Create(TElement element);
        Task Refresh(bool addEmptyElement = true);
        Task Update(TElement element);
        Task Delete(TElement element);

        #region Tmp
        // HACK for CartItemsRepository.
        // TODO Transform to filosophy of PortableShop. 

        TCollection List { get; }
        Task<bool> ReadList(bool addEmptyElement = true);
        CartItem AddProduct(IShoppingProduct product);
        void DeleteProduct(CartItem cartItem);
        int ProductsCount();
        public decimal CartValue();
        #endregion
    }
}