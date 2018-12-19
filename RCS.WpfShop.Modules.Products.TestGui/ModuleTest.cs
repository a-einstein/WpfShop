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
            // TODO Currently a typing problem, aparently because of Selenium versions.
            WindowsElement shopButton = testSession.FindElementByName("Shop");
            shopButton.Click();
        }
    }
}
