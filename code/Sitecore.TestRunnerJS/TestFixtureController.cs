namespace Sitecore.TestRunnerJS
{
  using System;
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
      Uri uri;
      if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
      {
        return new TestFixtureModel();
      }

      var queryParameters = HttpUtility.ParseQueryString(uri.Query);

      var normalizedUrl = uri.LocalPath.ToLowerInvariant();
      var applicationUrlParts = normalizedUrl.Split(new[] { this.settings.RootApplicationPath }, StringSplitOptions.None);

      if (applicationUrlParts.Length != 2)
      {
        return new TestFixtureModel();
      }

      var applicationUrlPart = applicationUrlParts[1];
      var applicationUrlParameterParts = applicationUrlPart.Split('/');

      var testFixtureParameter = queryParameters.Get(TestFixtureParameterName);
      if (!string.IsNullOrEmpty(testFixtureParameter))
      {
        applicationUrlParameterParts[applicationUrlParameterParts.Length - 1] = testFixtureParameter;
      }

      var pageRelativePath = string.Join(@"/", applicationUrlParameterParts);
      var testFixtureRelativePath = this.settings.RootTestFixturesFolder + "/" + pageRelativePath + ".js";

      var testFixturePath = this.fileService.MapPath("~/" + testFixtureRelativePath);

      return new TestFixtureModel { ExpectedPath = testFixtureRelativePath, IsExist = this.fileService.FileExists(testFixturePath) };
    }

    [HttpGet]
    public TestFixtureModel GetSettings(string application)
    {
      var relativePath = this.settings.RootTestFixturesFolder + "/" + application + ".json";
      var absolutePath = this.fileService.MapPath("~/" + relativePath);

      return new TestFixtureModel { ExpectedPath = relativePath, IsExist = this.fileService.FileExists(absolutePath) };
    }
  }
}
