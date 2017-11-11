using RCS.AdventureWorks.Common.DomainClasses;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemViewModel<I> : ViewModel where I : DomainClass
    {
        #region Item
        // Store the ID separately to enable a retry on an interrupted Refresh.
        // Note that a generic property is impossible, so the DomainClass is needed, determining the property's type.
        public int? ItemId { get; set; }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(I), typeof(ItemViewModel<I>), new PropertyMetadata(new PropertyChangedCallback(Item_Changed)));

        public I Item
        {
            get { return (I)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        private static void Item_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var itemViewModel = d as ItemViewModel<I>;
            itemViewModel.Title = itemViewModel.MakeTitle();
        }
        #endregion
    }
}