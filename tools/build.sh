#!/bin/sh
cd `git rev-parse --show-toplevel`

cmake -S . -B build -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DENABLE_TESTS=off -DCMAKE_BUILD_TYPE=Release -DTARGET_RPATH="./bin/Release/netcoreapp3.0;./build/Release"
cmake --build build --parallel 2 --config Release

cd dotnet_project/CfdCsharpProject
dotnet build -c Release
cd ../..

cd dotnet_project/CfdCsharpProject.xTests
dotnet build -c Release
cd ../..
