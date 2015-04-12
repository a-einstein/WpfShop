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

        private Uri actionPageSource = new Uri("/Views/ActionView.xaml", UriKind.Relative);

        public Uri ActionPageSource
        {
            get { return actionPageSource; }
            set { actionPageSource = value; }
        }
    }

    interface IMainViewModel
    {
        Uri DescriptionPageSource { get; set; }
        Uri ActionPageSource { get; set; }
    }
}
