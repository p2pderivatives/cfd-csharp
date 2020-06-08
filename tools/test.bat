@echo off
if exist "test.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject\bin\Release\netstandard2.1\cfdcs.* dotnet_project\CfdCsharpProject.xTests\bin\Release\netcoreapp3.0
COPY /Y /B build\Release\* dotnet_project\CfdCsharpProject.xTests\bin\Release\netcoreapp3.0

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet test -c Release --no-build
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit 1
)
cd ../..
