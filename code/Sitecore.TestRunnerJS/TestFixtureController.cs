namespace Sitecore.TestRunnerJS
{
  using System;
  using System.IO;
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Headers;
  using System.Web;
  using System.Web.Http;

  public class TestFixtureController : ApiController
  {
    private const string TestFixtureParameterName = "sc_testfixture";

    private readonly FileService fileService;

    private readonly ConfigSettings settings;

    public TestFixtureController()
      : this(new FileService(), new ConfigSettings())
    {
    }

    public TestFixtureController(FileService fileService, ConfigSettings settings)
    {
      this.fileService = fileService;
      this.settings = settings;
    }

    [HttpGet]
    public TestFixtureModel GetByUrl(string url)
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

          var testFixturePath = this.fileService.MapPath(
            "~/" + this.settings.RootTestFixturesFolder + "/" + pageRelativePath + ".js");

          var testFixtureRelativePath = this.fileService.GetRelativePath(testFixturePath);

          return new TestFixtureModel { ExpectedPath = testFixtureRelativePath, IsExist = this.fileService.FileExists(testFixturePath) };
        }
      }

      var notFoundPath = this.fileService.MapPath("~/sitecore/TestRunnerJS/assets/testsnotfound.js");
      return new TestFixtureModel();
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
