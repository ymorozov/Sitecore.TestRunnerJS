namespace Sitecore.TestRunnerJS
{
    using Sitecore.Configuration;

    public class ConfigSettings
  {
    public static string BootstrapModulePath
    {
      get { return Settings.GetSetting("TestRunnerJS.BootstrapModulePath", "/TestRunnerJS/bootstrapper.js"); }
    }

    public static string RequireJSSettingName
    {
      get { return Settings.GetSetting("TestRunnerJS.RequireJSSettingName", "Speak.Html.RequireJSBackwardCompatibilityFile"); }
    }

    public static string RootTestFixturesFolder
    {
      get { return Settings.GetSetting("TestRunnerJS.RootTestFixturesFolder", "/TestRunnerJS/Tests"); }
    }
  }
}
