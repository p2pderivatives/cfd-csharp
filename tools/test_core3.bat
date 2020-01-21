@echo off
if exist "test_core3.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject_Core3\bin\Release\netstandard2.1\cfdcs.* dotnet_project\CfdCsharpTestProject_Core3\bin\Release\netcoreapp3.0
COPY /Y /B build\Release\* dotnet_project\CfdCsharpTestProject_Core3\bin\Release\netcoreapp3.0

cd dotnet_project/CfdCsharpTestProject
CALL dotnet run -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..
