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
            PageFrame.Source = new Uri("/Pages/DescriptionPage.xaml", UriKind.Relative);
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Source = new Uri("/Pages/ActionPage.xaml", UriKind.Relative);
        }
    }
}
