namespace Sitecore.SpeakTest.Pipelines.HttpRequest
{
  using System.Collections;
  using System.Reflection;
  using Sitecore.Configuration;
  using Sitecore.Pipelines.HttpRequest;
  using Sitecore.Web;

  public class EnsureSpeakTestingEnabled : HttpRequestProcessor
  {
    private const string bootstrapModulePath = "/SpeakTest/bootstrap.js";

    private const string requireJSSettingName = "Speak.Html.RequireJSBackwardCompatibilityFile";

    private Hashtable GetSettings()
    {
      var type = typeof(Settings);
      var info = type.GetField("settings", BindingFlags.NonPublic | BindingFlags.Static);
      return (Hashtable)info.GetValue(null);
    }

    public override void Process(HttpRequestArgs args)
    {
      if (WebUtil.GetQueryString("sc_speaktest") != "1")
      {
        return;
      }

      if (Settings.GetSetting(requireJSSettingName) == bootstrapModulePath)
      {
        return;
      }

      this.GetSettings()[requireJSSettingName] = bootstrapModulePath;
    }
  }
}
