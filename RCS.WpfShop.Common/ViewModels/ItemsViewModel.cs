using System.Collections.ObjectModel;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemsViewModel<I> : ViewModel
    {
        #region Construct
        public ItemsViewModel()
        {
            Items = new ObservableCollection<I>();
        }
        #endregion

        #region Items
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<I>), typeof(ItemsViewModel<I>));

        // TODO Some sort of view would be more convenient to enable sorting in situ (filtering is no longer done so). But remember: that no longer applies when paging.
        public ObservableCollection<I> Items
        {
            get { return (ObservableCollection<I>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Convenience property to signal changes.
        // Note that just binding on Items.Count does not work.
        public int ItemsCount { get { return Items != null ? Items.Count : 0; } }
    }
    #endregion
}
