using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.ServiceReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model
{
    public class CartItemsRepository :
        Repository<List<CartItem>, CartItem>
    {
        #region Construction
        // Need a parameterless constructor for tests.
        public CartItemsRepository()
        { }

        public CartItemsRepository(IProductsService productsServiceClient = null)
            : base(productsServiceClient)
        { }
        #endregion

        #region CRUD
        // TODO Add messages to views.

        public override async Task Update(CartItem proxy)
        {
            // Use a simple function instead of CancellationToken .
            var task = Task.Run(() =>
            {
                // TODO Use Id, complying to DomainClass. That could be equal to ProductId for the moment.
                // Then this method could be generic. If desired, not all repositories really need to update.
                var foundItem = items.FirstOrDefault(item => item.ProductId == proxy.ProductId);

                if (foundItem != default)
                {
                    foundItem.Update(proxy);
                    return true;
                }
                else
                {
                    return false;
                }
            });

            await task;

            if (!task.Result)
                DisplayAlert(new Exception("Unexpected number of CartItems."));
        }

        public override async Task Delete(CartItem proxy)
        {
            await Task.Run(() =>
            {
                // TODO This could be generalized by applying something like IEqualityComparer on ProductId or even Id.
                var foundItem = items.FirstOrDefault(item => item.ProductId == proxy.ProductId);

                if (foundItem != default)
                {
                    items.Remove(foundItem);
                }
                else
                {
                    DisplayAlert(new Exception("CartItem not found."));
                }
            });
        }
        #endregion
    }
}
