// TODO Note that currently ServiceLocation cannot be updated.
// Both version 1.4. as 2.0.1 result in an exception as decribed here: 
// https://github.com/PrismLibrary/Prism/issues/1211
using Prism.Mef;
using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace RCS.WpfShop.Main
{
    class MainBootstrapper : MefBootstrapper
    {
        // This is for MEF.
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            // Discover anything in my own assembly. 
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(MainBootstrapper).Assembly));

            // Discover modules in the directory.
            // Module assemlies have to be copied there.
            // Note that this mechanism seems to need more cleanings than expected.
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
