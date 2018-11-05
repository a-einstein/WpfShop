using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Modules.Products.ViewModels;
using Unity;

namespace RCS.WpfShop.Modules.Products.Views
{
    public partial class ShoppingCartView : View
    {
        public ShoppingCartView()
        {
            InitializeComponent();
        }

        public ShoppingCartView(IUnityContainer container)
            : this()
        {
            ViewModel = container.Resolve<ShoppingCartViewModel>(nameof(ShoppingCartViewModel));
        }
    }
}
