using Common.DomainClasses;
using Demo.BaseClasses;
using Demo.Interfaces;
using Demo.Model;
using Demo.Views;
using Demo.Windows;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Demo.ViewModels
{
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, int, ProductCategory, ProductSubcategory>, IShopper
    {
        private Dispatcher uiDispatcher;

        public ProductsViewModel()
        {
            uiDispatcher = Dispatcher.CurrentDispatcher;
        }

        private bool filterInitialized;

        public override void Refresh()
        {
            Items.Clear();

            if (!filterInitialized)
            {
                Task.Run(async () => await InitializeFilters()).
                ContinueWith((previous) => ReadFiltered());
            }
            else
            {
                ReadFiltered();
            }
        }

        // TODO > This would better be handled inside the repository.
        protected override async Task InitializeFilters()
        {
            await Task.WhenAll
            (
                ProductCategoriesRepository.Instance.ReadList(),
                ProductSubcategoriesRepository.Instance.ReadList()
            ).ContinueWith((previous) =>
            {
                // Need to update on the UI thread.
                uiDispatcher.Invoke(delegate
                {
                    foreach (var item in ProductCategoriesRepository.Instance.List)
                    {
                        MasterFilterItems.Add(item);
                    }

                    foreach (var item in ProductSubcategoriesRepository.Instance.List)
                    {
                        detailFilterItemsSource.Add(item);
                    }

                    int masterDefaultId = 1;
                    MasterFilterValue = MasterFilterItems.FirstOrDefault(category => category.Id == masterDefaultId);

                    // Note that MasterFilterValue also determines DetailFilterItems.
                    int detailDefaultId = 1;
                    DetailFilterValue = DetailFilterItems.FirstOrDefault(subcategory => subcategory.Id == detailDefaultId);

                    TextFilterValue = default(string);

                    filterInitialized = true;
                });
            });
        }

        protected void ReadFiltered()
        {
            ProductCategory masterFilterValue = null;
            ProductSubcategory detailFilterValue = null;
            string textFilterValue = null;

            // Need to get these from the UI thread.
            uiDispatcher.Invoke(delegate
            {
                masterFilterValue = MasterFilterValue;
                detailFilterValue = DetailFilterValue;
                textFilterValue = TextFilterValue;
            });

            Task<IList<ProductsOverviewObject>>.Run(() =>
            {
                return ProductsRepository.Instance.ReadList(masterFilterValue, detailFilterValue, textFilterValue).Result;
            })
            .ContinueWith((previous) =>
            {
                // Need to update on the UI thread.
                uiDispatcher.Invoke(delegate
                {
                    foreach (var item in previous.Result)
                    {
                        Items.Add(item);
                    }

                    RaisePropertyChanged("ItemsCount");
                });
            });
        }

        protected override Func<ProductSubcategory, bool> DetailItemsSelector(bool preserveEmptyElement = true)
        {
            // TODO Value should be null value when there is no selection.
            // TODO An empty list should disable the selector. Currently it shows a blank list of apparently a default length 4.
            return subcategory => /*(MasterFilterValue != null) && (MasterFilterValue.Id != NoId) &&*/ ((preserveEmptyElement && subcategory.Id == NoId) || (subcategory.ProductCategoryId == MasterFilterValue.Id));
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<ProductsOverviewObject>(CartProduct);
        }

        protected override void ShowDetails(ProductsOverviewObject productsOverviewObject)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            View productView = new ProductView() { ViewModel = productViewModel };

            OkWindow productWindow = new OkWindow() { View = productView, };
            productWindow.SetBinding(Window.TitleProperty, new Binding("Item.Name") { Source = productViewModel });
            productWindow.Show();

            productViewModel.Refresh(productsOverviewObject.Id);
        }

        public ICommand CartCommand { get; set; }

        private void CartProduct(ProductsOverviewObject productsOverviewObject)
        {
            ShoppingCartViewModel.Instance.CartProduct(productsOverviewObject);
        }

        public override int NoId { get { return ProductsRepository.Instance.NoId; } }

        public override string TextFilterLabel { get { return ProductsRepository.Instance.TextFilterDescription; } }
    }
}
