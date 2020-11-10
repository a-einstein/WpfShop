using RCS.WpfShop.AdventureWorks.ServiceReferences;
using RCS.WpfShop.Resources;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows;
using Unity;
using static RCS.WpfShop.AdventureWorks.ServiceReferences.ProductsServiceClient;

namespace RCS.WpfShop.Modules.Products.Model
{
    public abstract class ProductsServiceConsumer : IDisposable
    {
        #region Service
        // TODO actually use this in client.
        private static TimeSpan Timeout { get; } = new TimeSpan(0, 0, 15);

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

                    // Note this no longer works, so the .config files have been disabled.
                    //container.LoadConfiguration();
                    //productsServiceClient = container.Resolve<IProductsService>();

                    // HACK Temporary solution to get going.
                    // TODO Use Core configuration? It is not based on the buildconfigurations.
                    var endpointConfiguration = EndpointConfiguration.WSHttpBinding_IProductsService;
                    var endpointAddress = new EndpointAddress("https://localhost:44300/ProductsService.svc/ProductsServiceW");

                    // TODO Use injection again. Include the parameters.
                    // Note there are more constructors that may be applied, or even add some of our own in a partial class.

                    // TODO Check why the TDOs are not reused in the service client.
                    productsServiceClient = new ProductsServiceClient(endpointConfiguration, endpointAddress);
                }

                return productsServiceClient;
            }
        }
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

        private static TraceSource traceSource = new TraceSource("MainTrace");

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
