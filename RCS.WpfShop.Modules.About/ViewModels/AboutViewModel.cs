using RCS.WpfShop.Common.ViewModels;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;

namespace RCS.WpfShop.Modules.About.ViewModels
{
    [Export]
    public class AboutViewModel : ViewModel
    {
        public static readonly DependencyProperty ApplicationVersionProperty =
            DependencyProperty.Register(nameof(ApplicationVersion), typeof(string), typeof(AboutViewModel), new PropertyMetadata(Assembly.GetExecutingAssembly().GetName().Version.ToString()));

        public string ApplicationVersion
        {
            get { return (string)GetValue(ApplicationVersionProperty); }
            set { SetValue(ApplicationVersionProperty, value); }
        }
    }
}
