using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.WpfShop.TestGui;

namespace RCS.WpfShop.Modules.Products.TestGui
{
    [TestClass]
    public class ModuleTest : GuiTest
    {
        #region Class level
        private static string destination = "Shop";
        private static string mainViewName = "ProductsView";
        private static string[] widgetNames = { "ShoppingCartView" };

        // Note this must have a distinctive signature: static, public, no return value, take single parameter type TestContext.
        // TODO Would like to share this among classes, but currently see no way because of the static nature and the ClassInitialize, 
        // attribute which only seems to be possible here.
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            StartSession(testContext);

            NavigateTo(destination);

            CheckMainContent(mainViewName);
            CheckWidgetsContent(widgetNames);
        }

        // Note that if not halted the application closes inmediately after the last test.
        [ClassCleanup]
        public static void CleanupClass()
        {
            EndSession();
        }
        #endregion

        #region Test level
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void InitializeTest() { }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void CleanupTest() { }

        // This assumes the database containing data that results in exactly these values as it is really queried.
        // Alternatively some stubs have to be created.
        [TestMethod]
        public void TestFilter()
        {
            // TODO Data could also retrieved and tested with the filter only partially filled.

            var masterFilterComboBox = testSession.FindElementByAccessibilityId("MasterFilterComboBox");
            Assert.IsNotNull(masterFilterComboBox);
            // Necessary to get the elements.
            masterFilterComboBox.Click();
            // Note that Click must be applied this way.
            // Finding the element separately resulted in only a brief display on screen after which the Display property was false, which prevented Click. 
            masterFilterComboBox.FindElementByName("Clothing").Click();

            var detailFilterComboBox = testSession.FindElementByAccessibilityId("DetailFilterComboBox");
            Assert.IsNotNull(detailFilterComboBox);
            detailFilterComboBox.Click();
            detailFilterComboBox.FindElementByName("Jerseys").Click();

            // Note this is a UserControl.
            var textFilterControl = testSession.FindElementByAccessibilityId("TextFilterTextBox");
            Assert.IsNotNull(textFilterControl);

            // The true text element within.
            var textFilterTextBox = textFilterControl.FindElementByClassName("TextBox");
            textFilterTextBox.Clear();
            textFilterTextBox.SendKeys("ye");
            // TODO Add enablement on the button depending on this control and test. Currently the control only colours on less than 3 characters.
            textFilterTextBox.SendKeys("l");

            var filterButton = testSession.FindElementByAccessibilityId("FilterButton");
            Assert.IsNotNull(filterButton);
            filterButton.Click();

            // TODO This might have to be improved because of the asynchronous nature of the data retrieval. 
            var itemsCountTextBlock = testSession.FindElementByAccessibilityId("ItemsCountTextBlock");
            Assert.IsNotNull(itemsCountTextBlock);
            Assert.AreEqual(itemsCountTextBlock.Text, "4");
        }

        // TODO Test basking.

        // TODO Test detail view...

        // TODO Test basket...
        #endregion
    }
}