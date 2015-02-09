namespace Sitecore.TestRunnerJS
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Http;

    public class TestFixtureController : ApiController
  {
    private const string TestFixtureParameterName = "sc_testfixture";

    [HttpGet]
    public HttpResponseMessage GetByUrl(string url)
    {
      var uri = new Uri(url);
      var queryParameters = HttpUtility.ParseQueryString(uri.Query);

      var normalizedUrl = uri.LocalPath.ToLowerInvariant();
      var applicationUrlParts = normalizedUrl.Split(new[] { @"/sitecore/client/applications/" }, StringSplitOptions.None);

      if (applicationUrlParts.Length == 2)
      {
        var applicationUrlPart = applicationUrlParts[1];
        var applicationUrlParameterParts = applicationUrlPart.Split('/');

        if (applicationUrlParameterParts.Length >= 1)
        {
          var testFixtureParameter = queryParameters.Get(TestFixtureParameterName);
          if (!string.IsNullOrEmpty(testFixtureParameter))
          {
            applicationUrlParameterParts[applicationUrlParameterParts.Length - 1] = testFixtureParameter;
          }

          var pageRelativePath = string.Join(@"/", applicationUrlParameterParts);

          var testFixturePath = HostingEnvironment.MapPath(
            "~/" + ConfigSettings.RootTestFixturesFolder + "/" + pageRelativePath + ".js");

          if (File.Exists(testFixturePath))
          {
            return this.GetResponseMessage(testFixturePath);
          }
        }
      }

      var notFoundPath = HostingEnvironment.MapPath("~/sitecore/TestRunnerJS/assets/testsnotfound.js");
      return this.GetResponseMessage(notFoundPath);
    }

    private HttpResponseMessage GetResponseMessage(string testFixturePath)
    {
      var result = new HttpResponseMessage(HttpStatusCode.OK);
      var stream = new FileStream(testFixturePath, FileMode.Open);
      result.Content = new StreamContent(stream);
      result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/javascript");
      return result;
    }
  }
}
