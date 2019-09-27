param([string] $configuration = "Release")
$ErrorActionPreference = "Stop"

# set execute directory to root
$RootDir = (Get-Item $PSScriptRoot).Parent.FullName
Push-Location $RootDir

Import-Module .\build\exechelper.ps1

# Install .NET tooling
exec .\build\dotnet-cli-install.ps1

## Gettting MSBuildPath ##
Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force
Install-Module VSSetup -Scope CurrentUser -Force
    $instance = Get-VSSetupInstance -All -Prerelease | Select-VSSetupInstance -Require 'Microsoft.Component.MSBuild' -Latest
    $installDir = $instance.installationPath

$msBuild = $installDir + '\MSBuild\Current\Bin\MSBuild.exe' # VS2019
if (![System.IO.File]::Exists($msBuild))
{
    $msBuild = $installDir + '\MSBuild\15.0\Bin\MSBuild.exe' # VS2017
    if (![System.IO.File]::Exists($msBuild))
    {
        $msBuild = 'C:\Program Files (x86)\MSBuild\MSBuild\15.0\Bin\MSBuild.exe'
        if (![System.IO.File]::Exists($msBuild))
        {
            $msBuild = 'C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe'
            if (![System.IO.File]::Exists($msBuild))
            {
                Write-Host "MSBuild doesn't exist. Exit."
                exit 1
            }
            else {
                $msbuildPath = "C:\Program Files (x86)\MSBuild\14.0\Bin"
            }
        }
        else {
            $msbuildPath = "C:\Program Files (x86)\MSBuild\MSBuild\15.0\Bin"
        }
    }
    else {
        $msbuildPath = $installDir + "\MSBuild\15.0\Bin"
    }
}
else {
    $msbuildPath = $installDir + "\MSBuild\Current\Bin"
}


exec $msBuild "Foundation.sln"


