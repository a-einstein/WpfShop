using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Interfaces;
using WpfTestApplication.Model;
using WpfTestApplication.Views;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : FilterItemsViewModel, IShopper
    {

        public ProductsViewModel()
        {
            ShoppingWrapper.Instance.PropertyChanged += ShoppingWrapper_PropertyChanged;

            itemsCollectionViewSource.Source = ShoppingWrapper.Instance.ProductsCollection;
            ShoppingWrapper.Instance.ProductsCollection.CollectionChanged += ProductsCollection_CollectionChanged;

            ItemsDataView = itemsCollectionViewSource.View;
        }


        void ProductsCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // This gets reached. Now itemsCollectionViewSource.View should reflect itemsCollectionViewSource.Source!
        }

        // Trying to force changes in View. Data is received!
        void ShoppingWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Products")
            {
                // TODO >> Does not work yet. Use ExecuteOnUiThread? Use RunWorkerCompletedEventArgs.Result? Does Items raise an event? 
                //Items = ShoppingWrapper.Instance.Products.DefaultView;
                //Items = ShoppingWrapper.Instance.ProductsHack.DefaultView;

                // Signal.
                //RaisePropertyChanged("Items");
                //ExecuteOnUiThread(() => RaisePropertyChanged("Items"));

                ItemsDataView.Refresh();
                ExecuteOnUiThread(() => ItemsDataView.Refresh());

                RaisePropertyChanged("ItemsDataView");
                ExecuteOnUiThread(() => RaisePropertyChanged("ItemsDataView"));

                // Get already retrieved data.
                //Refresh();
                //ExecuteOnUiThread(() => Refresh());

                RaisePropertyChanged("ItemsCount");
            }
        }

        public static void ExecuteOnUiThread(Action action)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;

            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(action);
            }
        }

        public override string MasterFilterSelectedValuePath { get { return "ProductCategoryID"; } }
        public override string DetailFilterSelectedValuePath { get { return "ProductSubcategoryID"; } }
        public override string DetailFilterMasterKeyPath { get { return "ProductCategoryID"; } }

        public override DataView GetData()
        {
            // TODO reduce loading time e.g. by making it asynchronous.
            return ShoppingWrapper.Instance.Products.DefaultView;
        }

        protected override void SetFilters()
        {
            MasterFilterItems = ShoppingWrapper.Instance.ProductCategories;
            DetailFilterItems = ShoppingWrapper.Instance.ProductSubcategories;

            // Note that MasterFilterValue also determines DetailFilterItems.
            MasterFilterValue = -1;
            DetailFilterValue = -1;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<object>(CartProduct);
        }

        protected override void ShowDetails(object parameter)
        {
            // TODO Generally distinguish between Windows, Pages and Views. 
            // Like
            // - Windows: Main, Details.
            // - Pages: About, Shopping.
            // - Views: Products, Product, ShoppingCart.

            Window productView = new ProductView();
            productView.Show();

            // Make GUI more responsive by opening first, then get the data.
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.ItemId = (int)parameter;
            productView.DataContext = productViewModel;
        }

        public ICommand CartCommand { get; set; }

        private void CartProduct(object parameter)
        {
            ShoppingCartViewModel.Instance.Increase((int)parameter);
        }
    }
}
