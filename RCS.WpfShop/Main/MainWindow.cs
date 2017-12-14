using RCS.WpfShop.Views;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Data;

namespace RCS.WpfShop.Main
{
    public class MainWindow : Window, IPartImportsSatisfiedNotification
    {
        #region Construction.
        public MainWindow()
        {
            Initialize();
        }

        private void Initialize()
        {
            // It is more convenient to have the dimensions here instead of in the view as the window does not scale.
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
        #endregion
    }
}