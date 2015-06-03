namespace Sitecore.TestRunnerJS.Pipelines.HttpRequest
{
  using Sitecore.Pipelines.HttpRequest;
  using Sitecore.TestRunnerJS;

  public class EnsureTestRunnerEnabled : HttpRequestProcessor
  {
    public const string EnableTestRunnerParameter = "sc_testrunnerjs";

    private readonly ConfigSettings settings;

    private readonly SitecoreWrapper wrapper;

    private object previousValue;

    public EnsureTestRunnerEnabled(ConfigSettings settings, SitecoreWrapper wrapper)
    {
      this.settings = settings;
      this.wrapper = wrapper;
    }

    public override void Process(HttpRequestArgs args)
    {
      var settingName = this.settings.RequireJSSettingName;
      var bootstrapPath = this.settings.BootstrapModulePath;
      var configurationSettings = this.wrapper.GetSitecoreSettings();

      if (this.wrapper.GetQueryString(EnableTestRunnerParameter) != "1")
      {
        if (this.wrapper.GetQueryString(EnableTestRunnerParameter) != "0")
        {
          if (this.previousValue != null)
          {
            this.wrapper.Debugging = true;
          }

          return;
        }

        if (this.previousValue == null)
        {
          return;
        }

        configurationSettings[settingName] = this.previousValue;
        this.wrapper.Debugging = false;

        return;
      }

      this.wrapper.Debugging = true;
      if ((string)configurationSettings[settingName] == bootstrapPath)
      {
        return;
      }

      this.previousValue = configurationSettings[settingName];
      configurationSettings[settingName] = bootstrapPath;
    }
  }
}
