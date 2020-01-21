setlocal
@echo off
if exist "build_cs_only.bat" (
  cd ..
)

cd dotnet_project/CfdCsharpProject
CALL dotnet build -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..

cd dotnet_project/CfdCsharpTestProject
CALL dotnet build -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..
