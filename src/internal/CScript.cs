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
        [Out] out IntPtr script_item_handle,
        [Out] out uint script_item_num);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetScriptItem(
        [In] IntPtr handle,
        [In] IntPtr script_item_handle,
        [In] uint index,
        [Out] out IntPtr script_item);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeScriptItemHandle(
        [In] IntPtr handle, [In] IntPtr script_item_handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdConvertScriptAsmToHex(
        [In] IntPtr handle,
        [In] string script_asm,
        [Out] out IntPtr script_hex);
  }
}
