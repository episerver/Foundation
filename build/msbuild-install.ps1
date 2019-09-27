param(
    [string]$version = "15.0",
    [string]$installDir = "$Env:UserProfile\.epi\msbuild"
)

function Say($str) {
    Write-Host "msbuild-install: $str"
}

function Is-MSBuild-Installed([string]$InstallRoot, [string]$RelativePathToPackage, [string]$SpecificVersion) {
    $MSBuildPackagePath = Join-Path -Path $InstallRoot -ChildPath $RelativePathToPackage | Join-Path -ChildPath $SpecificVersion
    return Test-Path $MSBuildPackagePath -PathType Container
}

$dotnetPackageRelativePath = "MSBuild"
$installed = Is-MSBuild-Installed -InstallRoot $installDir -RelativePathToPackage $dotnetPackageRelativePath -SpecificVersion $version

if (-not $installed) {
    Say "MSBuild has not been installed."

    $Url = 'https://aka.ms/vs/15/release/vs_buildtools.exe' # TODO: extract 15 from 15.0
    $msbuildInstaller = ([System.Environment]::ExpandEnvironmentVariables("%TEMP%\vs_BuildTools.exe"))

    Say "Start downloading MSBuild."
    Invoke-WebRequest -OutFile "$msBuildInstaller" $Url
    Say "Done downloading MSBuild."

    Say "Start installing MSBuild."
    $installDirCache = $installDir + "\cache"
    $installDirShared = $installDir + "\shared"
    #$process = Start-Process -FilePath "$msBuildInstaller" -Verb RunAs -Wait -PassThru -ArgumentList "uninstall --quiet --force --path install=$installDir --path cache=$installDirCache --path shared=$installDirShared"
    $process = Start-Process -FilePath "$msbuildInstaller" -Verb RunAs -Wait -PassThru -ArgumentList "--add Microsoft.VisualStudio.Workload.MSBuildTools --add Microsoft.VisualStudio.Workload.NetCoreBuildTools --add Microsoft.VisualStudio.Workload.ManagedDesktopBuildTools --add Microsoft.VisualStudio.Workload.WebBuildTools --add Microsoft.Net.Component.4.7.1.SDK --add Microsoft.Net.Component.4.7.1.TargetingPack --norestart --quiet --force --path install=$installDir --path cache=$installDirCache --path shared=$installDirShared"

    Remove-Item $msbuildInstaller

    if ($process.ExitCode -ne 0) {
        Say "Failed to install MSBuild."
        exit 1
    }
}

Say "MSBuild is installed."

exit 0