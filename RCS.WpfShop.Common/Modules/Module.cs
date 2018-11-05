using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace RCS.WpfShop.Common.Modules
{
    public abstract class Module : IModule
    {
        // Note call order is as below.

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        { }

        public IRegionManager regionManager;

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager = containerProvider.Resolve<IRegionManager>();
        }
    }
}
