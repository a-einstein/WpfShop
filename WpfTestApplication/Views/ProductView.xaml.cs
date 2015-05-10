using System.Windows;

namespace WpfTestApplication.Views
{
    public partial class ProductView : Window
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
