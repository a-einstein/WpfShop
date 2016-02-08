using System.Windows.Controls;

namespace Demo.Common
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
