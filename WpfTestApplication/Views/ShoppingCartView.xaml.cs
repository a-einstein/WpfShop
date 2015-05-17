using WpfTestApplication.BaseClasses;

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
            (ViewModel as ItemsViewModel).OnItemChanged();

            e.Handled = true;
        }
    }
}
