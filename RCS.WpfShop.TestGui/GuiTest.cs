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

        // Constants (but not markable as such.) 
        // TODO Find a more generic solution for this path.
        private static readonly string appDir = @"R:\RCS\shopping\clients\WpfShop\project\RCS.WpfShop\bin\Test";
        private static readonly string appPath = $"{appDir}\\RCS.WpfShop.exe";
        private static string controlTypeButtonLabel = "ControlType.Button";

        protected static WindowsDriver<WindowsElement> TestSession { get; private set; }

        public static void StartSession(TestContext testContext)
        {
            if (TestSession == null)
            {
                EndSession();

                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", appPath);
                appiumOptions.AddAdditionalCapability("appWorkingDir", appDir);

                // Note WinAppDriver.exe has to be started first.

                // Had to install Appium.WebDriver 4.0.0.4-beta with Selenium 3.141.0 to get this working.
                // This also implied making use of AppiumOptions.
                // https://github.com/appium/appium-dotnet-driver/issues/226
                TestSession = new WindowsDriver<WindowsElement>(new Uri(winAppDriverUrl), appiumOptions);

                Assert.IsNotNull(TestSession);
                Assert.IsNotNull(TestSession.SessionId);

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                TestSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
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
            TODO Get this straight.

            UPDATE 2
            For clarity: all current tests (unit or GUI) can be run in one go.
            TODO Check whether they can be run on a buildserver (Azure) together or that the tests should be split over kinds.
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
