namespace Sitecore.TestRunnerJS
{
  using Sitecore.Configuration;

  public class ConfigSettings
  {
    public virtual string BootstrapModulePath
    {
      get { return Settings.GetSetting("TestRunnerJS.BootstrapModulePath", "/TestRunnerJS/bootstrapper.js"); }
    }

    public virtual string RequireJSSettingName
    {
      get { return Settings.GetSetting("TestRunnerJS.RequireJSSettingName", "Speak.Html.RequireJSBackwardCompatibilityFile"); }
    }

    public virtual string RootTestFixturesFolder
    {
      get { return Settings.GetSetting("TestRunnerJS.RootTestFixturesFolder", "/TestRunnerJS/Tests"); }
    }

    public virtual string RootApplicationPath
    {
      get { return Settings.GetSetting("TestRunnerJS.RootApplicationPath", "/sitecore/client/applications/"); }
    }
  }
}
