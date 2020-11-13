using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System.Collections.ObjectModel;

namespace RCS.WpfShop.Modules.Products.Model
{
    // TODO This class could be shared with other shopping clients.
    // TODO Check for related classes.
    public abstract class Repository<TCollection, TElement> :
         ProductsServiceConsumer
         where TCollection : Collection<TElement>, new()
    {
        #region Construction
        public Repository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        public TCollection List { get; } = new TCollection();

        public void Clear()
        {
            List.Clear();
        }
        #endregion
    }
}