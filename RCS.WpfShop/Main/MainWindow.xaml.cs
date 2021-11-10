using Prism.Regions;
using RCS.WpfShop.Common;
using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RCS.WpfShop.Main
{
    public partial class MainWindow : Window
    {
        #region Construction.
        public MainWindow()
        {
            InitializeComponent();

            // It is more convenient to have the dimensions here instead of in the view as the window does not scale.
            Height = 900;
            Width = 900;
        }

        protected readonly IRegionManager regionManager;

        public MainWindow(IRegionManager regionManager)
            : this()
        {
            this.regionManager = regionManager;

            regionManager.RegisterViewWithRegion(Regions.MainWindowContent, typeof(MainView));
        }
        #endregion

        #region Activation.
        protected override void OnActivated(EventArgs e)
        {
            regionManager.RequestNavigate(Regions.MainWindowContent, new Uri(nameof(MainView), UriKind.Relative));

            // TODO Follow Category in Title.
            SetBinding(TitleProperty, new Binding(nameof(Title)) { Source = (regionManager.Regions[Regions.MainWindowContent].ActiveViews.FirstOrDefault() as View).ViewModel });
        }
        #endregion
    }
}