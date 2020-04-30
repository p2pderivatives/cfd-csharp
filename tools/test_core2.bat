@echo off
if exist "test_core2.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject_Core2\bin\Release\netstandard2.0\cfdcs.* dotnet_project\CfdCsharpProject_Core2.xTests\bin\Release\netcoreapp2.2
COPY /Y /B build\Release\* dotnet_project\CfdCsharpProject_Core2.xTests\bin\Release\netcoreapp2.2

cd dotnet_project/CfdCsharpProject_Core2.xTests
CALL dotnet test -c Release --no-build
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..
