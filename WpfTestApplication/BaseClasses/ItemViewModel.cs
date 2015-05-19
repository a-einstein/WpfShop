﻿using System;
using System.ComponentModel;
using System.Data;
using System.Windows;

namespace WpfTestApplication.BaseClasses
{
    abstract class ItemViewModel<T> : ViewModel where T : DataRow
    {
        public object ItemId
        {
            get { return GetItemId(); }
            set { Refresh(value); }
        }

        protected abstract object GetItemId();

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(T), typeof(ItemViewModel<T>), new PropertyMetadata(null));

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

        protected void GetItemCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception(databaseErrorMessage, e.Error);
            else
                Item = e.Result as T;
        }
    }
}