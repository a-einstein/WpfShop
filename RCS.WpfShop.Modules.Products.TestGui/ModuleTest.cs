using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using RCS.WpfShop.TestGui;

namespace RCS.WpfShop.Modules.Products.TestGui
{
    [TestClass]
    public class ModuleTest : GuiTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Setup(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }

        [TestMethod]
        public void OpenShop()
        {
            // Note that elements should be identified by inspect.exe first. 
            WindowsElement shopButton = testSession.FindElementByName("Shop");

            shopButton.Click();

            // Note that if not halted, the application closes inmediately by ClassCleanup.
            // TODO Add asserts, take async loading of data into account.
            // TODO Streamline use of ClassCleanup/TestCleanup, possibly in the base.
        }
    }
}
