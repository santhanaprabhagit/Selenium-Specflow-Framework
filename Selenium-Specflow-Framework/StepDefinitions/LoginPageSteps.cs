﻿using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Ngr.Ui_Tests.Pages;
using Ngr.Ui_Tests.DataProviders;

namespace Ngr.Ui_Tests.StepDefinitions
{
    [Binding]
    public sealed class LoginPageSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private LoginPagePOM loginPagePOM;
        private MainPagePOM mainPagePOM;
        private SettingsFileReader settingsFile;
        public LoginPageSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            loginPagePOM = new LoginPagePOM((IWebDriver)_scenarioContext["webDriver"]);
            mainPagePOM = new MainPagePOM((IWebDriver)_scenarioContext["webDriver"]);
            settingsFile = new SettingsFileReader();
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
                    username = settingsFile.GetGUIUserSetting("guiTestUserName");
                    password = settingsFile.GetGUIUserSetting("guiTestUserPassword");
                    break;
                case "Nextuser":
                    username = settingsFile.GetGUIUserSetting("guiAdvTestUserName");
                    password = settingsFile.GetGUIUserSetting("guiAdvTestUserPassword");
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
    }
}
