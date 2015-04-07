namespace Sitecore.TestRunnerJS.Tests
{
  using FluentAssertions;
  using NSubstitute;
  using Xunit;

  public class TestFixtureControllerTest
  {
    private readonly TestFixtureController controller;

    private FileService fileService;

    private ConfigSettings settings;

    public TestFixtureControllerTest()
    {
      this.fileService = Substitute.For<FileService>();
      this.settings = Substitute.For<ConfigSettings>();
      this.controller = new TestFixtureController(this.fileService, this.settings);
    }

    [Fact]
    public void ShouldReturnExpectedVirtualPathForFixture()
    {
      // arrange
      const string PageUrl = "http://www.example.com/sitecore/shell/sitecore/client/Applications/SampleApp/FirstPage?sc_testrunnerjs=1";
      const string ExpectedPath = "my/test/folder/sampleapp/firstpage.js";

      this.settings.RootTestFixturesFolder.Returns("my/test/folder");
      this.fileService
        .MapPath("~/my/test/folder/sampleapp/firstpage.js")
        .Returns(@"d:\website\my\test\folder\sampleapp\firstpage.js");
      this.fileService
        .GetRelativePath(@"d:\website\my\test\folder\sampleapp\firstpage.js")
        .Returns(ExpectedPath);

      // act
      var result = this.controller.GetByUrl(PageUrl);

      // assert
      result.ExpectedPath.Should().Be(ExpectedPath);
    }
  }
}
