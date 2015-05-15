using System.Windows;

namespace WpfTestApplication.Windows
{
    public partial class OkWindow : Window
    {
        public OkWindow()
        {
            InitializeComponent();
        }

        public FrameworkElement View
        {
            get { return viewControl.Content as FrameworkElement; }
            set { viewControl.Content = value; }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
