// Note that currently the Prism/ServiceLocation packages cannot be updated, as long as the Prism packages are not ALL on the 7.0 version.
// Some details can be seen here: https://github.com/PrismLibrary/Prism/issues/1211
// Note that for coherency it is best to pull in a package like MEF that pulls in its own dependencies.
// As described here: https://github.com/PrismLibrary/Prism-Documentation/blob/master/docs/getting-started/NuGet-Packages.md
// TODO Check Nuget updates.
// Note that probably this Bootstrapper code has to be changed for Application.
// See https://github.com/PrismLibrary/Prism/releases/tag/7.0.0-pre5

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
