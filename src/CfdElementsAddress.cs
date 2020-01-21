using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
public class ConfidentialAddress
{
  private string confidential_address;
  private Pubkey key;
  private Address unconfidenial_address;

  public ConfidentialAddress(string address_str)
  {
    confidential_address = address_str;
    using (ErrorHandle handle = new ErrorHandle())
    {
      CfdErrorCode ret = CElementsAddress.CfdParseConfidentialAddress(
          handle.GetHandle(), address_str,
          out IntPtr address,
          out IntPtr confidential_key, out int network_type);
      if (ret != CfdErrorCode.Success)
      {
        CUtil.ThrowError(handle, ret);
      }
      string unconfidential_address_str = CUtil.ConvertToString(address);
      string key_str = CUtil.ConvertToString(confidential_key);

      unconfidenial_address = new Address(unconfidential_address_str);
      key = new Pubkey(key_str);
    }
  }

  public ConfidentialAddress(Address address, Pubkey confidential_key)
  {
    unconfidenial_address = address;
    key = confidential_key;
    using (ErrorHandle handle = new ErrorHandle())
    {
      CfdErrorCode ret = CElementsAddress.CfdCreateConfidentialAddress(
          handle.GetHandle(), address.ToAddressString(), confidential_key.ToHexString(),
          out IntPtr confidential_addr_ptr);
      if (ret != CfdErrorCode.Success)
      {
        CUtil.ThrowError(handle, ret);
      }
      confidential_address = CUtil.ConvertToString(confidential_addr_ptr);
    }
  }

  public string ToAddressString()
  {
    return confidential_address;
  }

  public Pubkey GetConfidentialKey()
  {
    return key;
  }

  public Address GetAddress()
  {
    return unconfidenial_address;
  }
}
*/

  internal class CElementsAddress
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
