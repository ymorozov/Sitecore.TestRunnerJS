namespace Sitecore.TestRunnerJS.Extensions
{
  using System;

  public class TestsFailedException : Exception
  {
    public TestsFailedException(string message)
      : base(message)
    {
    }
  }
}
