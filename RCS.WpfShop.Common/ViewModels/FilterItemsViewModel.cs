﻿using Prism.Commands;
using RCS.WpfShop.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem> :
        ItemsViewModel<TItem>
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            FilterCommand = new DelegateCommand(async () => await RefreshView(), FilterCanExecute);
        }
        #endregion

        #region Refresh
        private bool initialized;

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized && !initialized)
            {
                Message = Labels.Initializing;

                initialized = await InitializeFilters();

                Message = String.Empty;
            }

            return initialized;
        }

        protected override async Task Read()
        {
            Message = Labels.Searching;

            var succeeded = await ReadFiltered();

            Message = succeeded && ItemsCount == 0 ? Labels.NotFound : String.Empty;

            await base.Read();
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
        private bool filterChanged;

        private bool FilterChanged
        {
            get => filterChanged;
            set
            {
                filterChanged = value;

                uiDispatcher.Invoke(() =>
                {
                    FilterCommand.RaiseCanExecuteChanged();
                });
            }
        }

        protected abstract Task<bool> InitializeFilters();

        public ObservableCollection<TMasterFilterItem> MasterFilterItems { get; } = new();

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

            viewModel.FilterChanged = true;
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems(bool addEmptyElement = true)
        {
            DetailFilterItems.Clear();

            var detailFilterItemsSelection = detailFilterItemsSource.Where(DetailFilterItemsSelector(addEmptyElement));

            // Store in a temporary structure first to avoid bothering the GUI.
            // Note that the query is executed on the foreach.
            foreach (var item in detailFilterItemsSelection)
            {
                DetailFilterItems.Add(item);
            }

            // Extra event. For some bindings (ItemsSource) those from ObservableCollection are enough, but for others (IsEnabled) this is needed.
            RaisePropertyChanged(nameof(DetailFilterItems));
        }

        protected abstract Func<TDetailFilterItem, bool> DetailFilterItemsSelector(bool addEmptyElement = true);

        protected readonly List<TDetailFilterItem> detailFilterItemsSource = new();

        public ObservableCollection<TDetailFilterItem> DetailFilterItems { get; } = new();

        public static readonly DependencyProperty DetailFilterValueProperty =
            DependencyProperty.Register(nameof(DetailFilterValue), typeof(TDetailFilterItem), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), new PropertyMetadata(new PropertyChangedCallback(OnDetailFilterValueChanged)));

        public TDetailFilterItem DetailFilterValue
        {
            get => (TDetailFilterItem)GetValue(DetailFilterValueProperty);
            set => SetValue(DetailFilterValueProperty, value);
        }

        private static void OnDetailFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = dependencyObject as FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>;

            viewModel.FilterChanged = true;
        }

        public static readonly DependencyProperty TextFilterValueProperty =
            DependencyProperty.Register(nameof(TextFilterValue), typeof(string), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>), new PropertyMetadata(OnTextFilterValueChanged));

        public string TextFilterValue
        {
            get => (string)GetValue(TextFilterValueProperty);
            set => SetValue(TextFilterValueProperty, value);
        }

        private static void OnTextFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = dependencyObject as FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>;

            viewModel.FilterChanged = true;
        }

        protected virtual bool FilterCanExecute()
        {
            return FilterChanged;
        }

        protected abstract Task<bool> ReadFiltered();

        public static readonly DependencyProperty FilterCommandProperty =
             DependencyProperty.Register(nameof(FilterCommand), typeof(DelegateCommand), typeof(FilterItemsViewModel<TItem, TMasterFilterItem, TDetailFilterItem>));

        // Note need explicit DelegateCommand for RaiseCanExecuteChanged.
        public DelegateCommand FilterCommand
        {
            get => (DelegateCommand)GetValue(FilterCommandProperty);
            private set => SetValue(FilterCommandProperty, value);
        }

        public override async Task RefreshView()
        {
            await base.RefreshView();

            FilterChanged = false;
        }
        #endregion
    }
}
