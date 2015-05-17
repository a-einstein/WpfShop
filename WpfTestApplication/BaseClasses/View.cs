using System.Windows.Controls;

namespace WpfTestApplication.BaseClasses
{
    public class View : UserControl
    {
        public View()
        {
            Loaded += View_Loaded;
        }

        protected virtual void View_Loaded(object sender, System.Windows.RoutedEventArgs e) { }

        public ViewModel ViewModel
        {
            get { return DataContext as ViewModel; }
            set { DataContext = value; }
        }
    }
}
