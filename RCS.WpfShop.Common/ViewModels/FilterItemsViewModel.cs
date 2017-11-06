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
        #region Construct
        protected override void SetCommands()
        {
            FilterCommand = new DelegateCommand(Refresh);
            DetailsCommand = new DelegateCommand<I>(ShowDetails);
        }
        #endregion

        #region Filtering
        protected abstract Task InitializeFilters();

        public ObservableCollection<FM> MasterFilterItems { get; } = new ObservableCollection<FM>();

        public static readonly DependencyProperty MasterFilterValueProperty =
            DependencyProperty.Register(nameof(MasterFilterValue), typeof(FM), typeof(ItemsViewModel<I>), new PropertyMetadata(new PropertyChangedCallback(OnMasterFilterValueChanged)));

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

            DetailFilterItems.Clear();

            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                DetailFilterItems.Add(item);
            }

            // To trigger the enablement.
            RaisePropertyChanged(nameof(DetailFilterItems));
        }

        protected abstract Func<FD, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected Collection<FD> detailFilterItemsSource = new Collection<FD>();
        public ObservableCollection<FD> DetailFilterItems { get; } = new ObservableCollection<FD>();

        public static readonly DependencyProperty DetailFilterValueProperty =
            DependencyProperty.Register(nameof(DetailFilterValue), typeof(FD), typeof(ItemsViewModel<I>));

        public FD DetailFilterValue
        {
            get { return (FD)GetValue(DetailFilterValueProperty); }
            set { SetValue(DetailFilterValueProperty, value); }
        }

        public static readonly DependencyProperty TextFilterValueProperty =
            DependencyProperty.Register(nameof(TextFilterValue), typeof(string), typeof(ItemsViewModel<I>));

        public string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set { SetValue(TextFilterValueProperty, value); }
        }

        public ICommand FilterCommand { get; private set; }
        #endregion

        #region Details
        public ICommand DetailsCommand { get; private set; }

        protected abstract void ShowDetails(I overviewObject);
        #endregion
    }
}
