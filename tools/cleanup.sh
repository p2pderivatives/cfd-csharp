#!/bin/sh
cd `git rev-parse --show-toplevel`

rm -rf build
rm -rf dotnet_project/CfdCsharpProject/bin
rm -rf dotnet_project/CfdCsharpProject/obj
rm -rf dotnet_project/CfdCsharpProject_Core2/bin
rm -rf dotnet_project/CfdCsharpProject_Core2/obj
rm -rf dotnet_project/CfdCsharpProject.xTests/bin
rm -rf dotnet_project/CfdCsharpProject.xTests/obj
rm -rf dotnet_project/CfdCsharpProject_Core2.xTests/bin
rm -rf dotnet_project/CfdCsharpProject_Core2.xTests/obj
