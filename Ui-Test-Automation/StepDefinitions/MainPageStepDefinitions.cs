using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using UiTest.Pages;

namespace Ui_Test.StepDefinitions
{
    [Binding]
    public sealed class MainPageStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private LoginPagePOM loginPagePOM;
        private MainPagePOM mainPagePOM;

        public MainPageStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            mainPagePOM = new MainPagePOM((IWebDriver)_scenarioContext["webDriver"]);
            loginPagePOM = new LoginPagePOM((IWebDriver)_scenarioContext["webDriver"]);

        }
        [When(@"User clicks on the logout link")]
        public void UserClicksOnTheLogoutLink()
        {
            mainPagePOM.clickLogout();
        }
        [Then(@"User see the login page")]
        public void UserSeeTheLoginPage()
        {
            Assert.IsTrue(loginPagePOM.isLoginTextFieldAvailble(), "Current page is not a login page");
        }


    }


}
