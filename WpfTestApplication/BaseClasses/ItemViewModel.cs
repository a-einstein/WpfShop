using System.Windows.Input;

namespace WpfTestApplication.BaseClasses
{
    abstract class ItemViewModel<T> : ViewModel
    {
        private int itemId;

        public int ItemId
        {
            get { return itemId; }
            set 
            { 
                itemId = value;
                LoadData();
            }
        }

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

        public ICommand OrderCommand { get; set; }
    }
}