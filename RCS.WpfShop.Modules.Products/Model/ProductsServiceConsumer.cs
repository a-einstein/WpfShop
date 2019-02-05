using Microsoft.Practices.Unity.Configuration;
using RCS.WpfShop.Resources;
using RCS.WpfShop.ServiceClients.Products.ProductsService;
using System;
using System.Windows;
using Unity;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        #region Service
        // TODO actually use this in client.
        static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);

        private IProductsService productsServiceClient;

        protected IProductsService ProductsServiceClient
        {
            get
            {
                if (productsServiceClient == null)
                {
                    var container = new UnityContainer();
                    // Note this is only configurable in the app.config of the application, not the module.
                    // TODO For testing a transformation of the config would be needed.
                    // TODO Note that when configuring the service in the application, it would be logical to define the modules there as well.
                    container.LoadConfiguration();
                    productsServiceClient = container.Resolve<IProductsService>();
                }

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
                //TODO This stems from ClientBase.
                //productsServiceClient?.Close();
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
        private static bool serviceErrorDisplaying;

        // Note this initializes to 2001.
        private static DateTime serviceErrorFirstDisplayed;

        // This value is tested on 3 service calls at startup. There is no multiplication operator.
        private static TimeSpan serviceErrorGraceTime = ProductsServiceConsumer.Timeout + ProductsServiceConsumer.Timeout;

        protected static void DisplayAlert(Exception exception)
        {
            // Try to prevent stacking muliple related messages, like at startup.
            if (!serviceErrorDisplaying && DateTime.Now > serviceErrorFirstDisplayed + serviceErrorGraceTime)
            {
                serviceErrorDisplaying = true;
                serviceErrorFirstDisplayed = DateTime.Now;

                var result = MessageBox.Show($"{Labels.ErrorService}\n\n{Labels.ErrorDetailsWanted}", $"{Labels.ShopName} - {Labels.Error}", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    MessageBox.Show(exception.Message, $"{Labels.ShopName} - {Labels.ErrorDetails}", MessageBoxButton.OK, MessageBoxImage.Information);

                serviceErrorDisplaying = false;
            }
        }
        #endregion
    }
}
