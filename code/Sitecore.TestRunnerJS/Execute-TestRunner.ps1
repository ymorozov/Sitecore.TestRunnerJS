param(
  [Parameter(Mandatory=$True,Position=0)]
  [string]$instanceName,

  [Parameter(Mandatory=$True,Position=1)]
  [string]$applicationName
)

$solution = $dte.Solution.FileName
$solutionPath = Split-Path $solution
$packages = Join-Path ($solutionPath) packages

$phantomPackage = Get-ChildItem $packages\PhantomJS*
$phantomPath = Join-Path ($phantomPackage) tools\phantomjs\phantomjs.exe

$runnerPackage = Get-ChildItem $packages\Sitecore.TestRunnerJS*
$runnerScript = Join-Path ($runnerPackage) tools\sitecore\TestRunnerJS\phantom\run.ps1

&$runnerScript -instanceName $instanceName -applicationName $applicationName -phantomPath $phantomPath
