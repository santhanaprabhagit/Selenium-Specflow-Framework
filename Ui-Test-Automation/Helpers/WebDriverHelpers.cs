using System.Runtime.InteropServices;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using System;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Codeuctivity;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace UiTest.Helpers
{
    public static class WebDriverHelpers
    {
        private static IConfiguration config;
        static WebDriverHelpers()
        {
            config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
        public static void createWebDriver(ScenarioContext scenContent, string targetBrowser, string chromeWDPath, string firefoxWDPath)
        {
            IWebDriver webDriver = null;
            const int maxRetry = 2;

            int tryCount = 0;
            bool initSuccess = false;

            while (!initSuccess)
            {
                try
                {
                    switch (targetBrowser.ToLower())
                    {
                        case "chrome":
                            webDriver = new ChromeDriver(chromeWDPath);
                            break;
                        case "firefox":
                            var firefoxOptions = new FirefoxOptions();
                            string webDriverFileName = "";
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            {
                                webDriverFileName = "geckodriver.exe";
                            }
                            else
                            {
                                webDriverFileName = "geckodriver";
                            }
                            FirefoxDriverService _firefoxDriverService = FirefoxDriverService.CreateDefaultService(firefoxWDPath, webDriverFileName);
                            _firefoxDriverService.Host = "::1";
                            webDriver = new FirefoxDriver(_firefoxDriverService, firefoxOptions);
                            break;
                        case "safari":
                            SafariOptions options = new SafariOptions();
                            webDriver = new SafariDriver(options);
                            break;
                    }

                    scenContent["webDriver"] = webDriver;
                    initSuccess = true;
                }
                catch (Exception ex)
                {
                    if (tryCount < maxRetry)
                    {
                        Debug.WriteLine(string.Format("==== Brower {0} initialization failure on Attempt #{1}.  Retry", targetBrowser, tryCount.ToString()));
                        Debug.WriteLine(string.Format("==== Exception Message = {0}", ex.Message));
                        cleanUpWebDriverInstances(targetBrowser);
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("==== Brower {0} initialization failure.  Max. retry count reached.  Halt!!", targetBrowser));
                        throw new Exception(string.Format("Fail to initialize {0} webdriver - {1}", targetBrowser, ex.Message));

                    }
                    tryCount++;
                }
            }
        }
        private static void cleanUpWebDriverInstances(string targetBrowser)
        {
            string targetProcessName = "";

            switch (targetBrowser.ToLower())
            {
                case "chrome":
                    targetProcessName = "ChromeDriver";
                    break;
                case "firefox":
                    targetProcessName = "geckodriver";
                    break;
                case "safari":
                    targetProcessName = "safaridriver";
                    break;
            }

            Process[] webDriverProcessList = Process.GetProcessesByName(targetProcessName);

            foreach (Process wdProcess in webDriverProcessList)
            {
                wdProcess.Kill(true);
            }

        }

        public static void browseURL(ScenarioContext scenContent, string url)
        {
            IWebDriver webDriver;

            webDriver = (IWebDriver)scenContent["webDriver"];
            webDriver.Url = url;
            scenContent["baseUrl"] = url;
        }
        public static void maximizeBrowserWindow(ScenarioContext scenContent)
        {
            IWebDriver webDriver;

            webDriver = (IWebDriver)scenContent["webDriver"];
            webDriver.Manage().Window.Maximize();
        }

        public static void resizeBrowser(ScenarioContext scenContent, int width, int height)
        {
            IWebDriver webDriver;

            webDriver = (IWebDriver)scenContent["webDriver"];
            System.Drawing.Size newBrowserSize = new System.Drawing.Size(width, height);
            webDriver.Manage().Window.Size = newBrowserSize;
        }

        public static string takeScrShot(ScenarioContext scenContent, string targetPath, string targetElementCSS = "", string elementImageName = "")
        {
            IWebDriver webDriver = (IWebDriver)scenContent["webDriver"];
            ISpecFlowOutputHelper sfOutputHelper = (ISpecFlowOutputHelper)scenContent["sfOutputHelper"];
            string currentTestName = scenContent.ScenarioInfo.Title;

            return takeScrShot(webDriver, sfOutputHelper, currentTestName, targetPath, targetElementCSS, elementImageName);
        }
        public static string takeScrShot(IWebDriver webDriver, ISpecFlowOutputHelper sfOutputHelper, string currentTestName, string targetPath, string targetElementCSS = "", string elementImageName = "", int cropImgWidth = 0, int cropImgHeight = 0)
        {
            string scrShotFilePath = targetPath + Path.DirectorySeparatorChar + @"ScrShot_" + currentTestName + ".png";
            Screenshot ss = ((ITakesScreenshot)webDriver).GetScreenshot();
            ss.SaveAsFile(scrShotFilePath, ScreenshotImageFormat.Png);

            if (targetElementCSS != "")
            {
                IWebElement targetElement = webDriver.FindElement(By.CssSelector(targetElementCSS));
                string cropImageFilePath = targetPath + Path.DirectorySeparatorChar + @"ScrShot_" + currentTestName + elementImageName + "_webElement.png";

                Image fullScrImage = Image.Load(scrShotFilePath);
                Rectangle cropRect = new Rectangle(
                    targetElement.Location.X, targetElement.Location.Y,
                    targetElement.Size.Width, targetElement.Size.Height
                );
                fullScrImage.Mutate(x => x.Crop(calCropRectagngle(targetElement, cropImgWidth, cropImgHeight)));
                fullScrImage.SaveAsPng(cropImageFilePath);

                sfOutputHelper.AddAttachment(cropImageFilePath);
                return cropImageFilePath;
            }
            else
            {
                sfOutputHelper.AddAttachment(scrShotFilePath);
                return scrShotFilePath;
            }
        }

        static private Rectangle calCropRectagngle(IWebElement targetElement, int cropWidth, int cropHeight)
        {
            if (cropWidth > targetElement.Size.Width || cropWidth == 0)
            {
                cropWidth = targetElement.Size.Width;
            }
            if (cropHeight > targetElement.Size.Height || cropHeight == 0)
            {
                cropHeight = targetElement.Size.Height;
            }
            Point centerPoint = new Point(targetElement.Size.Width / 2, targetElement.Size.Height / 2);
            Rectangle calRect = new Rectangle(
                centerPoint.X - cropWidth / 2 + targetElement.Location.X,
                centerPoint.Y - cropHeight / 2 + targetElement.Location.Y,
                cropWidth, cropHeight
            );

            return calRect;
        }
        public static bool compareImage(ISpecFlowOutputHelper sfOutputHelper, string refImageFullPath, string actualImageFullPath, string diffImageTargetFolder, string descrption = "", string compMaskImagePath = "", double tolerance = 0)
        {
            var diffImageFullPath = diffImageTargetFolder + Path.DirectorySeparatorChar + @"diffImage" + descrption + ".png";
            Image<SixLabors.ImageSharp.PixelFormats.Rgba32> expectedImage = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(refImageFullPath);
            Image<SixLabors.ImageSharp.PixelFormats.Rgba32> actualImage = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(actualImageFullPath);
            string outputExpectedImagePath = diffImageTargetFolder + Path.DirectorySeparatorChar + "diffExpectedImage_" + descrption + ".png";
            string outputActualImagePath = diffImageTargetFolder + Path.DirectorySeparatorChar + "diffActualImage_" + descrption + ".png";
            expectedImage.SaveAsPng(outputExpectedImagePath);
            actualImage.SaveAsPng(outputActualImagePath);

            sfOutputHelper.AddAttachment(outputExpectedImagePath);
            sfOutputHelper.AddAttachment(outputActualImagePath);

            ICompareResult compResult = null;
            if (compMaskImagePath == "")
            {
                compResult = ImageSharpCompare.CalcDiff(actualImageFullPath, refImageFullPath);
            }
            else
            {
                Image<SixLabors.ImageSharp.PixelFormats.Rgba32> maskImage = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(compMaskImagePath);
                string outputMaskImagePath = diffImageTargetFolder + Path.DirectorySeparatorChar + "diffMaskImage_" + descrption + ".png";
                maskImage.SaveAsPng(outputMaskImagePath);
                sfOutputHelper.AddAttachment(outputMaskImagePath);

                compResult = ImageSharpCompare.CalcDiff(actualImageFullPath, refImageFullPath, compMaskImagePath);
            }

            using (var fileStreamDifferenceMask = File.Create(diffImageFullPath))
            {
                using (var diffImage = ImageSharpCompare.CalcDiffMaskImage(actualImageFullPath, refImageFullPath))
                {
                    diffImage.SaveAsPng(fileStreamDifferenceMask);
                }
            }

            sfOutputHelper.AddAttachment(diffImageFullPath);
            return compResult.MeanError <= tolerance;
        }

        public static string getBaseUrl()
        {
            string baseUrl = config.GetSection("guiEnvSettings").GetValue<string>("baseGUIUri");
            return baseUrl;
        }
        public static void destroyWebDriver(ScenarioContext scenContent)
        {
            IWebDriver webDriver;

            webDriver = (IWebDriver)scenContent["webDriver"];
            webDriver.Close();
            webDriver.Dispose();
        }
    }
}
