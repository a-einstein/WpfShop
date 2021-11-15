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
        protected Module()
        {
            SetUpTracing();

            var typeName = GetType().Name;
            var currentDirectory = Directory.GetCurrentDirectory();
            TraceSource.TraceEvent(TraceEventType.Verbose, default, $"{typeName} {nameof(currentDirectory)} = {currentDirectory}");
        }

        public abstract void RegisterTypes(IContainerRegistry containerRegistry);

        protected IRegionManager regionManager;

        private TraceSource TraceSource { get; set; }

        // Note his occurs AFTER RegisterTypes.
        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager = containerProvider.Resolve<IRegionManager>();

            // Note that for MSIX the installation directory is put inside C:\Program Files\WindowsApps.
            // The working directory no longer is the installation directory, but depending of the target platform something like C:\WINDOWS\system32.
            // It needs to be explicitly set back, otherwise even the complete path is not enough to read or write files.
            var baseDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}";
            Directory.SetCurrentDirectory(baseDirectory);
        }

        // Currently implemented in both Main and Module(s).
        // See the comment elsewehere.
        private void SetUpTracing()
        {
            var logDirectoryName = "Logs";
            // This is needed as TextWriterTraceListener does not create directories.
            Directory.CreateDirectory(logDirectoryName);

            var typeName = GetType().Name;
            var fileName = $"{logDirectoryName}\\{typeName}.log";

            TraceSource = new(typeName);

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
                new TextWriterTraceListener(fileName, $"file{typeName}")
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
