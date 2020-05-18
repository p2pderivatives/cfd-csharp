setlocal
@echo off
if exist "coverage.bat" (
  cd ..
)

REM install for https://github.com/OpenCover/opencover/releases
REM install for https://github.com/danielpalme/ReportGenerator/releases

REM OpenCover install path
set OPENCOVER="%USERPROFILE%\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe"

REM test command
set TARGET="dotnet.exe"

REM test parameter
set TARGET_TEST="test -c Release --no-build dotnet_project\CfdCsharpProject.xTests\CfdCsharpProject.xTests.csproj"

REM OpenCover output file
set OUTPUT="coverage.xml"

REM coverage target
REM set FILTERS="+[*]*"
set FILTERS="+[cfdcs]* -[xunit]*"

REM execute OpenCover
%OPENCOVER% -register:user -target:%TARGET% -targetargs:%TARGET_TEST% -filter:%FILTERS% -oldstyle -output:%OUTPUT%

REM ReportGenerator install path
set REPORTGEN="%USERPROFILE%\.nuget\packages\reportgenerator\4.5.6\tools\netcoreapp3.0\ReportGenerator.exe"

REM ReportGenerator output html dir
set OUTPUT_DIR="Coverage"

REM generate html report.
%REPORTGEN% -reports:%OUTPUT% -targetdir:%OUTPUT_DIR%
