namespace Sitecore.TestRunnerJS.Extensions
{
    public class TestRunner
    {
      private readonly IAgentFactory agentFactory;

      public TestRunner(IAgentFactory agentFactory)
      {
        this.agentFactory = agentFactory;
      }

      public virtual void Execute(string url, string grep)
      {
        var agent = this.agentFactory.Create();
        var result = agent.Run();
      }
    }
}
