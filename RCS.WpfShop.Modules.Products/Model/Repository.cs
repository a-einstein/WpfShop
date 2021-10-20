using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using RCS.WpfShop.Common.Interfaces;
using System;
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

        // Async for future use, though currently not.
        public async Task Clear()
        {
            await Task.Run(() =>
            {
                List.Clear();
                items.Clear();
            }).ConfigureAwait(true);
        }

        public async Task<bool> Refresh(bool addEmptyElement = true)
        {
            try
            {
                await Clear().ConfigureAwait(true);
                await Read(addEmptyElement).ConfigureAwait(true);

                return true;
            }
            catch (Exception exception)
            {
                DisplayAlert(exception);
                return false;
            }
        }
        #endregion

        #region CRUD
        public virtual async Task Create(TElement element)
        {
            await Task.Run(() =>
            {
                items.Add(element);
            });
        }

        protected virtual async Task<bool> Read(bool addEmptyElement = true)
        {
            await VoidTask();
            return true;
        }

        public virtual async Task Update(TElement element)
        {
            await VoidTask();
        }

        public virtual async Task Delete(TElement element)
        {
            await Task.Run(() =>
            {
                items.Remove(element);
            });
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

        #region Utility
        private static Task VoidTask()
        {
            // HACK.
            return Task.Run(() => { });
        }
        #endregion
    }
}