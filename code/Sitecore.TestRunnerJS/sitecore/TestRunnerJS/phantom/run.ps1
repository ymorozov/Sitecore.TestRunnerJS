param(
  [Parameter(Mandatory=$True,Position=0)]
  [string]$instanceName,

  [Parameter(Mandatory=$True,Position=1)]
  [string]$applicationName,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$phantomPath,

  [Parameter(Mandatory=$False,Position=3)]
  [string]$grep,

  [Parameter(Mandatory=$False,Position=4)]
  [Switch]$invert
)

$loaderPath = "$PSScriptRoot\loader.js"
$reportPath = "$PSScriptRoot\report.js"
$timeout = 180000

if($grep){
  &$phantomPath $loaderPath $instanceName $applicationName -r $reportPath -t $timeout -g $grep -i $invert
} else {
  &$phantomPath $loaderPath $instanceName $applicationName -r $reportPath -t $timeout
}

if (!$?) { 
  throw "Failures reported in javascript integration tests." 
}