using Demo.ServiceClients.Products.ServiceReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Demo.Model.Test
{
    [TestClass()]
    public class ProductCategoriesRepositoryTest
    {
        [TestMethod()]
        // Note this conforms to asynchronous tests since VS 2012.
        public async Task ReadListest()
        {
            var target = ProductCategoriesRepository.Instance;
            var dto = ProductCategoryRowDto(target.NoId);

            target.Clear();
            target.CreateListElement(dto);

            var result = await target.ReadList();

            Assert.AreEqual(1, result.Count);
        }

        public static ProductCategoryRowDto ProductCategoryRowDto(object noId)
        {
            // TODO Put definition of dto in other project and reuse in service client to remove that dependency ?
            var dto = new ProductCategoryRowDto()
            {
                Name = "a Name",
                ProductCategoryID = (int)noId
            };

            return dto;
        }
    }
}