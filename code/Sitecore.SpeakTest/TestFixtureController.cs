namespace Sitecore.SpeakTest
{
  using System;
  using System.IO;
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Headers;
  using System.Web.Hosting;
  using System.Web.Http;

  public class TestFixtureController : ApiController
  {
    [HttpGet]
    public HttpResponseMessage GetByUrl(string url)
    {
      var applicationUrlParts = url.Split(new[] { @"/sitecore/client/applications/" }, StringSplitOptions.RemoveEmptyEntries);

      if (applicationUrlParts.Length == 2)
      {
        var applicationUrlPart = applicationUrlParts[1];
        var applicationUrlPartWithoutQuery = applicationUrlPart.Split('?')[0];
        var applicationUrlParameterParts = applicationUrlPartWithoutQuery.Split('/');

        if (applicationUrlParameterParts.Length >= 2)
        {
          var applicationName = applicationUrlParameterParts[0];
          var pageName = applicationUrlParameterParts[1];

          var testFixturePath = HostingEnvironment.MapPath(
            "~/" + ConfigSettings.RootTestFixturesFolder + "/" + applicationName + "/" + pageName + ".js");

          if (File.Exists(testFixturePath))
          {
            return this.GetResponseMessage(testFixturePath);
          }
        }
      }

      var notFoundPath = HostingEnvironment.MapPath("~/speaktest/assets/testsnotfound.js");
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
