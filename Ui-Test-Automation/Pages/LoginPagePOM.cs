using SeleniumExtras.PageObjects;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using UiTest.Helpers;

namespace UiTest.Pages
{
    class LoginPagePOM
    {
#pragma warning disable CS0649
        [FindsBy(How = How.XPath, Using = "//a[@href='/login']")]
        private IWebElement loginLink;
        [FindsBy(How = How.CssSelector, Using = "input[name=email]")]
        private IWebElement inputUserEmail;
        [FindsBy(How = How.CssSelector, Using = "input[name=password]")]
        private IWebElement inputUserPassword;
        [FindsBy(How = How.CssSelector, Using = "button[type='submit']")]
        private IWebElement buttonSubmit;
#pragma warning restore CS0649
        private IWebDriver webdriver;
        private WebDriverWait uiWait;

        public LoginPagePOM(IWebDriver webDriver)
        {
            webdriver = webDriver;
            PageFactory.InitElements(webDriver, this);
            uiWait = WaitHelper.createWebDriverWait(webDriver, 30);
        }
        public void clickLogin()
        {
            loginLink.Click();
        }
        public void enterLoginEmail(string userEmail)
        {
            inputUserEmail.SendKeys(userEmail);
        }
        public bool isLoginTextFieldAvailble()
        {
            return inputUserEmail.Enabled;
        }
        public void enterLoginPassword(string userPassword)
        {
            inputUserPassword.SendKeys(userPassword);
        }
        public void submitUserLogin()
        {
            buttonSubmit.Click();
        }
        public void waitForPageLoad()
        {
            uiWait.Until(WaitHelper.ElementIsDisplayed(inputUserEmail));
        }
        public void homePage()
        {
            uiWait.Until(WaitHelper.ElementIsDisplayed(inputUserEmail));
        }
    }
}
