namespace Sitecore.TestRunnerJS.Extensions
{
  public class PhantomFactory
  {
    private readonly ConfigManager configManager;

    public PhantomFactory(ConfigManager configManager)
    {
      this.configManager = configManager;
    }

    public virtual PhantomAgent Create()
    {
      return new PhantomAgent(this.configManager);
    }
  }
}
