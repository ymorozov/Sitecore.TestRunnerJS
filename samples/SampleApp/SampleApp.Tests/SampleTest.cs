namespace SampleApp.Tests
{
  using Sitecore.TestRunnerJS.Extensions;
  using Xunit;

  public class SampleTest
  {
    [Fact]
    public void ShouldRunPhantomTest()
    {
      var runner = TestRunner.Create();
      runner.Execute(
        "/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage?sc_testrunnerjs=1",
        "Another page Should load test from ActionTextBox into ResultTextBox on Action button click"
      );
    }
  }
}
