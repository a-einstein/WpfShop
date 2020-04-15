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
        // Created because of various occurrences of the following:
        // OpenQA.Selenium.WebDriverException: An element could not be located on the page using the given search parameters
        // Compare with this description about the following and various approaches.
        // https://github.com/Microsoft/WinAppDriver/issues/370
        // Currently the problem only remains in an Azure pipeline, which may have a different cause.
        public static IWebElement FindElement(this IWebDriver webDriver, By by)
        {
            // Note WebDriverWait is a DefaultWait.
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60))
            {
                PollingInterval = TimeSpan.FromSeconds(1)
            };

            // Note that elements should be identified using inspect.exe first.
            return wait.Until(driver => driver.FindElement(by));
        }
    }
}
