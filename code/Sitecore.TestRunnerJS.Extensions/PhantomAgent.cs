using System;

namespace Sitecore.TestRunnerJS.Extensions
{
  public class PhantomAgent
  {
    private readonly ConfigManager configManager;

    private const string PhantomJs = "phantomjs.exe";

    public PhantomAgent(ConfigManager configManager)
    {
      this.configManager = configManager;
    }

    public virtual AgentResult Run(string url, string grep)
    {
      throw new NotImplementedException();
    }
  }
}
