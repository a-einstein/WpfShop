using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductCategoriesRepositoryTest
    {
        [TestMethod()]
        public void ListTest()
        {
            // Note that injection is not possible in test classes.
            // "Test classes need to have an empty default constructor or no constructors at all."
            var target = new ProductCategoriesRepository();
            var element = new ProductCategory() { Name = "CategoryName" };

            target.Clear();
            target.List.Add(element);

            var result = target.List.Count;

            Assert.AreEqual(1, result);
        }
    }
}