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

        // TODO Make this more generic.
        private const string appPath = @"P:\projects\RCS\shopping\clients\WpfShop\RCS.WpfShop\bin\Debug\RCS.WpfShop.exe";
        private const string appWorkingDir = @"P:\projects\RCS\shopping\clients\WpfShop\RCS.WpfShop\bin\Debug";

        protected static WindowsDriver<WindowsElement> testSession;

        public static void SetUp(TestContext testContext)
        {
            if (testSession == null)
            {
                TearDown();

                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", appPath);
                appiumOptions.AddAdditionalCapability("appWorkingDir", appWorkingDir);

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

        public static void TearDown()
        {
            testSession?.Quit();
            testSession = null;
        }
    }
}
