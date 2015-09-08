using System.Windows.Input;

namespace Demo.Interfaces
{
    interface IShopper
    {
        ICommand CartCommand { get; set; }
    }
}
