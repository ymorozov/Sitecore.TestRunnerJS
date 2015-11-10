using Sitecore.Configuration;

namespace Sitecore.TestRunnerJS.Pipelines.HttpRequest
{
  using Sitecore.Pipelines.HttpRequest;
  using Sitecore.TestRunnerJS;

  public class EnsureTestRunnerEnabled : HttpRequestProcessor
  {
    public const string EnableTestRunnerParameter = "sc_testrunnerjs";

    private readonly ConfigSettings settings;

    private readonly SitecoreWrapper wrapper;

    private string previousValue;

    public EnsureTestRunnerEnabled(ConfigSettings settings, SitecoreWrapper wrapper)
    {
      this.settings = settings;
      this.wrapper = wrapper;
    }

    public override void Process(HttpRequestArgs args)
    {
      var bootstrapPath = this.settings.BootstrapModulePath;

      if (this.wrapper.GetQueryString(EnableTestRunnerParameter) != "1")
      {
        if (this.wrapper.GetQueryString(EnableTestRunnerParameter) != "0")
        {
          if (this.previousValue != null)
          {
            this.wrapper.Debugging = true;
          }

          return;
        }

        if (this.previousValue == null)
        {
          return;
        }

        this.wrapper.SetTestRunnerSetting(this.previousValue);
        this.wrapper.Debugging = false;

        return;
      }

      this.wrapper.Debugging = true;
      if (this.wrapper.GetTestRunnerSetting() == bootstrapPath)
      {
        return;
      }

      this.previousValue = this.wrapper.GetTestRunnerSetting();
      this.wrapper.SetTestRunnerSetting(bootstrapPath);
    }
  }
}
