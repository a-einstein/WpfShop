using System.Windows.Input;

namespace WpfTestApplication.Interfaces
{
    interface IShopper
    {
        ICommand CartCommand { get; set; }
    }
}
