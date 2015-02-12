param($installPath, $toolsPath, $package, $project)

$projectPath = Split-Path $project.FileName

Write-Host "#############################"
Write-Host "TestRunnerJS files location: $toolsPath"
Write-Host "Installation location: $projectPath"
Write-Host "#############################"

Copy-Item -Path "$toolsPath\*" -Destination "$projectPath" -Exclude "*.ps1" -recurse -Force