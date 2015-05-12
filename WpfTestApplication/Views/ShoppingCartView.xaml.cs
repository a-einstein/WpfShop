using System.Windows.Controls;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ShoppingCartView : UserControl
    {
        public ShoppingCartView()
        {
            InitializeComponent();
        }

        private void QuantityUpDown_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            // Note that currently a ShoppingCartViewModel is assumed, no interface necessary.
            ShoppingCartViewModel viewModel = DataContext as ShoppingCartViewModel;

            if (viewModel != null)
                viewModel.OnItemChanged();

            e.Handled = true;
        }
    }
}
