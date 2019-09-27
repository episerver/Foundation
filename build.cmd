@echo off
setlocal
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


IF "%1"=="Debug" (set Configuration=Debug) ELSE (set Configuration=Release)
ECHO Building in %Configuration%

powershell "%CD%\build\build.ps1" -configuration %Configuration%
EXIT /B %ERRORLEVEL%