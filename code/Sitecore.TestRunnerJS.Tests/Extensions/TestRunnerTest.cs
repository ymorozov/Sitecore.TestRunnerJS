namespace Sitecore.TestRunnerJS.Tests.Extensions
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.TestRunnerJS.Extensions;
  using Xunit;

  public class TestRunnerTest
  {
    private readonly PhantomAgent agent;

    private readonly PhantomFactory factory;

    public TestRunnerTest()
    {
      var config = Substitute.For<ConfigManager>();
      this.agent = Substitute.For<PhantomAgent>(config);
      this.factory = Substitute.For<PhantomFactory>(config);
    }

    [Fact]
    public void ShouldExecuteTests()
    {
      // arrange
      this.factory.Create().Returns(this.agent);
      var testRunner = new TestRunner(this.factory);
      this.agent.Run("some_url", "some_grep").Returns(new AgentResult());

      // act
      testRunner.Execute("some_url", "some_grep");

      // assert
      agent.Received().Run("some_url", "some_grep");
    }

    [Fact]
    public void ShouldThrowOnTestFiled()
    {
      // arrange
      this.factory.Create().Returns(this.agent);
      var testRunner = new TestRunner(this.factory);
      this.agent.Run("some_url", "some_grep").Returns(new AgentResult { FailedCount = 1, Message = "Tests failed" });

      // act
      Action a = () => testRunner.Execute("some_url", "some_grep");

      // assert
      a.ShouldThrow<TestsFailedException>().WithMessage("Tests failed");
    }
  }
}
