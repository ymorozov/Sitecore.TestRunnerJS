namespace Sitecore.TestRunnerJS.Pipelines.HttpRequest
{
  using System.Collections;
  using System.Reflection;
  using Sitecore.Configuration;
  using Sitecore.Pipelines.HttpRequest;
  using Sitecore.TestRunnerJS;
  using Sitecore.Web;

  public class EnsureTestRunnerEnabled : HttpRequestProcessor
  {
    private readonly ConfigSettings settings;

    private const string EnableTestRunnerParameter = "sc_testrunnerjs";

    private object previousValue;

    public EnsureTestRunnerEnabled(ConfigSettings settings)
    {
      this.settings = settings;
    }

    public override void Process(HttpRequestArgs args)
    {
      var settingName = this.settings.RequireJSSettingName;
      var bootstrapPath = this.settings.BootstrapModulePath;

      if (WebUtil.GetQueryString(EnableTestRunnerParameter) != "1")
      {
        if (WebUtil.GetQueryString(EnableTestRunnerParameter) == "0" && this.previousValue != null)
        {
          this.GetSettings()[settingName] = this.previousValue;
          Context.Diagnostics.Debugging = false;
        }

        return;
      }

      Context.Diagnostics.Debugging = true;
      if (Settings.GetSetting(settingName) == bootstrapPath)
      {
        return;
      }

      var settings = this.GetSettings();
      this.previousValue = settings[settingName];
      settings[settingName] = bootstrapPath;
    }

    private Hashtable GetSettings()
    {
      var type = typeof(Settings);
      var info = type.GetField("settings", BindingFlags.NonPublic | BindingFlags.Static);
      return (Hashtable)info.GetValue(null);
    }
  }
}
