#!/bin/sh
cd `git rev-parse --show-toplevel`

cp -rp dotnet_project/CfdCsharpProject_Core2/bin/Release/netstandard2.0/cfdcs.* dotnet_project/CfdCsharpProject_Core2.xTests/bin/Release/netcoreapp2.2
cp -rp build/Release/* dotnet_project/CfdCsharpProject_Core2.xTests/bin/Release/netcoreapp2.2

cd dotnet_project/CfdCsharpProject_Core2.xTests
dotnet test -c Release --no-build
cd ../..
