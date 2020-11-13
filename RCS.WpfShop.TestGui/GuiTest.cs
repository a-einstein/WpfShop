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

        protected static WindowsDriver<AppiumWebElement> TestSession { get; private set; }

        static GuiTest()
        {
            // Use the namespace as this testclass is functionally linked to it.
            var classNameParts = typeof(GuiTest).FullName.Split(new char[] { '.' });
            mainName = String.Join(".", classNameParts, 0, 2);

            appDir = $"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\..\\..\\{mainName}\\bin\\Test\\netcoreapp3.1";
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

                // Note WindowsDriver is an AppiumDriver.
                TestSession = new WindowsDriver<AppiumWebElement>(new Uri(winAppDriverUrl), appiumOptions);

                Assert.IsNotNull(TestSession);
                Assert.IsNotNull(TestSession.SessionId);
            }
        }

        public static void NavigateTo(string destination)
        {
            TestSession.FindElementWait(By.Name(destination)).Click();

            // Suggested: Test presence of module controls.
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
}
