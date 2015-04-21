using System.Windows.Controls;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class DescriptionView : Page
    {
        public DescriptionView()
        {
            InitializeComponent();

            DataContext = new DescriptionViewModel();
        }
    }
}
