using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
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

        public static void Setup(TestContext testContext)
        {
            if (testSession == null)
            {
                TearDown();

                // TODO Change obsoletes.

                // Create a new session to bring up the Alarms & Clock application
                var appCapabilities = new DesiredCapabilities();

                appCapabilities.SetCapability("app", appPath);
                appCapabilities.SetCapability("appWorkingDir", appWorkingDir);

                // TODO Had to revert Selenium from 3.14.1 to 3.12.1 because of an error message.
                testSession = new WindowsDriver<WindowsElement>(new Uri(winAppDriverUrl), appCapabilities);

                Assert.IsNotNull(testSession);
                Assert.IsNotNull(testSession.SessionId);

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                testSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            }
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (testSession != null)
            {
                testSession.Quit();
                testSession = null;
            }
        }
    }
}
