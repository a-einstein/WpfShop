﻿using RCS.WpfShop.AdventureWorks.Wrappers;
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
        where TElement : new()
    {
        #region Construction

        protected Repository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region Refresh
        protected readonly TCollection items = new();

        // Note this is directly accesible but not amendable.
        public ReadOnlyCollection<TElement> Items => items.AsReadOnly();

        // Async for future use, though currently not.
        public async Task Clear()
        {
            await Task.Run(() =>
            {
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
        public async Task Create(TElement element)
        {
            await Task.Run(() =>
            {
                items.Add(element);
            });
        }

        protected virtual async Task<bool> Read(bool addEmptyElement = true)
        {
            await Task.Run(() =>
            {
                if (addEmptyElement)
                {
                    var element = new TElement();
                    items.Add(element);
                }
            });

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

        #region Utility
        private static Task VoidTask()
        {
            // HACK.
            return Task.Run(() => { });
        }
        #endregion
    }
}