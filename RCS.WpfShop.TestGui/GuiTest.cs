using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;

namespace RCS.WpfShop.TestGui
{
    [TestClass]
    public class GuiTest
    {
        private const string winAppDriverUrl = "http://127.0.0.1:4723";

        private static string mainName;
        private static readonly string appDir;
        private static readonly string appPath;

        protected static WindowsDriver<WindowsElement> TestSession { get; private set; }

        static GuiTest()
        {
            // Use the namespace as this testclass is functionally linked to it.
            var classNameParts = typeof(GuiTest).FullName.Split(new char[] { '.' });
            mainName = String.Join(".", classNameParts, 0, 2);
                
            appDir = $"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\..\\{mainName}\\bin\\Test";
            appPath = $"{appDir}\\{mainName}.exe";
        }

        public static void StartSession(TestContext testContext)
        {
            if (TestSession == null)
            {
                EndSession();

                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", appPath);
                appiumOptions.AddAdditionalCapability("appWorkingDir", appDir);

                // Note WinAppDriver.exe has to be started first.

                TestSession = new WindowsDriver<WindowsElement>(new Uri(winAppDriverUrl), appiumOptions);

                Assert.IsNotNull(TestSession);
                Assert.IsNotNull(TestSession.SessionId);

                // Set implicit timeout to multiple of 0,5 seconds to make element search to retry every 500 ms for at most three times.
                // Currently increased for Azure environment. Note also an explicit wait on FindElement is used currently.
                TestSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            }
        }

        public static void NavigateTo(string destination)
        {
            // Note that elements should be identified by inspect.exe first.

            /*
             TODO This only worked while debugging the test (even without breaks). Built in Test configuration.
             After making this static and applying in ClassInitialize the problem was only temporarily gone.
             It is quite similar as decribe here:
             https://github.com/Microsoft/WinAppDriver/issues/370

             Tried:
             - The approach with DefaultWait.
             - Altering testSession.Manage().Timeouts().ImplicitWait.
             - Using Thread.Sleep.
             - Increasing ImplicitWait up to 10. It visibly halted the test, but did not help. 
             - Manually clicking the application to the foreground also did not help. even though the searched button is visible.
             - Extending FindElementByName with an explicit wait. 
            
             FindElementByName seemed the only feasible option.
             Also tried to set AutomationId by binding. 
             In the current dynamic setup this is not allowed for Name and x:Name, Tag did not work.

            UPDATE 1
            Running both tests COMBINED can succeed, possibly depending on the order of execution.
            Even the whole current list of tests (including non GUI tests) has worked.
 
            UPDATE 2
            For clarity: all current tests (unit or GUI) can be run in one go.
            TODO Check whether they can be run on a buildserver (Azure) together or that the tests should be split over kinds.

            UPDATE 3
            No longer had problems while running tests from VisualStudio.
            However, the same exception occurred in Azure, currently increased ImplicitWait.
            */

            // Try with explicit button and assertion.
            //var navigateButton = testSession.FindElementByName(destination);
            //Assert.AreEqual(navigateButton?.TagName, controlTypeButtonLabel);
            //navigateButton.Click();

            // try with direct click.
            //testSession.FindElementByName(destination).Click();

            // Try with explicit wait.
            TestSession.FindElement(By.Name(destination), 10).Click();

            // Test presence of module controls.
        }

        protected static void CheckMainContent(string name)
        {
            CheckElement(name);
        }

        protected static void CheckWidgetsContent(string[] names)
        {
            foreach (var name in names)
            {
                CheckElement(name);
            }
        }

        private static void CheckElement(string name)
        {
            var element = TestSession.FindElementByClassName(name);
            Assert.IsNotNull(element);
        }

        public static void EndSession()
        {
            TestSession?.Quit();
            TestSession = null;
        }
    }

    public static class Extensions
    {
        // Experimental
        public static IWebElement FindElement(this IWebDriver webDriver, By by, int timeoutInSeconds = 1)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(driver => driver.FindElement(by));
        }
    }
}
