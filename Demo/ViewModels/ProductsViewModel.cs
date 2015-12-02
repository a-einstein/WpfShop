using Common.DomainClasses;
using Demo.BaseClasses;
using Demo.Interfaces;
using Demo.Model;
using Demo.Views;
using Demo.Windows;
using Microsoft.Practices.Prism.Commands;
using System;
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
            Task.Run(async () =>
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

                await ProductsRepository.Instance.ReadList(masterFilterValue, detailFilterValue, textFilterValue);
            })
            .ContinueWith((previous) =>
            {
                // Need to update on the UI thread.
                uiDispatcher.Invoke(delegate
                {
                    foreach (var item in ProductsRepository.Instance.List)
                    {
                        Items.Add(item);
                    }

                    RaisePropertyChanged("ItemsCount");
                });
            });
        }

        protected override Func<ProductSubcategory, bool> DetailItemsSelector(bool preserveEmptyElement = true)
        {
            return subcategory => (preserveEmptyElement && subcategory.Id == NoId) || (MasterFilterValue != null && subcategory.ProductCategoryID == MasterFilterValue.Id);
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
    }
}
