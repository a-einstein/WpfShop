using System.Windows.Controls;

namespace WpfTestApplication.BaseClasses
{
    public class View : UserControl
    {
        public ViewModel ViewModel
        {
            get { return DataContext as ViewModel; }
            set { DataContext = value; }
        }
    }
}
