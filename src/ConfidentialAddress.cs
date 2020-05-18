using System;

namespace Cfd
{
  public class ConfidentialAddress : IEquatable<ConfidentialAddress>
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

    public bool Equals(ConfidentialAddress other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }
      return confidentialAddress.Equals(other.confidentialAddress, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as ConfidentialAddress) != null)
      {
        return Equals((ConfidentialAddress)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return confidentialAddress.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(ConfidentialAddress lhs, ConfidentialAddress rhs)
    {
      if (lhs is null)
      {
        if (rhs is null)
        {
          return true;
        }
        return false;
      }
      return lhs.Equals(rhs);
    }

    public static bool operator !=(ConfidentialAddress lhs, ConfidentialAddress rhs)
    {
      return !(lhs == rhs);
    }
  }
}
