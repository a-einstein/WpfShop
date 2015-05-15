using System.Windows;
using System.Windows.Controls;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class MainView : Window
    {
        private IMainViewModel mainViewModel;

        public MainView()
        {
            InitializeComponent();

            // TODO Make this explicit, which means this view should  be instantiated explicitly too.
            // TODO Maybe create a property ViewModel.
            // TODO Maybe get rid of the ViewModel. Check out examples of navigation, MVVM, ...
            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;

            ShoppingCartViewModel shoppingCartViewModel = ShoppingCartViewModel.Instance;
            shoppingCartView.DataContext = shoppingCartViewModel;
            shoppingCartViewModel.Refresh();

            Navigate(mainViewModel.AboutView, mainViewModel.AboutViewModel, aboutButton);
        }

        private void aboutButton_Checked(object sender, RoutedEventArgs e)
        {
            Navigate(mainViewModel.AboutView, mainViewModel.AboutViewModel, aboutButton);
        }

        private void productsButton_Checked(object sender, RoutedEventArgs e)
        {
            Navigate(mainViewModel.ProductsView, mainViewModel.ProductsViewModel, productsButton);
        }

        private void Navigate(FrameworkElement pageFrameView, ViewModel pageFrameViewModel, RadioButton radioButton)
        {
            radioButton.IsChecked = true;

            pageFrame.Content = pageFrameView;
            pageFrame.Navigate(pageFrameView);

            pageFrameView.DataContext = pageFrameViewModel;
            pageFrameViewModel.Refresh();
        }
    }
}
