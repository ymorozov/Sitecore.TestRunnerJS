namespace Sitecore.TestRunnerJS.Extensions
{
  using System.Configuration;

  public class ConfigManager
  {
    public virtual string PackagePath
    {
      get { return ConfigurationManager.AppSettings["Sitecore.TestRunnerJS.PackagePath"]; }
    }

    public virtual string ApplicationName
    {
      get { return ConfigurationManager.AppSettings["Sitecore.TestRunnerJS.ApplicationName"]; }
    }

    public virtual string InstanceName
    {
      get { return ConfigurationManager.AppSettings["Sitecore.TestRunnerJS.InstanceName"]; }
    }
  }
}
