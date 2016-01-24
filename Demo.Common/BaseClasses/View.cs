using System.Windows.Controls;

namespace Demo.Common
{
    public class View : UserControl
    {
        // Note this not suited for direct binding.
        public ViewModel ViewModel
        {
            get { return DataContext as ViewModel; }
            set { DataContext = value; }
        }
    }
}
