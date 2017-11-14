using System.Collections.ObjectModel;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class Repository<T> : ProductsServiceConsumer
    {
        #region CRUD
        public ObservableCollection<T> List = new ObservableCollection<T>();

        public void Clear()
        {
            List.Clear();
        }
        #endregion
    }
}