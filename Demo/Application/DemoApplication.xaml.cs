using System.Windows;
using System.Windows.Data;
using Demo.ViewModels;
using Demo.Views;

namespace Demo
{
    public partial class DemoApplication : Application
    {
        public string Title
        {
            get { return "WPF Demo"; }
        }

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

            Window applicationWindow = new Window()
            {
                Content = mainView,
                Height = 900,
                Width = 700
            };

            // This might be made dependent of navigation, so following the mainView.Model.
            applicationWindow.SetBinding(Window.TitleProperty, new Binding("Title") { Source = this });

            applicationWindow.Show();
        }
    }
}
