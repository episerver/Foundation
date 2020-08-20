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
set APPCMD=%windir%\system32\inetsrv\appcmd.exe

if "%~1" == "" goto loop

for /f "tokens=1-7*" %%a in ("%*") do (
	set APPNAME=%%a
	set FOUNDATIONDOMAIN=%%b
	set CMDOMAIN=%%c
	set LICENSEPATH=%%d
	set SQLSERVER=%%e
	set ADDITIONAL_SQLCMD=%%f
	set SETUP_CONRTOLLER=%%g
)
goto main

:loop
set APPNAME=
set FOUNDATIONDOMAIN=
set CMDOMAIN=
set LICENSEPATH=
set SQLSERVER=
set ADDITIONAL_SQLCMD=
set /p APPNAME=Enter your application name (required):
set /p FOUNDATIONDOMAIN=Enter your public domain name for foundation(optional, press Enter to leave it blank):
set /p CMDOMAIN=Enter your public domain name for commerce manager(optional, press Enter to leave it blank):
set /p LICENSEPATH=Enter your LICENSE path (optional, press Enter to leave it blank):
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
if "%FOUNDATIONDOMAIN%"=="" (set FOUNDATIONDOMAIN="%APPNAME%")
if "%CMDOMAIN%"=="" (set CMDOMAIN="%APPNAME%-cm")
if "%SQLSERVER%"=="" (set SQLSERVER=.)
if "%ADDITIONAL_SQLCMD%"=="" (set ADDITIONAL_SQLCMD=-E)
if "%LICENSEPATH%"=="" (set LICENSEPATH="")

cls
echo Your application name is: %APPNAME%
echo Your foundation domain name is: %FOUNDATIONDOMAIN%
echo Your commerce manager domain name is: %CMDOMAIN%
echo Your LICENSE path is: %LICENSEPATH%
echo Your SQL server name is: %SQLSERVER%
echo Your SQLCMD command is: sqlcmd -S %SQLSERVER% %ADDITIONAL_SQLCMD%
timeout 15

set cms_db=%APPNAME%.Cms
set commerce_db=%APPNAME%.Commerce
set user=%APPNAME%User
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
echo #                         ^|_____________^| ___                        #
echo #                         ^|             ^|/ _ \                       #
echo #                         ^|               ^| ^| ^|                      #
echo #                         ^|     EPI       ^|_^| ^|                      #
echo #                      ___^|             ^|\___/                       #
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

echo ## Set folder permissions ##
md "%ROOTPATH%\Build\Logs" 2>nul
icacls "%ROOTPATH%\\" /grant *S-1-1-0:(OI)(CI)F /T > nul

echo ## Restoring Nuget packages ##
echo ## Restoring Nuget packages ## >> Build\Logs\Build.log
.\build\nuget restore "%ROOTPATH%\Foundation.sln" >> Build\Logs\Build.log

echo ## NPM Install ##  
echo ## NPM Install >> Build\Logs\Build.log
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
"%InstallDir%%msBuildPath%" "%ROOTPATH%\Foundation.sln" /t:Clean,Build  >> Build\Logs\Build.log

:: Determine package folders
for /f " tokens=*" %%i in ('dir "Packages\EPiServer.CMS.Core*" /b /o:d') do (set cms_core=%%i) 
for /f " tokens=*" %%i in ('dir "Packages\EPiServer.Commerce.Core*" /b /o:d') do (set commerce_core=%%i) 
for /f " tokens=*" %%i in ('dir "Packages\EPiServer.Marketing.Testing*" /b /o:d') do (set testing_core=%%i) 
for /f " tokens=*" %%i in ('dir "Packages\EPiServer.Find.Cms.1*" /b /o:d') do (set find_cms=%%i) 

if "%cms_core%"=="" (
	set errorMessage=CMS Core package is missing. Please build the project before running the setup
	goto error
)
if "%testing_core%"=="" (
	set errorMessage=EPiServer.Marketing.Testing package is missing. Please build the project before running the setup
	goto error
)
if "%find_cms%"=="" (
	set errorMessage=EPiServer.Find.Cms package is missing. Please build the project before running the setup
	goto error
)
set sql=sqlcmd -S %SQLSERVER% %ADDITIONAL_SQLCMD%

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

echo ## Creating databases ##
echo ## Creating databases ## >> Build\Logs\Database.log
md "%SOURCEPATH%\appdata\db" 2>nul >> Build\Logs\Database.log
%sql% -Q "CREATE DATABASE [%cms_db%] COLLATE SQL_Latin1_General_CP1_CI_AS" >> Build\Logs\Database.log
%sql% -Q "CREATE DATABASE [%commerce_db%] COLLATE SQL_Latin1_General_CP1_CI_AS" >> Build\Logs\Database.log

%sql% -Q "SET NOCOUNT ON; SELECT ServerProperty('Edition')" -h -1 | findstr /c:"SQL Azure" 1>nul

IF %ERRORLEVEL% EQU 0 (
	echo ## Creating user in azure sql##
	echo ## Creating user in azure sql## >> Build\Logs\Database.log
	%sql% -d %cms_db% -Q "CREATE USER %user% WITH PASSWORD='%password%'" >> Build\Logs\Database.log
	%sql% -d %cms_db% -Q "ALTER ROLE db_owner ADD MEMBER %user%" >> Build\Logs\Database.log
	%sql% -d %commerce_db% -Q "CREATE USER %user% WITH PASSWORD='%password%'" >> Build\Logs\Database.log
	%sql% -d %commerce_db% -Q "ALTER ROLE db_owner ADD MEMBER %user%" >> Build\Logs\Database.log
) else (
	echo ## Creating user##
	echo ## Creating user## >> Build\Logs\Database.log
	%sql% -Q "EXEC sp_addlogin @loginame='%user%', @passwd='%password%', @defdb='%cms_db%'" >> Build\Logs\Database.log
	%sql% -d %cms_db% -Q "EXEC sp_adduser @loginame='%user%'" >> Build\Logs\Database.log
	%sql% -d %cms_db% -Q "EXEC sp_addrolemember N'db_owner', N'%user%'" >> Build\Logs\Database.log
	%sql% -d %commerce_db% -Q "EXEC sp_adduser @loginame='%user%'" >> Build\Logs\Database.log
	%sql% -d %commerce_db% -Q "EXEC sp_addrolemember N'db_owner', N'%user%'" >> Build\Logs\Database.log 
)


echo ## Installing CMS database ##
echo ## Installing CMS database ## >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%cms_core%\tools\EPiServer.Cms.Core.sql" >> Build\Logs\Database.log

echo ## Installing Commerce database ##
echo ## Installing Commerce database ## >> Build\Logs\Database.log
%sql% -d %commerce_db% -b -i "packages\%commerce_core%\tools\EPiServer.Commerce.Core.sql" >> Build\Logs\Database.log

echo ## Installing EPiServer.Find.Cms schema ##
echo ## Installing EPiServer.Find.Cms schema ## >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\1.0.1.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\12.2.8.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\12.4.2.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\13.0.4.sql" >> Build\Logs\Database.log

echo ## Installing EPiServer.Marketing schema ##
echo ## Installing EPiServer.Marketing schema ## >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.1.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.2.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.3.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.4.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.5.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.6.sql" >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.7.sql" >> Build\Logs\Database.log

echo ## Installing ASP.NET Identity ##
echo ## Installing ASP.NET Identity ## >> Build\Logs\Database.log
%sql% -d %commerce_db% -b -i "Build\SqlScripts\aspnet_identity.sql" >> Build\Logs\Database.log

echo ## Installing foundation configuration ##
echo ## Installing foundation configuration ## >> Build\Logs\Database.log
%sql% -d %commerce_db% -b -i "Build\SqlScripts\FoundationConfigurationSchema.sql" -v appname=%APPNAME% foundationhostname=%FOUNDATIONDOMAIN% cmhostname=%CMDOMAIN% >> Build\Logs\Database.log

echo ## Installing unique coupon schema ##
echo ## Installing unique coupon schema ## >> Build\Logs\Database.log
%sql% -d %commerce_db% -b -i "Build\SqlScripts\UniqueCouponSchema.sql" >> Build\Logs\Database.log

echo ## Installing Service API CMS ##
echo ## Installing Service API CMS ## >> Build\Logs\Database.log
%sql% -d %cms_db% -b -i "Build\SqlScripts\ServiceApiCms.sql" >> Build\Logs\Database.log

echo ## Installing Service API Commerce ##
echo ## Installing Service API Commerce ## >> Build\Logs\Database.log
%sql% -d %commerce_db% -b -i "Build\SqlScripts\ServiceApiCommerce.sql" >> Build\Logs\Database.log

echo ## Cleaning packages folder ##
echo ## Cleaning packages folder ## >> Build\Logs\Build.log
echo %ROOTPATH%\packages\*.*
RD "%ROOTPATH%\packages\" /Q /S >> Build\Logs\Build.log

echo ## Set folder permissions ##
echo ## Set folder permissions ## >> Build\Logs\Build.log
icacls "%ROOTPATH%" /grant Everyone:(OI)(CI)F /T > nul
attrib -r "%ROOTPATH%\*.*" /s > nul

echo ## Creating IIS application pools ##
echo ## Creating IIS application pools ## > Build\Logs\IIS.log
%APPCMD%  list site /name:"%APPNAME%"
IF "%ERRORLEVEL%" EQU "0" (
    %APPCMD% delete site "%APPNAME%"
) 

%APPCMD%  list apppool /name:"%APPNAME%"
IF "%ERRORLEVEL%" EQU "0" (
     %APPCMD% delete apppool "%APPNAME%"
) 

%APPCMD% add apppool /name:%APPNAME% >> Build\Logs\IIS.log
%APPCMD% set config -section:applicationPools "/[name='%APPNAME%'].processModel.loadUserProfile:true" >> Build\Logs\IIS.log

%APPCMD%  list site /name:"%APPNAME%-cm"
IF "%ERRORLEVEL%" EQU "0" (
     %APPCMD% delete site "%APPNAME%-cm"
) 

%APPCMD% list apppool /name:"%APPNAME%-cm"
IF "%ERRORLEVEL%" EQU "0" (
     %APPCMD% delete apppool "%APPNAME%-cm"
) 

%APPCMD%  add apppool /name:%APPNAME%-cm >> Build\Logs\IIS.log
%APPCMD%  set config -section:applicationPools "/[name='%APPNAME%-cm'].processModel.loadUserProfile:true" >> Build\Logs\IIS.log

echo ## Creating IIS applications ##
echo ## Creating IIS applications ## >> Build\Logs\IIS.log
%windir%\system32\inetsrv\appcmd add site /name:%APPNAME% /physicalPath:"%SOURCEPATH%\Foundation" /bindings:http/*:80:%APPNAME% >> Build\Logs\IIS.log
if "%FOUNDATIONDOMAIN%"=="" GOTO cmbinding
%windir%\system32\inetsrv\appcmd set site "%APPNAME%"  /+bindings.[protocol='http',bindingInformation='*:80:%FOUNDATIONDOMAIN%'] >> Build\Logs\IIS.log

:cmbinding
%windir%\system32\inetsrv\appcmd add site /name:%APPNAME%-cm /physicalPath:"%SOURCEPATH%\Foundation.CommerceManager" /bindings:http/*:80:%APPNAME%-cm >> Build\Logs\IIS.log
if "%CMDOMAIN%"=="" GOTO finish
%windir%\system32\inetsrv\appcmd set site "%APPNAME%-cm"  /+bindings.[protocol='http',bindingInformation='*:80:%CMDOMAIN%'] >> Build\Logs\IIS.log

:finish
echo ## Adding site to app pool ##
echo ## Adding site to app pool ## >> Build\Logs\IIS.log
%systemroot%\system32\inetsrv\APPCMD set app "%APPNAME%/" /applicationPool:%APPNAME% >> Build\Logs\IIS.log
%systemroot%\system32\inetsrv\APPCMD set app "%APPNAME%-cm/" /applicationPool:%APPNAME%-cm >> Build\Logs\IIS.log

echo ## Updating host file ##
find /C /I "%APPNAME%" %WINDIR%\system32\drivers\etc\hosts
if %ERRORLEVEL% NEQ 0 (
	echo: >> %WINDIR%\System32\drivers\etc\hosts
	echo 127.0.0.1 %APPNAME% >> %WINDIR%\System32\drivers\etc\hosts
)

find /C /I "%APPNAME%-cm" %WINDIR%\system32\drivers\etc\hosts
if %ERRORLEVEL% NEQ 0 (
	echo|set /p=127.0.0.1 %APPNAME%-cm >> %WINDIR%\System32\drivers\etc\hosts
)

echo ## Copying licence file ##
echo ## Copying licence file ## >> Build\Logs\IIS.log
if "%LICENSEPATH%"=="" ( echo License file not found! ) else ( xcopy "%LICENSEPATH%" "%SOURCEPATH%\Foundation" /Y >> Build\Logs\IIS.log)

echo ## Updating commerce manager URL ##
echo ## Updating commerce manager URL ## >> Build\Logs\IIS.log
if "%CMDOMAIN%"=="" (
    call "%ROOTDIR%\build\jrepl" "http://localhost:63149" "http://%APPNAME%-cm" /f "%SOURCEPATH%\Foundation\appSettings.config" /L /o -
) else (
    call "%ROOTDIR%\build\jrepl" "http://localhost:63149" "http://%CMDOMAIN%" /f "%SOURCEPATH%\Foundation\appSettings.config" /L /o -
)

echo ## Creating conectionstrings.config ##
echo ## Creating conectionstrings.config ## >> Build\Logs\Build.log
(
echo ^<connectionStrings^>
echo ^<clear /^>
echo ^<add name="EPiServerDB" connectionString="Data Source=%SQLSERVER%;Initial Catalog=%cms_db%;User Id=%user%;Password=%password%;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" /^>
echo ^<add name="EcfSqlConnection" connectionString="Data Source=%SQLSERVER%;Initial Catalog=%commerce_db%;User Id=%user%;Password=%password%;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" /^>
echo ^<!--  
echo ^<add name="EPiServerAzureBlobs" connectionString="DefaultEndpointsProtocol=https;AccountName=ChangeThis;AccountKey=ChangeThis" /^>
echo ^<add name="EPiServerAzureEvents" connectionString="Endpoint=sb://ChangeThis.servicebus.windows.net/;SharedAccessKeyName=ChangeThis;SharedAccessKey=ChangeThis" /^>
echo --^>
echo ^</connectionStrings^>
) > "%SOURCEPATH%\Foundation\connectionStrings.config"
(
echo ^<connectionStrings^>
echo ^<clear /^>
echo ^<add name="EPiServerDB" connectionString="Data Source=%SQLSERVER%;Initial Catalog=%cms_db%;User Id=%user%;Password=%password%;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" /^>
echo ^<add name="EcfSqlConnection" connectionString="Data Source=%SQLSERVER%;Initial Catalog=%commerce_db%;User Id=%user%;Password=%password%;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" /^>
echo ^<!--  
echo ^<add name="EPiServerAzureBlobs" connectionString="DefaultEndpointsProtocol=https;AccountName=ChangeThis;AccountKey=ChangeThis" /^>
echo ^<add name="EPiServerAzureEvents" connectionString="Endpoint=sb://ChangeThis.servicebus.windows.net/;SharedAccessKeyName=ChangeThis;SharedAccessKey=ChangeThis" /^>
echo --^>
echo ^</connectionStrings^>
) > "%SOURCEPATH%\Foundation.CommerceManager\connectionStrings.config"

if "%SETUP_CONRTOLLER%"=="" (
    start http://%FOUNDATIONDOMAIN%/
) else (
    start http://%FOUNDATIONDOMAIN%/setup
)
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
echo setup %APPNAME% %FOUNDATIONDOMAIN% %CMDOMAIN% %LICENSEPATH% %SQLSERVER% %ADDITIONAL_SQLCMD% >> resetup.cmd

pause