using Microsoft.Practices.Prism.Commands;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace WpfTestApplication.BaseClasses
{
    abstract class ItemsViewModel : ViewModel
    {
        public ItemsViewModel()
        {
            LoadData();

            DetailsCommand = new DelegateCommand<object>(ShowDetails);
        }

        protected DataTable Items;

        // Note this uses the DataView's standard functionality.
        // Note a CollectionViewSource.View apparently is not able to filter.
        // This could also be implemented using a ObservableCollection and/or IQueryable.
        public DataView ItemsDataView { get { return Items.DefaultView; } }

        public static readonly DependencyProperty ItemsFilterProperty =
            DependencyProperty.Register("ItemsFilter", typeof(string), typeof(ItemsViewModel), new PropertyMetadata(new PropertyChangedCallback(OnItemsFilterChanged)));

        public string ItemsFilter
        {
            get { return (string)GetValue(ItemsFilterProperty); }
            set { SetValue(ItemsFilterProperty, value); }
        }

        // Note this event appears on leaving the control, for instance tabbing out. Pressing the search button indirectly has the same effect.
        // TODO The action should be hooked up to the Enter key. This would demand implementation of a behaviour.
        private static void OnItemsFilterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemsViewModel viewModel = dependencyObject as ItemsViewModel;

            viewModel.SetFilter();
        }
        
        protected virtual void SetFilter()
        {
            Items.CaseSensitive = false;
            ItemsDataView.RowFilter = string.Format("Name LIKE '%{0}%'", ItemsFilter);
        }
        
        public ICommand DetailsCommand { get; private set; }

        protected abstract void ShowDetails(object p);
    }
}
