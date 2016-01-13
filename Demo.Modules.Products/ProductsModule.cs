using Demo.Common;
using Prism.Mef.Modularity;

namespace Demo.Modules.Products
{
    [ModuleExport("ProductsModule", typeof(DemoModule))]
    public class ProductsModule : DemoModule
    {
    }
}
