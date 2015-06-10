namespace Sitecore.TestRunnerJS.Extensions
{
  using System.Configuration;

  public class ConfigManager
  {
    public virtual string PhantomPath
    {
      get { return ConfigurationManager.AppSettings["PhantomPath"]; }
    }

    public virtual string TestRunnerPath
    {
      get { return ConfigurationManager.AppSettings["TestRunnerPath"]; }
    }
  }
}
