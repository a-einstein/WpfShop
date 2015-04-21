
namespace WpfTestApplication.BaseClasses
{
    abstract class ItemViewModel<T> : ViewModel
    {
        private T item;

        public T Item
        {
            get { return item; }
            set
            {
                item = value;
                RaisePropertyChanged("Item");
            }
        }
    }
}