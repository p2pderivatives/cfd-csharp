@echo off
if exist "cleanup.bat" (
  cd ..
)

rmdir /S /Q build

rmdir /S /Q dotnet_project\CfdCsharpProject\bin

rmdir /S /Q dotnet_project\CfdCsharpProject\obj

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2\bin

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2\obj

rmdir /S /Q dotnet_project\CfdCsharpProject.xTests\bin

rmdir /S /Q dotnet_project\CfdCsharpProject.xTests\obj

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2.xTests\bin

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2.xTests\obj

