using Microsoft.Practices.Prism.Commands;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Model;

namespace WpfTestApplication.ViewModels
{
    public class ShoppingCartViewModel : ItemsViewModel
    {
        private ShoppingCartViewModel() 
        {
            ShoppingWrapper.Instance.CartItems.ShoppingCartItemsRowChanged += CartItems_ShoppingCartItemsRowChanged;
            ShoppingWrapper.Instance.CartItems.ShoppingCartItemsRowDeleted += CartItems_ShoppingCartItemsRowChanged;

            Refresh();
        }

        private static volatile ShoppingCartViewModel instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static ShoppingCartViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ShoppingCartViewModel();
                    }
                }

                return instance;
            }
        }

        public override void Refresh()
        {
            // TODO Maybe make a separate property Items for this in a new wrapper class CartItems. And so forth. Check what this means for filtering.
            Items = ShoppingWrapper.Instance.CartItems.DefaultView;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            DeleteCommand = new DelegateCommand<object>(Delete);
        }

        public void CartProduct(int productId)
        {
            ShoppingWrapper.Instance.CartProduct(productId);
        }

        public ICommand DeleteCommand { get; set; }

        private void Delete(object parameter)
        {
            int cartItemID = (int)parameter;

            ShoppingWrapper.Instance.CartItemDelete(cartItemID);
        }

        private void CartItems_ShoppingCartItemsRowChanged(object sender, ProductsDataSet.ShoppingCartItemsRowChangeEvent e)
        {
            switch (e.Action)
            {
                case DataRowAction.Change:
                    // For directly bound controls. Currently only on Quantity.
                    e.Row.AcceptChanges();

                    UpdateTotals();
                    break;
                case DataRowAction.Add:
                case DataRowAction.Delete:
                    UpdateTotals();
                    break;
                case DataRowAction.ChangeCurrentAndOriginal:
                case DataRowAction.ChangeOriginal:
                case DataRowAction.Commit:
                case DataRowAction.Nothing:
                case DataRowAction.Rollback:
                default:
                    break;
            }
        }

        private void UpdateTotals()
        {
            RaisePropertyChanged("ItemsCount");

            ProductItemsCount = ShoppingWrapper.Instance.CartProductItemsCount();
            TotalValue = ShoppingWrapper.Instance.CartValue();
        }

        public static readonly DependencyProperty ProductItemCountProperty =
            DependencyProperty.Register("ProductItemsCount", typeof(int), typeof(ShoppingCartViewModel), new PropertyMetadata(0));

        public int ProductItemsCount
        {
            get { return (int)GetValue(ProductItemCountProperty); }
            set { SetValue(ProductItemCountProperty, value); }
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("TotalValue", typeof(Double), typeof(ShoppingCartViewModel), new PropertyMetadata(0.0));

        public Double TotalValue
        {
            get { return (Double)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }
    }
}
