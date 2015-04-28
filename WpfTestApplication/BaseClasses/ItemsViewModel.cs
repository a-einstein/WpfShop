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
            SetCommands();
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(DataTable), typeof(ItemsViewModel));

        // Note this uses the DataView's standard filtering functionality.
        // Note a CollectionViewSource.View apparently is not able to filter.
        // This could also be implemented using a ObservableCollection and/or IQueryable.
        public DataTable Items
        {
            get { return (DataTable )GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public virtual string MasterFilterLabel { get { return "Category"; } }
        public virtual string MasterFilterDisplayMemberPath { get { return "Name"; } }
        public abstract string MasterFilterSelectedValuePath { get; }

        public static readonly DependencyProperty MasterFilterItemsProperty =
            DependencyProperty.Register("MasterFilterItems", typeof(DataTable), typeof(ItemsViewModel));

        public DataTable MasterFilterItems
        {
            get { return (DataTable)GetValue(MasterFilterItemsProperty); }
            set { SetValue(MasterFilterItemsProperty, value); }
        }

        public static readonly DependencyProperty MasterFilterValueProperty =
            DependencyProperty.Register("MasterFilterValue", typeof(object), typeof(ItemsViewModel), new PropertyMetadata(new PropertyChangedCallback(OnMasterFilterValueChanged)));

        public object MasterFilterValue
        {
            get { return (object)GetValue(MasterFilterValueProperty); }
            set { SetValue(MasterFilterValueProperty, value); }
        }

        private static void OnMasterFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemsViewModel viewModel = dependencyObject as ItemsViewModel;
            viewModel.SetDetailFilterItems();
        }

        private void SetDetailFilterItems()
        {
            // Preserve empty value.
            string filter = string.Format("({0} = {1})", DetailFilterMasterKeyPath, -1);
            filter += (int)MasterFilterValue != -1 ? string.Format(" OR ({0} = {1})", DetailFilterMasterKeyPath, MasterFilterValue ) : null;
            
            DetailFilterItems.DefaultView.RowFilter = filter;
        }

        public virtual string DetailFilterLabel { get { return "Subcategory"; } }
        public virtual string DetailFilterDisplayMemberPath { get { return "Name"; } }
        public abstract string DetailFilterSelectedValuePath { get; }
        public abstract string DetailFilterMasterKeyPath { get; }
   
        public static readonly DependencyProperty DetailFilterItemsProperty =
            DependencyProperty.Register("DetailFilterItems", typeof(DataTable), typeof(ItemsViewModel));
     
        public DataTable DetailFilterItems
        {
            get { return (DataTable)GetValue(DetailFilterItemsProperty); }
            set { SetValue(DetailFilterItemsProperty, value); }
        }

        public object DetailFilterValue
        {
            get { return (object)GetValue(DetailFilterValueProperty); }
            set { SetValue(DetailFilterValueProperty, value); }
        }

        public static readonly DependencyProperty DetailFilterValueProperty =
            DependencyProperty.Register("DetailFilterValue", typeof(object), typeof(ItemsViewModel), new PropertyMetadata(OnDetailFilterValueChanged));

        private static void OnDetailFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemsViewModel viewModel = dependencyObject as ItemsViewModel;
        }

        public virtual string TextFilterLabel { get { return "Name"; } }
        public virtual string TextFilterValuePath { get { return "Name"; } }

        public static readonly DependencyProperty TextFilterValueProperty =
            DependencyProperty.Register("TextFilterValue", typeof(string), typeof(ItemsViewModel));

        public string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set { SetValue(TextFilterValueProperty, value); }
        }

        public ICommand FilterCommand { get; private set; }
        protected abstract void SetFilter(object p);
        
        public ICommand DetailsCommand { get; private set; }
        protected abstract void ShowDetails(object p);

        private void SetCommands()
        {
            FilterCommand = new DelegateCommand<object>(SetFilter);
            DetailsCommand = new DelegateCommand<object>(ShowDetails);
        }
    }
}
