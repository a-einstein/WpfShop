using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ViewModel : DependencyObject, INotifyPropertyChanged
    {
        #region Construction
        public ViewModel()
        {
            SetCommands();
        }

        protected virtual void SetCommands() { }
        #endregion

        #region Refresh
        public virtual async Task Refresh() { }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        // This signal can be particularly useful if a collection is entirely replaced, as the formerly bound collection no longer can.
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}