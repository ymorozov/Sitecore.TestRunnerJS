namespace Sitecore.TestRunnerJS.Pipelines.Initialize
{
  using System.Web.Http;
  using Sitecore.Pipelines;

  public class RegisterTestRunnerRoutes
  {
    public void Process(PipelineArgs args)
    {
      GlobalConfiguration.Configuration.Routes.MapHttpRoute(
        "TestFixture",
        "TestRunnerJS/testfixture/{action}",
        new { controller = "TestFixture" });
    }
  }
}
