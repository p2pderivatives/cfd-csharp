# Crypto Finance Development Kit for C# (CFD-CSHARP)

<!-- TODO: Write Summary and Overview

## Overview

-->

## Dependencies

- C# (7.1 or higher)
  - .NET Framework 4.7 or higher
  - .NET Core 2.2.110 or higher
- CMake (3.14.3 or higher)

### Windows

### MacOS

- [Homebrew](https://brew.sh/)

```Shell
# xcode cli tools
xcode-select --install

# install dependencies using Homebrew
brew install cmake dotnet
```

### Linux(Ubuntu)

```Shell
# install dependencies using APT package Manager
apt-get install -y build-essential cmake dotnet
```

cmake version 3.14.2 or lower, download from website and install cmake.
(https://cmake.org/download/)

go version 1.11 or lower, get `golang.org/dl/go1.13` or higher.
(https://www.mazn.net/blog/2019/02/03/1704.html)

---

## Build

### Using CMake

- Visual Studio 2019 x64
```Bash
# configure & build
cmake -S . -B build -G "Visual Studio 16 2019" -A x64
cmake -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DCMAKE_BUILD_TYPE=Release --build build
cmake --build build --parallel --config Release
dir .\build\Release
```

- Visual Studio 2017 x64
```Bash
# configure & build
cmake -S . -B build -G "Visual Studio 15 2017 Win64"
cmake -DENABLE_SHARED=on -DENABLE_JS_WRAPPER=off -DENABLE_CAPI=on -DCMAKE_BUILD_TYPE=Release --build build
cmake --build build --parallel --config Release
dir .\build\Release
```

**CMake options**

`cmake .. (CMake options) -DENABLE_JS_WRAPPER=off`

- `-DENABLE_ELEMENTS`: Enable functionalies for elements sidechain. [ON/OFF] (default:ON)
- `-DENABLE_SHARED`: Enable building a shared library. [ON/OFF] (default:ON)
- `-DENABLE_TESTS`: Enable building a testing codes. If enables this option, builds testing framework submodules(google test) automatically. [ON/OFF] (default:ON)
- `-DCMAKE_BUILD_TYPE=Release`: Enable release build.
- `-DCMAKE_BUILD_TYPE=Debug`: Enable debug build.
- `-DCFDCORE_DEBUG=on`: Enable cfd debug mode and loggings log files. [ON/OFF] (default:OFF)

---

## Example

### Test

```Shell
./build/Release/cfdcs_test.exe
```

### Example

- test/CfdTestMain.cs