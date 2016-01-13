using System.Collections.ObjectModel;

namespace Demo.Modules.Products.Model
{
    public abstract class Repository<T> : ProductsServiceConsumer
    {
        public Collection<T> List = new Collection<T>();

        public void Clear()
        {
            List.Clear();
        }
    }
}
