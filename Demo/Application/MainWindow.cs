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

            // TODO This might be made dependent of navigation, so following the mainView.Model.
            SetBinding(Window.TitleProperty, new Binding("MainTitle") { Source = this });
        }

        // TODO > Maybe combine this view into this window.
        [Import]
        public MainView MainView;

        public void OnImportsSatisfied()
        {
            Content = MainView;
        }
    }
}
