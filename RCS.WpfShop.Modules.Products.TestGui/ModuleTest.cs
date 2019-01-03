using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.WpfShop.TestGui;

namespace RCS.WpfShop.Modules.Products.TestGui
{
    [TestClass]
    public class ModuleTest : GuiTest
    {
        // Note this must have a distinctive signature: static, public, no return value, take single parameter type TestContext.
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            SetUp(testContext);

            NavigateTo();
        }

        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void InitializeTest() { }

        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void CleanupTest() { }

        // Note that if not halted the application closes inmediately after the last test.
        [ClassCleanup]
        public static void CleanupClass()
        {
            TearDown();
        }

        // TODO Put this (as abstract) in a baseclass, if possible at all
        public static void NavigateTo()
        {
            // Note that elements should be identified by inspect.exe first.

            // Note when this was a separate test it only worked while debugging (even without breaks).
            // It is quite similar as decribe here:
            // https://github.com/Microsoft/WinAppDriver/issues/370
            // Tried:
            // - The approach with DefaultWait.
            // - Altering testSession.Manage().Timeouts().ImplicitWait.
            // - Using Thread.Sleep.
            // FindElementByName seemed the only feasible option.
            // Also tried to set AutomationId by binding. 
            // In the current dynamic setup this is not allowed for Name and x:Name, Tag did not work.
            //
            // After making this static and applying in ClassInitialize the problem was gone.
            var navigateButton = testSession.FindElementByName("Shop");
            Assert.AreEqual(navigateButton?.TagName, "ControlType.Button");

            navigateButton.Click();

            // Test presence of module controls.

            // Note there may be more than 1 widget loaded.
            var shoppingCartView = testSession.FindElementByClassName("ShoppingCartView");
            Assert.IsNotNull(shoppingCartView);

            var mainViewMainContent = testSession.FindElementByClassName("ProductsView");
            Assert.IsNotNull(mainViewMainContent);
        }

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
    }
}
