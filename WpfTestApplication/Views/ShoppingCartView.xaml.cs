using System.Windows.Controls;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ShoppingCartView : UserControl
    {
        public ShoppingCartView()
        {
            InitializeComponent();

            DataContext = ShoppingCartViewModel.Instance;
        }
    }
}
