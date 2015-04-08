namespace Sitecore.TestRunnerJS
{
  using System.IO;
  using System.Web.Hosting;

  public class FileService
  {
    public virtual string MapPath(string virtualPath)
    {
      return HostingEnvironment.MapPath(virtualPath);
    }

    public virtual bool FileExists(string path)
    {
      return File.Exists(path);
    }
  }
}
