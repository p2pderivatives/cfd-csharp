@echo off
if exist "build.bat" (
  cd ..
)

CALL cmake -S . -B build -A x64 -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DCMAKE_BUILD_TYPE=Release -DCMAKE_GENERATOR_PLATFORM=x64
if not %ERRORLEVEL% == 0 (
    exit /b 1
)

CALL cmake --build build --parallel --config Release
if not %ERRORLEVEL% == 0 (
    exit /b 1
)
