namespace Sitecore.TestRunnerJS.Extensions
{
  public class TestRunner
  {
    private readonly PhantomFactory agentFactory;

    public TestRunner(PhantomFactory agentFactory)
    {
      this.agentFactory = agentFactory;
    }

    public virtual void Execute(string url, string grep)
    {
      var agent = this.agentFactory.Create();
      var result = agent.Run(url, grep);

      if (result.FailedCount > 0 || !string.IsNullOrEmpty(result.ErrorMessage))
      {
        string errorMessage = string.Empty;
        if (!string.IsNullOrEmpty(result.Message))
        {
          errorMessage += result.Message + "\r\n";
        }

        if (!string.IsNullOrEmpty(result.ErrorMessage))
        {
          errorMessage += "Error message:\r\n" + result.ErrorMessage;
        }

        throw new TestsFailedException(errorMessage);
      }
    }

    public static TestRunner Create()
    {
      return new TestRunner(new PhantomFactory(new ConfigManager()));
    }
  }
}
