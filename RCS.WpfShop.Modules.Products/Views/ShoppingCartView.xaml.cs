using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Modules.Products.ViewModels;

namespace RCS.WpfShop.Modules.Products.Views
{
    public partial class ShoppingCartView : View
    {
        public ShoppingCartView()
        {
            InitializeComponent();
        }

        // Note this couples to a specific class.
        // Note the parameter gets injected.
        public ShoppingCartView(ShoppingCartViewModel shoppingCartViewModel)
            : this()
        {
            ViewModel = shoppingCartViewModel;
        }
    }
}
