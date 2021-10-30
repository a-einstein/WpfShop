using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductSubcategoriesRepositoryTest
    {
        [TestMethod()]
        public async Task ListTestAsync()
        {
            // Note that injection is not possible in test classes.
            // "Test classes need to have an empty default constructor or no constructors at all."
            var target = new ProductSubcategoriesRepository();
            var element = new ProductSubcategory() { Name = "SubcategoryName" };

            await target.Clear();
            await target.Create(element);

            var result = target.Items.Count;

            Assert.AreEqual(1, result);
        }
    }
}
