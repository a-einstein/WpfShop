using System.Windows.Controls;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ProductsView : Page
    {
        public ProductsView()
        {
            InitializeComponent();

            // TODO Make this explicit, which means this view should  be instantiated explicitly too.
            DataContext = new ProductsViewModel();
        }
    }
}
