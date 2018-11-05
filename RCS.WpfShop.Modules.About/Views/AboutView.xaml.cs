using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Modules.About.ViewModels;
using RCS.WpfShop.Resources;

namespace RCS.WpfShop.Modules.About.Views
{
    public partial class AboutView : View
    {
        public AboutView()
        {
            Name = Labels.NavigateAbout;
            InitializeComponent();
        }

        // Note this couples to a specific class.
        public AboutView(AboutViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }
    }
}
