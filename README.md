# Crypto Finance Development Kit for C# (CFD-CSHARP)

<!-- TODO: Write Summary and Overview

## Overview

-->

## Dependencies

### Windows

- C# (7.1 or higher)
  - .NET Framework 4.7 or higher
  - .NET Core 2.2.110 or higher
- CMake (3.14.3 or higher)
- Build Tools for Visual Studio (2017 or higher)

### MacOS

- C# (7.1 or higher)
  - .NET Core 2.2.110 or higher
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

- C# (7.1 or higher)
  - .NET Core 2.2.110 or higher
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

- .NET Core 2.x
```Shell
# configure & build
./tools/build_core2.sh
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

echo ".NET Core 2.x"
./tools/test_core2.sh
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
