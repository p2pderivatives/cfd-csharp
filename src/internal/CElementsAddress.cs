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
        [In] string confidential_key,
        [Out] out IntPtr confidential_address);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdParseConfidentialAddress(
        [In] IntPtr handle,
        [In] string confidential_address,
        [Out] out IntPtr address,
        [Out] out IntPtr confidential_key,
        [Out] out int network_type);
  }
}
