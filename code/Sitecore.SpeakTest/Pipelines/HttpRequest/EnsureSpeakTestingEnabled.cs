namespace Sitecore.SpeakTest.Pipelines.HttpRequest
{
  using System.Collections;
  using System.Reflection;
  using Sitecore.Configuration;
  using Sitecore.Pipelines.HttpRequest;
  using Sitecore.Web;

  public class EnsureSpeakTestingEnabled : HttpRequestProcessor
  {
    private object _previousValue;

    public override void Process(HttpRequestArgs args)
    {
      if (WebUtil.GetQueryString("sc_speaktest") != "1")
      {
        if (WebUtil.GetQueryString("sc_speaktest") == "0" && _previousValue != null)
        {
          this.GetSettings()[ConfigSettings.RequireJSSettingName] = _previousValue;
          Context.Diagnostics.Debugging = false;
        }

        return;
      }

      if (Settings.GetSetting(ConfigSettings.RequireJSSettingName) == ConfigSettings.BootstrapModulePath)
      {
        return;
      }

      var settings = this.GetSettings();
      _previousValue = settings[ConfigSettings.RequireJSSettingName];
      settings[ConfigSettings.RequireJSSettingName] = ConfigSettings.BootstrapModulePath;
      Context.Diagnostics.Debugging = true;
    }

    private Hashtable GetSettings()
    {
      var type = typeof(Settings);
      var info = type.GetField("settings", BindingFlags.NonPublic | BindingFlags.Static);
      return (Hashtable)info.GetValue(null);
    }
  }
}
