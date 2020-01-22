using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  internal static class CElementsAddress
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdCreateConfidentialAddress(
        [In] IntPtr handle,
        [In] string address,
        [In] string confidentialKey,
        [Out] out IntPtr confidentialAddress);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdParseConfidentialAddress(
        [In] IntPtr handle,
        [In] string confidentialAddress,
        [Out] out IntPtr address,
        [Out] out IntPtr confidentialKey,
        [Out] out int networkType);
  }
}
