#!/bin/sh
cd `git rev-parse --show-toplevel`

cmake -S . -B build -DENABLE_CSHARP=off -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DENABLE_TESTS=off -DCMAKE_BUILD_TYPE=Release -DTARGET_RPATH="./bin/Release/netcoreapp2.0;./build/Release"
cmake --build build --parallel 2 --config Release

cd dotnet_project/CfdCsharpProject_Core2
dotnet build -c Release
cd ../..

cd dotnet_project/CfdCsharpProject_Core2.xTests
dotnet build -c Release
cd ../..
