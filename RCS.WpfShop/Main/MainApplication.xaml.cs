using CommonServiceLocator;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using RCS.WpfShop.Resources;
using RCS.WpfShop.ViewModels;
using RCS.WpfShop.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RCS.WpfShop.Main
{
    public partial class MainApplication : PrismApplication
    {
        #region Construction
        // Note the call order is as below.

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // TODO Maybe use the standard name again. Maybe there is some use in the base. Currently this creates a loop.
            //base.OnStartup(e);

            SetupExceptionHandling();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainViewModel>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            // Note that often a rebuild is necessary to update the modules.
            // Note that the order of discovery is alphabetically, the order of activation seems abrbitrary or reversed.
            // Currently I regulate the activation by the ModuleDependency attribute.
            var catalog = new DirectoryModuleCatalog() { ModulePath = @".\Modules" };

            return catalog;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        { }

        protected override Window CreateShell()
        {
            var mainWindow = Container.Resolve<MainWindow>();

            return mainWindow;
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
