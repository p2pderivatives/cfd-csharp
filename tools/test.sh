#!/bin/sh
cd `git rev-parse --show-toplevel`

cp -rp dotnet_project/CfdCsharpProject/bin/Release/netstandard2.1/cfdcs.* dotnet_project/CfdCsharpTestProject/bin/Release/netcoreapp3.0
cp -rp build/Release/* dotnet_project/CfdCsharpTestProject/bin/Release/netcoreapp3.0

cd dotnet_project/CfdCsharpTestProject
dotnet run -c Release
cd ../..
