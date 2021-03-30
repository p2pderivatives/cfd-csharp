cmake_minimum_required(VERSION 3.13)

# configure.ac: AC_INIT([cfd-cs], [1.0]
set(PACKAGE_VERSION   1.0.0)

####################
# options
####################
include(./cmake/CfdCommonOption.cmake)
include(./cmake/CfdWallyOption.cmake)

option(CFD_SHARED "force shared build (ON or OFF. default:OFF)" OFF)
option(ENABLE_CAPI  "enable c-api (ON or OFF. default:ON)" ON)
option(ENABLE_JSONAPI  "enable json-api (ON or OFF. default:ON)" ON)

option(ENABLE_CSHARP  "enable csharp build for .NET Framework (ON or OFF. default:OFF)" OFF)

set(GENERATE_WALLY ON CACHE BOOL "" FORCE)
set(EXCLUDE_WALLYCORE_LIB ON CACHE BOOL "" FORCE)

if(NOT ${CMAKE_GENERATOR} MATCHES "Visual Studio ([^9]|[9][0-9])")
set(ENABLE_CSHARP OFF CACHE BOOL "" FORCE)
endif()

####################
# common setting
####################
include(./cmake/CfdCommonSetting.cmake)


####################
# subdirectories
####################
project(cfd_all_test CXX)
if(NOT CFD_SINGLE_BUILD)
add_subdirectory(external)
endif()

# if(${ENABLE_CSHARP})
# add_subdirectory(src)
# endif()


####################
# install & export
####################
set(prefix            ${CMAKE_INSTALL_PREFIX})
set(exec_prefix       ${prefix})
set(libdir            ${exec_prefix}/lib)
set(includedir        ${prefix}/include)

configure_file(pc/cfd-cs.pc.in  cfd-cs.pc  @ONLY)
install(FILES ${CMAKE_CURRENT_BINARY_DIR}/cfd-cs.pc
        DESTINATION ${CMAKE_INSTALL_LIBDIR}/pkgconfig)

####################
# uninstall
####################
if(NOT TARGET uninstall)
configure_file(
  ${CMAKE_CURRENT_SOURCE_DIR}/pc/cmake_uninstall.cmake.in
  ${CMAKE_CURRENT_BINARY_DIR}/cmake_uninstall.cmake
  IMMEDIATE @ONLY)

add_custom_target(uninstall
  COMMAND ${CMAKE_COMMAND} -P
  ${CMAKE_CURRENT_BINARY_DIR}/cmake_uninstall.cmake)
endif()
