#!/bin/sh
cd `git rev-parse --show-toplevel`

cp -rp dotnet_project/CfdCsharpProject/bin/Release/net5.0/cfdcs.* dotnet_project/CfdCsharpProject.xTests/bin/Release/net5.0
cp -rp build/Release/* dotnet_project/CfdCsharpProject.xTests/bin/Release/net5.0

cd dotnet_project/CfdCsharpProject.xTests
dotnet test -f net5.0 -c Release --no-build
if [ $? -gt 0 ]; then
  cd ../..
  exit 1
fi

cd ../..
