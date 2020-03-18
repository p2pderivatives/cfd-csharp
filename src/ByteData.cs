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
    private string data;

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
      if (bytes == null)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to bytes is null.");
      }
      data = StringUtil.FromBytes(bytes);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hex">hex string</param>
    public ByteData(string hex)
    {
      if (hex == null)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to hex is null.");
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
  }
}
