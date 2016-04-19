using Prism.Modularity;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace RCS.WpfShop.Common.Modules
{
    public abstract class Module : IModule
    {
        public virtual void Initialize() { }

        [Import]
        public IRegionManager RegionManager;
    }
}
