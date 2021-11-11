using Microsoft.Practices.Unity.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common;
using RCS.WpfShop.Common.Interfaces;
using RCS.WpfShop.Common.Modules;
using RCS.WpfShop.Modules.Products.Model;
using RCS.WpfShop.Modules.Products.ViewModels;
using RCS.WpfShop.Modules.Products.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Unity;
using static RCS.WpfShop.AdventureWorks.ServiceReferences.ProductsServiceClient;

namespace RCS.WpfShop.Modules.Products
{
    // Use the ModuleDependency to order activation.
    // It is fair to assume the presence of AboutModule though this is not really dependent.
    // Anyway, there does not seem to be a simple other way.
    [ModuleDependency("AboutModule")]
    public class ProductsModule : Module
    {
        #region IModule
        // Note the most useful documentation so far has been: https://www.tutorialsteacher.com/ioc/register-and-resolve-in-unity-container
        // TODO Make use of Core for injection?
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Read from the old .config files because they can be transformed based on the buildconfiguration.
            // The new method with .json files can only work with environment variables, which are unpractical to set.
            // Note this is the actual filename in Core, there is no default for this method.
            // If the exe.config file is named here, the result is just empty.
            // Alternatively the dll.config could be renamed or copied to exe.config by an action in the project file. https://stackoverflow.com/questions/45034007/using-app-config-in-net-core
            var configurationNameBase = $"{AppDomain.CurrentDomain.FriendlyName}.dll";

            var serviceConfiguration = ReadServiceConfiguration(configurationNameBase);
            var unityConfiguration = ReadUnityConfiguration(configurationNameBase);

            var container = containerRegistry.GetContainer();

            container.RegisterInstance(serviceConfiguration);

            // Register singleton depending on build configuration.
            container.LoadConfiguration(unityConfiguration, "Products");

            // Interfaces for constructor injections.
            container.RegisterSingleton<IRepository<List<ProductCategory>, ProductCategory>, ProductCategoriesRepository>();
            container.RegisterSingleton<IRepository<List<ProductSubcategory>, ProductSubcategory>, ProductSubcategoriesRepository>();
            container.RegisterSingleton<IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int>, ProductsRepository>();
            container.RegisterSingleton<IRepository<List<CartItem>, CartItem>, CartItemsRepository>();

            // Types for constructor injections, implicitly using repositories.
            container.RegisterSingleton<CartViewModel>();

            containerRegistry.RegisterForNavigation<CartView>();
            containerRegistry.RegisterForNavigation<ProductsView>();
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);

            // As the type has to be used here, a RequestNavigate elsewhere can only use the class name (as string), 
            // not any other identification like a functional name.

            regionManager.RegisterViewWithRegion(Regions.MainViewWidgets, typeof(CartView));

            // Activate the Cart which is generally visible when this module is present.
            regionManager.RequestNavigate(Regions.MainViewWidgets, new Uri(nameof(CartView), UriKind.Relative));

            regionManager.RegisterViewWithRegion(Regions.MainViewMain, typeof(ProductsView));
        }
        #endregion

        #region Utility
        static EndpointAndAddressConfiguration ReadServiceConfiguration(string configurationNameBase)
        {
            var configurationFileName = $"{configurationNameBase}.config";

            // Use xml reading instead of ConfigurationManager as there are no appropriate classes of ConfigurationSection available.
            var document = new XmlDocument();
            document.Load(configurationFileName);

            // Read limited part of the current configuration in file. 
            var endpointNode = document.DocumentElement.SelectSingleNode("/configuration/system.serviceModel/client/endpoint");
            var endpointAttributes = endpointNode.Attributes;

            var serviceConfiguration = new EndpointAndAddressConfiguration(
                Enum.Parse<EndpointConfiguration>(endpointAttributes["bindingConfiguration"].Value),
                endpointAttributes["address"].Value);

            return serviceConfiguration;
        }

        static UnityConfigurationSection ReadUnityConfiguration(string configurationNameBase)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(configurationNameBase);
            var sectionUnity = configuration.GetSection("unity") as UnityConfigurationSection;
            return sectionUnity;
        }
        #endregion
    }
}
