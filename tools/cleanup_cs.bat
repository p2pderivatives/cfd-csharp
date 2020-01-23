@echo off
if exist "cleanup_cs.bat" (
  cd ..
)

rmdir /S /Q dotnet_project\CfdCsharpProject\bin

rmdir /S /Q dotnet_project\CfdCsharpProject\obj

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2\bin

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2\obj

rmdir /S /Q dotnet_project\CfdCsharpTestProject\bin

rmdir /S /Q dotnet_project\CfdCsharpTestProject\obj

rmdir /S /Q dotnet_project\CfdCsharpTestProject_Core2\bin

rmdir /S /Q dotnet_project\CfdCsharpTestProject_Core2\obj

