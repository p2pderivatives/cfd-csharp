@echo off
if exist "test_core3.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject\bin\Release\netstandard2.1\cfdcs.* dotnet_project\CfdCsharpProject.xTests\bin\Release\netcoreapp3.1
COPY /Y /B build\Release\* dotnet_project\CfdCsharpProject.xTests\bin\Release\netcoreapp3.1

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet test -f netcoreapp3.1 -c Release --no-build
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit 1
)
cd ../..
