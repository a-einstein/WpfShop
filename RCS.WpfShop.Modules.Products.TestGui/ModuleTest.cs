using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.WpfShop.Modules.Products.Views;
using RCS.WpfShop.Resources;
using RCS.WpfShop.ServiceClients.Products.Mock;
using RCS.WpfShop.TestGui;

namespace RCS.WpfShop.Modules.Products.TestGui
{
    [TestClass]
    public class ModuleTest : GuiTest
    {
        #region Class level
        private static readonly string destination = Labels.NavigateShop;
        private const string mainViewName = nameof(ProductsView);
        private static readonly string[] widgetNames = { nameof(ShoppingCartView) };

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
            var serviceClient = ProductsServiceClient.Mock;

            // TODO Data could also retrieved and tested with the filter only partially filled.

            var categoriesExpected = serviceClient.Object.GetProductCategories();
            var categoryExpectedOrder = 2;
            var categoryExpected = categoriesExpected[categoryExpectedOrder - 1];

            // Note literals are used for the control names here as they are fields in the view classes.
            var masterFilterComboBox = TestSession.FindElementByAccessibilityIdWait("MasterFilterComboBox");
            Assert.IsNotNull(masterFilterComboBox);

            // Necessary to actually get the elements created in the GUI.
            masterFilterComboBox.Click();

            // TODO Check length of list?

            // Note that Click must be applied this way. Finding the element separately resulted in only a brief display on screen after which the Display property was false, which prevented Click. 
            masterFilterComboBox.FindElementByName(categoryExpected.Name).Click();

            var subcategoriesExpected = serviceClient.Object.GetProductSubcategories();
            var subcategoryExpectedOrder = 1;
            var subcategoryExpected = subcategoriesExpected.FindAll(subcategory => subcategory.ProductCategoryId == categoryExpected.Id)[subcategoryExpectedOrder - 1];

            var detailFilterComboBox = TestSession.FindElementByAccessibilityIdWait("DetailFilterComboBox");
            Assert.IsNotNull(detailFilterComboBox);
            detailFilterComboBox.Click();
            detailFilterComboBox.FindElementByName(subcategoryExpected.Name).Click();

            var searchString = $"{categoryExpected.Id}.{subcategoryExpected.Id}";
            var productsOverviewExpected = serviceClient.Object.GetProductsOverviewBy(categoryExpected.Id, subcategoryExpected.Id, searchString);

            // Note this is a UserControl.
            var textFilterControl = TestSession.FindElementByAccessibilityIdWait("TextFilterTextBox");
            Assert.IsNotNull(textFilterControl);

            // The true text element within.
            var textFilterTextBox = textFilterControl.FindElementByClassName("TextBox");
            textFilterTextBox.Clear();
            textFilterTextBox.SendKeys(searchString.Substring(0, 2));

            // TODO Add enablement on the button depending on this control and test. Currently the control only colours on less than 3 characters.
            textFilterTextBox.SendKeys(searchString.Substring(2, 1));

            var filterButton = TestSession.FindElementByAccessibilityIdWait("FilterButton");
            Assert.IsNotNull(filterButton);
            filterButton.Click();

            var itemsCountTextBlock = TestSession.FindElementByAccessibilityIdWait("ItemsCountTextBlock");
            Assert.IsNotNull(itemsCountTextBlock);
            Assert.AreEqual(itemsCountTextBlock.Text, productsOverviewExpected.Count.ToString());
        }

        // TODO Test basking.

        // TODO Test detail view...

        // TODO Test basket...
        #endregion
    }
}