using System;

namespace Sitecore.TestRunnerJS
{
  using System.Collections;
  using System.Reflection;
  using Sitecore.Configuration;
  using Sitecore.Web;

  public class SitecoreWrapper
  {
    private readonly ConfigSettings settings;

    private SettingsSwitcher settingsSwitcher;

    public SitecoreWrapper(ConfigSettings settings)
    {
      this.settings = settings;
    }


    public virtual bool Debugging
    {
      get { return Context.Diagnostics.Debugging; }
      set { Context.Diagnostics.Debugging = value; }
    }

    public virtual void SetTestRunnerSetting(string value)
    {
      this.settingsSwitcher = new SettingsSwitcher(this.settings.RequireJSSettingName, value); 
    }

    public virtual string GetTestRunnerSetting()
    {
      return Settings.GetSetting(this.settings.RequireJSSettingName);
    }

    public virtual string GetQueryString(string key)
    {
      return WebUtil.GetQueryString(key);
    }
  }
}
