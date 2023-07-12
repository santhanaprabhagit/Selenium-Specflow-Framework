using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Ngr.Ui_Tests.Pages;

namespace Ngr.Ui_Tests.StepDefinitions
{
    [Binding]
    public sealed class LoginPageStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private LoginPagePOM loginPagePOM;
        private MainPagePOM mainPagePOM;
        private IConfiguration envConfig;
        public LoginPageStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            loginPagePOM = new LoginPagePOM((IWebDriver)_scenarioContext["webDriver"]);
            mainPagePOM = new MainPagePOM((IWebDriver)_scenarioContext["webDriver"]);
            envConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appSettings.json")
                .Build();
        }

        [Given(@"User is login in using '([^']*)' account")]
        public void UserIsLoginInUsingAccount(string accountType)
        {
            loginPagePOM.clickLogin();
            string username = "";
            string password = "";
            switch (accountType)
            {
                case "Testuser":
                    username = GetGUIUserSetting("guiTestUserName");
                    password = GetGUIUserSetting("guiTestUserPassword");
                    break;
                case "Nextuser":
                    username = GetGUIUserSetting("guiAdvTestUserName");
                    password = GetGUIUserSetting("guiAdvTestUserPassword");
                    break;
                default:
                    Assert.Fail(accountType + " is not recognized user account type");
                    break;
            }
            loginPagePOM.waitForPageLoad();
            loginPagePOM.enterLoginEmail(username);
            loginPagePOM.enterLoginPassword(password);
            loginPagePOM.submitUserLogin();
            Assert.IsTrue(mainPagePOM.getLoggedInUserText().Contains("Logged in"), "User is not logged in");
        }

        private string GetGUIUserSetting(string paramKey)
        {
            return envConfig.GetSection("guiUserSettings").GetValue<string>(paramKey);
        }
    }
}
