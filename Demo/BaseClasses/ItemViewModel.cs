using Common.DomainClasses;
using System.Windows;

namespace Demo.BaseClasses
{
    abstract class ItemViewModel<T,U> : ViewModel where T : DomainClass
    {
        /// <summary>
        /// A value enabling recognition of empty Items.
        /// </summary>
        public abstract U NoId { get; }

        public object ItemId
        {
            get { return GetItemId(); }
            set { Refresh(value); }
        }

        protected abstract object GetItemId();

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(T), typeof(ItemViewModel<T,U>), new PropertyMetadata(null));

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