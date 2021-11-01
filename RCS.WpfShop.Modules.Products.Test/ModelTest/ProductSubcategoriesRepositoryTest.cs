using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Modules.Products.Test.BaseClasses;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductSubcategoriesRepositoryTest :
       RepositoryTest<ProductSubcategoriesRepository, ProductSubcategory>
    {
        [TestMethod()]
        public override async Task ListTestAsync()
        {
            await ListTestAsync(4);
        }
    }
}
