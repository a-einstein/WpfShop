namespace WpfTestApplication.BaseClasses
{
    abstract class ItemViewModel<T> : ViewModel
    {
        private int itemId;

        // TODO Is this needed? 
        // TODO Is it updated correctly and smartly?
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
    }
}