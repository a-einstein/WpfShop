using Prism.Commands;
using RCS.WpfShop.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem> : ItemsViewModel<TItem>
    {
        #region Refresh
        private bool initialized;

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized && !initialized)
            {
                Message = Labels.Initializing;

                initialized = await InitializeFilters();

                Message = string.Empty;
            }

            return initialized;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            FilterCommand = new DelegateCommand(async () => await Refresh());
        }

        protected override async Task<bool> Read()
        {
            Message = Labels.Searching;

            var succeeded = await ReadFiltered();

            Message = succeeded && ItemsCount == 0 ? Labels.NotFound : string.Empty;

            return succeeded;
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        #endregion

        #region Filtering
        protected abstract Task<bool> InitializeFilters();

        public static readonly DependencyProperty MasterFilterItemsProperty =
           DependencyProperty.Register(nameof(MasterFilterItems), typeof(ObservableCollection<TMasterFilterItem>), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public ObservableCollection<TMasterFilterItem> MasterFilterItems
        {
            get => (ObservableCollection<TMasterFilterItem>)GetValue(MasterFilterItemsProperty);
            set => SetValue(MasterFilterItemsProperty, value);
        }

        public static readonly DependencyProperty MasterFilterValueProperty =
            DependencyProperty.Register(nameof(MasterFilterValue), typeof(TMasterFilterItem), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), new PropertyMetadata(OnMasterFilterValueChanged));

        public TMasterFilterItem MasterFilterValue
        {
            get => (TMasterFilterItem)GetValue(MasterFilterValueProperty);
            set => SetValue(MasterFilterValueProperty, value);
        }

        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        private static void OnMasterFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var viewModel = dependencyObject as FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>;

            viewModel.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems(bool addEmptyElement = true)
        {
            var detailFilterItemsSelection = detailFilterItemsSource.Where(DetailFilterItemsSelector(addEmptyElement));

            var detailFilterItems = new ObservableCollection<TDetailFilterItem>();

            // Store in a temporary structure first to avoid bothering the GUI.
            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                detailFilterItems.Add(item);
            }

            DetailFilterItems = detailFilterItems;
        }

        protected abstract Func<TDetailFilterItem, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<TDetailFilterItem> detailFilterItemsSource = new Collection<TDetailFilterItem>();

        public static readonly DependencyProperty DetailFilterItemsProperty =
           DependencyProperty.Register(nameof(DetailFilterItems), typeof(ObservableCollection<TDetailFilterItem>), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public ObservableCollection<TDetailFilterItem> DetailFilterItems
        {
            get => (ObservableCollection<TDetailFilterItem>)GetValue(DetailFilterItemsProperty);
            set => SetValue(DetailFilterItemsProperty, value);
        }

        public static readonly DependencyProperty DetailFilterValueProperty =
            DependencyProperty.Register(nameof(DetailFilterValue), typeof(TDetailFilterItem), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public TDetailFilterItem DetailFilterValue
        {
            get => (TDetailFilterItem)GetValue(DetailFilterValueProperty);
            set => SetValue(DetailFilterValueProperty, value);
        }

        public static readonly DependencyProperty TextFilterValueProperty =
            DependencyProperty.Register(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        public string TextFilterValue
        {
            get => (string)GetValue(TextFilterValueProperty);
            set => SetValue(TextFilterValueProperty, value);
        }

        protected abstract Task<bool> ReadFiltered();

        public static readonly DependencyProperty FilterCommandProperty =
             DependencyProperty.Register(nameof(FilterCommand), typeof(ICommand), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        // Note this does not work as explicit interface implementation.
        public ICommand FilterCommand
        {
            get => (ICommand)GetValue(FilterCommandProperty);
            private set => SetValue(FilterCommandProperty, value);
        }
        #endregion
    }
}
