using System;
using System.Runtime.InteropServices;

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
      var clist = hex.ToCharArray();
      for (int i = 0; i < hex.Length / 2; ++i)
      {
        // buffer[i] = (byte)((Convert.ToByte(clist[i * 2]) << 4) | Convert.ToByte(clist[(i * 2) + 1]));
        buffer[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
      }
      return buffer;
    }

    public static string FromBytes(byte[] bytes)
    {
      return BitConverter.ToString(bytes).Replace("-", "");
    }
  }
}
