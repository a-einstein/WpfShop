using Prism.Modularity;
using RCS.WpfShop.Common;
using RCS.WpfShop.Resources;
using RCS.WpfShop.ViewModels;
using RCS.WpfShop.Views;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RCS.WpfShop.Main
{
    public class MainWindow : Window, IPartImportsSatisfiedNotification
    {
        public MainWindow()
        {
            Initialize();
        }

        private void Initialize()
        {
            // It is more convenient to have the dimensions here instead of the view as the window does not scale.
            Height = 900;
            Width = 900;
        }

        [Import]
        public MainView MainView;

        public void OnImportsSatisfied()
        {
            Content = MainView;

            SetBinding(Window.TitleProperty, new Binding(nameof(Title)) { Source = MainView.ViewModel });
        }
    }
}
