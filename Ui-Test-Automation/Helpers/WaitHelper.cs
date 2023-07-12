using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ngr.Ui_Tests.Helpers
{
    public static class WaitHelper
    {
        public static Func<IWebDriver, bool> ElementIsDisplayed(IWebElement element)
        {
            return (driver) =>
            {
                try
                {
                    return element.Displayed;
                }
                catch (Exception)
                {
                    return false;
                }
            };
        }

        public static Func<IWebDriver, bool> ElementIsEnabled(IWebElement element)
        {
            return (driver) =>
            {
                try
                {
                    return element.Enabled;
                }
                catch (Exception)
                {
                    return false;
                }
            };
        }
        public static Func<IWebDriver, bool> ElementNotAvailable(IWebElement element)
        {
            return (driver) =>
            {
                try
                {
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            };
        }

        public static WebDriverWait createWebDriverWait(IWebDriver webDriver, int timeout, int pollInterval = 1)
        {
            WebDriverWait uiWait = new WebDriverWait(webDriver, timeout: TimeSpan.FromSeconds(timeout))
            {
                PollingInterval = TimeSpan.FromSeconds(pollInterval),
            };
            uiWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            return uiWait;
        }
    }
}
