using Prism.Commands;
using Prism.Regions;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Common.Windows;
using RCS.WpfShop.Modules.Products.Model;
using RCS.WpfShop.Modules.Products.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    public class ProductsViewModel : FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper
    {
        #region Construction
        ProductCategoriesRepository productCategoriesRepository;
        ShoppingCartViewModel shoppingCartViewModel;

        public ProductsViewModel(
            ProductCategoriesRepository productCategoriesRepository,
            ShoppingCartViewModel shoppingCartViewModel)
        {
            this.productCategoriesRepository = productCategoriesRepository;
            this.shoppingCartViewModel = shoppingCartViewModel;
        }
        #endregion

        #region INavigationAware
        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
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
                Items = ProductsRepository.Instance.List;
                initialized = true;
            }

            return initialized;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<ProductsOverviewObject>(CartProduct);
        }
        #endregion

        #region Filtering
        // TODO This would better be handled inside the repository.
        protected override async Task<bool> InitializeFilters()
        {
            var results = await Task.WhenAll
            (
                productCategoriesRepository.ReadList(),
                ProductSubcategoriesRepository.Instance.ReadList()
            );

            var succeeded = results.All(result => result);

            if (succeeded)
                // Need to update on the UI thread.
                uiDispatcher.Invoke(delegate
                {
                    var masterFilterItems = new ObservableCollection<ProductCategory>();

                    foreach (var item in productCategoriesRepository.List)
                    {
                        masterFilterItems.Add(item);
                    }

                    // To trigger the enablement.
                    MasterFilterItems = masterFilterItems;

                    foreach (var item in ProductSubcategoriesRepository.Instance.List)
                    {
                        detailFilterItemsSource.Add(item);
                    }

                    var masterDefaultId = 1;
                    MasterFilterValue = MasterFilterItems?.FirstOrDefault(category => category.Id == masterDefaultId);

                    // Note that MasterFilterValue also determines DetailFilterItems.
                    var detailDefaultId = 1;
                    DetailFilterValue = DetailFilterItems?.FirstOrDefault(subcategory => subcategory.Id == detailDefaultId);

                    TextFilterValue = default;
                });

            return succeeded;
        }

        protected override async Task<bool> ReadFiltered()
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

            var result = await ProductsRepository.Instance.ReadList(masterFilterValue, detailFilterValue, textFilterValue);
            var succeeded = result != null;

            if (succeeded)
            {
                // Need to update on the UI thread.
                // TODO Still true?
                uiDispatcher.Invoke(delegate
                {
                    foreach (var item in result)
                        Items.Add(item);
                });
            }

            return succeeded;
        }

        protected override Func<ProductSubcategory, bool> DetailFilterItemsSelector(bool preserveEmptyElement = true)
        {
            return subcategory =>
                MasterFilterValue != null &&
                !MasterFilterValue.IsEmpty &&
                (preserveEmptyElement && subcategory.IsEmpty || subcategory.ProductCategoryId == MasterFilterValue.Id);
        }
        #endregion

        #region Details
        protected override void ShowDetails(ProductsOverviewObject productsOverviewObject)
        {
            var productViewModel = new ProductViewModel(shoppingCartViewModel) { ItemId = productsOverviewObject.Id };
            var productView = new ProductView() { ViewModel = productViewModel };

            var productWindow = new OkWindow() { View = productView };
            productWindow.Show();
        }
        #endregion

        #region Shopping
        public static readonly DependencyProperty CartCommandProperty =
             DependencyProperty.Register(nameof(CartCommand), typeof(ICommand), typeof(FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>));

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand
        {
            get => (ICommand)GetValue(CartCommandProperty);
            set => SetValue(CartCommandProperty, value);
        }

        private void CartProduct(ProductsOverviewObject productsOverviewObject)
        {
            shoppingCartViewModel.CartProduct(productsOverviewObject);
        }
        #endregion
    }
}
