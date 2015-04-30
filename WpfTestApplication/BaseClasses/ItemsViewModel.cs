using System.Data;
using System.Windows;

namespace WpfTestApplication.BaseClasses
{
    abstract class ItemsViewModel : ViewModel
    {
        public ItemsViewModel()
        {
            LoadData();
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(DataView), typeof(ItemsViewModel));

        // Note this uses the DataView's standard filtering functionality.
        // Note a CollectionViewSource.View apparently is not able to filter.
        // This could also be implemented using a ObservableCollection and/or IQueryable.
        public DataView Items
        {
            get { return (DataView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Convenience property to signal changes.
        public int ItemsCount { get { return Items.Count; } }
    }
}
