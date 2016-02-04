using Demo.Common;
using Demo.Modules.Products.ViewModels;
using System.ComponentModel.Composition;

namespace Demo.Modules.Products.Views
{
    [Export]
    // TODO This way of ordering actually does not work. Also see elsewhere.
    //[ViewSortHint("20")]
    public partial class ProductsView : View
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // To avoid this the ViewModel should be set by an explicit import again.
        // There seem to be no other options on the attribute.
        [ImportingConstructor]
        public ProductsView(ProductsViewModel viewModel)
            : this()
        {
            ViewModel = viewModel;
        }
    }
}
