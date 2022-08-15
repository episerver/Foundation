@echo off
cd /d %~dp0
openfiles > NUL 2>&1
if %ERRORLEVEL% NEQ 0 (
	set "errorMessage=Build.cmd script must be run in an elevated (admin) command prompt"
	goto error
)

mode con:cols=120 lines=5000
set ROOTPATH=%cd%
set ROOTDIR=%cd%
set SOURCEPATH=%ROOTPATH%\src

if "%~1" == "" goto loop

set APPNAME=%~1
set SQLSERVER=%~2
set ADDITIONAL_SQLCMD=%~3
goto main

:loop
set SQLSERVER=
set ADDITIONAL_SQLCMD=
set /p APPNAME=Enter your app name (required):
set /p SQLSERVER=Enter your SQL server name (optional, press Enter for default (.) local server):
set /p ADDITIONAL_SQLCMD=Enter your sqlcmd command (optional, press Enter for default (-E) windows auth):

set check=false
if "%APPNAME%"=="" (set check=true)
if "%check%"=="true" (
	echo Parameters missing, application name is required, foundation domain name, commerce manager domain name and LICENSE path are optional
	pause
	cls
	goto loop
)

:main
if "%SQLSERVER%"=="" (set SQLSERVER=.)
if "%ADDITIONAL_SQLCMD%"=="" (set ADDITIONAL_SQLCMD=-E)

cls
echo Your application name is: %APPNAME%
echo Your SQL server name is: %SQLSERVER%
echo Your SQLCMD command is: sqlcmd -S %SQLSERVER% %ADDITIONAL_SQLCMD%
timeout 15

set cms_db=%APPNAME%.Cms
set commerce_db=%APPNAME%.Commerce
set user=%cms_db%User
set password=Episerver123!
set errorMessage = "" 

cls
echo ######################################################################
echo #     Grab a tea or coffee, this could take around 5 to 10 mins      #
echo ######################################################################
echo #                                                                    #
echo #                         (  )   (   )  )                            #
echo #                          ) (   )  (  (                             #
echo #                          ( )  (    ) )                             #
echo #                          _____________                             #
echo #                         ^|_____________^| ___                      #
echo #                         ^|             ^|/ _ \                     #
echo #                         ^|             ^| ^| ^|                    #
echo #                         ^| Optimizely  ^|_^| ^|                    #
echo #                      ___^|             ^|\___/                     #
echo #                     /    \___________/    \                        #
echo #                     \_____________________/                        #
echo #                                                                    #
echo ######################################################################

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

echo ## NPM Install ##  
echo ## NPM Install > Build\Logs\Build.log
cd %SOURCEPATH%\Foundation
CALL npm ci
IF %errorlevel% NEQ 0 (
	set errorMessage=%errorlevel%
	goto error
)
CALL npm run dev
cd %ROOTPATH%

echo ## Clean and build ##
echo ## Clean and build ## >> Build\Logs\Build.log				 
"%InstallDir%%msBuildPath%" Foundation.sln /t:Clean,Build  >> Build\Logs\Build.log

set sql=sqlcmd -S %SQLSERVER% %ADDITIONAL_SQLCMD%
echo ## %sql% ##

echo ## Dropping databases ##
echo ## Dropping databases ## > Build\Logs\Database.log
%sql% -Q "EXEC msdb.dbo.sp_delete_database_backuphistory N'%cms_db%'" >> Build\Logs\Database.log
%sql% -Q "if db_id('%cms_db%') is not null ALTER DATABASE [%cms_db%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE" >> Build\Logs\Database.log
%sql% -Q "if db_id('%cms_db%') is not null DROP DATABASE [%cms_db%]" >> Build\Logs\Database.log
%sql% -Q "EXEC msdb.dbo.sp_delete_database_backuphistory N'%commerce_db%'" >> Build\Logs\Database.log
%sql% -Q "if db_id('%commerce_db%') is not null ALTER DATABASE [%commerce_db%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE" >> Build\Logs\Database.log
%sql% -Q "if db_id('%commerce_db%') is not null DROP DATABASE [%commerce_db%]" >> Build\Logs\Database.log

echo ## Dropping user ##
echo ## Dropping user ## >> Build\Logs\Database.log
%sql% -Q "if exists (select loginname from master.dbo.syslogins where name = '%user%') EXEC sp_droplogin @loginame='%user%'" >> Build\Logs\Database.log

dotnet tool update EPiServer.Net.Cli --global --add-source https://nuget.optimizely.com/feed/packages.svc/
dotnet-episerver create-cms-database ".\src\Foundation\Foundation.csproj" -S "%SQLSERVER%" %ADDITIONAL_SQLCMD%  --database-name "%APPNAME%.Cms"
dotnet-episerver create-commerce-database ".\src\Foundation\Foundation.csproj" -S "%SQLSERVER%" %ADDITIONAL_SQLCMD%  --database-name "%APPNAME%.Commerce" --reuse-cms-user

echo ## Installing foundation configuration ##
echo ## Installing foundation configuration ## >> Build\Logs\Database.log
%sql% -d %commerce_db% -b -i "Build\SqlScripts\FoundationConfigurationSchema.sql" -v appname=%APPNAME% >> Build\Logs\Database.log

echo ## Installing unique coupon schema ##
echo ## Installing unique coupon schema ## >> Build\Logs\Database.log
%sql% -d %commerce_db% -b -i "Build\SqlScripts\UniqueCouponSchema.sql" >> Build\Logs\Database.log

:error
if NOT "%errorMessage%"=="" echo %errorMessage%

echo Run resetup.cmd to resetup solution
echo @echo off > resetup.cmd
echo cls >> resetup.cmd
echo echo ###################################################################### >> resetup.cmd
echo echo #           Rebuid the current application from default              # >> resetup.cmd
echo echo ###################################################################### >> resetup.cmd
echo echo #                                                                    # >> resetup.cmd
echo echo #       NOTE: This will **DROP** the existing DB                     # >> resetup.cmd
echo echo #             and resetup so use with caution!!                      # >> resetup.cmd
echo echo #                                                                    # >> resetup.cmd
echo echo #       Crtl+C NOW if you are unsure!                                # >> resetup.cmd
echo echo #                                                                    # >> resetup.cmd
echo echo ###################################################################### >> resetup.cmd
echo pause >> resetup.cmd
echo setup %APPNAME% %SQLSERVER% "%ADDITIONAL_SQLCMD%" >> resetup.cmd

pause
