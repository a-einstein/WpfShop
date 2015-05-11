using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.ViewModels;

namespace WpfTestApplication.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            // TODO Make this explicit, which means this view should  be instantiated explicitly too.
            // Maybe get rid of the ViewModel. Check out examples of navigation, MVVM, ...
            DataContext = new MainViewModel();

            // TODO This may not be needed anymore once yhe asynchronous retrieval works.
            pageFrame.NavigationService.LoadCompleted += FrameNavigationService_LoadCompleted;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            pageFrame.Source = (DataContext as IMainViewModel).AboutPageSource;
        }

        private IMainViewModel mainViewModel;

        private FrameworkElement pageFrameView;
        private ViewModel pageFrameViewModel;

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            //pageFrame.Source = (DataContext as IMainViewModel).ProductsPageSource;

            mainViewModel = (DataContext as IMainViewModel);

            pageFrameView = mainViewModel.ProductsView;

            // TODO This may not be needed anymore once yhe asynchronous retrieval works.
            pageFrameView.Loaded += FrameView_Loaded;

            pageFrame.Content = pageFrameView;
            //pageFrame.Navigate(pageFrameView);

            pageFrameViewModel = mainViewModel.ProductsViewModel;
            pageFrameView.DataContext = pageFrameViewModel;
        }

        void FrameView_Loaded(object sender, RoutedEventArgs e)
        {
            // This does not work. Apparently is is not FULLY loaded.
            // A synschronous Refresh seems to put it on hold.
            ItemsViewModel itemsViewModel = (sender as FrameworkElement).DataContext as ItemsViewModel;

            if (itemsViewModel != null)
            {
                //itemsViewModel.Refresh();
            }

            // This freezes the entire UI for Products!
            //pageFrameViewModel.Refresh();
        }

        void FrameNavigationService_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // This does not work. Apparently is is not FULLY completed.
            // A synschronous Refresh seems to put it on hold.
            ItemsViewModel itemsViewModel = (pageFrame.Content as FrameworkElement).DataContext as ItemsViewModel;
            
            if (itemsViewModel != null)
            {
                //itemsViewModel.Refresh();
            }

            // This freezes the entire UI for Products!
            //pageFrameViewModel.Refresh();
        }
    }
}
