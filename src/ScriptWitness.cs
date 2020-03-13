using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public class ScriptWitness
  {
    private readonly string[] hexArray;

    public ScriptWitness()
    {
      hexArray = new string[0];
    }

    public ScriptWitness(string[] hexArray)
    {
      if (hexArray == null)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to script size.");
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
