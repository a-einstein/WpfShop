using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Modules.Products.ViewModels;

namespace RCS.WpfShop.Modules.Products.Views
{
    public partial class CartView : View
    {
        public CartView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // Note the parameter gets injected.
        public CartView(CartViewModel shoppingCartViewModel)
            : this()
        {
            ViewModel = shoppingCartViewModel;
        }
    }
}
