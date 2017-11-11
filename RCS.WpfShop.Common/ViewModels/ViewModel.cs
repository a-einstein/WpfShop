using RCS.WpfShop.Resources;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RCS.WpfShop.Common.ViewModels
{
    public abstract class ViewModel : DependencyObject, INotifyPropertyChanged
    {
        #region Refresh
        public virtual async Task Refresh()
        {
            Clear();

            if (await Initialize())
            {
                await Read();
            }
        }

        protected virtual void Clear() { }

        private bool initialized;
        protected Dispatcher uiDispatcher;

        protected virtual async Task<bool> Initialize()
        {
            if (!initialized)
            {
                uiDispatcher = Dispatcher.CurrentDispatcher;
                // Deliberately put here instead of in constructor to avoid problems because of overrides.
                SetCommands();

                initialized = true;
            }

            return initialized;
        }

        protected virtual void SetCommands() { }

        protected virtual async Task<bool> Read() { return true; }

        // Note Did not succeed to set default by static member.
        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register(nameof(Title), typeof(string), typeof(ViewModel), new PropertyMetadata(Labels.ShopName));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public virtual string MakeTitle() { return Labels.ShopName; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Currently no longer used.
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}