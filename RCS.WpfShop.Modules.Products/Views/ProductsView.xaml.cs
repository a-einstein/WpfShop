using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Modules.Products.ViewModels;
using RCS.WpfShop.Resources;

namespace RCS.WpfShop.Modules.Products.Views
{
    public partial class ProductsView : View
    {
        public ProductsView()
        {
            Name = Labels.NavigateShop;
            InitializeComponent();
        }

        // Note this couples to a specific class.
        public ProductsView(ProductsViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }
    }
}
