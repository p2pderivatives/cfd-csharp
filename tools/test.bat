@echo off
if exist "test.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject\bin\Release\net5.0\cfdcs.* dotnet_project\CfdCsharpProject.xTests\bin\Release\net5.0
COPY /Y /B build\Release\* dotnet_project\CfdCsharpProject.xTests\bin\Release\net5.0

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet test -f net5.0 -c Release --no-build
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit 1
)
cd ../..
