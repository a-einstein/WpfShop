using System;
using System.Windows;

namespace WpfTestApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Source = new Uri("/Views/DescriptionView.xaml", UriKind.Relative);
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Source = new Uri("/Views/ActionView.xaml", UriKind.Relative);
        }
    }
}
