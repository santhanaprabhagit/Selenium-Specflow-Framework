using SeleniumExtras.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Ngr.Ui_Tests.Helpers;

namespace Ngr.Ui_Tests.Pages
{
    class MainPagePOM
    {
#pragma warning disable CS0649
        [FindsBy(How = How.XPath, Using = "//a[@href='/logout']")]
        private IWebElement logout;
        [FindsBy(How = How.XPath, Using = "//i[contains(@class,'fa-user')]//parent::a")]
        private IWebElement loggedInUser;
#pragma warning restore CS0649

        private IWebDriver webdriver;
        private WebDriverWait uiWait;

        public MainPagePOM(IWebDriver webDriver)
        {
            webdriver = webDriver;
            PageFactory.InitElements(webDriver, this);
            uiWait = WaitHelper.createWebDriverWait(webDriver, 30);
        }
        public void clickLogout()
        {
            logout.Click();
        }
        public string getLoggedInUserText()
        {
            return loggedInUser.GetAttribute("innerText");
        }
    }
}
