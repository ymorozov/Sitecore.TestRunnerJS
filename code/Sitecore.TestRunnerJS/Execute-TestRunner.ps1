param(
  [Parameter(Mandatory=$True,Position=0)]
  [string]$instanceName,

  [Parameter(Mandatory=$True,Position=1)]
  [string]$applicationName,

  [Parameter(Mandatory=$False,Position=2)]
  [string]$grep,

  [Parameter(Mandatory=$False,Position=3)]
  [Switch]$invert,

  [Parameter(Mandatory=$False)]
  [string]$outputReportPath,

  [Parameter(Mandatory=$False)]
  [string]$url
)

$packages = Split-Path (Split-Path $PSScriptRoot)

$phantomPackage = Get-ChildItem $packages\PhantomJS*
if($phantomPackage -is [system.array])
{
  $phantomPackage = $phantomPackage[$phantomPackage.Length-1]
}

$phantomPath = Join-Path ($phantomPackage) tools\phantomjs\phantomjs.exe
$runnerScript = Join-Path ($PSScriptRoot) sitecore\TestRunnerJS\phantom\run.ps1

&$runnerScript $instanceName $applicationName $phantomPath $grep $invert -outputReportPath $outputReportPath -url $url