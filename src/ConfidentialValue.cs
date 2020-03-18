using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public class ConfidentialValue
  {
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
      return satoshiValue == 0 && commitmentValue.Length == 66;
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
        return satoshiValue.ToString() + " (" + commitmentValue + ")";
      }
    }

    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(commitmentValue);
    }
  }
}
