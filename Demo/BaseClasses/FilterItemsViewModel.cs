﻿using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Demo.BaseClasses
{
    public abstract class FilterItemsViewModel<T,U,V,W> : ItemsViewModel<T,U>
    {
        public FilterItemsViewModel()
        {
            DetailFilterItems = new ObservableCollection<W>();

            InitializeFilters();
        }

        protected abstract void InitializeFilters();

        public virtual string MasterFilterLabel { get { return "Category"; } }
        public virtual string MasterFilterDisplayMemberPath { get { return "Name"; } }

        public static readonly DependencyProperty MasterFilterItemsProperty =
            DependencyProperty.Register("MasterFilterItems", typeof(ObservableCollection<V>), typeof(ItemsViewModel<T,U>));

        public ObservableCollection<V> MasterFilterItems
        {
            get { return (ObservableCollection<V>)GetValue(MasterFilterItemsProperty); }
            set { SetValue(MasterFilterItemsProperty, value); }
        }

        public static readonly DependencyProperty MasterFilterValueProperty =
            DependencyProperty.Register("MasterFilterValue", typeof(V), typeof(ItemsViewModel<T,U>), new PropertyMetadata(new PropertyChangedCallback(OnMasterFilterValueChanged)));

        public V MasterFilterValue
        {
            get { return (V)GetValue(MasterFilterValueProperty); }
            set { SetValue(MasterFilterValueProperty, value); }
        }

        // Note this function does NOT filter Items, just updates DetailFilterItems and DetailFilterValue.
        // Currently the FilterCommand is just bound to a Button, implying it always has to be activated explicitly.
        private static void OnMasterFilterValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var viewModel = dependencyObject as FilterItemsViewModel<T, U, V, W>;

            viewModel.DetailFilterValue = default(W);
            viewModel.SetDetailFilterItems();
        }

        // TODO Some sort of view would be more convenient.
        private void SetDetailFilterItems(bool addEmptyElement = true)
        {
            var detailItemsSelection = detailFilterItemsSource.Where(DetailItemsSelector());

            DetailFilterItems.Clear();

            foreach (var item in detailItemsSelection)
            {
                DetailFilterItems.Add(item);
            }
        }

        protected abstract Func<W, bool> DetailItemsSelector(bool addEmptyElement = true);

        public virtual string DetailFilterLabel { get { return "Subcategory"; } }
        public virtual string DetailFilterDisplayMemberPath { get { return "Name"; } }
        public abstract string DetailFilterMasterKeyPath { get; }

        protected ObservableCollection<W> detailFilterItemsSource;

        public static readonly DependencyProperty DetailFilterItemsProperty =
            DependencyProperty.Register("DetailFilterItems", typeof(ObservableCollection<W>), typeof(ItemsViewModel<T, U>));

        public ObservableCollection<W> DetailFilterItems
        {
            get { return (ObservableCollection<W>)GetValue(DetailFilterItemsProperty); }
            set { SetValue(DetailFilterItemsProperty, value); }
        }

        public W DetailFilterValue
        {
            get { return (W)GetValue(DetailFilterValueProperty); }
            set { SetValue(DetailFilterValueProperty, value); }
        }

        public static readonly DependencyProperty DetailFilterValueProperty =
            DependencyProperty.Register("DetailFilterValue", typeof(W), typeof(ItemsViewModel<T,U>));

        public virtual string TextFilterLabel { get { return "Name"; } }
        public virtual string TextFilterValuePath { get { return "Name"; } }

        public static readonly DependencyProperty TextFilterValueProperty =
            DependencyProperty.Register("TextFilterValue", typeof(string), typeof(ItemsViewModel<T,U>));

        public string TextFilterValue
        {
            get { return (string)GetValue(TextFilterValueProperty); }
            set { SetValue(TextFilterValueProperty, value); }
        }

        public ICommand FilterCommand { get; private set; }

        protected virtual void Filter()
        {
            Refresh();
        }

        public ICommand DetailsCommand { get; private set; }
        protected abstract void ShowDetails(T p);

        protected override void SetCommands()
        {
            FilterCommand = new DelegateCommand(Filter);
            DetailsCommand = new DelegateCommand<T>(ShowDetails);
        }
    }
}
