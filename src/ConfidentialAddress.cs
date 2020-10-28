using System;

namespace Cfd
{
  /// <summary>
  /// confidential address class.
  /// </summary>
  public class ConfidentialAddress : IEquatable<ConfidentialAddress>
  {
    private readonly string confidentialAddress;
    private readonly Pubkey key;
    private readonly Address unconfidenialAddress;

    /// <summary>
    /// constructor by address string.
    /// </summary>
    /// <param name="addressString">address string.</param>
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

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="address">address</param>
    /// <param name="confidentialKey">confidential key</param>
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

    /// <summary>
    /// get confidential address string.
    /// </summary>
    /// <returns>confidential address string.</returns>
    public string ToAddressString()
    {
      return confidentialAddress;
    }

    /// <summary>
    /// get confidential key.
    /// </summary>
    /// <returns></returns>
    public Pubkey GetConfidentialKey()
    {
      return key;
    }

    /// <summary>
    /// get address object.
    /// </summary>
    /// <returns>address object</returns>
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
