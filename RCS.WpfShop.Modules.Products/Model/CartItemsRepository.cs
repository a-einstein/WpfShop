using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class CartItemsRepository :
        Repository<ObservableCollection<CartItem>, CartItem>
    {
        #region Construction
        public CartItemsRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        // TODO Add messages to views.
        private const string cartItemsNumberExceptionMessage = "Unexpected number of found ShoppingCartItems.";

        // Note that the cart is only kept in memory and is not preserved. 
        // It is anticipated that only real orders would be preserved and stored on the server.
        public override CartItem AddProduct(IShoppingProduct product)
        {
            var existingCartItems = List.Where(cartItem => cartItem.ProductId == product.Id);
            var cartItems = existingCartItems.ToList();
            var existingCartItemsCount = cartItems.Count;

            CartItem productCartItem;

            switch (existingCartItemsCount)
            {
                case 0:
                    productCartItem = new CartItem()
                    {
                        ProductId = product.Id.Value,
                        Name = product.Name,
                        ProductSize = product.Size,
                        ProductSizeUnitMeasureCode = product.SizeUnitMeasureCode,
                        ProductColor = product.Color,
                        ProductListPrice = product.ListPrice,
                        Quantity = 1
                    };

                    List.Add(productCartItem);
                    break;
                case 1:
                    productCartItem = cartItems.First();

                    productCartItem.Quantity += 1;
                    productCartItem.Value = productCartItem.ProductListPrice * productCartItem.Quantity;
                    break;
                default:
                    throw new Exception(cartItemsNumberExceptionMessage);
            }

            return productCartItem;
        }

        public override void DeleteProduct(CartItem cartItem)
        {
            List.Remove(cartItem);
        }
        #endregion

        #region Aggregates
        public override int ProductsCount()
        {
            return List.Count > 0
                ? List.Sum(cartItem => cartItem.Quantity)
                : 0;
        }

        public override decimal CartValue()
        {
            return List.Count > 0
                ? List.Sum(cartItem => cartItem.Value)
                : 0;
        }
        #endregion
    }
}
