using System.Windows;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();

            DataContext = new ProductViewModel();
        }
    }
}
