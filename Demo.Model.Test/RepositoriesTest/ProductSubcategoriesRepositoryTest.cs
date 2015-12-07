using Common.DomainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Model.Test
{
    [TestClass()]
    public class ProductSubcategoriesRepositoryTest
    {
        [TestMethod()]
        public void ListTest()
        {
            var target = ProductSubcategoriesRepository.Instance;
            var element = ProductSubcategory(target.NoId);

            target.Clear();
            target.List.Add(element);

            var result = target.List.Count;

            Assert.AreEqual(1, result);
        }

        public static ProductSubcategory ProductSubcategory(object noId)
         {
             var instance = new ProductSubcategory()
             {
                 Name = "a Name",
                 ProductCategoryId = (int)noId,
                 Id = (int)noId
             };

             return instance;
         }
     }
 }
