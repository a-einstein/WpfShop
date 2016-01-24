using Prism.Modularity;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace Demo.Common
{
    public abstract class DemoModule : IModule
    {
        public virtual void Initialize() { }

        [Import]
        public IRegionManager RegionManager;
    }
}
