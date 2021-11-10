using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.IO;

namespace RCS.WpfShop.Common.Modules
{
    public abstract class Module : IModule
    {
        // Note call order is as below.

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        { }

        public IRegionManager regionManager;

        protected static TraceSource TraceSource { get; } = new("ModulesTrace");

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager = containerProvider.Resolve<IRegionManager>();

            // Note that for MSIX the installation directory is put inside C:\Program Files\WindowsApps.
            // The working directory no longer is the installation directory, but depending of the target platform something like C:\WINDOWS\system32.
            // It needs to be explicitly set back, otherwise even the complete path is not enough to read or write files.
            var baseDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}";
            Directory.SetCurrentDirectory(baseDirectory);

            SetUpTracing();
        }

        // Currently implemented in both Main and Module(s).
        // See the comment elsewehere.
        private static void SetUpTracing()
        {
            var executableName = AppDomain.CurrentDomain.FriendlyName;

            // Needed for TextWriterTraceListener.
            Trace.AutoFlush = true;

            // For unknown reasons this didn't work for any TraceEventType with lower priority than Verbose.
            TraceSource.Switch = new SourceSwitch("mainLevel", "Verbose");

            // Note that a DefaultTraceListener is added.
            // Remove to better control the indiviudal levels.
            TraceSource.Listeners.Remove("Default");

            TraceSource.Listeners.AddRange(new TraceListener[]
            {
                // Note that module trace files could not be shared with that of the executable.
                new TextWriterTraceListener("RCS.Modules.log", "file")
                {
                    Filter = new EventTypeFilter(SourceLevels.Verbose),
                    TraceOutputOptions = TraceOptions.DateTime
                },
                new ConsoleTraceListener()
                {
                    Filter = new EventTypeFilter(SourceLevels.Warning),
                }
            });
        }
    }
}
