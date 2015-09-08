using Microsoft.Practices.Prism.Commands;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Interfaces;
using WpfTestApplication.Model;
using WpfTestApplication.Views;
using WpfTestApplication.Windows;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : FilterItemsViewModel, IShopper
    {
        public override string MasterFilterSelectedValuePath { get { return "ProductCategoryID"; } }
        public override string DetailFilterSelectedValuePath { get { return "ProductSubcategoryID"; } }
        public override string DetailFilterMasterKeyPath { get { return "ProductCategoryID"; } }

        // TODO This should become parameterized, currently it assumes retrieval of an entire table.
        public override void Refresh()
        {
            ShoppingWrapper.Instance.BeginGetProducts(new RunWorkerCompletedEventHandler(GetItemsCompleted));
        }

        protected override void SetFilters()
        {
            ShoppingWrapper.Instance.BeginGetProductCategories(GetCategoriesCompleted);
        }

        protected void GetCategoriesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception(databaseErrorMessage, e.Error);
            else
            {
                MasterFilterItems = (e.Result as DataTable).DefaultView;

                // Note that Categories have been retrieved first.
                ShoppingWrapper.Instance.BeginGetProductSubcategories(GetSubcategoriesCompleted);
            }
        }

        protected void GetSubcategoriesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception(databaseErrorMessage, e.Error);
            else
            {
                DetailFilterItems = (e.Result as DataTable).DefaultView;

                // Note that both Categories and Subcategories have been retrieved.
                // Note that MasterFilterValue also determines DetailFilterItems.
                MasterFilterValue = NoId;
                DetailFilterValue = NoId;
            }
        }

        protected void GetFiltervaluesCompleted()
        {
            if (MasterFilterItems != null && DetailFilterItems != null)
            {
            }
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<object>(CartProduct);
        }

        protected override void ShowDetails(object parameter)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            View productView = new ProductView() { ViewModel = productViewModel };

            OkWindow productWindow = new OkWindow() { View = productView, };
            // TODO Make the type of the Item and Items explicit for this model by a parameter?
            productWindow.SetBinding(Window.TitleProperty, new Binding("Item.Name") { Source = productViewModel });
            productWindow.Show();

            productViewModel.Refresh((int)parameter);
        }

        public ICommand CartCommand { get; set; }

        private void CartProduct(object parameter)
        {
            ShoppingCartViewModel.Instance.CartProduct((int)parameter);
        }

        public override object NoId { get { return ShoppingWrapper.Instance.NoId; } }
    }
}
