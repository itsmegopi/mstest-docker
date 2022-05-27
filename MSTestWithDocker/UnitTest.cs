using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Reflection;

namespace MSTestWithDocker
{
    [TestClass]
    public class UnitTest
    {
        readonly string test_url = "https://lambdatest.github.io/sample-todo-app/"; // website or page to test
        readonly string itemName = "Yey, Let's add it to list";
        readonly bool isLocal = false; // if you are running docker or jenkins etc.. make it false
        private IWebDriver _driver;

        [TestInitialize]
        public void TestInitialize()
        {
            /// My chrome _driver is in bin folder 
            var pathLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var options = new ChromeOptions();
            if (isLocal)
            {
                // Local Selenium Web_driver
                _driver = new ChromeDriver(pathLoc, options);
            }
            else
            {
                // if this is not used then chrome session will not create 
                /// https://developers.google.com/web/tools/puppeteer/troubleshooting#tips
                options.AddArgument("--disable-dev-shm-usage");
                // remote hub path
                var remoteWeb_driverUrl = "http://standalone-chrome:4444/wd/hub";
                _driver = new RemoteWebDriver(new Uri(remoteWeb_driverUrl), options);
            }
            IWait<IWebDriver> wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(test_url);
        }
        
        [TestMethod]
        public void VerifyItem()
        {
            // Click on First Check box
            IWebElement firstCheckBox = _driver.FindElement(By.Name("li1"));
            firstCheckBox.Click();

            // Click on Second Check box
            IWebElement secondCheckBox = _driver.FindElement(By.Name("li2"));
            secondCheckBox.Click();

            // Enter Item name
            IWebElement textfield = _driver.FindElement(By.Id("sampletodotext"));
            textfield.SendKeys(itemName);

            // Click on Add button
            IWebElement addButton = _driver.FindElement(By.Id("addbutton"));
            addButton.Click();

            // Verified Added Item name
            IWebElement itemtext = _driver.FindElement(By.XPath("/html/body/div/div/div/ul/li[6]/span"));
            string getText = itemtext.Text;
            Assert.IsTrue(itemName.Contains(getText));

            /* Perform wait to check the output in this MSTest tutorial for Selenium */
            System.Threading.Thread.Sleep(10000);

            _driver.Quit();
        }
    }
}
