using Common.DomainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductSubcategoriesRepositoryTest
    {
        [TestMethod()]
        public void ListTest()
        {
            var target = ProductSubcategoriesRepository.Instance;
            var element = new ProductSubcategory() { Name = "SubcategoryName" };

            target.Clear();
            target.List.Add(element);

            var result = target.List.Count;

            Assert.AreEqual(1, result);
        }
     }
 }
