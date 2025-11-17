@echo off

if "%1"=="--ui" goto run_ui
goto run_default


:run_ui
cd GameMode
Charter.console.exe
cd ..
goto end


:run_default
cd TestMode
dotnet run "%1"
cd ..
goto end

:end