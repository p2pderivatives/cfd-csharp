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
        CfdFreeBuffer(address);
      }
      return result;
    }

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdGetSupportedFunction([Out] ulong supportFlag);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    public static extern CfdErrorCode CfdInitialize();

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdFinalize([In] bool isFinishProcess);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // internal static extern CfdErrorCode CfdCreateHandle([Out] out IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdCreateSimpleHandle([Out] out IntPtr handle);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdCloneHandle([In] IntPtr source, [Out] IntPtr handle);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdCopyErrorState([In] IntPtr source, [In] IntPtr destination);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeHandle([In] IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    static extern CfdErrorCode CfdFreeBuffer([In] IntPtr address);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdGetLastErrorCode([In] IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetLastErrorMessage(
        [In] IntPtr handle,
        [Out] out IntPtr message);
  }
}
