using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    /// <summary>
    /// Single level Viewmodel on CartItem.
    /// </summary>
    [DebuggerDisplay("{Name} : {ProductListPrice} x {Quantity} = {Value}")]
    public class CartItemViewModel : ViewModel
    {
        #region Construction
        public CartItemViewModel(CartItem cartItem)
        {
            CartItem = cartItem;

            // Update binding.
            Quantity = CartItem.Quantity;
        }

        // TODO As with Repository.Items one could argue to not directly expose the individual items too.
        // So that would imply holding a copy which is read and updates indirectly.

        /// <summary>
        /// Reference into the repository. 
        /// The model for this object.
        /// </summary>
        public CartItem CartItem { get; }

        private void UpdateValue()
        {
            Value = ProductListPrice * Quantity;
        }
        #endregion

        #region Bindable but not updateable.
        public int? Id => CartItem.Id;
        public string Name => CartItem.Name;
        public int ProductId => CartItem.ProductId;
        public string ProductSize => CartItem.ProductSize;
        public string ProductSizeUnitMeasureCode => CartItem.ProductSizeUnitMeasureCode;
        public string ProductColor => CartItem.ProductColor;
        public decimal ProductListPrice => CartItem.ProductListPrice;
        #endregion

        #region Bindable and updateable.
        public int Quantity
        {
            get => CartItem.Quantity;
            set
            {
                CartItem.Quantity = value;

                // Do this before the PropertyChanged, to be available too.
                UpdateValue();

                // Need this because this is no BindableProperty, for I also want to use CartItem.Quantity instead of duplicating it.
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(decimal), typeof(CartItemViewModel));

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }
        #endregion
    }
}