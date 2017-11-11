using Prism.Commands;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemsViewModel<I> : ViewModel
    {
        #region Items
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<I>), typeof(ItemsViewModel<I>), new PropertyMetadata(new PropertyChangedCallback(Items_Changed)));

        // TODO Some sort of view would be more convenient to enable sorting in situ (filtering is no longer done so). But remember: that no longer applies when paging.
        public ObservableCollection<I> Items
        {
            get { return (ObservableCollection<I>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private static void Items_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemsViewModel = d as ItemsViewModel<I>;
            itemsViewModel.Items.CollectionChanged += itemsViewModel.Items_CollectionChanged;
        }

        protected virtual void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Assign instead of just RaisePropertyChanged as a clearer approach.
            ItemsCount = Items?.Count ?? 0;
        }

        // Note that one cannot directly bind on Items.Count.
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register(nameof(ItemsCount), typeof(int), typeof(ItemsViewModel<I>), new PropertyMetadata(0));

        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }
        #endregion

        #region Refresh
        protected override void Clear()
        {
            base.Clear();

            Items?.Clear();
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DetailsCommand = new DelegateCommand<I>(ShowDetails);
        }
        #endregion

        #region Details
        public static readonly DependencyProperty DetailsCommandProperty =
             DependencyProperty.Register(nameof(DetailsCommand), typeof(ICommand), typeof(ItemsViewModel<I>));

        // Note this does not work as explicit interface implementation.
        public ICommand DetailsCommand
        {
            get { return (ICommand)GetValue(DetailsCommandProperty); }
            private set { SetValue(DetailsCommandProperty, value); }
        }

        protected virtual void ShowDetails(I overviewObject) { }
        #endregion
    }
}
