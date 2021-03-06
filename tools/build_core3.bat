setlocal
@echo off
if exist "build_core3.bat" (
  cd ..
)

CALL cmake -S . -B build -A x64 -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DENABLE_TESTS=off -DCMAKE_BUILD_TYPE=Release
if not %ERRORLEVEL% == 0 (
    exit 1
)

CALL cmake --build build --parallel 4 --config Release
if not %ERRORLEVEL% == 0 (
    exit 1
)

cd dotnet_project/CfdCsharpProject
CALL dotnet build -f netstandard2.1 -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit 1
)
cd ../..

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet build -f netcoreapp3.1 -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit 1
)
cd ../..
