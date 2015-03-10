using SampleApp.Controllers;

// Register routes on application start.
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SampleResponseController), "RegisterRoutes")]

namespace SampleApp.Controllers
{
  using System.Web.Http;

  public class SampleResponseController : ApiController
  {
    [HttpGet]
    // /test/SampleResponse/ProcessSelectedItem?selectedItem=<ITEM_NAME>
    public string ProcessSelectedItem(string selectedItem)
    {
      return string.Format("Selected item \"{0}\" was processed on server", selectedItem);
    }

    [HttpPost]
    public object GetItems()
    {
      return new object[]
             {
               new { Name="My item 1", Value="Value 1" },
               new { Name="My item 2", Value="Value 2" },
               new { Name="My item 3", Value="Value 3" }
             };
    }

    // Routes to register on application start
    public static void RegisterRoutes()
    {
      GlobalConfiguration.Configuration.Routes.MapHttpRoute(
        "SampleControllerRoute",
        "test/{controller}/{action}",
        new { controller = "SampleResponseController" });
    }
  }
}