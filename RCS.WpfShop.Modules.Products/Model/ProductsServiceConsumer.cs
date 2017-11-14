using RCS.WpfShop.Resources;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System;
using System.Windows;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        #region Service
        private ProductsServiceClient productsServiceClient;

        protected ProductsServiceClient ProductsServiceClient
        {
            get
            {
                if (productsServiceClient == null)
                    productsServiceClient = new ProductsServiceClient();

                return productsServiceClient;
            }
        }
        #endregion

        #region IDisposable

        // Check out the IDisposable documentation for details on the pattern applied here.
        // Note that it can have implications on derived classes too.

        // Has Dispose already been called?
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            // Free managed objects here.
            if (disposing)
            {
                productsServiceClient?.Close();
            }

            // Free unmanaged objects here.
            { }

            disposed = true;
        }

        ~ProductsServiceConsumer()
        {
            Dispose(false);
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
