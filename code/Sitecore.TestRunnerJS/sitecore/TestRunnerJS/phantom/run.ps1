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
  [Switch]$invert,

  [Parameter(Mandatory=$False)]
  [string]$outputReportPath

  [Parameter(Mandatory=$False)]
  [string]$url
)

$loaderPath = "$PSScriptRoot\loader.js"
$reportPath = "$PSScriptRoot\report.js"
$timeout = 180000

$executionExpression = "`"$phantomPath`" `"$loaderPath`" `"$instanceName`" `"$applicationName`" -r `"$reportPath`" -t $timeout"

if($grep){
  $executionExpression = "$executionExpression -g `"$grep`""
}

if($invert){
  $executionExpression = "$executionExpression -i"
}

if($outputReportPath){
  $executionExpression = "$executionExpression -o `"$outputReportPath`""
}

if($url){
  $executionExpression = "$executionExpression -u `"$url`""
}

Invoke-Expression "& $executionExpression"

if ($lastexitcode -gt 0) { 
  throw "Failures reported in javascript integration tests." 
}