using Sitecore.SpeakTest;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApiConfiguration), "Register")]
namespace Sitecore.SpeakTest
{
  using System.Web.Http;

  public static class WebApiConfiguration
  {
    public static void Register()
    {
      GlobalConfiguration.Configuration.Routes.MapHttpRoute(
        "TestFixture",
        "testspeak/testfixture/{action}", 
        new { controller = "TestFixture" });
    }
  }
}
