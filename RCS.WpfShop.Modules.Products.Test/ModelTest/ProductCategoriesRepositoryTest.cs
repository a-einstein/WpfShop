using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    // TODO Make this generic?
    [TestClass()]
    public class ProductCategoriesRepositoryTest
    {
        [TestMethod()]
        public async Task ListTestAsync()
        {
            // Initialize.

            // Note that injection is not possible in test classes.
            // "Test classes need to have an empty default constructor or no constructors at all."
            var target = new ProductCategoriesRepository();

            /*
            // TODO This does not work (yet).
            await target.Refresh();
            // TODO Better control these numbers.
            Assert.AreEqual(target.Items.Count, 2);
            */

            await target.Clear();
            Assert.AreEqual(target.Items.Count, 0);

            // Create

            var item1 = Category(1);
            await target.Create(item1);
            Assert.AreEqual(target.Items.Count, 1);

            var item2 = Category(2);
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

        static ProductCategory Category(int suffix)
        {
            return new ProductCategory() { Name = $"{nameof(ProductCategory)}{suffix}" };
        }
    }
}