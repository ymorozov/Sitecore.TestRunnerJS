
namespace Sitecore.TestRunnerJS.Tests.Extensions
{
  using NSubstitute;
  using Sitecore.TestRunnerJS.Extensions;
  using Xunit;

  public class TestRunnerTest
  {
    [Fact]
    public void ShouldExecuteTests()
    {
      // arrange
      var factory = Substitute.For<PhantomFactory>();
      factory.Create();

      var testRunner = new TestRunner(factory);

      // act
      testRunner.Execute("some_url", "some_grep");

      // assert

    }
  }
}
