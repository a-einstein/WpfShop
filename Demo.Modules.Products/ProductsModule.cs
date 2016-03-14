using Demo.Common;
using Demo.Common.Modules;
using Demo.Modules.Products.Views;
using Prism.Mef.Modularity;
using Prism.Modularity;

namespace Demo.Modules.Products
{
    // Note that ContractName and TypeIdentity remain IModule despite the parameters.
    // However the moduleName needs to be unique to avoid conflicts.
    // This is just a container in which simple exports have to be discovered.
    [ModuleExport("ProductsModule", typeof(IModule))]
    public class ProductsModule : DemoModule
    {
        public override void Initialize()
        {
            // As the type has to be used here, a RequestNavigate elsewhere can only use the class name (as string), 
            // not any other identification like a functional name.
            RegionManager.RegisterViewWithRegion(Regions.Widgets, typeof(ShoppingCartView));
            RegionManager.RegisterViewWithRegion(Regions.Main, typeof(ProductsView));
        }
    }
}
