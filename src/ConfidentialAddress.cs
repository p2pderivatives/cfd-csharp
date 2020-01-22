using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
public class ConfidentialAddress
{
  private readonly string confidential_address;
  private readonly Pubkey key;
  private readonly Address unconfidenial_address;

  public ConfidentialAddress(string address_str)
  {
    confidential_address = address_str;
    using (var handle = new ErrorHandle())
    {
      var ret = CElementsAddress.CfdParseConfidentialAddress(
          handle.GetHandle(), address_str,
          out IntPtr address,
          out IntPtr confidential_key, out int network_type);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      var unconfidential_address_str = CCommon.ConvertToString(address);
      var key_str = CCommon.ConvertToString(confidential_key);

      unconfidenial_address = new Address(unconfidential_address_str);
      key = new Pubkey(key_str);
    }
  }

  public ConfidentialAddress(Address address, Pubkey confidential_key)
  {
    unconfidenial_address = address;
    key = confidential_key;
    using (var handle = new ErrorHandle())
    {
      var ret = CElementsAddress.CfdCreateConfidentialAddress(
          handle.GetHandle(), address.ToAddressString(), confidential_key.ToHexString(),
          out IntPtr confidential_addr_ptr);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      confidential_address = CCommon.ConvertToString(confidential_addr_ptr);
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
}
