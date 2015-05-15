using System.Windows;
using WpfTestApplication.ViewModels;
using WpfTestApplication.Views;

namespace WpfTestApplication
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ShoppingCartViewModel shoppingCartViewModel = ShoppingCartViewModel.Instance;
            ShoppingCartView shoppingCartView = new ShoppingCartView() { DataContext = shoppingCartViewModel };
            shoppingCartViewModel.Refresh();

            AboutViewModel aboutViewModel = new AboutViewModel();
            AboutView aboutView = new AboutView() { DataContext = aboutViewModel };

            ProductsViewModel productsViewModel = new ProductsViewModel();
            ProductsView productsView = new ProductsView() { DataContext = productsViewModel };

            MainViewModel mainViewModel = new MainViewModel();
            MainView mainView = new MainView()
            {
                DataContext = mainViewModel,
                ShoppingCart = shoppingCartView,
                AboutView = aboutView,
                ProductsView = productsView
            };
            mainView.Initialize();

            Window mainWindow = new Window()
            {
                Height = 900,
                Width = 700,
                Content = mainView
            };

            mainWindow.Show();
        }
    }
}
