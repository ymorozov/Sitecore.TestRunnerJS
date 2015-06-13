namespace Sitecore.TestRunnerJS.Extensions
{
  using System;
  using System.Diagnostics;
  using System.IO;

  public class PhantomAgent
  {
    private readonly ConfigManager config;

    private const string FileName = "powershell.exe";

    private const string ArgsPattern = "-NoProfile -ExecutionPolicy unrestricted -file \"Execute-TestRunner.ps1\" \"{0}\" \"{1}\" \"-grep\" \"{2}\" \"-url\" \"{3}\"";

    public PhantomAgent(ConfigManager configManager)
    {
      this.config = configManager;
    }

    public virtual AgentResult Run(string url, string grep)
    {
      var args = string.Format(ArgsPattern, this.config.InstanceName, this.config.ApplicationName, grep, url);

      var path = Path.Combine(this.config.PackagePath, "tools");
      var startInfo = new ProcessStartInfo(FileName, args)
                        {
                          WorkingDirectory = path,
                          UseShellExecute = false,
                          RedirectStandardError = true,
                          RedirectStandardOutput = true,
                          CreateNoWindow = true
                        };

      var proc = Process.Start(startInfo);
      proc.WaitForExit();

      var errorMessage = proc.StandardError.ReadToEnd();
      if (errorMessage != string.Empty)
      {
        throw new InvalidOperationException(errorMessage);
      }

      var outputMessage = proc.StandardOutput.ReadToEnd();
      var exitCode = proc.ExitCode;

      return new AgentResult { FailedCount = exitCode, Message = outputMessage };
    }
  }
}
