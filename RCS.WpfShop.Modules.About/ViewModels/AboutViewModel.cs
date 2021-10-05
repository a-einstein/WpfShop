using Prism.Regions;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Resources;
using System.Reflection;

namespace RCS.WpfShop.Modules.About.ViewModels
{
    public class AboutViewModel : ViewModel
    {
        #region INavigationAware
        public override bool IsNavigationTarget(NavigationContext navigationContext) => true;
        #endregion

        #region Information
        public string ApplicationVersion => Assembly.GetEntryAssembly().GetName().Version.ToString();

        public string DeveloperLinkUri => Labels.DeveloperLinkUri;

        public string DeveloperLinkText => Labels.DeveloperLinkText;

        public string DocumentationLinkUri => Labels.DocumentationLinkUri;

        public string DocumentationLinkText => Labels.DocumentationLinkText;
        #endregion
    }
}
