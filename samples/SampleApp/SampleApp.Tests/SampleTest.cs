namespace SampleApp.Tests
{
  using Sitecore.TestRunnerJS.Extensions;
  using Xunit;

  public class SampleTest
  {
    [Fact]
    public void ShouldRunPhantomTest()
    {
      // Create instance of runner that is responsible for running JS integration tests
      var runner = TestRunner.Create();

      // Execute specified test
      // In order to execute one should pass page under test and 
      // mocha grep that will filter out and run necessary tests
      runner.Execute(
        "/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage?sc_testrunnerjs=1",
        "Another page Should load test from ActionTextBox into ResultTextBox on Action button click"
      );
    }
  }
}
