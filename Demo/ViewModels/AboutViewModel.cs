using Demo.BaseClasses;

namespace Demo.ViewModels
{
    class AboutViewModel : ViewModel
    {
        private string text = "This project is built by Robert Stroethoff for demonstration of various technical aspects in WPF.\n\nVery briefly said it contains the following.\n- The MVVM pattern.\n- Asynchronous client-server connection to a database.\n- Xaml binding, converting, updating, layout, styling, triggering.\n- Unit testing.\n";

        public string Text
        {
            get { return text; }
        }
    }
}
