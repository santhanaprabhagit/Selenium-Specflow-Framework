using Microsoft.Extensions.Configuration;
using Ngr.Ui_Tests.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace Ngr.Ui_Tests.DataProviders
{
    public class SettingsFileReader
    {
        private static IConfiguration envConfig;
        public SettingsFileReader()
        {
            envConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
        public string GetGUIUserSetting(string paramKey)
        {
            return envConfig.GetSection("guiUserSettings").GetValue<string>(paramKey);
        }
        public string getBaseUrl()
        {
            return envConfig.GetSection("guiEnvSettings").GetValue<string>("baseGUIUri");
        }
        public string getTargetBrowser()
        {
            return envConfig.GetSection("guiEnvSettings").GetValue<string>("targetBrowser");
        }
        public string getChromeWebdriverPath()
        {
            return envConfig.GetSection("guiEnvSettings").GetValue<string>("chomeWebDriverPath");
        }
        public string getFirefoxWebdriverPath()
        {
            return envConfig.GetSection("guiEnvSettings").GetValue<string>("firefoxWebDriverPath");
        }
        public string getTestScreenshotPath()
        {
            return envConfig.GetSection("guiEnvSettings").GetValue<string>("testScreenShotPath");
        }
    }
}
