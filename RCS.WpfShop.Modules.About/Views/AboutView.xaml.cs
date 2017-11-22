using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Modules.About.ViewModels;
using System.ComponentModel.Composition;

namespace RCS.WpfShop.Modules.About.Views
{
    [Export]
    // TODO This way of ordering actually does not work. Also see elsewhere.
    //[ViewSortHint("10")]
    public partial class AboutView : View
    {
        public AboutView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // To avoid this the ViewModel should be set by an explicit import again.
        // There seem to be no other options on the attribute.
        [ImportingConstructor]
        public AboutView(AboutViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }

        // TODO Put this in UserControl.
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
    }
}
