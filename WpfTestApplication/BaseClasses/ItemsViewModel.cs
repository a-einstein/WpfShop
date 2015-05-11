using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Data;

namespace WpfTestApplication.BaseClasses
{
    public abstract class ItemsViewModel : ViewModel
    {
        protected CollectionViewSource itemsCollectionViewSource = new CollectionViewSource();

        // TODO This should become parameterized (like ItemViewModel), currently it assumes retrieval of entire table.
        public override void Refresh()
        {
            Items = GetData();
        }

        /*
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(DataView), typeof(ItemsViewModel));
         * */

        // Note this uses the DataView's standard filtering functionality.
        // Note this signals its own changes by IBindingListView, IBindingList.
        // Note a CollectionViewSource.View apparently is not able to filter.
        // This could also be implemented using a ObservableCollection and/or IQueryable.
        // TODO Change the type to some interface? Check references (like Items.Count and filtering).
        /*
        public DataView Items
        {
            get { return (DataView)GetValue(ItemsProperty); }
            set {
                SetValue(ItemsProperty, null);
                RaisePropertyChanged("Items");

                SetValue(ItemsProperty, value); 
                RaisePropertyChanged("Items");

            }
        }
         * */

        private DataView items;

        public DataView Items
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged("Items");
            }
        }

        public static readonly DependencyProperty ItemsViewProperty =
            DependencyProperty.Register("ItemsDataView", typeof(ICollectionView), typeof(ItemsViewModel));

        public ICollectionView ItemsDataView
        {
            get { return (ICollectionView)GetValue(ItemsViewProperty); }
            set { SetValue(ItemsViewProperty, value); }
        }
        /*
        { get; set; }
         * */

        // Convenience property to signal changes.
        // Note that just binding on Items.Count does not work.
        public int ItemsCount
        {
            get
            {
                //return Items != null ? Items.Count : 0;

                // TODO Get filtered count.
                return ItemsDataView != null ? (itemsCollectionViewSource.Source as ObservableCollection<object>).Count : 0;
            }
        }

        public abstract DataView GetData();
    }
}
