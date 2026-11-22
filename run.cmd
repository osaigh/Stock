@echo off
rem Runs the Stock Trader application locally. See run.ps1 for the options, for example: run.cmd -Client Angular
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0run.ps1" %*
exit /b %ERRORLEVEL%
