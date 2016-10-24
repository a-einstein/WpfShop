using Prism.Mef.Modularity;
using Prism.Modularity;
using RCS.WpfShop.Common;
using RCS.WpfShop.Common.Modules;
using RCS.WpfShop.Modules.Products.Views;

namespace RCS.WpfShop.Modules.Products
{
    // Note that ContractName and TypeIdentity remain IModule despite the parameters.
    // However the moduleName needs to be unique to avoid conflicts.
    // This is just a container in which simple exports have to be discovered.
    [ModuleExport(nameof(ProductsModule), typeof(IModule))]
    public class ProductsModule : Module
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
