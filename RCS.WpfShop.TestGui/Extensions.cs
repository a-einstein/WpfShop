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
        // Experimental
        public static IWebElement FindElement(this IWebDriver webDriver, By by)
        {
            // Note WebDriverWait is a DefaultWait.
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60))
            { 
                PollingInterval=TimeSpan.FromSeconds(1)
            };

            return wait.Until(driver => driver.FindElement(by));
        }
    }
}
