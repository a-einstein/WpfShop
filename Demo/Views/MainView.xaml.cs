using Demo.Common;
using Demo.Common.Regions;
using Demo.ViewModels;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace Demo.Views
{
    // Note that apparently this class needs to be exported to get its imports right.
    [Export]
    public partial class MainView : View, IPartImportsSatisfiedNotification
    {
        [Import(AllowRecomposition = false)]
        public IRegionManager RegionManager;

        public MainView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // To avoid this the ViewModel should be set by an explicit import again.
        // There seem to be no other options on the attribute.
        [ImportingConstructor]
        public MainView(MainViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }

        public void OnImportsSatisfied()
        {
            // TODO This way of ordering actually does not work. Also see elsewhere.
            // The first view to be shown depends on the registering order, which depends on loading order of modules, 
            // which currrently is by alphabetic order in the modules directory, which currently gives the about module as first.

            //RegionManager.RequestNavigate(RegionNames.MainRegion, infoViewUri);
        }

        #region WidgetView

        private static Uri widgetViewUri = new Uri("ShoppingCartView", UriKind.Relative);

        #endregion

        // TODO These views should be an abstract row of configurations or controls which could be discovered or configured externally.

        #region InfoView

        private static Uri infoViewUri = new Uri("AboutView", UriKind.Relative);

        private void InfoButton_Checked(object sender, RoutedEventArgs e)
        {
            RegionManager.RequestNavigate(RegionNames.MainRegion, infoViewUri);
        }

        #endregion

        #region OverView

        private static Uri overViewUri = new Uri("ProductsView", UriKind.Relative);

        private void OverviewButton_Checked(object sender, RoutedEventArgs e)
        {
            RegionManager.RequestNavigate(RegionNames.MainRegion, overViewUri);
        }

        #endregion
    }
}
