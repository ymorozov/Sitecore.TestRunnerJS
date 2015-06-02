namespace Sitecore.TestRunnerJS
{
  using System.Collections;
  using System.Reflection;
  using Sitecore.Configuration;
  using Sitecore.Web;

  public class SitecoreWrapper
  {
    public virtual bool Debugging
    {
      get { return Context.Diagnostics.Debugging; }
      set { Context.Diagnostics.Debugging = value; }
    }

    public virtual Hashtable GetSitecoreSettings()
    {
      var type = typeof(Settings);
      var info = type.GetField("settings", BindingFlags.NonPublic | BindingFlags.Static);
      return (Hashtable)info.GetValue(null);
    }

    public virtual string GetQueryString(string key)
    {
      return WebUtil.GetQueryString(key);
    }
  }
}
