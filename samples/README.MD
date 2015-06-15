SampleApp
===============

Sample SPEAK application with JS integration tests

## How to install
1. Copy serialization folder into data folder of your website.
2. Revert "core" database.
3. Copy content of SampleApp folder into your site Website folder.
4. Open SampleApp.sln in Visual Studio.
5. Compile solution.
6. Restart Visual Studio and open Package Manager Console. This action will trigger TestRunnerJS installation.

## How to run in browser
1. Open following URLs:  
`/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage?sc_testrunnerjs=1`  
`/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage/Pages/SecondPage?sc_testfixture=additionaltests&sc_testrunnerjs=1`  
`/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage/Pages/SecondPage?sc_testrunnerjs=1`  
2. Ensure you have opened browser console and check tests output there

## How to run using powershell script
1. Open Package Manager Console in Visual Studio
2. Run in console `Execute-TestRunner <instance_name> <app_name>`  
Where:  
`<instance_name>` - Your website instance name  
`<app_name>` - Speak application name (SampleApp) 

## Running js integration tests using xUnit or othe C# test frameworks
1. Open app.config in SampleApp.Tests
2. Change to the actual values instance name, application name and Sitecore.TestRunnerJS package path settings
3. Run sample test that is located in SampleTest class