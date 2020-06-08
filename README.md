# Crypto Finance Development Kit for C# (CFD-CSHARP)

<!-- TODO: Write Summary and Overview

## Overview

-->

## Dependencies

### Windows

- C# (8.0 or higher)
  - .NET Core 3.0 or higher
- CMake (3.14.3 or higher)
- Build Tools for Visual Studio (2017 or higher)

### MacOS

- C# (8.0 or higher)
  - .NET Core 3.0 or higher
- CMake (3.14.3 or higher)

- [Homebrew](https://brew.sh/)

```Shell
# xcode cli tools
xcode-select --install

# install dependencies using Homebrew
brew install cmake
brew cask install dotnet-sdk
```

### Linux(Ubuntu)

- C# (8.0 or higher)
  - .NET Core 3.0 or higher
- CMake (3.14.3 or higher)

```Shell
# install dependencies using APT package Manager
apt-get install -y build-essential cmake
```

dotnet install is see manual.
(ex. https://docs.microsoft.com/dotnet/core/install/linux-package-manager-ubuntu-1804)

cmake version 3.14.2 or lower, download from website and install cmake.
(https://cmake.org/download/)

---

## Build

### Windows (Visual Studio)

```Cmd
# configure & build
.\tools\build.bat
```

### Windows (.NET Core only)

- tools/build.bat
```Cmd
# configure & build
.\tools\build_core3.bat
```

### Linux & MacOSX

- .NET Core 3.x
```Shell
# configure & build
./tools/build.sh
```

---

## Example

### Test

- Windows (Visual Studio)
```Cmd
.\tools\test.bat
```

- Windows (.NET Core only)
```Cmd
.\tools\test_core3.bat
```

- Linux & MacOSX
```Shell
echo ".NET Core 3.x"
./tools/test.sh
```

### Example

- test/CfdTestMain.cs
  - blind/unblind: TestBlindTx()

## Note

### Git connection:

Git repository connections default to HTTPS.
However, depending on the connection settings of GitHub, you may only be able to connect via SSH.
As a countermeasure, forcibly establish SSH connection by setting `CFD_CMAKE_GIT_SSH=1` in the environment variable.

- Windows: (On the command line. Or set from the system setting screen.)
```
set CFD_CMAKE_GIT_SSH=1
```

- MacOS & Linux(Ubuntu):
```
export CFD_CMAKE_GIT_SSH=1
```

### Ignore git update for CMake External Project:

Depending on your git environment, you may get the following error when checking out external:
```
  Performing update step for 'libwally-core-download'
  Current branch cmake_build is up to date.
  No stash entries found.
  No stash entries found.
  No stash entries found.
  CMake Error at /workspace/cfd-core/build/external/libwally-core/download/libwally-core-download-prefix/tmp/libwally-core-download-gitupdate.cmake:133 (message):


    Failed to unstash changes in:
    '/workspace/cfd-core/external/libwally-core/'.

    You will have to resolve the conflicts manually
```

This phenomenon is due to the `git update` related command.
Please set an environment variable that skips update processing.

- Windows: (On the command line. Or set from the system setting screen.)
```
set CFD_CMAKE_GIT_SKIP_UPDATE=1
```

- MacOS & Linux(Ubuntu):
```
export CFD_CMAKE_GIT_SKIP_UPDATE=1
```
