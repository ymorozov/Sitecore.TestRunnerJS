namespace Sitecore.SpeakTest
{
  using Sitecore.Configuration;

  public class ConfigSettings
  {
    public static string BootstrapModulePath
    {
      get { return Settings.GetSetting("SpeakTest.BootstrapModulePath", "/SpeakTest/bootstrapper.js"); }
    }

    public static string RequireJSSettingName
    {
      get { return Settings.GetSetting("SpeakTest.RequireJSSettingName", "Speak.Html.RequireJSBackwardCompatibilityFile"); }
    }

    public static string RootTestFixturesFolder
    {
      get { return Settings.GetSetting("SpeakTest.RootTestFixturesFolder", "/SpeakTest/Tests"); }
    }
  }
}
