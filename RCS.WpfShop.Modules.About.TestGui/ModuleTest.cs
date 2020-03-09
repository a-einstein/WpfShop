using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.WpfShop.TestGui;

namespace RCS.WpfShop.Modules.About.TestGui
{
    [TestClass]
    public class ModuleTest : GuiTest
    {
        #region Class level
        private static readonly string destination = "About";
        private static readonly string mainViewName = "AboutView";
        private static readonly string[] widgetNames = { };

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            StartSession(testContext);

            NavigateTo(destination);

            CheckMainContent(mainViewName);
            CheckWidgetsContent(widgetNames);
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            EndSession();
        }
        #endregion

        #region Test level
        /// <summary>
        /// Just a (temporary) stub to fire this test up.
        /// </summary>
        [TestMethod]
        public void TestStart()
        { }
        #endregion
    }
}