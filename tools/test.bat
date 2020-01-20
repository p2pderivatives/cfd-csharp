@echo off
if exist "test.bat" (
  cd ..
)

cd build\Release
.\cfdcs_test.exe
