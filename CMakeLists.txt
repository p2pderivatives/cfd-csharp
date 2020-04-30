cmake_minimum_required(VERSION 3.13)

# configure.ac: AC_INIT([cfd-cs], [1.0]
set(PACKAGE_VERSION   1.0.0)

####################
# options
####################
# use "cmake -DCMAKE_BUILD_TYPE=Debug" or "cmake-js -D"
# option(ENABLE_DEBUG "enable debugging (ON or OFF. default:OFF)" OFF)
option(ENABLE_TESTS "enable code tests (ON or OFF. default:ON)" ON)
option(ENABLE_SHARED "enable shared library (ON or OFF. default:ON)" ON)
option(ENABLE_ELEMENTS "enable elements code (ON or OFF. default:ON)" ON)
option(ENABLE_CAPI  "enable c-api (ON or OFF. default:ON)" ON)
option(ENABLE_CSHARP  "enable csharp build for .NET Framework (ON or OFF. default:OFF)" OFF)

if(NOT WIN32)
#option(TARGET_RPATH "target rpath list (separator is ';') (default:)" "")
set(TARGET_RPATH "" CACHE STRING "target rpath list (separator is ';') (default:)")
option(ENABLE_COVERAGE "enable code coverage (ON or OFF. default:OFF)" OFF)
else()
set(TARGET_RPATH "")
set(ENABLE_COVERAGE FALSE)
endif()

set(ENABLE_SHARED ON CACHE BOOL "" FORCE)
set(ENABLE_ELEMENTS ON CACHE BOOL "" FORCE)
set(ENABLE_CAPI ON CACHE BOOL "" FORCE)

if(NOT ${CMAKE_GENERATOR} MATCHES "Visual Studio ([^9]|[9][0-9])")
set(ENABLE_CSHARP OFF CACHE BOOL "" FORCE)
endif()

####################
# common setting
####################
set(WORK_WINDOWS_BINARY_DIR_NAME  $<IF:$<CONFIG:Debug>,Debug,Release>)
if(NOT CFD_OBJ_BINARY_DIR)
set(CFD_OBJ_BINARY_DIR   ${CMAKE_BINARY_DIR}/${WORK_WINDOWS_BINARY_DIR_NAME})
set(CFD_ROOT_BINARY_DIR  ${CMAKE_BINARY_DIR})
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY  ${CFD_OBJ_BINARY_DIR})
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY  ${CFD_OBJ_BINARY_DIR}) # for linux
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY  ${CFD_OBJ_BINARY_DIR})
endif()


####################
# function
####################
function(transform_makefile_srclist INPUT_FILE OUTPUT_FILE)
  file(READ ${INPUT_FILE} MAKEFILE_INC_TEXT)

  string(REGEX REPLACE "\\\\\n" "@@@@LF@@@@" MAKEFILE_INC_TEXT ${MAKEFILE_INC_TEXT})
  string(REGEX REPLACE "([a-zA-Z_][a-zA-Z0-9_]*)[\t ]*=[\t ]*([^\n]*)" "SET(\\1 \\2)" MAKEFILE_INC_TEXT ${MAKEFILE_INC_TEXT})
  string(REPLACE "@@@@LF@@@@" "\n" MAKEFILE_INC_TEXT ${MAKEFILE_INC_TEXT})
  string(REGEX REPLACE "\\$\\(([a-zA-Z_][a-zA-Z0-9_]*)\\)" "\${\\1}" MAKEFILE_INC_TEXT ${MAKEFILE_INC_TEXT})
  string(REGEX REPLACE "@([a-zA-Z_][a-zA-Z0-9_]*)@" "\${\\1}" MAKEFILE_INC_TEXT ${MAKEFILE_INC_TEXT})
  file(WRITE ${OUTPUT_FILE} ${MAKEFILE_INC_TEXT})
endfunction()


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
