using System.Windows;
using WpfTestApplication.BaseClasses;

namespace WpfTestApplication.Windows
{
    public partial class OkWindow : Window
    {
        public OkWindow()
        {
            InitializeComponent();
        }

        public View View
        {
            get { return viewControl.Content as View; }
            set { viewControl.Content = value; }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
