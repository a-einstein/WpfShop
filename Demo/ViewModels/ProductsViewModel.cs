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

namespace Demo.ViewModels
{
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, int, ProductCategory, ProductSubcategory>, IShopper
    {
        public override string DetailFilterMasterKeyPath { get { return "ProductCategoryID"; } }

        private static object productsLock = new object();
        private static object masterFilterItemsLock = new object();
        private static object detailFilterItemsLock = new object();

        public ProductsViewModel()
        {
            // TODO The other way around, pass a collection from here?
            // No, as long as ProductsRepository and CartItemsRepository remain a coherent set of singletons, or a least coherent sets.
            Items = ProductsRepository.Instance.List;
            BindingOperations.EnableCollectionSynchronization(Items, productsLock);

            MasterFilterItems = ProductCategoriesRepository.Instance.List;
            BindingOperations.EnableCollectionSynchronization(MasterFilterItems, masterFilterItemsLock);

            detailFilterItemsSource = ProductSubcategoriesRepository.Instance.List;
            BindingOperations.EnableCollectionSynchronization(detailFilterItemsSource, detailFilterItemsLock);
        }

        public override async void Refresh()
        {
            // TODO >> Assure that InitializeFilters has terminated.

            // Note this implicitly works on Items.
            // TODO This is not very clear.

            // TODO Check for errors.
            await ProductsRepository.Instance.ReadList(MasterFilterValue, DetailFilterValue, TextFilterValue);

            RaisePropertyChanged("ItemsCount");
        }

        // TODO This may be generalized.
        protected override async void InitializeFilters()
        {
            var getCategoriesTask = ProductCategoriesRepository.Instance.ReadList();
            var getSubcategoriesTask = ProductSubcategoriesRepository.Instance.ReadList();

            await Task.WhenAll(getCategoriesTask, getSubcategoriesTask);

            int masterDefaultId = 1;
            MasterFilterValue = MasterFilterItems.FirstOrDefault(category => category.Id == masterDefaultId);

            // Note that MasterFilterValue also determines DetailFilterItems.
            int detailDefaultId = 1;
            DetailFilterValue = DetailFilterItems.FirstOrDefault(subcategory => subcategory.Id == detailDefaultId);

            TextFilterValue = default(string);
        }

        protected override Func<ProductSubcategory, bool> DetailItemsSelector(bool preserveEmptyElement = true)
        {
            return subcategory => (preserveEmptyElement && subcategory.Id == NoId) || (subcategory.ProductCategoryID == MasterFilterValue.Id);
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
