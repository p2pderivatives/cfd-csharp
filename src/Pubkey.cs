/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// public key data class.
  /// </summary>
  public class Pubkey
  {
    public const uint UncompressLength = 65;
    public const uint CompressLength = 33;
    private string pubkey;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public Pubkey()
    {
      pubkey = "";
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">pubkey byte array</param>
    public Pubkey(byte[] bytes)
    {
      if ((bytes == null) || (bytes.Length > UncompressLength))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to pubkey size.");
      }
      pubkey = StringUtil.FromBytes(bytes);
      // FIXME check format
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="pubkeyHex">pubkey hex string</param>
    public Pubkey(string pubkeyHex)
    {
      if ((pubkeyHex == null) || (pubkeyHex.Length > UncompressLength * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to pubkey size.");
      }
      pubkey = pubkeyHex;
      // FIXME check format
    }

    /// <summary>
    /// pubkey hex string.
    /// </summary>
    /// <returns>pubkey hex string</returns>
    public string ToHexString()
    {
      return pubkey;
    }

    /// <summary>
    /// pubkey byte array.
    /// </summary>
    /// <returns>pubkey byte array</returns>
    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(pubkey);
    }
  }
}
