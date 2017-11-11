using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class FilterItemsViewModel<I, FM, FD> : ItemsViewModel<I>
    {
        #region Refresh
        private bool initialized;

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized && !initialized)
            {
                initialized = await InitializeFilters();
            }

            return (initialized);
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            FilterCommand = new DelegateCommand(async () => await Refresh());
        }

        protected override async Task<bool> Read()
        {
            var succeeded = await ReadFiltered();

            return succeeded;
        }
        #endregion

        #region Filtering
        protected abstract Task<bool> InitializeFilters();

        public static readonly DependencyProperty MasterFilterItemsProperty =
           DependencyProperty.Register(nameof(MasterFilterItems), typeof(ObservableCollection<FM>), typeof(FilterItemsViewModel<I, FM, FD>));

        public ObservableCollection<FM> MasterFilterItems
        {
            get { return (ObservableCollection<FM>)GetValue(MasterFilterItemsProperty); }
            set { SetValue(MasterFilterItemsProperty, value); }
        }

        public static readonly DependencyProperty MasterFilterValueProperty =
            DependencyProperty.Register(nameof(MasterFilterValue), typeof(FM), typeof(FilterItemsViewModel<I, FM, FD>), new PropertyMetadata(new PropertyChangedCallback(OnMasterFilterValueChanged)));

        public FM MasterFilterValue
        {
            get { return (FM)GetValue(MasterFilterValueProperty); }
            set { SetValue(MasterFilterValueProperty, value); }
        }

        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        private static void OnMasterFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var viewModel = dependencyObject as FilterItemsViewModel<I, FM, FD>;

            viewModel.SetDetailFilterItems();
            viewModel.DetailFilterValue = viewModel.DetailFilterItems.FirstOrDefault();
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems(bool addEmptyElement = true)
        {
            var detailFilterItemsSelection = detailFilterItemsSource.Where(DetailFilterItemsSelector());

            var detailFilterItems = new ObservableCollection<FD>(); ;

            // Store in a temporary structure first to avoid bothering the GUI.
            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                detailFilterItems.Add(item);
            }

            DetailFilterItems = detailFilterItems;
        }

        protected abstract Func<FD, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<FD> detailFilterItemsSource = new Collection<FD>();

        public static readonly DependencyProperty DetailFilterItemsProperty =
           DependencyProperty.Register(nameof(DetailFilterItems), typeof(ObservableCollection<FD>), typeof(FilterItemsViewModel<I, FM, FD>));

        public ObservableCollection<FD> DetailFilterItems
        {
            get { return (ObservableCollection<FD>)GetValue(DetailFilterItemsProperty); }
            set { SetValue(DetailFilterItemsProperty, value); }
        }

        public static readonly DependencyProperty DetailFilterValueProperty =
            DependencyProperty.Register(nameof(DetailFilterValue), typeof(FD), typeof(FilterItemsViewModel<I, FM, FD>));

        public FD DetailFilterValue
        {
            get { return (FD)GetValue(DetailFilterValueProperty); }
            set { SetValue(DetailFilterValueProperty, value); }
        }

        public static readonly DependencyProperty TextFilterValueProperty =
            DependencyProperty.Register(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<I, FM, FD>));

        public string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set { SetValue(TextFilterValueProperty, value); }
        }

        protected abstract Task<bool> ReadFiltered();

        public static readonly DependencyProperty FilterCommandProperty =
             DependencyProperty.Register(nameof(FilterCommand), typeof(ICommand), typeof(FilterItemsViewModel<I, FM, FD>));

        // Note this does not work as explicit interface implementation.
        public ICommand FilterCommand
        {
            get { return (ICommand)GetValue(FilterCommandProperty); }
            private set { SetValue(FilterCommandProperty, value); }
        }
        #endregion
    }
}
