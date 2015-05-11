using System;
using System.Windows;
using System.Windows.Media;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Views;

namespace WpfTestApplication.ViewModels
{
    class MainViewModel : ViewModel, IMainViewModel
    {
        private Uri aboutPageSource = new Uri("/Views/AboutView.xaml", UriKind.Relative);

        // TODO Does this belong here?
        public Uri AboutPageSource
        {
            get { return aboutPageSource; }
            set { aboutPageSource = value; }
        }

        private Uri productsPageSource = new Uri("/Views/ProductsView.xaml", UriKind.Relative);

        // TODO Does this belong here?
        public Uri ProductsPageSource
        {
            get
            {
                return productsPageSource;
            }
            set { productsPageSource = value; }
        }

        public FrameworkElement ProductsView { get { return new ProductsView(); } }
        public ViewModel ProductsViewModel { get { return new ProductsViewModel(); } }
    }

    interface IMainViewModel
    {
        Uri AboutPageSource { get; set; }
        Uri ProductsPageSource { get; set; }

        //FrameworkElement AboutFrameContent { get; set; }
        //ViewModel AboutViewModel { get; }

        FrameworkElement ProductsView { get; }
        ViewModel ProductsViewModel { get; }
    }
}
