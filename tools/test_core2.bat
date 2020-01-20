@echo off
if exist "test_core2.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject_Core2\bin\Release\netstandard2.0\cfdcs.* dotnet_project\CfdCsharpTestProject_Core2\bin\Release\netcoreapp2.0
COPY /Y /B build\Release\* dotnet_project\CfdCsharpTestProject_Core2\bin\Release\netcoreapp2.0

cd dotnet_project/CfdCsharpTestProject_Core2
CALL dotnet run -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..
