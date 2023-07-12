using System;
using Microsoft.Extensions.Configuration;
using Ngr.Ui_Tests.Helpers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace Ngr.Ui_Tests
{
    [Binding]
    public sealed class Hooks1
    {
        public static IConfiguration envConfig;
        private readonly ScenarioContext _scenarioContext;
        private static bool initiateGUI;

        public Hooks1(ScenarioContext scenarioContext, ISpecFlowOutputHelper sfOuputHelper)
        {
            _scenarioContext = scenarioContext;
            _scenarioContext["sfOutputHelper"] = sfOuputHelper;
        }
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            envConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appSettings.json")
                .Build();
            initiateGUI = (envConfig.GetValue<string>("testProjectType").ToLower() == "gui");
        }
        [BeforeScenario]
        public void BeforeScenario()
        {

            if (Hooks1.initiateGUI == true)
            {
                WebDriverHelpers.createWebDriver(
                    _scenarioContext,
                    envConfig.GetSection("guiEnvSettings").GetValue<string>("targetBrowser"),
                    envConfig.GetSection("guiEnvSettings").GetValue<string>("chomeWebDriverPath"),
                    envConfig.GetSection("guiEnvSettings").GetValue<string>("firefoxWebDriverPath")
                );
                WebDriverHelpers.maximizeBrowserWindow(_scenarioContext);
                WebDriverHelpers.browseURL(_scenarioContext, envConfig.GetSection("guiEnvSettings").GetValue<string>("baseGUIUri"));
            }
            else
            {
                _scenarioContext["debugMsg"] = envConfig.GetValue<string>("testProjectType");
            }
        }
        [AfterScenario]
        public void AfterScenario()
        {
            if (Hooks1.initiateGUI)
            {
                WebDriverHelpers.takeScrShot(_scenarioContext, envConfig.GetSection("guiEnvSettings").GetValue<string>("testScreenShotPath"));
                WebDriverHelpers.destroyWebDriver(_scenarioContext);
            }
        }
    }
}
