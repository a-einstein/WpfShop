using System;
using System.Data;
using ShoppingCartItemsDataTable = Demo.Model.ProductsDataSet.ShoppingCartItemsDataTable;
using ShoppingCartItemsRow = Demo.Model.ProductsDataSet.ShoppingCartItemsRow;
using ShoppingCartsDataTable = Demo.Model.ProductsDataSet.ShoppingCartsDataTable;
using ShoppingCartsRow = Demo.Model.ProductsDataSet.ShoppingCartsRow;

namespace Demo.Model
{
    public class CartItemsRepository
    {
        private CartItemsRepository()
        { }

        private static volatile CartItemsRepository instance;
        private static object syncRoot = new Object();

        public static CartItemsRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CartItemsRepository();
                    }
                }

                return instance;
            }
        }

        // TODO This table might be removed alltogether, as only one cart is used. Then the current relation and non nullable key should be remove, or made nullable.
        private ShoppingCartsDataTable Carts
        {
            get
            {
                return ShoppingWrapper.Instance.ProductsDataSet.ShoppingCarts;
            }
        }

        private ShoppingCartsRow cart;

        private ShoppingCartsRow Cart
        {
            get
            {
                if (cart == null)
                {
                    const string cartId = "1";

                    Carts.AddShoppingCartsRow(cartId);
                    Carts.AcceptChanges();

                    cart = Carts.Rows.Find(cartId) as ShoppingCartsRow;
                }

                return cart;
            }
        }

        // Note that simply the whole table is used, as all items belong to the user.
        // It is only kept in memory and not preserved. It is anticipated that only real orders are preserved.
        private ShoppingCartItemsDataTable CartItems { get { return ShoppingWrapper.Instance.ProductsDataSet.ShoppingCartItems; } }

        public DataView Items { get { return CartItems.DefaultView; } }

        private const string cartItemsNumberExceptionMessage = "Unexpected number of found ShoppingCartItems.";
        private const string productNotFoundExceptionMessage = "Product not found.";

        public void AddProduct(int productId)
        {
            var productQuery = string.Format("ProductID = {0}", productId);
            var existingCartItems = Items.Table.Select(productQuery);

            if (existingCartItems.Length == 0)
            {
                var now = DateTime.Now;
                var productRow = ShoppingWrapper.Instance.ProductsDataSet.ProductsOverview.FindByProductID((int)productId);

                if (productRow != null)
                {
                    // Note that ShoppingCartId currently is non nullable.
                    CartItems.AddShoppingCartItemsRow(Cart, 1, productRow, now, now);

                    CartItems.AcceptChanges();
                }
                else
                    throw new Exception(productNotFoundExceptionMessage);
            }
            else if (existingCartItems.Length == 1)
            {
                var cartItem = existingCartItems[0] as ShoppingCartItemsRow;
                cartItem.Quantity += 1;

                cartItem.AcceptChanges();
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);
        }

        public void DeleteProduct(int cartItemID)
        {
            var cartItem = CartItems.FindByShoppingCartItemID(cartItemID);

            if (cartItem != null)
            {
                cartItem.Delete();
                CartItems.AcceptChanges();
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);
        }

        public int ProductsCount()
        {
            return CartItems.Count > 0
            ? Convert.ToInt32(CartItems.Compute("Sum(Quantity)", null))
            : 0;
        }

        public double CartValue()
        {
            return CartItems.Count > 0
            ? Convert.ToDouble(CartItems.Compute("Sum(Value)", null))
            : 0.0;
        }
    }
}
