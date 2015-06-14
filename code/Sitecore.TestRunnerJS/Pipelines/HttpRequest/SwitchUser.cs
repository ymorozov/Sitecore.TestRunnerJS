namespace Sitecore.TestRunnerJS.Pipelines.HttpRequest
{
  using Sitecore.Pipelines.HttpRequest;
  using Sitecore.Security.Accounts;

  public class SwitchUser : HttpRequestProcessor
  {
    public override void Process(HttpRequestArgs args)
    {
      var switcher = new UserSwitcher("sitecore\\admin", true);
    }
  }
}
