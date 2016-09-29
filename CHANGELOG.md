0.11.2
* [FIX] Remove jQuery usage from loader.js

0.11.1  
* [FIX] Use SettingSwitcher instead of Reflection to change setting value 

0.11.0  
* [NEW] Set default root application path in config   

0.10.0  
* [NEW] Run JS integration tests from C# test frameworks such as xUnit or NUnit.   
* [NEW] Create additional Sitecore.TestRunnerJS.Extensions nuget package for additional tasks such as running from C#

0.9.1  
* [FIX] Escape phantomjs input parameters  
* [FIX] Should fail on phantomjs error 

0.9.0  
* [NEW] #7: Generate html report  
* [NEW] #13: Run different sets of tests by grep  
* [NEW] Specify page under test url from command line  
* [NEW] Imporve command line parameters  

0.8.0  
* [NEW] #2: Load test fixtures and phantomjs settings directly from server  
* [NEW] Show info message if testing disabled in test fixture  
* [NEW] #8: Move all SPEAK debug output into console group  
* [NEW] #5: Output fake requests/responses into console  
* [NEW] #3: Display expected application settings file location if not found  

0.7.2  
* [FIX] Support PhantomJS from 1.9.1 to 1.9.8 

0.7.1  
* [FIX] #9: Fix issue with not working assertions in asynchronous tests 

0.7.0  
* [NEW] #6: Run all tests from VS in Package Manager Console

0.6.3  
* [FIX] Restore TestRunner files in projects on Package Manage Console init

0.6.2  
* [FIX] Install TestRunner nuget package as project level package

0.6.1  
* [FIX] Fix issue with routes registration on newest Sitecore. Use initialize pipeline instead of WebActivator

0.6.0  
* Initial public release with nuget package