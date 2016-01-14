using Demo.Common;
using Demo.Resources;
using Demo.ViewModels;
using Demo.Views;
using Prism.Modularity;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

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

        public static readonly DependencyProperty MainTitleProperty =
            DependencyProperty.Register("MainTitle", typeof(string), typeof(MainWindow), new PropertyMetadata(Labels.MainTitle));

        public string MainTitle
        {
            get { return (string)GetValue(MainTitleProperty); }
            set { SetValue(MainTitleProperty, value); }
        }

        private void Initialize()
        {
            // It is more convenient to have the dimensions here instead of the view as the window does not scale.
            Height = 900;
            Width = 900;
 
            // TODO > This might be made dependent of navigation, so following the mainView.Model.
            SetBinding(Window.TitleProperty, new Binding("MainTitle") { Source = this });
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
