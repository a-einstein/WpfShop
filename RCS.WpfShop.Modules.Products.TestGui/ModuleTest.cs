using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.WpfShop.TestGui;

namespace RCS.WpfShop.Modules.Products.TestGui
{
    [TestClass]
    public class ModuleTest : GuiTest
    {
        // Note this attribute can't be used in just a baseclass.
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Setup(testContext);
        }

        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }

        // Note this attribute can't be used in just a baseclass.
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        // TODO Put this (as abstract) in a baseclass.
        // TODO This could be TestInitialize, or rather ClassInitialize.
        [TestMethod]
        public void TestNavigateTo()
        {
            // Note that elements should be identified by inspect.exe first. 
            var navigateButton = testSession.FindElementByName("Shop");
            Assert.AreEqual(navigateButton.TagName, "ControlType.Button");

            navigateButton.Click();

            // Test presence of module controls.

            // Note there may be more than 1 widget loaded.
            var shoppingCartView = testSession.FindElementByClassName("ShoppingCartView");
            Assert.IsNotNull(shoppingCartView);

            var mainViewMainContent = testSession.FindElementByClassName("ProductsView");
            Assert.IsNotNull(mainViewMainContent);

            // TODO Test data loading into somewhere, take async character into account.

            // Note that if not halted, the application closes inmediately by ClassCleanup.
        }

        // TODO Test filtering...

        // TODO Test basking.

        // TODO Test detail view...

        // TODO Test basket...
    }
}
