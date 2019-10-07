@echo off
openfiles > NUL 2>&1
if %ERRORLEVEL% NEQ 0 (
	set "errorMessage=Build.bat script must be run in an elevated (admin) command prompt"
	goto error
)

mode con:cols=120 lines=4000
set ROOTPATH=%cd%
set ROOTDIR=%cd%

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

echo ## Restoring Nuget packages ##
echo ## Restoring Nuget packages ## >> Build\Logs\Pack.log
.\build\nuget restore %ROOTPATH%\Foundation.sln >> Build\Logs\Pack.log

echo ## Clean and build ##
echo ## Clean and build ## >> Build\Logs\Pack.log				 
"%InstallDir%""%msBuildPath%" %ROOTPATH%\Foundation.sln /t:Clean,Build  >> Build\Logs\Pack.log

echo ## Prepare content for packages ##
echo ## Prepare content for packages ## >> Build\Logs\Pack.log	
powershell "%ROOTPATH%\build\prepare-content.ps1" >> Build\Logs\Pack.log	


echo ## Get Version ##
echo ## Get Version## >> Build\Logs\Pack.log
set /p Version=<.\build\version.props
echo ## Version %Version% ##
echo ## Version %Version% ## >> Build\Logs\Pack.log


for /F "skip=1 delims=" %%F in ('
    wmic PATH Win32_LocalTime GET Day^,Month^,Year /FORMAT:TABLE
') do (
    for /F "tokens=1-3" %%L in ("%%F") do (
        set CurrYear=%%N
    )
)
echo ## Year %CurrYear% ##
echo ## Year %CurrYear% ## >> Build\Logs\Pack.log

.\build\nuget pack ./build/Nuspecs/Foundation.campaign.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Cms.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Cms.Personalization.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Commerce.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Commerce.Personalization.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Find.Cms.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Find.Commerce.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .
.\build\nuget pack ./build/Nuspecs/Foundation.Social.nuspec -Properties Configuration=Release;Year=%CurrYear% -Version %Version% -OutputDirectory ./artifacts -BasePath .

pause
