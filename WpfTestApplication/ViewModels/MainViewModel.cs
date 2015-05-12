using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Views;

namespace WpfTestApplication.ViewModels
{
    class MainViewModel : ViewModel, IMainViewModel
    {
        // TODO Does this belong here?
        public FrameworkElement AboutView { get { return new AboutView(); } }
        public ViewModel AboutViewModel { get { return new AboutViewModel(); } }

        // TODO Does this belong here?
        public FrameworkElement ProductsView { get { return new ProductsView(); } }
        public ViewModel ProductsViewModel { get { return new ProductsViewModel(); } }
    }

    interface IMainViewModel
    {
        FrameworkElement AboutView { get; }
        ViewModel AboutViewModel { get; }

        FrameworkElement ProductsView { get; }
        ViewModel ProductsViewModel { get; }
    }
}
