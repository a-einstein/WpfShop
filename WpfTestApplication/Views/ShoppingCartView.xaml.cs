using System.Windows;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ShoppingCartView : Window
    {
        public ShoppingCartView()
        {
            InitializeComponent();

            DataContext = new ShoppingCartViewModel();
        }
    }
}
