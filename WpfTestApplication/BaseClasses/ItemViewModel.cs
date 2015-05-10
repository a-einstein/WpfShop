﻿using System.Windows;

namespace WpfTestApplication.BaseClasses
{
    abstract class ItemViewModel<T> : ViewModel
    {
        public object ItemId 
        {
            get
            {
                return GetItemId();
            }
            set
            {
                Item = GetData(value);
            }
        }

        protected abstract object GetItemId();

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(T), typeof(ItemViewModel<T>), new PropertyMetadata(null));

        public T Item
        {
            get { return (T)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
         
        protected abstract T GetData(object Id);
   }
}