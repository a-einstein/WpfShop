using Prism.Commands;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemsViewModel<TItem> : ViewModel
    {
        #region Construction
        protected ItemsViewModel()
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DetailsCommand = new DelegateCommand<TItem>(ShowDetails);
        }
        #endregion

        #region Items
        public ObservableCollection<TItem> Items { get; } = new ObservableCollection<TItem>();

        protected virtual void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Assign instead of just RaisePropertyChanged as a clearer approach.
            ItemsCount = Items?.Count ?? 0;
        }

        // Note that one cannot directly bind on Items.Count.
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register(nameof(ItemsCount), typeof(int), typeof(ItemsViewModel<TItem>), new PropertyMetadata(0));

        public int ItemsCount
        {
            get => (int)GetValue(ItemsCountProperty);
            set => SetValue(ItemsCountProperty, value);
        }
        #endregion

        #region Refresh
        protected override void Clear()
        {
            base.Clear();

            Items?.Clear();
        }
        #endregion

        #region Details
        public static readonly DependencyProperty DetailsCommandProperty =
             DependencyProperty.Register(nameof(DetailsCommand), typeof(ICommand), typeof(ItemsViewModel<TItem>));

        // Note this does not work as explicit interface implementation.
        public ICommand DetailsCommand
        {
            get => (ICommand)GetValue(DetailsCommandProperty);
            private set => SetValue(DetailsCommandProperty, value);
        }

        protected virtual void ShowDetails(TItem overviewObject) { }
        #endregion
    }
}
