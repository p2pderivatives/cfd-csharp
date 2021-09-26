@echo off
if exist "formatter.bat" (
  cd ..
)

REM  -v diag

dotnet format .\dotnet_project\CfdCsharpProject\CfdCsharpProject.csproj

dotnet format .\dotnet_project\CfdCsharpProject.xTests\CfdCsharpProject.xTests.csproj 
