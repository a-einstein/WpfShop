using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace RCS.WpfShop.TestGui
{
    [TestClass]
    public class GuiTest
    {
        private const string winAppDriverUrl = "http://127.0.0.1:4723";

        // Constants (but not markable as such.) 
        protected static string appDir = @"P:\projects\RCS\shopping\clients\WpfShop\RCS.WpfShop\bin\Test";
        protected static string appPath = $"{appDir}\\RCS.WpfShop.exe";
        protected static string controlTypeButtonLabel = "ControlType.Button";

        protected static WindowsDriver<WindowsElement> testSession;

        public static void StartSession(TestContext testContext)
        {
            if (testSession == null)
            {
                EndSession();

                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", appPath);
                appiumOptions.AddAdditionalCapability("appWorkingDir", appDir);

                // Note WinAppDriver.exe has to be started first.

                // Had to install Appium.WebDriver 4.0.0.4-beta with Selenium 3.141.0 to get this working.
                // This also implied making use of AppiumOptions.
                // https://github.com/appium/appium-dotnet-driver/issues/226
                testSession = new WindowsDriver<WindowsElement>(new Uri(winAppDriverUrl), appiumOptions);

                Assert.IsNotNull(testSession);
                Assert.IsNotNull(testSession.SessionId);

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                testSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            }
        }

        public static void NavigateTo(string destination)
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
            var navigateButton = testSession.FindElementByName(destination);
            Assert.AreEqual(navigateButton?.TagName, controlTypeButtonLabel);

            navigateButton.Click();

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
            var element = testSession.FindElementByClassName(name);
            Assert.IsNotNull(element);
        }

        public static void EndSession()
        {
            testSession?.Quit();
            testSession = null;
        }
    }
}
