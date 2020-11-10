using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using RCS.WpfShop.Common;
using RCS.WpfShop.Common.Modules;
using RCS.WpfShop.Modules.Products.ViewModels;
using RCS.WpfShop.Modules.Products.Views;
using System;
using Unity;

namespace RCS.WpfShop.Modules.Products
{
    // Use the ModuleDependency to order activation.
    // It is fair to assume the presence of AboutModule though this is not really dependant.
    // Anyway, there does not seem to be a simple other way.
    [ModuleDependency("AboutModule")]
    public class ProductsModule : Module
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // TODO Make use of Core for injection?

            base.RegisterTypes(containerRegistry);

            var container = containerRegistry.GetContainer();
            container.RegisterSingleton<ShoppingCartViewModel>();

            containerRegistry.RegisterForNavigation<ShoppingCartView>();
            containerRegistry.RegisterForNavigation<ProductsView>();
         }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);

            // As the type has to be used here, a RequestNavigate elsewhere can only use the class name (as string), 
            // not any other identification like a functional name.

            regionManager.RegisterViewWithRegion(Regions.MainViewWidgets, typeof(ShoppingCartView));

            // Activate the ShoppingCart which is generally visible when this module is present.
            regionManager.RequestNavigate(Regions.MainViewWidgets, new Uri(nameof(ShoppingCartView), UriKind.Relative));

            regionManager.RegisterViewWithRegion(Regions.MainViewMain, typeof(ProductsView));
        }
    }
}
