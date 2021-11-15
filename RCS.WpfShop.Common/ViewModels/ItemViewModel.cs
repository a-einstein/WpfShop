using RCS.AdventureWorks.Common.DomainClasses;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemViewModel<TItem> : ViewModel where TItem : DomainClass
    {
        #region Item
        // Store the ID separately to enable a retry on an interrupted Refresh.
        // Note that a generic property is impossible, so the DomainClass is needed, determining the property's type.
        public int? ItemId { get; init; }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(TItem), typeof(ItemViewModel<TItem>), new PropertyMetadata(Item_Changed));

        public TItem Item
        {
            get => (TItem)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        private static void Item_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemViewModel = d as ItemViewModel<TItem>;
            itemViewModel.Title = itemViewModel.MakeTitle();
        }
        #endregion
    }
}