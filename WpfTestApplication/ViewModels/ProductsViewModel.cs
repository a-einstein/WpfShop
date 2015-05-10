using Microsoft.Practices.Prism.Commands;
using System.Data;
using System.Windows;
using System.Windows.Input;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Interfaces;
using WpfTestApplication.Model;
using WpfTestApplication.Views;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : FilterItemsViewModel, IShopper
    {
        public override string MasterFilterSelectedValuePath { get { return "ProductCategoryID"; } }
        public override string DetailFilterSelectedValuePath { get { return "ProductSubcategoryID"; } }
        public override string DetailFilterMasterKeyPath { get { return "ProductCategoryID"; } }

        protected override DataView GetData()
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
