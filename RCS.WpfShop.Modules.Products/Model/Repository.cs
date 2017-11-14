using RCS.WpfShop.Resources;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class Repository<T> : ProductsServiceConsumer
    {
        #region CRUD
        public ObservableCollection<T> List = new ObservableCollection<T>();

        public void Clear()
        {
            List.Clear();
        }
        #endregion

        #region Error handling
        protected void DisplayAlert(Exception exception)
        {
            var result = MessageBox.Show(Labels.ErrorService, Labels.Error, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                MessageBox.Show(exception.Message, Labels.ErrorDetails, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}