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
    private readonly string hex_data;

    /// <summary>
    /// Constructor. (empty blinder)
    /// </summary>
    public BlindFactor()
    {
      hex_data = "0000000000000000000000000000000000000000000000000000000000000000";
    }

    /// <summary>
    /// Constructor. (valid blind factor)
    /// </summary>
    /// <param name="blind_factor_hex">blinder hex</param>
    public BlindFactor(string blind_factor_hex)
    {
      hex_data = blind_factor_hex;
    }

    /// <summary>
    /// blinder hex string.
    /// </summary>
    /// <returns>blinder hex string</returns>
    public string ToHexString()
    {
      return hex_data;
    }
  }
}
