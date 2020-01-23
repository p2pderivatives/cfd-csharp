using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  internal static class CScript
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdParseScript(
        [In] IntPtr handle,
        [In] string script,
        [Out] out IntPtr scriptItemHandle,
        [Out] out uint scriptItemNum);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetScriptItem(
        [In] IntPtr handle,
        [In] IntPtr scriptItemHandle,
        [In] uint index,
        [Out] out IntPtr scriptItem);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeScriptItemHandle(
        [In] IntPtr handle, [In] IntPtr scriptItemHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdConvertScriptAsmToHex(
        [In] IntPtr handle,
        [In] string scriptAsm,
        [Out] out IntPtr scriptHex);
  }
}
