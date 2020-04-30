@echo off
if exist "build.bat" (
  cd ..
)

CALL cmake -S . -B build -A x64 -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DENABLE_TESTS=off -DCMAKE_BUILD_TYPE=Release -DCMAKE_GENERATOR_PLATFORM=x64
if not %ERRORLEVEL% == 0 (
    exit /b 1
)

CALL cmake --build build --parallel 4 --config Release
if not %ERRORLEVEL% == 0 (
    exit /b 1
)

cd dotnet_project/CfdCsharpProject.xTests
CALL dotnet build -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..
