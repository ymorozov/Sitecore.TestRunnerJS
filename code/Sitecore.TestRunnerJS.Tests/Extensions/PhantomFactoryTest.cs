namespace Sitecore.TestRunnerJS.Tests.Extensions
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.TestRunnerJS.Extensions;
  using Xunit;

  public class PhantomFactoryTest
  {
    [Fact]
    public void ShouldCreatePhantomAgent()
    {
      // arrange
      var config = Substitute.For<ConfigManager>();
      var factory = new PhantomFactory(config);

      // act
      var result = factory.Create();

      // assert
      result.Should().NotBeNull();
    }
  }
}
