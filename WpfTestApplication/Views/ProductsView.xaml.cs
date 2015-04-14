using System.Windows.Controls;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ProductsView : Page
    {
        public ProductsView()
        {
            InitializeComponent();

            // Crude MVVM implementation.
            DataContext = new ProductsViewModel();
        }
    }
}
