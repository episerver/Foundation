@echo off

set APPCMD=%windir%\system32\inetsrv\appcmd.exe
set errorMessage = "" 

if "%~1" == "" (
	set errorMessage=Missing input parameter app name and physical path. Domain is optional.
	goto error
)

for /f "tokens=1-3*" %%a in ("%*") do (
	set APPNAME=%%a
	set PHYSICALPATH=%%b
	set DOMAIN=%%c
)
set APPNAMEBLANK=%APPNAME:"=%
goto configureSite

:configureSite
%APPCMD% add site /name:"%APPNAME%" /physicalPath:"%PHYSICALPATH%" /bindings:http/*:80:"%APPNAME%"
if %DOMAIN%=="" GOTO configurePool
%APPCMD% set site "%APPNAME%" /+bindings.[protocol='http',bindingInformation='*:80:%DOMAIN%']
%APPCMD% set site "%APPNAME%" /+bindings.[protocol='https',bindingInformation='*:443:%DOMAIN%']

:configurePool
%APPCMD% add apppool /name:"%APPNAME%"
%APPCMD% set apppool "%APPNAME%" /processmodel.identityType:LocalSystem
%APPCMD% set apppool "%APPNAME%" /processmodel.idleTimeout:0.01:00:00
:: App name must be followed by a forward slash
%APPCMD% set app "%APPNAME%/" /applicationPool:"%APPNAME%"
%APPCMD% start sites "%APPNAME%"

:: Update hosts file, first with a blank line then the new site
find /C /I "%APPNAMEBLANK%" %WINDIR%\system32\drivers\etc\hosts
if %ERRORLEVEL% NEQ 0 (
	echo: >> "%windir%\system32\drivers\etc\hosts"
	echo | set /p hostdata = 127.0.0.1 %APPNAMEBLANK% >> "%windir%\system32\drivers\etc\hosts"
)

echo Setup finished

:error
if NOT "%errorMessage%"=="" echo %errorMessage%