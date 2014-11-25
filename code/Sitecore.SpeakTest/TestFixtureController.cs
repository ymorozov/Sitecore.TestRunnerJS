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
      var normalizedUrl = url.ToLowerInvariant();
      var applicationUrlParts = normalizedUrl.Split(new[] { @"/sitecore/client/applications/" }, StringSplitOptions.RemoveEmptyEntries);

      if (applicationUrlParts.Length == 2)
      {
        var applicationUrlPart = applicationUrlParts[1];
        var applicationUrlPartWithoutQuery = applicationUrlPart.Split('?')[0];
        var applicationUrlParameterParts = applicationUrlPartWithoutQuery.Split('/');

        if (applicationUrlParameterParts.Length >= 2)
        {
          var pageRelativePath = string.Join(@"/", applicationUrlParameterParts);

          var testFixturePath = HostingEnvironment.MapPath(
            "~/" + ConfigSettings.RootTestFixturesFolder + "/" + pageRelativePath + ".js");

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
