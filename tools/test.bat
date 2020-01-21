@echo off
if exist "test.bat" (
  cd ..
)

CALL .\build\Release\cfdcs_test.exe
if not %ERRORLEVEL% == 0 (
    exit /b 1
)
