@echo off
if exist "formatter.bat" (
  cd ..
)

REM  -v diag

dotnet format -w dotnet_project\CfdCsharpProject\CfdCsharpProject.csproj

dotnet format -w dotnet_project\CfdCsharpProject.xTests\CfdCsharpProject.xTests.csproj 
