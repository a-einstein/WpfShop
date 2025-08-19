using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
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

        // Note that to succeed it's generally better to move Visual Studio
        // to a DIFFERENT SCREEN than where the test applications appear.

        static GuiTest()
        {
            // Use the namespace as this testclass is functionally linked to it.
            var classNameParts = typeof(GuiTest).FullName.Split(new[] { '.' });
            mainName = String.Join(".", classNameParts, 0, 2);

            // TODO This should follow the current framework.
            appDir = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\..\\{mainName}\\bin\\Test\\net9.0-windows10.0.26100.0";
            appPath = $"{appDir}\\{mainName}.exe";
        }

        protected static void StartSession(TestContext testContext)
        {
            if (TestSession == null)
            {
                EndSession();

                var appiumOptions = new AppiumOptions();

                appiumOptions.AddAdditionalCapability("app", appPath);
                appiumOptions.PlatformName = "windows";
                appiumOptions.AddAdditionalCapability("appWorkingDir", appDir);

                // Debugging.
                /*
                var capabilities = appiumOptions.ToCapabilities();
                var optionsDictionary = appiumOptions.ToDictionary();
                */

                // Note WinAppDriver.exe has to be started first.

                // Note WindowsDriver is an AppiumDriver.

                // Note Appium.WebDriver cannot be higher than 4.4.5.
                // Otherwise exception "Bad capabilities. Specify either app or appTopLevelWindow to create a session".
                // Because of just a prefix-issue as in https://github.com/appium/dotnet-client/issues/487
                // Previous commit had been adapted to 8.0.0.
                TestSession = new WindowsDriver<AppiumWebElement>(new Uri(winAppDriverUrl), appiumOptions);

                Assert.IsNotNull(TestSession);
                Assert.IsNotNull(TestSession.SessionId);
            }
        }

        protected static void NavigateTo(string destination)
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
            var byClassName = By.ClassName(name);
            var element = TestSession.FindElement(byClassName);

            Assert.IsNotNull(element);
        }

        protected static void EndSession()
        {
            TestSession?.Quit();
            TestSession = null;
        }
    }
}