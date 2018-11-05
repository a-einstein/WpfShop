using Prism.Ioc;
using RCS.WpfShop.Common;
using RCS.WpfShop.Common.Modules;
using RCS.WpfShop.Modules.About.Views;

namespace RCS.WpfShop.Modules.About
{
    // TODO Change filename.
    public class AboutModule : Module
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            containerRegistry.RegisterForNavigation<AboutView>();
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
 
            // As the type has to be used here, a RequestNavigate elsewhere can only use the class name (as string), 
            // not any other identification like a functional name.
            regionManager.RegisterViewWithRegion(Regions.MainViewMain, typeof(AboutView));
        }
    }
}