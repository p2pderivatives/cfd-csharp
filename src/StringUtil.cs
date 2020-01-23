using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public static class StringUtil
  {
    public static byte[] ToBytes(string hex)
    {
      var buffer = new byte[hex.Length / 2];
      for (int i = 0; i < hex.Length / 2; ++i)
      {
        buffer[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
      }
      return buffer;
    }

    public static string FromBytes(byte[] bytes)
    {
      return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
  }
}
