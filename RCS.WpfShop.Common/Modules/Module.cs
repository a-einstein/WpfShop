using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Diagnostics;

namespace RCS.WpfShop.Common.Modules
{
    public abstract class Module : IModule
    {
        // Note call order is as below.

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        { }

        public IRegionManager regionManager;

        protected static TraceSource TraceSource { get; } = new TraceSource("ModulesTrace");

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager = containerProvider.Resolve<IRegionManager>();

            SetUpTracing();
        }

        // Note This needed because the config is not read when installed by MSIX.
        // Note This does not work when installed by MSIX.
        // Note Currently implemented in both Main and Module(s).

        // TODO Tracing now works in the normal environment, but not after installation by MSIX.
        // This might be cause by writing rights in the installation folder.
        // https://www.advancedinstaller.com/msix-introduction.html#package-support-framework-aka-psf

        // TODO Can this be done again by the config file?
        static void SetUpTracing()
        {
            Trace.AutoFlush = true;

            TraceSource.Switch = new SourceSwitch("mainLevel", "Verbose");

            // Note that module trace files could not be shared with that of the executable.
            var fileListener = new TextWriterTraceListener("RCS.Modules.log", "file")
            {
                Filter = new EventTypeFilter(SourceLevels.Verbose),
                TraceOutputOptions = TraceOptions.DateTime
            };

            TraceSource.Listeners.Add(fileListener);

            var consoleListener = new ConsoleTraceListener()
            {
                Filter = new EventTypeFilter(SourceLevels.Warning),
            };

            TraceSource.Listeners.Add(consoleListener);
        }
    }
}
