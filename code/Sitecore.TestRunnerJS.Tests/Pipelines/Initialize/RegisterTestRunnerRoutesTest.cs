namespace Sitecore.TestRunnerJS.Tests
{
  using System;
  using System.Net.Http;
  using System.Web.Http;
  using FluentAssertions;
  using Sitecore.TestRunnerJS.Pipelines.Initialize;
  using Xunit;

  public class RegisterTestRunnerRoutesTest
  {
    private readonly HttpConfiguration config;

    public RegisterTestRunnerRoutesTest()
    {
      var processor = new RegisterTestRunnerRoutes();
      processor.Process(null);
      this.config = new HttpConfiguration();
      foreach (var route in GlobalConfiguration.Configuration.Routes)
      {
        this.config.Routes.Add(Guid.NewGuid().ToString(), route);
      }
    }

    [Fact]
    public void ShouldRegisterTestFixtureRoute()
    {
      // arrange
      var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/TestRunnerJS/testfixture/getbyurl?url=url");

      // act
      var routeTester = new RouteTester(this.config, request);

      // assert
      routeTester.GetControllerType().Should().Be(typeof(TestFixtureController));
      routeTester.GetActionName().Should().Be(ReflectionHelpers.GetMethodName((TestFixtureController p) => p.GetByUrl(string.Empty)));
    }
  }
}
