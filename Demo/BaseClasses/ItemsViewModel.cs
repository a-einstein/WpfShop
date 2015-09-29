using System.Data;
using System.Windows;

namespace Demo.BaseClasses
{
    public abstract class ItemsViewModel : ViewModel
    {
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(DataView), typeof(ItemsViewModel));

        // Note this uses the DataView's standard filtering functionality.
        // Note this signals its own changes by IBindingListView, IBindingList.
        // Note a CollectionViewSource.View apparently is not able to filter.
        // This could also be implemented using a ObservableCollection and/or IQueryable.
        public virtual DataView Items
        {
            get { return (DataView)GetValue(ItemsProperty); }
            set 
            { 
                SetValue(ItemsProperty, value);
                RaisePropertyChanged("ItemsCount");
            }
        }

        // Convenience property to signal changes.
        // Note that just binding on Items.Count does not work.
        public int ItemsCount { get { return Items != null ? Items.Count : 0; } }

        /// <summary>
        /// A value enabling recognition of empty Items.
        /// Note that adding a type to the class as a parameter instead of using object did not work in the comparison.
        /// </summary>
        public abstract object NoId { get; }
    }
}
