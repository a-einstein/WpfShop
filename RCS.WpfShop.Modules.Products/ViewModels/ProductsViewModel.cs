using Prism.Commands;
using Prism.Regions;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.Interfaces;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Common.Windows;
using RCS.WpfShop.Modules.Products.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    public class ProductsViewModel :
        FilterItemsViewModel<ProductsOverviewObject, ProductCategory, ProductSubcategory>, IShopper
    {
        #region Construction
        public ProductsViewModel(
            IRepository<List<ProductCategory>, ProductCategory> productCategoriesRepository,
            IRepository<List<ProductSubcategory>, ProductSubcategory> productSubcategoriesRepository,
            IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> productsRepository,
            CartViewModel cartViewModel)
        {
            ProductCategoriesRepository = productCategoriesRepository;
            ProductSubcategoriesRepository = productSubcategoriesRepository;
            ProductsRepository = productsRepository;

            CartViewModel = cartViewModel;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<ProductsOverviewObject>(CartProduct);
        }
        #endregion

        #region Services
        private IRepository<List<ProductCategory>, ProductCategory> ProductCategoriesRepository { get; }
        private IRepository<List<ProductSubcategory>, ProductSubcategory> ProductSubcategoriesRepository { get; }
        private IFilterRepository<List<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> ProductsRepository { get; }

        CartViewModel CartViewModel { get; }
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
                initialized = true;
            }

            return initialized;
        }
        #endregion

        #region Filtering
        // TODO This would better be handled inside the repository.
        protected override async Task<bool> InitializeFilters()
        {
            var succeeded = await ProductCategoriesRepository.Refresh().ConfigureAwait(true);
            succeeded &= await ProductSubcategoriesRepository.Refresh().ConfigureAwait(true);

            if (succeeded)
            {

                foreach (var item in ProductCategoriesRepository.Items)
                {
                    MasterFilterItems.Add(item);
                }

                // Extra event. For some bindings (ItemsSource) those from ObservableCollection are enough, but for others (IsEnabled) this is needed.
                RaisePropertyChanged(nameof(MasterFilterItems));

                foreach (var item in ProductSubcategoriesRepository.Items)
                {
                    detailFilterItemsSource.Add(item);
                }

                const int masterDefaultId = 1;
                MasterFilterValue = MasterFilterItems?.FirstOrDefault(category => category.Id == masterDefaultId);

                // Note that MasterFilterValue also determines DetailFilterItems.
                const int detailDefaultId = 1;
                DetailFilterValue = DetailFilterItems?.FirstOrDefault(subcategory => subcategory.Id == detailDefaultId);

                TextFilterValue = default;
            }

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

            var task = ProductsRepository.Refresh(masterFilterValue, detailFilterValue, textFilterValue);
            await task.ConfigureAwait(true);
            var succeeded = task.Status != TaskStatus.Faulted;

            if (succeeded)
            {
                uiDispatcher.Invoke(delegate
                {
                    foreach (var item in ProductsRepository.Items)
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
            // Note this enables opening multiple windows.
            var productViewModel = new ProductViewModel(ProductsRepository, CartViewModel) { ItemId = productsOverviewObject.Id };
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
            CartViewModel.CartProduct(productsOverviewObject);
        }
        #endregion
    }
}
