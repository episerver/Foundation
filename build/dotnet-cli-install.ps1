param(
  [string]$version = "",
  [string]$installDir = "$Env:UserProfile\.epi\dotnet"
)

Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
& (Join-Path $PSScriptRoot dotnet-install.ps1) -Version $version -Architecture x64 -InstallDir $installDir
if($LASTEXITCODE -ne 0) { throw "Failed" }