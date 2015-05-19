using WpfTestApplication.BaseClasses;

namespace WpfTestApplication.ViewModels
{
    class AboutViewModel : ViewModel
    {
        private string text = "This project is built by Robert Stroethoff for demonstration of various technical aspects in WPF.\n\nVery briefly said it contains the following.\n- The MVVM pattern.\n- Data manipulation based on a DataSet, asynchronously working on a database.\n- Xaml binding, converting, updating, layout, and styling.";

        public string Text
        {
            get { return text; }
        }
    }
}
