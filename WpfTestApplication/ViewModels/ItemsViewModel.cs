using System.ComponentModel;
using System.Data;
using System.Windows;

namespace WpfTestApplication.ViewModels
{
    abstract class ItemsViewModel : DependencyObject
    {
        public ItemsViewModel()
        {
            LoadData();
        }

        protected abstract void LoadData();

        protected DataTable Items;

        // Note this uses the DataView's standard functionality.
        // Note a CollectionViewSource.View apparently is not able to filter.
        // TODO this could be implemented using a ObservableCollection and/or IQueryable.
        public DataView ItemsView { get { return Items.DefaultView; } }

        public static readonly DependencyProperty ItemsFilterProperty =
            DependencyProperty.Register("ItemsFilter", typeof(string), typeof(ItemsViewModel), new PropertyMetadata(new PropertyChangedCallback(OnItemsFilterChanged)));

        public string ItemsFilter
        {
            get { return (string)GetValue(ItemsFilterProperty); }
            set { SetValue(ItemsFilterProperty, value); }
        }

        // Note this event appears on leaving the control.
        // TODO The action should be hooked up to the Button.
        // TODO The action should be hooked up to the Enter key.
        private static void OnItemsFilterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemsViewModel viewModel = dependencyObject as ItemsViewModel;

            viewModel.SetFilter();
        }

         protected virtual void SetFilter()
        {
            Items.CaseSensitive = false;
            ItemsView.RowFilter = string.Format("Name LIKE '%{0}%'", ItemsFilter);
        }

        protected static bool NullOrEmpty(string value)
        {
            return (value == null || value.Trim() == string.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This signal can be particularly useful if a collection is entirely replaced, as the formerly bound collection no longer can.
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
