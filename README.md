# Selenium Cucumber Framework

---

## Framework Purpose
This project aims to give an inspiring or sample of automation test framework that uses Selenium and Specflow with C# as the programming language.
Sample web page used to test : https://www.automationexercise.com/
Please register at the above website and update your credentials into appsettings.json

## Tools and Libraries
This project using 2 main tools, Selenium and Specflow.
On the other hand, I using some of the tools that support this great framework.
The complete list of tools, you can see in the `Selenium-Specflow-Framework.csproj` file.

## Code development
* The C# based Specflow BDD test framework
* Page object Model

## Requirements
* Download and Install Visual studio 2022 or latest
* Add specflow to Visual Studio via the "Manage Extensions" menu
* Download the chromedriver for the installed chrome browser version(chrome driver path needs to be updated in appsettings.json) 

## Running Tests

* Clone this project from Git path https://github.com/prabhadevarajgit/Ui-Test-Automation.git
* Build your project and ensure the build is success.
* Refer appsettings.json file to update below details to work based on need
        * guiTestUserName
        * guiTestUserPassword
        * testScreenShotPath
        * baseGUIUri
* Build your project once again and ensure the build is success
* Open Test explorer and Run tests
* Results/Report - Working in progress for the better reporting format

## Adding new tests/feature file

* Create a new SpecFlow feature by right-clicking on the project name --> Add --> New Item --> Visual C# Items --> SpecFlow Feature File. Name the feature
* Add your steps on the above feature file
* Generate / Create a step defintion for the same
* Implement the Method


## The following should be noted
* Specflow hook (for setup and teardown events on test cases/ whole test run) will take effect on underling test project execution


