using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Modules.Products.Test.BaseClasses;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductCategoriesRepositoryTest :
        RepositoryTest<ProductCategoriesRepository, ProductCategory>
    { }
}