param(
  [Parameter(Mandatory=$True,Position=0)]
  [string]$instanceName,

  [Parameter(Mandatory=$True,Position=1)]
  [string]$applicationName
)

$packages = Split-Path (Split-Path $PSScriptRoot)

$phantomPackage = Get-ChildItem $packages\PhantomJS*
if($phantomPackage -is [system.array])
{
  $phantomPackage = $phantomPackage[$phantomPackage.Length-1]
}

$phantomPath = Join-Path ($phantomPackage) tools\phantomjs\phantomjs.exe
$runnerScript = Join-Path ($PSScriptRoot) sitecore\TestRunnerJS\phantom\run.ps1

&$runnerScript -instanceName $instanceName -applicationName $applicationName -phantomPath $phantomPath