using Prism.Commands;
using Prism.Regions;
using RCS.WpfShop.Common;
using RCS.WpfShop.Common.Navigation;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Common.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Construction
        protected readonly IRegionManager regionManager;

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
        #endregion

        #region INavigationAware
        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // Use navigation to create an event for initialization.
            return true;
        }
        #endregion

        #region Refresh
        private bool initialized;

        protected override async Task<bool> Initialize()
        {
            var baseInitialized = await base.Initialize();

            if (baseInitialized && !initialized)
            {
                var viewObjects = regionManager.Regions[Regions.MainViewMain].Views;

                foreach (var viewObject in viewObjects)
                {
                    var view = (View)viewObject;

                    // Cannot use the actual Views in the list because of resulting errors in the visual tree.
                    var destination = new Destination() { DisplayName = view.Name, Uri = new Uri(view.GetType().Name, UriKind.Relative) };

                    MainViews.Add(destination);
                }

                initialized = true;
            }

            return initialized;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            NavigateCommand = new DelegateCommand<Destination>(Navigate);
        }
        #endregion

        #region Navigation
        public static readonly DependencyProperty MainViewsProperty =
            DependencyProperty.Register(nameof(MainViews), typeof(ObservableCollection<Destination>), typeof(MainViewModel), new PropertyMetadata(new ObservableCollection<Destination>()));

        public ObservableCollection<Destination> MainViews
        {
            get => (ObservableCollection<Destination>)GetValue(MainViewsProperty);
            set => SetValue(MainViewsProperty, value);
        }

        public static readonly DependencyProperty NavigateCommandProperty =
            DependencyProperty.Register(nameof(NavigateCommand), typeof(ICommand), typeof(MainViewModel));

        public ICommand NavigateCommand
        {
            get => (ICommand)GetValue(NavigateCommandProperty);
            set => SetValue(NavigateCommandProperty, value);
        }

        private void Navigate(Destination destination)
        {
            regionManager.RequestNavigate(Regions.MainViewMain, destination.Uri);
        }
        #endregion
    }
}
