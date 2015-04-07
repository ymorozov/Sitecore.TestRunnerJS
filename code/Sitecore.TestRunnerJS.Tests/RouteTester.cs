namespace Sitecore.TestRunnerJS.Tests
{
  using System;
  using System.Net.Http;
  using System.Web.Http;
  using System.Web.Http.Controllers;
  using System.Web.Http.Dispatcher;
  using System.Web.Http.Hosting;

  public class RouteTester
  {
    private readonly HttpRequestMessage request;

    private readonly IHttpControllerSelector controllerSelector;

    private readonly HttpControllerContext controllerContext;

    public RouteTester(HttpConfiguration config, HttpRequestMessage request)
    {
      this.request = request;
      var routeData = config.Routes.GetRouteData(this.request);
      this.request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
      controllerSelector = new DefaultHttpControllerSelector(config);
      controllerContext = new HttpControllerContext(config, routeData, this.request);
    }

    public string GetActionName()
    {
      if (this.controllerContext.ControllerDescriptor == null)
      {
        this.GetControllerType();
      }

      var actionSelector = new ApiControllerActionSelector();
      var descriptor = actionSelector.SelectAction(this.controllerContext);
      return descriptor.ActionName;
    }

    public Type GetControllerType()
    {
      var descriptor = this.controllerSelector.SelectController(this.request);
      this.controllerContext.ControllerDescriptor = descriptor;
      return descriptor.ControllerType;
    }
  }
}
