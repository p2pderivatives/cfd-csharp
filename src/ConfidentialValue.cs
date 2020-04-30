using System;
using System.Globalization;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public class ConfidentialValue
  {
    const UInt32 CommitmentSize = 33;
    private readonly string commitmentValue;
    private readonly long satoshiValue;

    public ConfidentialValue()
    {
      commitmentValue = "";
      satoshiValue = 0;
    }

    public ConfidentialValue(long satoshiValue)
    {
      this.commitmentValue = "";
      this.satoshiValue = satoshiValue;
    }

    public ConfidentialValue(string commitmentValue)
    {
      this.commitmentValue = commitmentValue;
      this.satoshiValue = 0;
    }

    public ConfidentialValue(string commitmentValue, long satoshiValue)
    {
      this.commitmentValue = commitmentValue;
      this.satoshiValue = satoshiValue;
    }

    public bool IsEmpty()
    {
      return satoshiValue == 0 && commitmentValue.Length == 0;
    }

    public bool HasBlinding()
    {
      return satoshiValue == 0 && commitmentValue.Length == (CommitmentSize * 2);
    }

    public long GetSatoshiValue()
    {
      return satoshiValue;
    }

    public string ToHexString()
    {
      return commitmentValue;
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

    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(commitmentValue);
    }
  }
}
