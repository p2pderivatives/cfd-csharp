@echo off
if exist "test_direct.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject\bin\Release\net5.0\cfdcs.* dotnet_project\CfdCsharpProject.xTests\bin\Release\net5.0

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet test -f net5.0 -c Release --no-build

cd ../..
