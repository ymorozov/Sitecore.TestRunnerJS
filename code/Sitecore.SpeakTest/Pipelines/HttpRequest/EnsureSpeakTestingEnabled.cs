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

    private const string bootstrapModulePath = "/SpeakTest/bootstrapper.js";

    private const string requireJSSettingName = "Speak.Html.RequireJSBackwardCompatibilityFile";

    public override void Process(HttpRequestArgs args)
    {
      if (WebUtil.GetQueryString("sc_speaktest") != "1")
      {
        if (WebUtil.GetQueryString("sc_speaktest") == "0" && _previousValue != null)
        {
          this.GetSettings()[requireJSSettingName] = _previousValue;
          Context.Diagnostics.Debugging = false;
        }

        return;
      }

      if (Settings.GetSetting(requireJSSettingName) == bootstrapModulePath)
      {
        return;
      }

      var settings = this.GetSettings();
      _previousValue = settings[requireJSSettingName];
      settings[requireJSSettingName] = bootstrapModulePath;
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
