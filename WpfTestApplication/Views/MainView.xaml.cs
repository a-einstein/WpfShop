using System.Windows;
using System.Windows.Controls;
using WpfTestApplication.BaseClasses;

namespace WpfTestApplication.Views
{
    public partial class MainView : View
    {
        public MainView()
        {
            InitializeComponent();
        }

        protected override void View_Loaded(object sender, RoutedEventArgs e)
        {
            base.View_Loaded(sender, e);

            Navigate(aboutPage, aboutButton);
        }

        public View ShoppingCart
        {
            get { return shoppingCart.Content as View; }
            set { shoppingCart.Content = value; }
        }

        // TODO Should be a row of configurations or controls. Navigation is to be worked out priorly.

        View aboutView;
        Page aboutPage;

        public View AboutView
        {
            get { return aboutView; }
            set 
            { 
                aboutView = value;
                aboutPage = new Page() { Content = aboutView };
            }
        }

        private void aboutButton_Checked(object sender, RoutedEventArgs e)
        {
            Navigate(aboutPage, aboutButton);
        }

        View productsView;
        Page productsPage;

        public View ProductsView
        {
            get { return productsView; }
            set 
            { 
                productsView = value;
                productsPage = new Page() { Content = productsView };
            }
        }

        private void productsButton_Checked(object sender, RoutedEventArgs e)
        {
            Navigate(productsPage, productsButton);
        }

        private void Navigate(Page page, RadioButton radioButton)
        {
            radioButton.IsChecked = true;

            pageFrame.Content = page;
            pageFrame.Navigate(page);

            (page.Content as View).ViewModel.Refresh();
        }
    }
}
