using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public class ConfidentialAddress
  {
    private readonly string confidentialAddress;
    private readonly Pubkey key;
    private readonly Address unconfidenialAddress;

    public ConfidentialAddress(string addressString)
    {
      confidentialAddress = addressString;
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdParseConfidentialAddress(
            handle.GetHandle(), addressString,
            out IntPtr address,
            out IntPtr confidentialKey, out int networkType);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var unconfidenialAddressStr = CCommon.ConvertToString(address);
        var keyStr = CCommon.ConvertToString(confidentialKey);

        unconfidenialAddress = new Address(unconfidenialAddressStr);
        key = new Pubkey(keyStr);
      }
    }

    public ConfidentialAddress(Address address, Pubkey confidentialKey)
    {
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      if (confidentialKey is null)
      {
        throw new ArgumentNullException(nameof(confidentialKey));
      }
      unconfidenialAddress = address;
      key = confidentialKey;
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateConfidentialAddress(
            handle.GetHandle(), address.ToAddressString(), confidentialKey.ToHexString(),
            out IntPtr outputConfidentialAddr);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        confidentialAddress = CCommon.ConvertToString(outputConfidentialAddr);
      }
    }

    public string ToAddressString()
    {
      return confidentialAddress;
    }

    public Pubkey GetConfidentialKey()
    {
      return key;
    }

    public Address GetAddress()
    {
      return unconfidenialAddress;
    }
  }
}
