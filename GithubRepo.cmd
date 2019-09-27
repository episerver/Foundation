ECHO Creating github repo zip file

powershell "%CD%\build\create-github-repo.ps1"
EXIT /B %ERRORLEVEL%