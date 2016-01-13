using Demo.Common;
using System.ComponentModel.Composition;

namespace Demo.Modules.Products.Views
{
    [Export("OverView", typeof(View))]
    public partial class ProductsView : View
    {
        public ProductsView()
        {
            InitializeComponent();
        }
    }
}
