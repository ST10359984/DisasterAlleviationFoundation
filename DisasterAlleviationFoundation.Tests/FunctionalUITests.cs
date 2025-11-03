using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class FunctionalUITests
    {
        private IWebDriver _driver = null!;

        private const string AppUrl = "http://localhost:5270";

        private const string TestUserEmail = "test@example.com";
        private const string TestUserPassword = "Password123!";

        [TestInitialize]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            _driver = new ChromeDriver();
        }

        [TestMethod]
        public void Login_ShouldSucceed_WithValidCredentials()
        {
            _driver.Navigate().GoToUrl(AppUrl);

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            _driver.FindElement(By.LinkText("Login")).Click();

            var emailField = wait.Until(d => d.FindElement(By.Id("Input_Email")));

            emailField.SendKeys(TestUserEmail);
            _driver.FindElement(By.Id("Input_Password")).SendKeys(TestUserPassword);
            _driver.FindElement(By.Id("login-submit")).Click();

            var helloLink = wait.Until(d => d.FindElement(By.LinkText($"Hello {TestUserEmail}!")));

            Assert.IsNotNull(helloLink);
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}
