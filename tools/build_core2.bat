setlocal
@echo off
if exist "build_core2.bat" (
  cd ..
)

REM set PATH=%PATH:C:\Program Files\Git\usr\bin;=%
REM set PATH=%PATH:C:\Program Files (x86)\Git\usr\bin;=%

CALL cmake -S . -B build -A x64 -DENABLE_CSHARP=off -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DENABLE_TESTS=off -DCMAKE_BUILD_TYPE=Release
if not %ERRORLEVEL% == 0 (
    exit /b 1
)

CALL cmake --build build --parallel 4 --config Release
if not %ERRORLEVEL% == 0 (
    exit /b 1
)

cd dotnet_project/CfdCsharpProject_Core2
CALL dotnet build -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..

cd dotnet_project/CfdCsharpProject_Core2.xTests
CALL dotnet build -c Release
if not %ERRORLEVEL% == 0 (
    cd ../..
    exit /b 1
)
cd ../..
