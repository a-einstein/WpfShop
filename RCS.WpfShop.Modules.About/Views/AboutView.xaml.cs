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

            // Note for only during Debug, and just my code unset.
            // There maybe multiple exceptions about .resources files. They can be ignored.
            // Setting NeutralLanguage on all modules did not help. Neither did unchecking all exception settings.
            // https://stackoverflow.com/questions/40555206/initializecomponent-throws-exception-resources-not-found
            // https://stackoverflow.com/questions/67701884/net-5-filenotfoundexception-resources 
            // TODO Find a better solution.

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
