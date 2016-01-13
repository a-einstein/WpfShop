using Demo.Common;
using Demo.ViewModels;
using Demo.Views;
using Prism.Modularity;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Data;

namespace Demo
{
    public class MainWindow : Window, IPartImportsSatisfiedNotification
    {
        // Import any discovered component satisfying the combination of contractName and contractType.
        // For each import there probably should only be one made available in all supplied modules and elsewhere.

        [Import("WidgetView", typeof(View))]
        public View WidgetView;

        [Import("WidgetViewModel", typeof(ViewModel), RequiredCreationPolicy = CreationPolicy.Shared)]
        public ViewModel WidgetViewModel;

        [Import("InfoView", typeof(View))]
        public View InfoView;

        [Import("InfoViewModel", typeof(ViewModel))]
        public ViewModel InfoViewModel;

        [Import("OverView", typeof(View))]
        public View OverView;

        [Import("OverViewModel", typeof(ViewModel))]
        public ViewModel OverViewModel;

        public MainWindow()
        {
            Initialize();
        }

        public string DemoTitle
        {
            // TODO Use resources.
            // TODO Check elsewehere too.
            get { return "WPF Demo"; }
        }

        private void Initialize()
        {
            // TODO This could be put into the View?
            Height = 900;
            Width = 900;

            // TODO > This might be made dependent of navigation, so following the mainView.Model.
            SetBinding(Window.TitleProperty, new Binding("DemoTitle") { Source = this });
        }

        public void OnImportsSatisfied()
        {
            WidgetView.ViewModel = WidgetViewModel;
            InfoView.ViewModel = InfoViewModel;
            OverView.ViewModel = OverViewModel;

            MainViewModel mainViewModel = new MainViewModel();

            // TODO > Use regions instead of a properties?
            MainView mainView = new MainView()
            {
                ViewModel = mainViewModel,

                WidgetView = WidgetView,
                InfoView = InfoView,
                ProductsView = OverView
            };

            Content = mainView;
        }
    }
}
