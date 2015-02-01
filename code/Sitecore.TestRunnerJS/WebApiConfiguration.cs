using Sitecore.TestRunnerJS;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApiConfiguration), "Register")]
namespace Sitecore.TestRunnerJS
{
    using System.Web.Http;

    public static class WebApiConfiguration
  {
    public static void Register()
    {
      GlobalConfiguration.Configuration.Routes.MapHttpRoute(
        "TestFixture",
        "TestRunnerJS/testfixture/{action}", 
        new { controller = "TestFixture" });
    }
  }
}
