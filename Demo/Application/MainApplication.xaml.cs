using System.Windows;

// TODO Maybe change namespace.
namespace Demo
{
    public partial class MainApplication : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // TODO > Maybe use the standard name again. Maybe there is some use in the base.
            //base.OnStartup(e);

            var bootstrapper = new MainBootstrapper();

            bootstrapper.Run();
        }
    }
}
