#!/bin/sh
cd `git rev-parse --show-toplevel`

cp -rp dotnet_project/CfdCsharpProject/bin/Release/netstandard2.1/cfdcs.* dotnet_project/CfdCsharpProject.xTests/bin/Release/netcoreapp3.1
cp -rp build/Release/* dotnet_project/CfdCsharpProject.xTests/bin/Release/netcoreapp3.1

cd dotnet_project/CfdCsharpProject.xTests
dotnet test -f netcoreapp3.1 -c Release --no-build
if [ $? -gt 0 ]; then
  cd ../..
  exit 1
fi

cd ../..
