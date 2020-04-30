#!/bin/sh
cd `git rev-parse --show-toplevel`

dotnet format -w dotnet_project/CfdCsharpProject/CfdCsharpProject.csproj

dotnet format -w dotnet_project/CfdCsharpProject.xTests/CfdCsharpProject.xTests.csproj 
