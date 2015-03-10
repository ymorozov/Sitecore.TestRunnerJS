// Contains list of all pages with test fixtures to run under phantomjs on CI
testPages = [
  // Runs test fixture firstpage.js for page 'FirstPage'
  "/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage?sc_testrunnerjs=1",
  
  // Runs test fixture additionaltests.js for page 'SecondPage'
  "/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage/Pages/SecondPage?sc_testfixture=additionaltests&sc_testrunnerjs=1",
  
  // Runs test fixture secondpage.js for page 'SecondPage'
  "/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage/Pages/SecondPage?sc_testrunnerjs=1"
];