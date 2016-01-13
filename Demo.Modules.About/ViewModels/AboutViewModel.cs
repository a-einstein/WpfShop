using Demo.Common;
using System.ComponentModel.Composition;

namespace Demo.Modules.About.ViewModels
{
    [Export("InfoViewModel", typeof(ViewModel))]
    public class AboutViewModel : ViewModel
    {
    }
}
