@echo off
if exist "clitest.bat" (
  cd ..
)

cd dotnet_project\CfdCsharpProject.CliTests\bin\Debug\netcoreapp3.0

.\cfdcs_test.exe

pause
