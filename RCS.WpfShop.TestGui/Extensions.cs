using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        // Currently the problem only remains in an Azure pipeline, which may have a different cause.

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
                Timeout = TimeSpan.FromSeconds(30),
                PollingInterval = TimeSpan.FromSeconds(1),
            };

            // Note this exception will still be returned after the Timeout if the Until fails.
            wait.IgnoreExceptionTypes(typeof(WebDriverException));

            return wait;
        }
    }
}
