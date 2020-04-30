@echo off
if exist "cleanup_cs.bat" (
  cd ..
)

rmdir /S /Q dotnet_project\CfdCsharpProject\bin

rmdir /S /Q dotnet_project\CfdCsharpProject\obj

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2\bin

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2\obj

rmdir /S /Q dotnet_project\CfdCsharpProject.xTests\bin

rmdir /S /Q dotnet_project\CfdCsharpProject.xTests\obj

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2.xTests\bin

rmdir /S /Q dotnet_project\CfdCsharpProject_Core2.xTests\obj

