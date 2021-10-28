﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RCS.WpfShop.Common.Interfaces
{
    public interface IRepository<TCollection, TElement>
        where TCollection : List<TElement>, new()
    {
        ReadOnlyCollection<TElement> Items { get; }

        // CRUD.
        // TODO Make generally Task<bool>.
        Task Create(TElement element);
        // TODO Why not Read?
        Task<bool> Refresh(bool addEmptyElement = true);
        Task Update(TElement element);
        Task Delete(TElement element);
    }
}