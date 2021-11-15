using Prism.Regions;
using RCS.WpfShop.Resources;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RCS.WpfShop.Common.ViewModels
{
    // TODO Use BindableBase? Does not seem to offer advantages currently.
    public abstract class ViewModel : DependencyObject, INotifyPropertyChanged, INavigationAware
    {
        #region Construction
        protected virtual void SetCommands() { }
        #endregion

        #region INavigationAware
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Currently synchronous because of apparent threading problem with asynchronous attempts,
            // even when using uiDispatcher as elsewhere.
            // Note INavigationAware.OnNavigatedTo is synchronous. 
            RefreshView();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        { }
        #endregion

        #region Refresh
        public virtual async Task RefreshView()
        {
            if (await Initialize())
            {
                ClearView();

                await Read();
            }
        }

        protected virtual void ClearView() { }

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

        protected virtual async Task Read() { }

        // Note Did not succeed to set default by static member.
        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register(nameof(Title), typeof(string), typeof(ViewModel), new PropertyMetadata(Labels.ShopName));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // TODO Possibly have an UpdateTitle like in PortableShop. Eiher for the window title or a text in the view.
        protected virtual string MakeTitle() { return Labels.ShopName; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Note OnPropertyChanged can't be used, as it differs from Xamarin.Forms.BindableObject. 
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}