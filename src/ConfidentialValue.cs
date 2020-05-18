using System;
using System.Globalization;

namespace Cfd
{
  public class ConfidentialValue
  {
    const uint CommitmentSize = 33;
    const uint UnblindSize = 9;
    private readonly string commitmentValue;
    private readonly long satoshiValue;

    public ConfidentialValue()
    {
      commitmentValue = "";
      satoshiValue = 0;
    }

    public ConfidentialValue(long satoshiValue)
    {
      commitmentValue = "";
      this.satoshiValue = satoshiValue;
    }

    public ConfidentialValue(string commitmentValue) : this(commitmentValue, 0)
    {
      // do nothing
    }

    public ConfidentialValue(byte[] commitmentValue)
    {
      if (commitmentValue is null)
      {
        throw new ArgumentNullException(nameof(commitmentValue));
      }
      if ((commitmentValue.Length != 0) &&
        (commitmentValue.Length != CommitmentSize) &&
        (commitmentValue.Length != UnblindSize))
      {
        throw new ArgumentException("Invalind commitmentValue size.");
      }
      this.commitmentValue = StringUtil.FromBytes(commitmentValue);
      satoshiValue = 0;
    }

    public ConfidentialValue(string commitmentValue, long satoshiValue)
    {
      if (commitmentValue is null)
      {
        throw new ArgumentNullException(nameof(commitmentValue));
      }
      if ((commitmentValue.Length != 0) &&
        (commitmentValue.Length != CommitmentSize * 2) &&
        (commitmentValue.Length != UnblindSize * 2))
      {
        throw new ArgumentException("Invalind commitmentValue size.");
      }
      this.commitmentValue = commitmentValue;
      this.satoshiValue = satoshiValue;
    }

    public bool IsEmpty()
    {
      return satoshiValue == 0 && commitmentValue.Length == 0;
    }

    public bool HasBlinding()
    {
      return commitmentValue.Length == (CommitmentSize * 2);
    }

    public long GetSatoshiValue()
    {
      return satoshiValue;
    }

    public override string ToString()
    {
      if (HasBlinding())
      {
        return commitmentValue;
      }
      else
      {
        return satoshiValue.ToString(CultureInfo.InvariantCulture) + " (" + commitmentValue + ")";
      }
    }
    public string ToHexString()
    {
      if (commitmentValue.Length == 0)
      {
        using (var handle = new ErrorHandle())
        {
          var ret = NativeMethods.CfdGetConfidentialValueHex(
             handle.GetHandle(), satoshiValue, false, out IntPtr valueHex);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          return CCommon.ConvertToString(valueHex);
        }
      }
      else
      {
        return commitmentValue;
      }
    }

    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(ToHexString());
    }
  }
}
