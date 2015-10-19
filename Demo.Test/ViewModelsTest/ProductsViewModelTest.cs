using Demo.Model;
using Demo.Model.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.ViewModels.Test
{
    [TestClass()]
    // TODO Decide what to do with this class.
    // See comment on test below.
    public class ProductsViewModelTest
    {
        // Currently deactivated as it instantiates ProductsServiceClient by multiple entities.
        //[TestMethod()]
        // TODO It is doubtful if this is useful as a unit test as it depends on the repository.
        // In fact currently a test on that class has been made that is comparable with this.
        public void RefreshTest()
        {
            var target = new ProductsViewModel();

            var dto = ModelTest.ProductsOverviewRowDto(1, target.NoId);

            var repository = ProductsRepository.Instance;

            repository.Clear();
            repository.CreateListElement(dto);

            target.Refresh();

            Assert.AreEqual(1, target.Items.Count);
        }
    }
}