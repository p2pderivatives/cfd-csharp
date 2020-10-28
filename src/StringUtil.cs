using System;
using System.Globalization;

namespace Cfd
{
  /// <summary>
  /// string utility class.
  /// </summary>
  public static class StringUtil
  {
    /// <summary>
    /// convert hex to bytes.
    /// </summary>
    /// <param name="hex">hex string</param>
    /// <returns>byte array</returns>
    public static byte[] ToBytes(string hex)
    {
      if (hex is null)
      {
        throw new ArgumentNullException(nameof(hex));
      }
      var buffer = new byte[hex.Length / 2];
      for (int i = 0; i < hex.Length / 2; ++i)
      {
        buffer[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
      }
      return buffer;
    }

    /// <summary>
    /// convert hex from bytes.
    /// </summary>
    /// <param name="bytes">byte array</param>
    /// <returns>hex string</returns>
    public static string FromBytes(byte[] bytes)
    {
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      return BitConverter.ToString(bytes).Replace(
        "-", "", StringComparison.Ordinal)
        .ToLower(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// convert hex from ByteData object.
    /// </summary>
    /// <param name="bytes">ByteData object</param>
    /// <returns>hex string</returns>
    public static string FromBytes(ByteData bytes)
    {
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      return FromBytes(bytes.ToBytes());
    }
  }
}
