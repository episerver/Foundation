$pathToApp = './publish'
$appPoolName = 'david_cms13-upgrade'
$appcmd = 'C:\Windows\System32\inetsrv\appcmd.exe'

# Stop app pool so w3wp releases all file locks
Write-Host "Stopping app pool '$appPoolName'..."
& $appcmd stop apppool $appPoolName

# Kill any stale 'dotnet run' Foundation processes that lock bin/ files
Get-Process Foundation -ErrorAction SilentlyContinue | Stop-Process -Force

# Kill all w3wp.exe processes to release any file locks on the publish directory
# (dev environment: acceptable to bring down all sites briefly)
Get-Process w3wp -ErrorAction SilentlyContinue | Stop-Process -Force
Write-Host "Killed w3wp.exe processes."
Start-Sleep -Seconds 2

# Force full non-incremental build so source changes are always compiled
Write-Host "Building..."
dotnet build src/Foundation/Foundation.csproj -c Debug --no-restore --no-incremental
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed. Restarting app pool without deploying."
    & $appcmd start apppool $appPoolName
    exit 1
}

# Publish using the freshly built output
Write-Host "Publishing to $pathToApp..."
dotnet publish src/Foundation/Foundation.csproj -c Debug -o $pathToApp --no-restore --no-build

# dotnet publish overwrites web.config and removes environmentVariables.
# Restore ASPNETCORE_ENVIRONMENT so Development appsettings and error pages work.
$webConfig = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
        <remove name="WebDAV"/>
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\Foundation.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Development" />
        </environmentVariables>
      </aspNetCore>
      <security>
        <requestFiltering>
          <requestLimits maxAllowedContentLength="4294967295"/>
        </requestFiltering>
      </security>
      <modules>
        <remove name="WebDAVModule"/>
      </modules>
    </system.webServer>
    <system.web>
      <httpRuntime maxRequestLength="2147483647">
      </httpRuntime>
    </system.web>
  </location>
</configuration>
<!--ProjectGuid: 856C325A-2CC0-422C-B964-1BD10EA14C99-->
'@
Set-Content "$pathToApp\web.config" $webConfig
Write-Host "Restored web.config with ASPNETCORE_ENVIRONMENT=Development"

# Start the app pool
Write-Host "Starting app pool '$appPoolName'..."
& $appcmd start apppool $appPoolName
Write-Host "Done. Site is live."
