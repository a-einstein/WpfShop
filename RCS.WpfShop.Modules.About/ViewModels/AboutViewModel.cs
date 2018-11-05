using Prism.Regions;
using RCS.WpfShop.Common.ViewModels;
using System.Reflection;
using System.Windows;

namespace RCS.WpfShop.Modules.About.ViewModels
{
    public class AboutViewModel : ViewModel
    {
        #region INavigationAware
        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        #endregion

        #region Information
        public static readonly DependencyProperty ApplicationVersionProperty =
                DependencyProperty.Register(nameof(ApplicationVersion), typeof(string), typeof(AboutViewModel), new PropertyMetadata(Assembly.GetExecutingAssembly().GetName().Version.ToString()));

        public string ApplicationVersion
        {
            get { return (string)GetValue(ApplicationVersionProperty); }
            set { SetValue(ApplicationVersionProperty, value); }
        }
        #endregion
    }
}
