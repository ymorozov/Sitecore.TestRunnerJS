namespace Sitecore.TestRunnerJS.Tests
{
  using FluentAssertions;
  using NSubstitute;
  using Xunit;

  public class TestFixtureControllerTest
  {
    private readonly TestFixtureController controller;

    private readonly FileService fileService;

    private readonly ConfigSettings settings;

    public TestFixtureControllerTest()
    {
      this.fileService = Substitute.For<FileService>();

      this.settings = Substitute.For<ConfigSettings>();
      this.settings.RootApplicationPath.Returns("/sitecore/client/applications/");

      this.controller = new TestFixtureController(this.fileService, this.settings);
    }

    [Theory]
    [InlineData("http://www.example.com/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage", "/sitecore/client/applications/")]
    [InlineData("http://www.example.com/sitecore/shell/sitecore/client/my_custom_folder/SampleApp/FirstPage", "/sitecore/client/my_custom_folder/")]
    public void ShouldReturnExpectedVirtualPathForFixture(string pageUrl, string rootPath)
    {
      // arrange
      const string ExpectedPath = "my/test/folder/sampleapp/firstpage.js";

      this.settings.RootApplicationPath.Returns(rootPath);
      this.settings.RootTestFixturesFolder.Returns("my/test/folder");
      this.fileService
        .MapPath("~/my/test/folder/sampleapp/firstpage.js")
        .Returns(@"d:\website\my\test\folder\sampleapp\firstpage.js");

      // act
      var result = this.controller.GetByUrl(pageUrl);

      // assert
      result.ExpectedPath.Should().Be(ExpectedPath);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldReturnWhetherExpectedPathIsExist(bool isExist)
    {
      // arrange
      const string PageUrl = "http://www.example.com/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage";
      this.settings.RootTestFixturesFolder.Returns("some_folder");

      this.fileService.FileExists(Arg.Any<string>()).Returns(isExist);

      // act
      var result = this.controller.GetByUrl(PageUrl);

      // assert
      result.IsExist.Should().Be(isExist);
    }

    [Theory]
    [InlineData("http://www.example.com/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage?&sc_testfixture=new", "/sitecore/client/applications/")]
    [InlineData("http://www.example.com/sitecore/shell/sitecore/client/my_custom_folder/SampleApp/FirstPage?&sc_testfixture=new", "/sitecore/client/my_custom_folder/")]
    public void ShouldTakeFileNameFromTestFixtureQueryParameterIfSpecified(string pageUrl, string rootPath)
    {
      // arrange
      const string ExpectedPath = "my/test/folder/sampleapp/new.js";

      this.settings.RootApplicationPath.Returns(rootPath);
      this.settings.RootTestFixturesFolder.Returns("my/test/folder");
      this.fileService
        .MapPath("~/my/test/folder/sampleapp/new.js")
        .Returns(@"d:\website\my\test\folder\sampleapp\new.js");
      
      // act
      var result = this.controller.GetByUrl(pageUrl);

      // assert
      result.ExpectedPath.Should().Be(ExpectedPath);
    }

    [Theory]
    [InlineData("wrong_url")]
    [InlineData("http://www.example.com/SampleApp/FirstPage")]
    public void ShouldReturnNothingForIncorrectUrl(string url)
    {
      // act
      var result = this.controller.GetByUrl(url);

      // assert
      result.ExpectedPath.Should().BeNullOrEmpty();
      result.IsExist.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnPathForApplicationSettings()
    {
      // arrange
      const string ExpectedPath = "app/folder/sampleapp.json";

      this.settings.RootTestFixturesFolder.Returns("app/folder");

      this.fileService
        .MapPath("~/app/folder/sampleapp.json")
        .Returns(@"d:\website\my\test\folder\sampleapp.json");

      // act
      var result = this.controller.GetSettings("sampleapp");

      // assert
      result.ExpectedPath.Should().Be(ExpectedPath);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldReturnWhetherExpectedSettingsAreExist(bool isExist)
    {
      // arrange
      this.settings.RootTestFixturesFolder.Returns("some_folder");
      this.fileService.FileExists(Arg.Any<string>()).Returns(isExist);

      // act
      var result = this.controller.GetSettings("myapp");

      // assert
      result.IsExist.Should().Be(isExist);
    }
  }
}
