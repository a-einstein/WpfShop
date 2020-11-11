using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using RCS.WpfShop.Common;
using RCS.WpfShop.Common.Modules;
using RCS.WpfShop.Modules.Products.Model;
using RCS.WpfShop.Modules.Products.ViewModels;
using RCS.WpfShop.Modules.Products.Views;
using System;
using Unity;
using static RCS.WpfShop.AdventureWorks.ServiceReferences.ProductsServiceClient;

namespace RCS.WpfShop.Modules.Products
{
    // Use the ModuleDependency to order activation.
    // It is fair to assume the presence of AboutModule though this is not really dependant.
    // Anyway, there does not seem to be a simple other way.
    [ModuleDependency("AboutModule")]
    public class ProductsModule : Module
    {
        #region IModule
        // Note the most useful documentation so far has been: https://www.tutorialsteacher.com/ioc/register-and-resolve-in-unity-container
        // TODO Make use of Core for injection?
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            var container = containerRegistry.GetContainer();

            // TODO Make registration or injection dependent on configuration, maybe in Core.
            // Note this used to be done by container.LoadConfiguration(). 
            // This no longer works, so the transformed .config files have been disabled.
            var serviceConfiguration = new EndpointAndAddressConfiguration(EndpointConfiguration.WSHttpBinding_IProductsService, "https://localhost:44300/ProductsService.svc/ProductsServiceW");
            container.RegisterInstance(serviceConfiguration);

            container.RegisterSingleton<IProductsService, ProductsServiceClient>();
            //container.RegisterSingleton<IProductsService, ServiceClients.Products.Mock.ProductsServiceClient>();

            container.RegisterSingleton<ProductCategoriesRepository>();
            container.RegisterSingleton<ProductSubcategoriesRepository>();
            container.RegisterSingleton<ProductsRepository>();
            container.RegisterSingleton<CartItemsRepository>();

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
        #endregion
    }
}
