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

        if (result.FailedCount > 0)
        {
          throw new TestsFailedException(result.Message);
        }
      }

      public static TestRunner Create()
      {
        return new TestRunner(new PhantomFactory(new ConfigManager()));
      }
    }
}
