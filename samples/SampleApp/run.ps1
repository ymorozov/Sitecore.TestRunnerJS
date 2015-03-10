param(
  [Parameter(Mandatory=$True,Position=0)]
  [string]$instanceName,

  [Parameter(Mandatory=$True,Position=1)]
  [string]$applicationName
)

$websitePath = $PSScriptRoot
$nugetPath = "$PSScriptRoot\.nuget\nuget.exe"
$packagesPath = "$websitePath\packages"

$phantomVersion = "1.9.8"
$testRunnerVersion = "0.6.3"

&$nugetPath 'install' 'phantomjs' '-Version' $phantomVersion '-o' $packagesPath
&$nugetPath 'install' 'Sitecore.TestRunnerJS' '-Version' $testRunnerVersion '-o' $packagesPath

$phantomPath = "$packagesPath\PhantomJS.$phantomVersion\tools\phantomjs\phantomjs.exe"
Copy-Item -Path "$packagesPath\Sitecore.TestRunnerJS.$testRunnerVersion\tools\*" -Destination "$websitePath" -recurse -Force

$phantomScriptsFolderPath = "$websitePath\sitecore\TestRunnerJS\phantom"
$loaderPath = "$phantomScriptsFolderPath\loader.js"
$reportPath = "$phantomScriptsFolderPath\report.js"

&$phantomPath $loaderPath $instanceName $applicationName $reportPath 180000

if (!$?) { 
  throw "Failures reported in javascript integration tests." 
}