param(
  [Parameter(Mandatory=$True,Position=0)]
  [string]$instanceName,

  [Parameter(Mandatory=$True,Position=1)]
  [string]$applicationName,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$phantomPath
)

$loaderPath = "$PSScriptRoot\loader.js"
$reportPath = "$PSScriptRoot\report.js"

&$phantomPath $loaderPath $instanceName $applicationName $reportPath 180000

if (!$?) { 
  throw "Failures reported in javascript integration tests." 
}