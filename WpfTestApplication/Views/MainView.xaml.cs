using System.Windows;
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

            pageFrameView = mainViewModel.AboutView;
            pageFrameViewModel = mainViewModel.AboutViewModel;

            Navigate();
        }

        private FrameworkElement pageFrameView;
        private ViewModel pageFrameViewModel;

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrameView = mainViewModel.AboutView;
            pageFrameViewModel = mainViewModel.AboutViewModel;

            Navigate();
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrameView = mainViewModel.ProductsView;
            pageFrameViewModel = mainViewModel.ProductsViewModel;

            Navigate();
        }

        private void Navigate()
        {
            pageFrame.Content = pageFrameView;
            pageFrame.Navigate(pageFrameView);

            pageFrameView.DataContext = pageFrameViewModel;
            pageFrameViewModel.Refresh();
        }
    }
}
