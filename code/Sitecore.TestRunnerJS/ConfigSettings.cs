namespace Sitecore.TestRunnerJS
{
  using System;
  using Sitecore.Configuration;

  public class ConfigSettings
  {
    private static ConfigSettings instance = new ConfigSettings();

    public static ConfigSettings Instance
    {
      get
      {
        return instance;
      }

      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }

        instance = value;
      }
    }

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
  }
}
