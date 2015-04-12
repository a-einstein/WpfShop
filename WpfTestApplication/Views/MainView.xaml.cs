using System.Windows;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            // Crude MVVM implementation.
            DataContext = new MainViewModel();
        }

        private void DescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = (DataContext as IMainViewModel).DescriptionPageSource;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = (DataContext as IMainViewModel).ActionPageSource;
        }
    }
}
