using System;
using System.ComponentModel;
using System.Data;
using System.Windows;

namespace WpfTestApplication.BaseClasses
{
    public abstract class ItemsViewModel : ViewModel
    {
        protected void GetItemsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception(databaseErrorMessage, e.Error);
            else
                Items = (e.Result as DataTable).DefaultView;
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(DataView), typeof(ItemsViewModel));

        // Note this uses the DataView's standard filtering functionality.
        // Note this signals its own changes by IBindingListView, IBindingList.
        // Note a CollectionViewSource.View apparently is not able to filter.
        // This could also be implemented using a ObservableCollection and/or IQueryable.
        // TODO Change the type to some interface? Check references (like Items.Count and filtering).
        public DataView Items
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
    }
}
