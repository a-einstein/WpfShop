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

            CheckForUpdate();

            var bootstrapper = new MainBootstrapper();
            bootstrapper.Run();
        }
        #endregion

        #region Update
        string updatingCaption = $"{Labels.ShopName} - {Labels.Updating}";

        /*
        This routine is geared towards manually updating by a zip file on GitHub, for it is not possile to set up the structure as needed for the standard ClickOnce update.
        Alternatively that may be set up on Azure or elsewehere.
        */
        private void CheckForUpdate()
        {
#if DEBUG
            MessageBox.Show("To debug\n1. Attach process now.\n2. Have breaks set.\n3. Continue.", updatingCaption, MessageBoxButton.OK, MessageBoxImage.Information);
#endif
            // Note this only works when installed by ClickOnce.
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var updateLocation = ApplicationDeployment.CurrentDeployment.UpdateLocation;

                var manifestXml = GetWebXml(updateLocation);

                if (manifestXml != null && UpdateAvailable(manifestXml))
                {
                    var result = MessageBox.Show(Labels.UpdateAvailable, updatingCaption, MessageBoxButton.OKCancel, MessageBoxImage.Question);

                    if ((MessageBoxResult.OK == result))
                    {
                        var supportUrl = SupportUrl(manifestXml);

                        if (!string.IsNullOrEmpty(supportUrl?.ToString()))
                        {
                            MessageBox.Show($"{Labels.UpdateInstructions}\n\n{Labels.Closing}", updatingCaption, MessageBoxButton.OK, MessageBoxImage.Information);

                            System.Diagnostics.Process.Start(supportUrl.ToString());

                            Application.Current.Shutdown();
                        }
                        else
                        {
                            MessageBox.Show($"{Labels.UpdateNoInformation}\n\n{Labels.UpdateContinueCurrent}", updatingCaption, MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private XmlDocument GetWebXml(Uri location)
        {
            string fileString;

            try
            {
                // This might fail in various ways, e.g. lacking a connection or a incorrect location.
                var webClient = new WebClient() { Encoding = Encoding.UTF8 };
                fileString = webClient.DownloadString(location);
            }
            catch (Exception)
            {
                MessageBox.Show($"{Labels.UpdateCheckFailed}\n\n{Labels.UpdateContinueCurrent}", updatingCaption, MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            // Filter out some possible heading.
            var xmlStart = fileString.IndexOfAny(new[] { '<' });

            var xmlString = fileString.Substring(xmlStart);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            return xmlDocument;
        }

        private static bool UpdateAvailable(XmlDocument manifestXml)
        {
            var identityNode = GetNode(manifestXml, "assemblyIdentity");

            var latestVersion = new Version(identityNode?.Attributes["version"].Value);

            return (latestVersion > ApplicationDeployment.CurrentDeployment.CurrentVersion);
        }

        private static Uri SupportUrl(XmlDocument manifestXml)
        {
            var descriptionNode = GetNode(manifestXml, "description");

            var supportAttribute = descriptionNode?.Attributes["asmv2:supportUrl"];

            return (!string.IsNullOrEmpty(supportAttribute?.Value))
                ? new Uri(supportAttribute?.Value)
                : null;
        }

        private static XmlNode GetNode(XmlDocument xmlDocument, string tag)
        {
            var taggedNodes = xmlDocument.GetElementsByTagName(tag);

            return (taggedNodes != null && taggedNodes.Count > 0)
                ? taggedNodes[0]
                : null;
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
