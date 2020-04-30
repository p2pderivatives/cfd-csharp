using System;
/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// byte data class.
  /// </summary>
  public class ByteData
  {
    private readonly string data;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public ByteData()
    {
      data = "";
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">byte array</param>
    public ByteData(byte[] bytes)
    {
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      data = StringUtil.FromBytes(bytes);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hex">hex string</param>
    public ByteData(string hex)
    {
      if (hex is null)
      {
        throw new ArgumentNullException(nameof(hex));
      }
      data = hex;
    }

    /// <summary>
    /// hex string.
    /// </summary>
    /// <returns>hex string</returns>
    public string ToHexString()
    {
      return data;
    }

    /// <summary>
    /// byte array.
    /// </summary>
    /// <returns>byte array</returns>
    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(data);
    }

    public uint GetSize()
    {
      return (uint)data.Length / 2;
    }

    public uint GetLength()
    {
      return GetSize();
    }

    public bool IsEmpty()
    {
      return data.Length == 0;
    }
  }
}
