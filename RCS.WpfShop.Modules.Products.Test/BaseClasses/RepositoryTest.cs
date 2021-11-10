using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.AdventureWorks.Mock;
using RCS.WpfShop.Modules.Products.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Test.BaseClasses
{
    public abstract class RepositoryTest<TRepository, TElement> :
        ModelTest
        where TRepository : Repository<List<TElement>, TElement>, new()
        where TElement : DomainClass, new()
    {
        public abstract Task ListTestAsync();

        /// <summary>
        /// Basic test with regards to base class.
        /// </summary>
        /// <returns></returns>
        protected async Task ListTestAsync(int expectedServiceCount)
        {
            // Initialize.
            // Note that injection is not possible in test classes.
            // "Test classes need to have an empty default constructor or no constructors at all."

            var serviceClient = ProductsServiceClient.Mock.Object;
            var target = new TRepository() { ProductsServiceClient = serviceClient };

            await TestRefresh(expectedServiceCount, target);

            // Clear.

            await target.Clear();
            Assert.AreEqual(target.Items.Count, 0);

            // Create

            var item1 = Element(1);
            await target.Create(item1);
            Assert.AreEqual(target.Items.Count, 1);

            var item2 = Element(2);
            await target.Create(item2);
            Assert.AreEqual(target.Items.Count, 2);

            // Update.

            item1.Name = $"{item1.Name}Adapted";

            // Note currently a void operation.
            await target.Update(item1);

            // Note currently just checking status quo.
            Assert.AreEqual(target.Items.Count, 2);

            // Delete.

            await target.Delete(item1);
            Assert.AreEqual(target.Items.Count, 1);
        }

        protected virtual async Task TestRefresh(int expectedServiceCount, TRepository target)
        {
            // TODO Better control of expected numbers.

            await target.Refresh(addEmptyElement: false);
            Assert.AreEqual(target.Items.Count, expectedServiceCount);

            await target.Refresh(addEmptyElement: true);
            Assert.AreEqual(target.Items.Count, expectedServiceCount + 1);
        }

        private static TElement Element(int suffix)
        {
            return new TElement() { Name = $"{nameof(TElement)}{suffix}" };
        }
    }
}
