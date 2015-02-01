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
    private const string EnableTestRunnerParameter = "sc_testrunnerjs";

    private object _previousValue;

    public override void Process(HttpRequestArgs args)
    {
      if (WebUtil.GetQueryString(EnableTestRunnerParameter) != "1")
      {
        if (WebUtil.GetQueryString(EnableTestRunnerParameter) == "0" && _previousValue != null)
        {
          this.GetSettings()[ConfigSettings.RequireJSSettingName] = _previousValue;
          Context.Diagnostics.Debugging = false;
        }

        return;
      }

      Context.Diagnostics.Debugging = true;
      if (Settings.GetSetting(ConfigSettings.RequireJSSettingName) == ConfigSettings.BootstrapModulePath)
      {
        return;
      }

      var settings = this.GetSettings();
      _previousValue = settings[ConfigSettings.RequireJSSettingName];
      settings[ConfigSettings.RequireJSSettingName] = ConfigSettings.BootstrapModulePath;
    }

    private Hashtable GetSettings()
    {
      var type = typeof(Settings);
      var info = type.GetField("settings", BindingFlags.NonPublic | BindingFlags.Static);
      return (Hashtable)info.GetValue(null);
    }
  }
}
