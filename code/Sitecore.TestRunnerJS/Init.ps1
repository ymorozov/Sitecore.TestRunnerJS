param($installPath, $toolsPath, $package)

$projects = Get-Project -All

foreach ($project in $projects)
{
  $projectPath = Split-Path $project.FileName
  if ($projectPath)
  {
    $path = Join-Path ($projectPath) packages.config
    if (Test-Path $path)
    {
      $xml = [xml]$packages = Get-Content $path
      foreach ($package in $packages.packages.package)
      {
        $id = $package.id
        if ($id -eq "Sitecore.TestRunnerJS")
        {
          if (Test-Path "$projectPath\App_Config") {
            Write-Host "TestRunnerJS files location: $toolsPath"
            Write-Host "Installation location: $projectPath"

            Copy-Item -Path "$toolsPath\*" -Destination "$projectPath" -Exclude "*.ps1" -recurse -Force
            break
          }
        }
      }
    }
  }
}