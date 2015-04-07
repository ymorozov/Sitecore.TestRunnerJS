namespace Sitecore.TestRunnerJS
{
  using System.IO;
  using System.Web;
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

    public virtual string GetRelativePath(string absolutePath)
    {
      var relativePath = absolutePath.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], string.Empty);
      return relativePath.Replace(@"\", "/");
    }
  }
}
