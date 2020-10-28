using System;

namespace Cfd
{
  /// <summary>
  /// script witness stack class.
  /// </summary>
  public class ScriptWitness
  {
    private readonly string[] hexArray;

    /// <summary>
    /// constructor.
    /// </summary>
    public ScriptWitness()
    {
      hexArray = Array.Empty<string>();
    }

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="hexArray">hex string array.</param>
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

    /// <summary>
    /// get hex string from array.
    /// </summary>
    /// <param name="index">array index</param>
    /// <returns>hex string</returns>
    public string ToHexString(uint index)
    {
      if (index >= hexArray.Length)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to index.");
      }
      return hexArray[index];
    }

    /// <summary>
    /// get bytes from array.
    /// </summary>
    /// <param name="index">array index</param>
    /// <returns>byte array</returns>
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

    /// <summary>
    /// get array length.
    /// </summary>
    /// <returns>array length.</returns>
    public uint GetCount()
    {
      return (uint)hexArray.Length;
    }
  }
}
