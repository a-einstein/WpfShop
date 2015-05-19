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

            Window mainWindow = new Window()
            {
                Content = mainView,
                Height = 900,
                Width = 700
            };

            mainWindow.Show();
        }
    }
}
