using System.Windows;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();

            DataContext = new ProductViewModel();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
