using TechTalk.SpecFlow;
using OpenQA.Selenium;
using NUnit.Framework;
using Ngr.Ui_Tests.Pages;
using System;

namespace Ngr.Ui_Tests.StepDefinitions
{
    [Binding]
    public sealed class MainPageSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private LoginPagePOM loginPagePOM;
        private MainPagePOM mainPagePOM;

        public MainPageSteps(ScenarioContext scenarioContext)
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
