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
brew install cmake dotnet
```

### Linux(Ubuntu)

- C# (7.1 or higher)
  - .NET Core 2.2.110 or higher
- CMake (3.14.3 or higher)

```Shell
# install dependencies using APT package Manager
apt-get install -y build-essential cmake dotnet
```

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

```Shell
# configure & build
.\tools\build.sh
```

---

## Example

### Test

- Windows (Visual Studio)
```Cmd
./tools/test.bat
```

- Windows (.NET Core only)
```Cmd
./tools/test_core3.bat
```

- Linux & MacOSX
```Shell
./tools/test.sh
```

### Example

- test/CfdTestMain.cs
  - blind/unblind: TestBlindTx()
