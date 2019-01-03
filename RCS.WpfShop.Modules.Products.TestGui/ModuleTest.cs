using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.WpfShop.TestGui;

namespace RCS.WpfShop.Modules.Products.TestGui
{
    [TestClass]
    public class ModuleTest : GuiTest
    {
        // Note this attribute can't be used in just a baseclass.
        // TODO Does this have to be static?
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            SetUp(testContext);
        }

        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void InitializeTest() { }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void CleanupTest() { }

        // Note this attribute can't be used in just a baseclass.
        // TODO Does this have to be static?
        [ClassCleanup]
        public static void CleanupClass()
        {
            TearDown();
        }

        // TODO Put this (as abstract) in a baseclass.
        // TODO This could be TestInitialize, or rather ClassInitialize.
        [TestMethod]
        public void TestNavigateTo()
        {
            // Note that elements should be identified by inspect.exe first.

            // TODO This only works while debugging (even without breaks).
            // It is quite similar as decribe here:
            // https://github.com/Microsoft/WinAppDriver/issues/370
            // Tried:
            // - The approach with DefaultWait.
            // - Altering testSession.Manage().Timeouts().ImplicitWait.
            // - Using Thread.Sleep.
            // FindElementByName seems the only feasible option. 
            var navigateButton = testSession.FindElementByName("Shop");
            Assert.AreEqual(navigateButton?.TagName, "ControlType.Button");

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
