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

    private readonly TestRunner testRunner;

    public TestRunnerTest()
    {
      var config = Substitute.For<ConfigManager>();
      this.agent = Substitute.For<PhantomAgent>(config);
      this.factory = Substitute.For<PhantomFactory>(config);
      this.factory.Create().Returns(this.agent);
      this.testRunner = new TestRunner(this.factory);
    }

    [Fact]
    public void ShouldExecuteTests()
    {
      // arrange
      this.agent.Run("some_url", "some_grep").Returns(new AgentResult());

      // act
      this.testRunner.Execute("some_url", "some_grep");

      // assert
      agent.Received().Run("some_url", "some_grep");
    }

    [Fact]
    public void ShouldThrowOnTestFiled()
    {
      // arrange
      this.agent.Run("some_url", "some_grep").Returns(new AgentResult { FailedCount = 1, Message = "Tests failed" });

      // act
      Action a = () => this.testRunner.Execute("some_url", "some_grep");

      // assert
      a.ShouldThrow<TestsFailedException>().WithMessage("Tests failed");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldThrowOnErrorMessage(string errorMessage)
    {
      // arrange
      this.agent.Run("some_url", "some_grep").Returns(new AgentResult { ErrorMessage = errorMessage });

      // act
      Action a = () => this.testRunner.Execute("some_url", "some_grep");

      // assert
      a.ShouldNotThrow<TestsFailedException>();
    }

    [Fact]
    public void ShouldThrowIfErrorMessage()
    {
      // arrange
      this.agent.Run("some_url", "some_grep").Returns(new AgentResult { ErrorMessage = "Error message" });

      // act
      Action a = () => this.testRunner.Execute("some_url", "some_grep");

      // assert
      a.ShouldThrow<TestsFailedException>();
    }

    [Theory]
    [InlineData("msg", "err", "msg\r\nError message:\r\nerr")]
    [InlineData(null, "err", "Error message:\r\nerr")]
    [InlineData("msg", null, "msg\r\n")]
    public void ShouldCombineMessages(string message, string errorMessage, string result)
    {
      // arrange
      this.agent.Run("some_url", "some_grep").Returns(new AgentResult
                                                      {
                                                        Message = message,
                                                        ErrorMessage = errorMessage,
                                                        FailedCount = 1
                                                      });

      // act
      Action a = () => this.testRunner.Execute("some_url", "some_grep");

      // assert
      a.ShouldThrow<TestsFailedException>().WithMessage(result);
    }
  }
}
