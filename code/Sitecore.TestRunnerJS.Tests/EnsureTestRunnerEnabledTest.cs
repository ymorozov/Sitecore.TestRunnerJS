namespace Sitecore.TestRunnerJS.Tests
{
  using System.Collections;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.TestRunnerJS.Pipelines.HttpRequest;
  using Xunit;

  public class EnsureTestRunnerEnabledTest
  {
    private readonly SitecoreWrapper wrapper;

    private readonly ConfigSettings settings;

    private readonly EnsureTestRunnerEnabled processor;

    private readonly Hashtable sitecoreSettings;

    private const string RequireJSSettingName = "RequireJSSetting";

    private const string RequireJSSettingValue = "RequireJSSettingValue";

    private const string BootstrapModulePath = "BootstrapModulePath";

    public EnsureTestRunnerEnabledTest()
    {
      this.sitecoreSettings = new Hashtable { { RequireJSSettingName, RequireJSSettingValue } };

      this.wrapper = Substitute.For<SitecoreWrapper>();
      this.wrapper.GetSitecoreSettings().Returns(this.sitecoreSettings);

      this.settings = Substitute.For<ConfigSettings>();
      this.settings.RequireJSSettingName.Returns(RequireJSSettingName);
      this.settings.BootstrapModulePath.Returns(BootstrapModulePath);

      this.processor = new EnsureTestRunnerEnabled(this.settings, this.wrapper);
    }

    [Fact]
    public void ShouldEnableDebugOnTestrunnerEnabling()
    {
      // arrange
      this.wrapper.GetQueryString(EnsureTestRunnerEnabled.EnableTestRunnerParameter).Returns("1");

      // act
      this.processor.Process(null);

      // assert
      this.wrapper.Debugging.Should().BeTrue();
    }

    [Fact]
    public void ShouldSwitchBootstrapModuleOnTestrunnerEnabling()
    {
      // arrange
      this.wrapper.GetQueryString(EnsureTestRunnerEnabled.EnableTestRunnerParameter).Returns("1");

      // act
      this.processor.Process(null);

      // assert
      this.sitecoreSettings[RequireJSSettingName].Should().Be(BootstrapModulePath);
    }

    [Fact]
    public void ShouldRestoreBootstrapModuleOnTestRunnerDisablingFromQueryString()
    {
      // arrange
      this.EnableTestRunner();
      this.wrapper.GetQueryString(EnsureTestRunnerEnabled.EnableTestRunnerParameter).Returns("0");


      // act
      this.processor.Process(null);

      // assert
      this.sitecoreSettings[RequireJSSettingName].Should().Be(RequireJSSettingValue);
    }

    [Fact]
    public void ShouldRestoreDebugModeIfTestRunnerIsOn()
    {
      // arrange
      this.EnableTestRunner();
      this.wrapper.Debugging = false;
      this.wrapper.GetQueryString(EnsureTestRunnerEnabled.EnableTestRunnerParameter).Returns((string)null);

      // act
      this.processor.Process(null);

      // assert
      this.wrapper.Debugging.Should().BeTrue();
    }

    private void EnableTestRunner()
    {
      this.wrapper.GetQueryString(EnsureTestRunnerEnabled.EnableTestRunnerParameter).Returns("1");
      this.processor.Process(null);
    }
  }
}
