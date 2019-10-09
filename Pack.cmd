@echo off

mode con:cols=120 lines=4000
set ROOTPATH=%cd%
set ROOTDIR=%cd%
set Configuration=%1
set Version=%2

echo ## Building Foundation please check the Build\Logs directory if you receive errors
echo ## Gettting MSBuildPath ##
for /f "usebackq tokens=*" %%i in (`.\build\vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set InstallDir=%%i
)

for %%v in (15.0, 14.0) do (
  if exist "%InstallDir%\MSBuild\%%v\Bin\MSBuild.exe" (
    set msBuildPath=\MSBuild\%%v\Bin\MSBuild.exe
	goto :finish
  )
  set  msBuildPath=\MSBuild\Current\Bin\MSBuild.exe
)

:finish
echo msbuild.exe path: %InstallDir%%msBuildPath%
md "%ROOTPATH%\Build\Logs" 2>nul
echo ## Restoring Nuget packages ##
echo ## Restoring Nuget packages ## >> Build\Logs\Pack.log
.\build\nuget restore %ROOTPATH%\Foundation.sln >> Build\Logs\Pack.log

echo ## Clean and build ##
echo ## Clean and build ## >> Build\Logs\Pack.log				 
"%InstallDir%""%msBuildPath%" %ROOTPATH%\Foundation.sln /t:Clean,Build /p:Configuration=%Configuration% >> Build\Logs\Pack.log

echo ## Prepare content for packages ##
echo ## Prepare content for packages ## >> Build\Logs\Pack.log	
powershell "%ROOTPATH%\build\prepare-content.ps1" >> Build\Logs\Pack.log	


for /F "skip=1 delims=" %%F in ('
    wmic PATH Win32_LocalTime GET Day^,Month^,Year /FORMAT:TABLE
') do (
    for /F "tokens=1-3" %%L in ("%%F") do (
        set CurrYear=%%N
    )
)
echo ## Year %CurrYear% ##
echo ## Year %CurrYear% ## >> Build\Logs\Pack.log

.\build\nuget pack ./build/Nuspecs/Foundation.campaign.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Cms.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Cms.Personalization.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Commerce.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Commerce.Personalization.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Find.Cms.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Find.Commerce.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Social.nuspec -Properties Configuration=%Configuration%;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts/nuget -BasePath .

