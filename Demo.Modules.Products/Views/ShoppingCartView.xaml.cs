using Demo.Common;
using System.ComponentModel.Composition;

namespace Demo.Modules.Products.Views
{
    [Export("WidgetView", typeof(View))]
    public partial class ShoppingCartView : View
    {
        public ShoppingCartView()
        {
            InitializeComponent();
        }
    }
}
