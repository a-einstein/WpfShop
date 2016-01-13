using Demo.Common;
using Prism.Mef.Modularity;

namespace Demo.Modules.About
{
    // Note that ContractName and TypeIdentity remain IModule.
    // This is just a container in which simple exports have to be discovered.
    [ModuleExport("AboutModule", typeof(DemoModule))]
    public class AboutModule : DemoModule
    {
    }
}