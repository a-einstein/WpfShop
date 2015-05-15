using WpfTestApplication.BaseClasses;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ShoppingCartView : View
    {
        public ShoppingCartView()
        {
            InitializeComponent();
        }

        private void QuantityUpDown_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            // Note that currently a ShoppingCartViewModel is assumed, no interface necessary.
            // TODO Check this.
            ShoppingCartViewModel viewModel = DataContext as ShoppingCartViewModel;

            if (viewModel != null)
                viewModel.OnItemChanged();

            e.Handled = true;
        }
    }
}
