@echo off
if exist "test_direct.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject\bin\Release\netstandard2.1\cfdcs.* dotnet_project\CfdCsharpProject.xTests\bin\Release\netcoreapp3.0

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet test -c Release --no-build

cd ../..
