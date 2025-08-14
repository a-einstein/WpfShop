using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;

namespace RCS.WpfShop.TestGui
{
    public static class Extensions
    {
        // Note that elements should be identified using inspect.exe first.

        // Created because of various occurrences of the following:
        // OpenQA.Selenium.WebDriverException: An element could not be located on the page using the given search parameters
        // Compare with this description about the following and various approaches.
        // https://github.com/Microsoft/WinAppDriver/issues/370
        //
        // Currently occurrence of the the problem is pretty unpredictable, and may or may not be while running in Visusal Studio or in an Azure pipeline, 
        // which may have a different cause.
        // Changing Timeout seemed to help, even though it wasn't actually used as long as set. Setting the value back did not recreate the problem, 
        // which makes no sense.
        //
        // Exceptions still seem to originate here, but I have not been able to catch them.
        // Not applying MainApplication.SetupExceptionHandling prevents them from turning up through the GUI.
        // The actual GUI test may continue undisturbed despite exceptions and even report success.

        public static AppiumWebElement FindElementWait(this WindowsDriver<AppiumWebElement> windowsDriver, By by)
        {
            return StandardWait(windowsDriver).Until(driver => driver.FindElement(by));
        }

        public static AppiumWebElement FindElementByAccessibilityIdWait(this WindowsDriver<AppiumWebElement> windowsDriver, string id)
        {
            return StandardWait(windowsDriver).Until(driver => driver.FindElementByAccessibilityId(id));
        }

        private static DefaultWait<WindowsDriver<AppiumWebElement>> StandardWait(WindowsDriver<AppiumWebElement> windowsDriver)
        {
            var wait = new DefaultWait<WindowsDriver<AppiumWebElement>>(windowsDriver)
            {
                Timeout = TimeSpan.FromSeconds(20),
                PollingInterval = TimeSpan.FromSeconds(1),
            };

            // Note this exception will still be returned after the Timeout if the Until fails.
            wait.IgnoreExceptionTypes(typeof(WebDriverException));

            return wait;
        }
    }
}
