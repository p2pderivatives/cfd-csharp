using System.Collections.Generic;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// blind factor data class.
  /// </summary>
  public class BlindFactor
  {
    public const uint Size = 32;
    private readonly string hexString;

    /// <summary>
    /// Constructor. (empty blinder)
    /// </summary>
    public BlindFactor()
    {
      hexString = "0000000000000000000000000000000000000000000000000000000000000000";
    }

    /// <summary>
    /// Constructor. (valid blind factor)
    /// </summary>
    /// <param name="blindFactorHex">blinder hex</param>
    public BlindFactor(string blindFactorHex)
    {
      if (string.IsNullOrEmpty(blindFactorHex))
      {
        hexString = "0000000000000000000000000000000000000000000000000000000000000000";
      }
      else if ((blindFactorHex == null) || (blindFactorHex.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to blindFactor size.");
      }
      hexString = blindFactorHex;
    }

    /// <summary>
    /// Constructor. (valid blind factor)
    /// </summary>
    /// <param name="blindFactorHex">blinder</param>
    public BlindFactor(byte[] bytes)
    {
      if ((bytes == null) || (bytes.Length != Size))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to blindFactor size.");
      }
      var blindFactorBytes = CfdCommon.ReverseBytes(bytes);
      this.hexString = StringUtil.FromBytes(blindFactorBytes);
    }

    /// <summary>
    /// blinder hex string.
    /// </summary>
    /// <returns>blinder hex string</returns>
    public string ToHexString()
    {
      return hexString;
    }

    /// <summary>
    /// blinder byte array.
    /// </summary>
    /// <returns>blinder byte array</returns>
    public byte[] GetBytes()
    {
      var blindFactorBytes = StringUtil.ToBytes(hexString);
      return CfdCommon.ReverseBytes(blindFactorBytes);
    }
  }
}
