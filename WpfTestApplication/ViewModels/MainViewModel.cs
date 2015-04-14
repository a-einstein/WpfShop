using System;

namespace WpfTestApplication.ViewModels
{
    class MainViewModel : IMainViewModel
    {
        private Uri descriptionPageSource = new Uri("/Views/DescriptionView.xaml", UriKind.Relative);

        public Uri DescriptionPageSource
        {
            get { return descriptionPageSource; }
            set { descriptionPageSource = value; }
        }

        private Uri productsPageSource = new Uri("/Views/ProductsView.xaml", UriKind.Relative);

        public Uri ProductsPageSource
        {
            get { return productsPageSource; }
            set { productsPageSource = value; }
        }
    }

    interface IMainViewModel
    {
        Uri DescriptionPageSource { get; set; }
        Uri ProductsPageSource { get; set; }
    }
}
