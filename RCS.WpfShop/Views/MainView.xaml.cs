using Prism.Regions;
using RCS.WpfShop.Common.Views;
using RCS.WpfShop.ViewModels;

namespace RCS.WpfShop.Views
{
    public partial class MainView : View
    {
        #region Construction
        public MainView()
        {
            InitializeComponent();
        }

        private IRegionManager regionManager;

        private MainView(IRegionManager regionManager)
            : this()
        {
            this.regionManager = regionManager;
        }

        public MainView(MainViewModel viewModel, IRegionManager regionManager)
            : this(regionManager)
        {
            ViewModel = viewModel;
        }
        #endregion
    }
}
