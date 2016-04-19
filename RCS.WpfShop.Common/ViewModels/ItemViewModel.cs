using Common.DomainClasses;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ItemViewModel<T, U> : ViewModel where T : DomainClass
    {
        public int? ItemId
        {
            get { return Item != null ? Item.Id : null; }
            set { Refresh(value); }
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(T), typeof(ItemViewModel<T, U>), new PropertyMetadata(null));

        public T Item
        {
            get { return (T)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public override void Refresh()
        {
            Refresh(ItemId);
        }

        public abstract void Refresh(object Id);
    }
}