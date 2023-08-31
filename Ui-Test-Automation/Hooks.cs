using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Ngr.Ui_Tests.DataProviders;
using Ngr.Ui_Tests.Helpers;
using TechTalk.SpecFlow;

namespace Ngr.Ui_Tests
{
    [Binding]
    public sealed class Hooks
    {
        public static IConfiguration envConfig;
        private readonly ScenarioContext _scenarioContext;
        private SettingsFileReader settingsFile;


        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            settingsFile = new SettingsFileReader();
        }
        [BeforeScenario]
        public void BeforeScenario()
        {
            WebDriverHelpers.createWebDriver(
                    _scenarioContext,
                    settingsFile.getTargetBrowser(),
                    settingsFile.getChromeWebdriverPath(),
                    settingsFile.getFirefoxWebdriverPath()
                );
            WebDriverHelpers.maximizeBrowserWindow(_scenarioContext);
            WebDriverHelpers.browseURL(_scenarioContext, settingsFile.getBaseUrl());
        }
        [AfterScenario]
        public void AfterScenario()
        {
            if (_scenarioContext.ScenarioExecutionStatus.ToString().Equals("TestError"))
            {
                WebDriverHelpers.takeScrShot(_scenarioContext, settingsFile.getTestScreenshotPath());
                WebDriverHelpers.destroyWebDriver(_scenarioContext);
            }
            else
            {
                WebDriverHelpers.destroyWebDriver(_scenarioContext);
            }
        }
        [AfterTestRun]
        public static void GenerateTestReport()
        {
            Thread.Sleep(1000); //Wait time before generating the report post execution
            string currentDirectory = Directory.GetCurrentDirectory();
            string reportBatchFileName = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\")) + "LivingDocHtmlReport.bat";
            string batchFilePath = Path.Combine(currentDirectory, reportBatchFileName);

            if (File.Exists(batchFilePath))
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(batchFilePath)
                    {
                        WorkingDirectory = currentDirectory,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (Process process = new Process())
                    {
                        process.StartInfo = startInfo;

                        process.Start();
                        process.WaitForExit();
                        int exitCode = process.ExitCode;
                        Console.WriteLine($"Batch file exited with code: {exitCode}");
                   
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Batch file '{reportBatchFileName}' not found in the specified folder.");
            }
        }
    }
       
    }
