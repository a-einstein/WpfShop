using System.Windows.Controls;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ActionView : Page
    {
        public ActionView()
        {
            InitializeComponent();

            // Crude MVVM implementation.
            DataContext = new ActionViewModel();
        }
    }
}
