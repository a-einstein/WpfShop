using System.Collections.ObjectModel;

namespace Demo.Model
{
    public abstract class Repository<T> : ProductsServiceConsumer
    {
        protected ObservableCollection<T> list = new ObservableCollection<T>();

        public ObservableCollection<T> List { get { return list; } }

        public void Clear()
        {
            List.Clear();
        }
    }
}
