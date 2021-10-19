using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using RCS.WpfShop.Common.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class Repository<TCollection, TElement> :
        ProductsServiceConsumer,
        IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        #region Construction
        public Repository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region Refresh
        public TCollection List { get; } = new TCollection();

        protected readonly TCollection items = new TCollection();

        // Note this is directly accesible but not amendable.
        public ReadOnlyCollection<TElement> Items => items.AsReadOnly();

        public void Clear()
        {
            List.Clear();
        }
        #endregion

        #region CRUD
        public Task Create(TElement element)
        {
            throw new System.NotImplementedException();
        }

        public Task Refresh(bool addEmptyElement = true)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(TElement element)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(TElement element)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Tmp
        // HACK for CartItemsRepository.
        // TODO Transform to filosophy of PortableShop. 

        public virtual Task<bool> ReadList(bool addEmptyElement = true)
        {
            throw new System.NotImplementedException();
        }

        public virtual CartItem AddProduct(IShoppingProduct product)
        {
            throw new System.NotImplementedException();
        }

        public virtual void DeleteProduct(CartItem cartItem)
        {
            throw new System.NotImplementedException();
        }

        public virtual int ProductsCount()
        {
            throw new System.NotImplementedException();
        }

        public virtual decimal CartValue()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}