using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  internal static class CCommon
  {
    /// <summary>
    /// Get string from cfd's pointer address, and free address buffer.
    /// </summary>
    /// <param name="address">pointer address</param>
    /// <returns>string</returns>
    public static string ConvertToString(IntPtr address)
    {
      var result = "";
      if (address != IntPtr.Zero)
      {
        result = Marshal.PtrToStringAnsi(address);
        NativeMethods.CfdFreeBuffer(address);
      }
      return result;
    }

    public static CfdErrorCode CfdInitialize()
    {
      return NativeMethods.CfdInitialize();
    }

    internal static CfdErrorCode CfdCreateSimpleHandle(
        [Out] out IntPtr handle)
    {
      return NativeMethods.CfdCreateSimpleHandle(out handle);
    }

    internal static CfdErrorCode CfdFreeHandle([In] IntPtr handle)
    {
      return NativeMethods.CfdFreeHandle(handle);
    }

    internal static CfdErrorCode CfdGetLastErrorMessage(
        [In] IntPtr handle,
        [Out] out IntPtr message)
    {
      return NativeMethods.CfdGetLastErrorMessage(handle, out message);
    }

    internal static CfdErrorCode CfdRequestExecuteJson(
        [In] IntPtr handle,
        [In] string requestName,
        [In] string requestJsonString,
        [Out] out IntPtr responseJsonString)
    {
      return NativeMethods.CfdRequestExecuteJson(handle, requestName,
          requestJsonString, out responseJsonString);
    }

    internal static CfdErrorCode CfdSerializeByteData(
        [In] IntPtr handle,
        [In] string buffer,
        [Out] out IntPtr output)
    {
      return NativeMethods.CfdSerializeByteData(handle, buffer, out output);
    }
  }
}
