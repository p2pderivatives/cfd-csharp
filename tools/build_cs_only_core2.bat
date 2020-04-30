setlocal
@echo off
if exist "build_cs_only.bat" (
  cd ..
)

cd dotnet_project/CfdCsharpProject_Core2
CALL dotnet build -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..

cd dotnet_project/CfdCsharpProject_Core2.xTests
CALL dotnet build -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..
