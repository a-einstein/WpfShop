using Demo.Common;
using System.Windows;
using System.Windows.Controls;

namespace Demo.Views
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

            // TODO > Should become first view available.
            Navigate(infoPage, infoButton);
        }

        public View WidgetView
        {
            get { return shoppingCart.Content as View; }
            set { shoppingCart.Content = value; }
        }

        // TODO > Views should be a abstract row of configurations or controls.

        #region InfoView

        View infoView;
        Page infoPage;

        public View InfoView
        {
            get { return infoView; }
            set 
            { 
                infoView = value;
                infoPage = new Page() { Content = infoView };
            }
        }

        private void InfoButton_Checked(object sender, RoutedEventArgs e)
        {
            Navigate(infoPage, infoButton);
        }

        #endregion

        #region OverView

        View overView;
        Page overViewPage;

        public View ProductsView
        {
            get { return overView; }
            set 
            { 
                overView = value;
                overViewPage = new Page() { Content = overView };
            }
        }

        private void productsButton_Checked(object sender, RoutedEventArgs e)
        {
            Navigate(overViewPage, overViewButton);
        }

        #endregion
        
        private void Navigate(Page page, RadioButton radioButton)
        {
            radioButton.IsChecked = true;

            pageFrame.Content = page;
            pageFrame.Navigate(page);

            (page.Content as View).ViewModel.Refresh();
        }
    }
}
