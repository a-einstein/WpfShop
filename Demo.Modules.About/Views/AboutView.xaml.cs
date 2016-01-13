using Demo.Common;
using System.ComponentModel.Composition;

namespace Demo.Modules.About.Views
{
    [Export("InfoView", typeof(View))]
    public partial class AboutView : View
    {
        public AboutView()
        {
            InitializeComponent();
        }
    }
}
