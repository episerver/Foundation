@echo off
openfiles > NUL 2>&1
if %ERRORLEVEL% NEQ 0 (
	set "errorMessage=Build.bat script must be run in an elevated (admin) command prompt"
	goto error
)

mode con:cols=120 lines=5000
set ROOTPATH=%cd%
set ROOTDIR=%cd%
set APPCMD=%windir%\system32\inetsrv\appcmd.exe
if "%~1" == "" goto loop

set APPNAME=%1
set FOUNDATIONDOMAIN=%2
set CMDOMAIN=%3
set LICENSEPATH=%4
set SQLSERVER=%5
set ADDITIONAL_SQLCMD=%6
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

echo ## Building Foundation please check the BuildLogs directory if you receive errors
echo ## Gettting MSBuildPath ##
for /f "usebackq tokens=*" %%i in (`vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
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
icacls %ROOTPATH%\ /grant Everyone:(OI)(CI)F /T > BuildLogs\Build.log

echo ## Restoring Nuget packages ##
echo ## Restoring Nuget packages ## >> BuildLogs\Build.log
nuget restore %ROOTPATH%\Foundation.sln >> BuildLogs\Build.log

echo ## NPM Install ##  
echo ## NPM Install >> BuildLogs\Build.log
cd %ROOTPATH%\Foundation
call npm install --no-audit
cd %ROOTPATH%

echo ## Gulp Install ##
echo ## Gulp Install ## >> BuildLogs\Build.log			  
call gulp -b "%ROOTPATH%\Foundation" --color --gulpfile "%ROOTPATH%\Foundation\Gulpfile.js" >> BuildLogs\Build.log

echo ## Clean and build ##
echo ## Clean and build ## >> BuildLogs\Build.log				 
"%InstallDir%""%msBuildPath%" %ROOTPATH%\Foundation.sln /t:Clean,Build  >> BuildLogs\Build.log

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
echo ## Dropping databases ## > BuildLogs\Database.log
%sql% -Q "EXEC msdb.dbo.sp_delete_database_backuphistory N'%cms_db%'" >> BuildLogs\Database.log
%sql% -Q "if db_id('%cms_db%') is not null ALTER DATABASE [%cms_db%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE" >> BuildLogs\Database.log
%sql% -Q "if db_id('%cms_db%') is not null DROP DATABASE [%cms_db%]" >> BuildLogs\Database.log
%sql% -Q "EXEC msdb.dbo.sp_delete_database_backuphistory N'%commerce_db%'" >> BuildLogs\Database.log
%sql% -Q "if db_id('%commerce_db%') is not null ALTER DATABASE [%commerce_db%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE" >> BuildLogs\Database.log
%sql% -Q "if db_id('%commerce_db%') is not null DROP DATABASE [%commerce_db%]" >> BuildLogs\Database.log

echo ## Dropping user ##
echo ## Dropping user ## >> BuildLogs\Database.log
%sql% -Q "if exists (select loginname from master.dbo.syslogins where name = '%user%') EXEC sp_droplogin @loginame='%user%'" >> BuildLogs\Database.log

echo ## Creating databases ##
echo ## Creating databases ## >> BuildLogs\Database.log
%sql% -Q "CREATE DATABASE [%cms_db%] ON (NAME = N'%cms_db%', FILENAME = N'%ROOTPATH%\appdata\db\%cms_db%.mdf') LOG ON (NAME = N'%cms_db%_log', FILENAME = N'%ROOTPATH%\appdata\db\%cms_db%.ldf') COLLATE SQL_Latin1_General_CP1_CI_AS" >> BuildLogs\Database.log
%sql% -Q "CREATE DATABASE [%commerce_db%] ON (NAME = N'%commerce_db%', FILENAME = N'%ROOTPATH%\appdata\db\%commerce_db%.mdf') LOG ON (NAME = N'%commerce_db%_log', FILENAME = N'%ROOTPATH%\appdata\db\%commerce_db%.ldf') COLLATE SQL_Latin1_General_CP1_CI_AS" >> BuildLogs\Database.log

echo ## Creating user ##
echo ## Creating user ## >> BuildLogs\Database.log
%sql% -Q "EXEC sp_addlogin @loginame='%user%', @passwd='%password%', @defdb='%cms_db%'" >> BuildLogs\Database.log
%sql% -d %cms_db% -Q "EXEC sp_adduser @loginame='%user%'" >> BuildLogs\Database.log
%sql% -d %cms_db% -Q "EXEC sp_addrolemember N'db_owner', N'%user%'" >> BuildLogs\Database.log
%sql% -d %commerce_db% -Q "EXEC sp_adduser @loginame='%user%'" >> BuildLogs\Database.log
%sql% -d %commerce_db% -Q "EXEC sp_addrolemember N'db_owner', N'%user%'" >> BuildLogs\Database.log

echo ## Installing CMS database ##
echo ## Installing CMS database ## >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%cms_core%\tools\EPiServer.Cms.Core.sql" >> BuildLogs\Database.log

echo ## Installing Commerce database ##
echo ## Installing Commerce database ## >> BuildLogs\Database.log
%sql% -d %commerce_db% -b -i "packages\%commerce_core%\tools\EPiServer.Commerce.Core.sql" >> BuildLogs\Database.log

echo ## Installing EPiServer.Find.Cms schema ##
echo ## Installing EPiServer.Find.Cms schema ## >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\1.0.1.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\12.2.8.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\12.4.2.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%find_cms%\tools\epiupdates\sql\13.0.4.sql" >> BuildLogs\Database.log

echo ## Installing EPiServer.Marketing schema ##
echo ## Installing EPiServer.Marketing schema ## >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.1.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.2.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.3.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.4.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.5.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.6.sql" >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "packages\%testing_core%\tools\epiupdates\sql\1.0.0.7.sql" >> BuildLogs\Database.log

echo ## Installing ASP.NET Identity ##
echo ## Installing ASP.NET Identity ## >> BuildLogs\Database.log
%sql% -d %commerce_db% -b -i ".\SqlScripts\aspnet_identity.sql" >> BuildLogs\Database.log

echo ## Installing foundation configuration ##
echo ## Installing foundation configuration ## >> BuildLogs\Database.log
%sql% -d %commerce_db% -b -i "SqlScripts\FoundationConfigurationSchema.sql" -v appname=%APPNAME% foundationhostname=%FOUNDATIONDOMAIN% cmhostname=%CMDOMAIN% >> BuildLogs\Database.log

echo ## Installing unique coupon schema ##
echo ## Installing unique coupon schema ## >> BuildLogs\Database.log
%sql% -d %commerce_db% -b -i "SqlScripts\UniqueCouponSchema.sql" >> BuildLogs\Database.log

echo ## Installing Service API CMS ##
echo ## Installing Service API CMS ## >> BuildLogs\Database.log
%sql% -d %cms_db% -b -i "SqlScripts\ServiceApiCms.sql" >> BuildLogs\Database.log

echo ## Installing Service API Commerce ##
echo ## Installing Service API Commerce ## >> BuildLogs\Database.log
%sql% -d %commerce_db% -b -i "SqlScripts\ServiceApiCommerce.sql" >> BuildLogs\Database.log

echo ## Cleaning packages folder ##
echo ## Cleaning packages folder ## >> BuildLogs\Build.log
echo %ROOTPATH%\packages\*.*
RD %ROOTPATH%\packages\ /Q /S >> BuildLogs\Build.log

echo ## Set folder permissions ##
echo ## Set folder permissions ## >> BuildLogs\Build.log
icacls %ROOTPATH% /grant Everyone:(OI)(CI)F /T >> BuildLogs\Build.log
attrib -r %ROOTPATH%\*.* /s >> BuildLogs\Build.log

echo ## Creating IIS application pools ##
echo ## Creating IIS application pools ## > BuildLogs\IIS.log
%APPCMD%  list site /name:"%APPNAME%"
IF "%ERRORLEVEL%" EQU "0" (
    %APPCMD% delete site "%APPNAME%"
) 

%APPCMD%  list apppool /name:"%APPNAME%"
IF "%ERRORLEVEL%" EQU "0" (
     %APPCMD% delete apppool "%APPNAME%"
) 

%APPCMD% add apppool /name:%APPNAME% >> BuildLogs\IIS.log
%APPCMD% set config -section:applicationPools "/[name='%APPNAME%'].processModel.loadUserProfile:true" >> BuildLogs\IIS.log

%APPCMD%  list site /name:"%APPNAME%-cm"
IF "%ERRORLEVEL%" EQU "0" (
     %APPCMD% delete site "%APPNAME%-cm"
) 

%APPCMD% list apppool /name:"%APPNAME%-cm"
IF "%ERRORLEVEL%" EQU "0" (
     %APPCMD% delete apppool "%APPNAME%-cm"
) 

%APPCMD%  add apppool /name:%APPNAME%-cm >> BuildLogs\IIS.log
%APPCMD%  set config -section:applicationPools "/[name='%APPNAME%-cm'].processModel.loadUserProfile:true" >> BuildLogs\IIS.log

echo ## Creating IIS applications ##
echo ## Creating IIS applications ## >> BuildLogs\IIS.log
%windir%\system32\inetsrv\appcmd add site /name:%APPNAME% /physicalPath:"%ROOTPATH%\Foundation" /bindings:http/*:80:%APPNAME% >> BuildLogs\IIS.log
if "%FOUNDATIONDOMAIN%"=="" GOTO cmbinding
%windir%\system32\inetsrv\appcmd set site "%APPNAME%"  /+bindings.[protocol='http',bindingInformation='*:80:%FOUNDATIONDOMAIN%'] >> BuildLogs\IIS.log

:cmbinding
%windir%\system32\inetsrv\appcmd add site /name:%APPNAME%-cm /physicalPath:"%ROOTPATH%\Foundation.CommerceManager" /bindings:http/*:80:%APPNAME%-cm >> BuildLogs\IIS.log
if "%CMDOMAIN%"=="" GOTO finish
%windir%\system32\inetsrv\appcmd set site "%APPNAME%-cm"  /+bindings.[protocol='http',bindingInformation='*:80:%CMDOMAIN%'] >> BuildLogs\IIS.log

:finish
echo ## Adding site to app pool ##
echo ## Adding site to app pool ## >> BuildLogs\IIS.log
%systemroot%\system32\inetsrv\APPCMD set app "%APPNAME%/" /applicationPool:%APPNAME% >> BuildLogs\IIS.log
%systemroot%\system32\inetsrv\APPCMD set app "%APPNAME%-cm/" /applicationPool:%APPNAME%-cm >> BuildLogs\IIS.log

echo ## Updating host file ##
find /C /I "%APPNAME%" %WINDIR%\system32\drivers\etc\hosts
if %ERRORLEVEL% NEQ 0 echo 127.0.0.1 %APPNAME% >> %WINDIR%\System32\drivers\etc\hosts

find /C /I "%APPNAME%-cm" %WINDIR%\system32\drivers\etc\hosts
if %ERRORLEVEL% NEQ 0 echo 127.0.0.1 %APPNAME%-cm >> %WINDIR%\System32\drivers\etc\hosts

echo ## Copying licence file ##
echo ## Copying licence file ## >> BuildLogs\IIS.log
if "%LICENSEPATH%"=="" ( echo License file not found! ) else ( xcopy "%LICENSEPATH%" "%ROOTPATH%\Foundation" /Y >> BuildLogs\IIS.log)

echo ## Updating commerce manager URL ##
if "%CMDOMAIN%"=="" (
    call %ROOTDIR%\jrepl "http://localhost:63149" "http://%APPNAME%-cm" /f "%ROOTPATH\%Foundation\appSettings.config" /L /o -
) else (
    call %ROOTDIR%\jrepl "http://localhost:63149" "http://%CMDOMAIN%" /f "%ROOTPATH\%Foundation\appSettings.config" /L /o -
)

echo ## Creating conectionstrings.config ##
echo ## Creating conectionstrings.config ## >> BuildLogs\Build.log
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
) > %ROOTPATH\%Foundation\connectionStrings.config
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
) > %ROOTPATH\%Foundation.CommerceManager\connectionStrings.config


start http://%FOUNDATIONDOMAIN%/setup
:error
if NOT "%errorMessage%"=="" echo %errorMessage%

pause