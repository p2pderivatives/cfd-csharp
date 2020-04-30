cmake_minimum_required(VERSION 3.13)

cmake_policy(SET CMP0076 NEW)
#cmake_policy(SET CMP0015 NEW)

####################
# options
####################
# set target visual studio, and arch type:
# https://cmake.org/cmake/help/v3.14/generator/Visual%20Studio%2016%202019.html

# use "cmake -DCMAKE_BUILD_TYPE=Debug" or "cmake-js -D"
# option(ENABLE_DEBUG "enable debugging (ON or OFF. default:OFF)" OFF)
option(ENABLE_ELEMENTS "enable elements code (ON or OFF. default:ON)" ON)
option(ENABLE_BITCOIN  "enable bitcoin code (ON or OFF. default:ON)" ON)
option(ENABLE_CAPI  "enable c-api (ON or OFF. default:ON)" ON)
option(ENABLE_SHARED "enable shared library (ON or OFF. default:ON)" ON)
option(CFD_SHARED "force shared build (ON or OFF. default:ON)" ON)

if(NOT WIN32)
#option(TARGET_RPATH "target rpath list (separator is ';') (default:)" "")
set(TARGET_RPATH "" CACHE STRING "target rpath list (separator is ';') (default:)")
option(ENABLE_COVERAGE "enable code coverage (ON or OFF. default:OFF)" OFF)
else()
set(TARGET_RPATH "")
set(ENABLE_COVERAGE FALSE)
endif()

if("${CMAKE_BUILD_TYPE}" STREQUAL "Debug")
set(ENABLE_DEBUG  TRUE)
set_property(DIRECTORY APPEND PROPERTY COMPILE_DEFINITIONS $<$<CONFIG:Debug>:DEBUGBUILD>)
if(ENABLE_COVERAGE)
set(STACK_PROTECTOR_OPT  "")
else()
set(STACK_PROTECTOR_OPT  $<IF:$<CXX_COMPILER_ID:MSVC>,/GS,-fstack-check -fstack-protector>)
endif()
else()
set(ENABLE_DEBUG  FALSE)
set(STACK_PROTECTOR_OPT  "")
endif() # CMAKE_BUILD_TYPE

####################
# common setting
####################
set(WORK_WINDOWS_BINARY_DIR_NAME  $<IF:$<CONFIG:Debug>,Debug,Release>)
if(NOT CFD_OBJ_BINARY_DIR)
set(CFD_OBJ_BINARY_DIR   ${CMAKE_BINARY_DIR}/${WORK_WINDOWS_BINARY_DIR_NAME})
set(CFD_ROOT_BINARY_DIR  ${CMAKE_BINARY_DIR})
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY  ${CFD_OBJ_BINARY_DIR})
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY  ${CFD_OBJ_BINARY_DIR}) # for linux
endif()

if(NOT CFD_SRC_ROOT_DIR)
set(CFD_SRC_ROOT_DIR   ${CMAKE_SOURCE_DIR})
endif()

if(NOT ENABLE_BITCOIN)
set(CFD_BITCOIN_USE   CFD_DISABLE_BITCOIN)
else()
set(CFD_BITCOIN_USE   "")
endif()

if(NOT ENABLE_ELEMENTS)
set(CFD_ELEMENTS_USE   CFD_DISABLE_ELEMENTS)
else()
set(CFD_ELEMENTS_USE   "")
endif()

if(NOT WIN32)
if(APPLE)
set(CMAKE_MACOSX_RPATH 1)
endif()
set(CMAKE_SKIP_BUILD_RPATH  FALSE)
set(CMAKE_BUILD_WITH_INSTALL_RPATH TRUE)
set(CMAKE_INSTALL_RPATH_USE_LINK_PATH TRUE)

if(TARGET_RPATH)
set(CMAKE_INSTALL_RPATH "${TARGET_RPATH};./;@rpath")
else()
set(CMAKE_INSTALL_RPATH "./;@rpath")
endif()
endif()

if(ENABLE_DEBUG AND ENABLE_COVERAGE)
set(COLLECT_COVERAGE ON)
set(PROFILE_ARCS_OPT -fprofile-arcs)
set(TEST_COVERAGE_OPT -ftest-coverage)
set(GCOV_LIBRARY gcov)
set(COVERAGE 1)
else()
set(PROFILE_ARCS_OPT  "")
set(TEST_COVERAGE_OPT "")
set(GCOV_LIBRARY  "")
endif()

####################
# cfd-csharp
####################

project(cfdcs CSharp)
include(CSharpUtilities)
# https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/compiler-options/langversion-compiler-option
set(CFDCS_CSHARP_VERSION  "7.0")

if(NOT CMAKE_GENERATOR_PLATFORM)
if(${CMAKE_EXE_LINKER_FLAGS} STREQUAL "/machine:x64")
set(CFDCS_PLATFORM_ARCH "x64")
else()
set(CFDCS_PLATFORM_ARCH "x86")
endif()
else()
if(${CMAKE_GENERATOR_PLATFORM} STREQUAL "x64")
set(CFDCS_PLATFORM_ARCH "x64")
else()
set(CFDCS_PLATFORM_ARCH "x86")
endif()
endif()
set(CMAKE_CSharp_FLAGS "/platform:${CFDCS_PLATFORM_ARCH} /langversion:${CFDCS_CSHARP_VERSION}")

set(CFD_LIBRARY cfd)
set(CFDCORE_LIBRARY cfdcore)
set(LIBWALLY_LIBRARY wally)
set(UNIVALUE_LIBRARY univalue)

# configure_file("Properties/AssemblyInfo.cs.template" "Properties/AssemblyInfo.cs")

add_library(${PROJECT_NAME} SHARED)

target_sources(${PROJECT_NAME}
  PRIVATE
    internal/CCommon.cs
    internal/ErrorHandle.cs
    internal/NativeMethods.cs
    Address.cs
    BlindFactor.cs
    ByteData.cs
    CfdCommon.cs
    UtxoData.cs
    CoinUtil.cs
    ConfidentialAddress.cs
    ConfidentialAsset.cs
    ConfidentialTransaction.cs
    ConfidentialValue.cs
    Descriptor.cs
    ExtPrivkey.cs
    ExtPubkey.cs
    HDWallet.cs
    OutPoint.cs
    Privkey.cs
    Pubkey.cs
    Script.cs
    ScriptUtil.cs
    ScriptWitness.cs
    SignParameter.cs
    StringUtil.cs
    Transaction.cs
    Txid.cs
)

target_compile_options(${PROJECT_NAME}
  PRIVATE
    /unsafe
)
#     -fstack-protector

target_link_libraries(${PROJECT_NAME}
  PRIVATE
    ${CFD_LIBRARY}
)

# Setup the .NET Framework version.
set_property(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.7.2")

# Setup the default assembly references.
set_property(TARGET ${PROJECT_NAME} PROPERTY VS_DOTNET_REFERENCES
	"System"
    "System.Core"
)

