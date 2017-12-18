using RCS.WpfShop.Resources;
using System;
using System.Deployment.Application;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml;

namespace RCS.WpfShop.Main
{
    public partial class MainApplication : Application
    {
        #region Construction
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // TODO Maybe use the standard name again. Maybe there is some use in the base. Currently this creates a loop.
            //base.OnStartup(e);

            SetupExceptionHandling();

            var bootstrapper = new MainBootstrapper();
            bootstrapper.Run();
        }
        #endregion

        #region Error handling
        private void SetupExceptionHandling()
        {
            DispatcherUnhandledException += Dispatcher_UnhandledException;

            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            HandleException(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            HandleException(e.Exception);
        }

        private bool closing;

        // TODO Still searching for a way to handle all exceptions centrally. Currently there are multiple of this function.
        // Rethrow is not really an option as that impairs other functionality in the catches, besides being wrapped in awaits.
        // Otherwise centralize this function somewhere. 
        protected void HandleException(Exception exception)
        {
            // Avoid multiple exception messages.
            if (!closing)
            {
                closing = true;

                // Pity the No button cannot be made default.
                var result = MessageBox.Show($"{Labels.ErrorUnknown}\n\n{Labels.ErrorDetailsWanted}", $"{Labels.ShopName} - {Labels.Error}", MessageBoxButton.YesNo, MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                    MessageBox.Show(exception.Message, $"{Labels.ShopName} - {Labels.ErrorDetails}", MessageBoxButton.OK, MessageBoxImage.Information);

                MessageBox.Show(Labels.Closing, $"{Labels.ShopName} - {Labels.Close}", MessageBoxButton.OK, MessageBoxImage.Warning);

                Application.Current?.Shutdown();
            }
        }
        #endregion
    }
}
