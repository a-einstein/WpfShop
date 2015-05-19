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
            ShoppingCartView shoppingCartView = new ShoppingCartView() { ViewModel = shoppingCartViewModel };

            AboutViewModel aboutViewModel = new AboutViewModel();
            AboutView aboutView = new AboutView() { ViewModel = aboutViewModel };

            ProductsViewModel productsViewModel = new ProductsViewModel();
            ProductsView productsView = new ProductsView() { ViewModel = productsViewModel };

            MainViewModel mainViewModel = new MainViewModel();
            MainView mainView = new MainView()
            {
                ViewModel = mainViewModel,
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
