@echo off
if exist "test_direct.bat" (
  cd ..
)

COPY /Y /B dotnet_project\CfdCsharpProject\bin\Release\netstandard2.1\cfdcs.* dotnet_project\CfdCsharpProject.xTests\bin\Release\netcoreapp3.1

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet test -f netcoreapp3.1 -c Release --no-build

cd ../..
