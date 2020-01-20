@echo off
if exist "build.bat" (
  cd ..
)

cmake -S . -B build -G "Visual Studio 16 2019" -A x64 -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DCMAKE_BUILD_TYPE=Release -DCMAKE_GENERATOR_PLATFORM=x64
cmake --build build --parallel --config Release
