using RCS.WpfShop.AdventureWorks.ServiceReferences;
using RCS.WpfShop.Resources;
using System;
using System.Diagnostics;
using System.Windows;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        #region Construction
        // Note the default is for test instantiation.
        protected ProductsServiceConsumer(IProductsService productsServiceClient = null)
        {
            ProductsServiceClient = productsServiceClient;
        }
        #endregion

        #region Service
        // TODO actually use this in client. May have to be moved to registration.
        private static TimeSpan Timeout { get; } = new(0, 0, 15);

        // Public set to enable parameterless constructor for tests.
        public IProductsService ProductsServiceClient { get; set; }
        #endregion

        #region IDisposable

        // Check out the IDisposable documentation for details on the pattern applied here.
        // Note that it can have implications on derived classes too.

        // Has Dispose already been called?
        private bool disposed;

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
        private static readonly TimeSpan serviceErrorGraceTime = Timeout + Timeout;

        private static TraceSource traceSource = new("MainTrace");

        protected static void DisplayAlert(Exception exception)
        {
            // Try to prevent stacking muliple related messages, like at startup.
            if (!serviceErrorDisplaying && DateTime.Now > serviceErrorFirstDisplayed + serviceErrorGraceTime)
            {
                serviceErrorDisplaying = true;
                serviceErrorFirstDisplayed = DateTime.Now;

                traceSource.TraceEvent(TraceEventType.Error, default, exception.Message);

                var result = MessageBox.Show($"{Labels.ErrorService}\n\n{Labels.ErrorDetailsWanted}", $"{Labels.ShopName} - {Labels.Error}", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    MessageBox.Show(exception.Message, $"{Labels.ShopName} - {Labels.ErrorDetails}", MessageBoxButton.OK, MessageBoxImage.Information);

                serviceErrorDisplaying = false;
            }
        }
        #endregion
    }
}
