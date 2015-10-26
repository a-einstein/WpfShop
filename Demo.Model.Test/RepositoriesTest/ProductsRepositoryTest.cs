using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Demo.Model.Test
{
    [TestClass()]
    public class ProductsRepositoryTest : ModelTest
    {
        [TestMethod()]
        // Note this conforms to asynchronous tests since VS 2012.
        public async Task ReadListTest()
        {
            var target = ProductsRepository.Instance;
            var dto = ProductsOverviewRowDto(1, target.NoId);

            target.Clear();
            target.CreateListElement(dto);

            var result = await target.ReadList();

            Assert.AreEqual(1, result.Count);
        }
    }
}