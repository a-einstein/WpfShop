using RCS.AdventureWorks.Common.DomainClasses;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemViewModel<I> : ViewModel where I : DomainClass
    {
        #region Item
        public int? ItemId
        {
            get { return Item?.Id; }
            set { Refresh(value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(I), typeof(ItemViewModel<I>), new PropertyMetadata(null));

        public I Item
        {
            get { return (I)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
        #endregion

        #region Refresh
        public override void Refresh()
        {
            Refresh(ItemId);
        }

        public abstract void Refresh(object Id);
        #endregion
    }
}