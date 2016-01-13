using Demo.Common;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef;
using Prism.Modularity;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace Demo
{
    class MainBootstrapper : MefBootstrapper
    {
        // This is for MEF.
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            // Discover modules in the directory.
            var directoryCatalog = new DirectoryCatalog("Modules");
            AggregateCatalog.Catalogs.Add(directoryCatalog);
        }

        protected override DependencyObject CreateShell()
        {
            return new MainWindow();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = Shell as Window;
            Application.Current.MainWindow.Show();
        }
    }
}
