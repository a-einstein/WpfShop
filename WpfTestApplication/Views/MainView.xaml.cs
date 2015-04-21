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

        private void DescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = (DataContext as IMainViewModel).DescriptionPageSource;
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = (DataContext as IMainViewModel).ProductsPageSource;
        }
    }
}
