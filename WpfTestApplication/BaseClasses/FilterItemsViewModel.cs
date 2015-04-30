using Microsoft.Practices.Prism.Commands;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace WpfTestApplication.BaseClasses
{
    abstract class FilterItemsViewModel : ItemsViewModel
    {
        public FilterItemsViewModel()
        {
            SetCommands();
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
            FilterItemsViewModel viewModel = dependencyObject as FilterItemsViewModel;
            viewModel.SetDetailFilterItems();
        }

        private void SetDetailFilterItems()
        {
            string filter = null;

            // Preserve empty value.
            filter = string.Format("({0} = {1})", DetailFilterMasterKeyPath, -1);

            filter += (int)MasterFilterValue != -1 ? string.Format(" OR ({0} = {1})", DetailFilterMasterKeyPath, MasterFilterValue) : null;

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
            DependencyProperty.Register("DetailFilterValue", typeof(object), typeof(ItemsViewModel));

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

        protected virtual void SetFilter(object p)
        {
            string filter = null;

            filter = AddFilter(filter, MasterFilterSelectedValuePath, (int)MasterFilterValue);
            filter = AddFilter(filter, DetailFilterSelectedValuePath, (int)DetailFilterValue);
            filter = AddFilter(filter, TextFilterValuePath, TextFilterValue);

            Items.Table.CaseSensitive = false;
            Items.RowFilter = filter;

            RaisePropertyChanged("ItemsCount");
        }

        private string AddFilter(string filter, string newFilterValuePath, int newFilterValue)
        {
            // Note that newFilterValue is assumed int and Nullable, which is represented as -1.

            filter += !NullOrEmpty(filter) && newFilterValue != -1 ? " AND " : null;
            filter += (int)newFilterValue != -1 ? string.Format("({0} = {1})", newFilterValuePath, newFilterValue) : null;

            return filter;
        }

        private string AddFilter(string filter, string newFilterValuePath, string newFilterValue)
        {
            filter += !NullOrEmpty(filter) && !NullOrEmpty(newFilterValue) ? " AND " : null;
            filter += !NullOrEmpty(newFilterValue) ? string.Format("({0} LIKE '%{1}%')", newFilterValuePath, newFilterValue) : null;

            return filter;
        }

        public ICommand DetailsCommand { get; private set; }
        protected abstract void ShowDetails(object p);

        private void SetCommands()
        {
            FilterCommand = new DelegateCommand<object>(SetFilter);
            DetailsCommand = new DelegateCommand<object>(ShowDetails);
        }
    }
}
