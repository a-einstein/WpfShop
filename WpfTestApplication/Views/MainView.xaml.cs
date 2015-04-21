using System.Windows;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = (DataContext as IMainViewModel).AboutPageSource;
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = (DataContext as IMainViewModel).ProductsPageSource;
        }
    }
}
