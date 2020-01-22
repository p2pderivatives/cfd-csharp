using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
      hexString = blindFactorHex;
    }

    /// <summary>
    /// blinder hex string.
    /// </summary>
    /// <returns>blinder hex string</returns>
    public string ToHexString()
    {
      return hexString;
    }
  }
}
