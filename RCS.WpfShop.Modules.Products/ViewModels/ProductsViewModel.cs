using Prism.Commands;
using Prism.Regions;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.Interfaces;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Common.Windows;
using RCS.WpfShop.Modules.Products.Views;
using System;
using System.Collections.ObjectModel;
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
            IRepository<ObservableCollection<ProductCategory>, ProductCategory> productCategoriesRepository,
            IRepository<ObservableCollection<ProductSubcategory>, ProductSubcategory> productSubcategoriesRepository,
            IFilterRepository<ObservableCollection<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> productsRepository,
            ShoppingCartViewModel shoppingCartViewModel)
        {
            ProductCategoriesRepository = productCategoriesRepository;
            ProductSubcategoriesRepository = productSubcategoriesRepository;
            ProductsRepository = productsRepository;

            ShoppingCartViewModel = shoppingCartViewModel;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<ProductsOverviewObject>(CartProduct);
        }
        #endregion

        #region Services
        IRepository<ObservableCollection<ProductCategory>, ProductCategory> ProductCategoriesRepository { get; }
        IRepository<ObservableCollection<ProductSubcategory>, ProductSubcategory> ProductSubcategoriesRepository { get; }
        IFilterRepository<ObservableCollection<ProductsOverviewObject>, ProductsOverviewObject, ProductCategory, ProductSubcategory, int> ProductsRepository { get; }

        ShoppingCartViewModel ShoppingCartViewModel { get; }
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
            var results = await Task.WhenAll
            (
                ProductCategoriesRepository.ReadList(),
                ProductSubcategoriesRepository.ReadList()
            );

            var succeeded = results.All(result => result);

            if (succeeded)

                foreach (var item in ProductCategoriesRepository.List)
                {
                    MasterFilterItems.Add(item);
                }

            // Extra event. For some bindings (ItemsSource) those from ObservableCollection are enough, but for others (IsEnabled) this is needed.
            RaisePropertyChanged(nameof(MasterFilterItems));

            foreach (var item in ProductSubcategoriesRepository.List)
            {
                detailFilterItemsSource.Add(item);
            }

            var masterDefaultId = 1;
            MasterFilterValue = MasterFilterItems?.FirstOrDefault(category => category.Id == masterDefaultId);

            // Note that MasterFilterValue also determines DetailFilterItems.
            var detailDefaultId = 1;
            DetailFilterValue = DetailFilterItems?.FirstOrDefault(subcategory => subcategory.Id == detailDefaultId);

            TextFilterValue = default;

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

            var result = await ProductsRepository.ReadList(masterFilterValue, detailFilterValue, textFilterValue);
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
            // Note this enables opening multiple windows.
            var productViewModel = new ProductViewModel(ProductsRepository, ShoppingCartViewModel) { ItemId = productsOverviewObject.Id };
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
            ShoppingCartViewModel.CartProduct(productsOverviewObject);
        }
        #endregion
    }
}
