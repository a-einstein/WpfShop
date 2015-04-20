using System.Windows;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();

            // Crude MVVM implementation.
            DataContext = new ProductViewModel();
        }
    }
}
