#!/bin/sh
cd `git rev-parse --show-toplevel`

cp -rp dotnet_project/CfdCsharpProject_Core2/bin/Release/netstandard2.0/cfdcs.* dotnet_project/CfdCsharpTestProject_Core2/bin/Release/netcoreapp2.0
cp -rp build/Release/* dotnet_project/CfdCsharpTestProject_Core2/bin/Release/netcoreapp2.0

cd dotnet_project/CfdCsharpTestProject_Core2
dotnet run -c Release
cd ../..
