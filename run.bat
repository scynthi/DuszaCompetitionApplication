@echo off

if "%1"=="--ui" goto run_ui
goto run_default


:run_ui
cd GameMode
Charter.console.exe
goto end


:run_default
cd TestMode
dotnet run "%1"
goto end

:end