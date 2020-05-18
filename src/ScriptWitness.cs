using System;

namespace Cfd
{
  public class ScriptWitness
  {
    private readonly string[] hexArray;

    public ScriptWitness()
    {
      hexArray = Array.Empty<string>();
    }

    public ScriptWitness(string[] hexArray)
    {
      if (hexArray is null)
      {
        throw new ArgumentNullException(nameof(hexArray));
      }
      this.hexArray = hexArray;
      for (uint index = 0; index < hexArray.Length; ++index)
      {
        StringUtil.ToBytes(hexArray[index]);
      }
    }

    public string ToHexString(uint index)
    {
      if (index >= hexArray.Length)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to index.");
      }
      return hexArray[index];
    }

    public byte[] GetBytes(uint index)
    {
      if (index >= hexArray.Length)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to index.");
      }
      return StringUtil.ToBytes(hexArray[index]);
    }

    public string[] GetHexStringArray()
    {
      return hexArray;
    }

    public uint GetCount()
    {
      return (uint)hexArray.Length;
    }
  }
}
