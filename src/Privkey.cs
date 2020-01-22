using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /// <summary>
  /// private key data class.
  /// </summary>
  public class Privkey
  {
    public const uint Size = 32;
    private string privkey;
    private string privkeyWif;
    private Pubkey pubkey;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public Privkey()
    {
      privkey = "";
      pubkey = new Pubkey();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">privkey byte array</param>
    public Privkey(byte[] bytes)
    {
      if ((bytes == null) || (bytes.Length != Size))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to privkey size.");
      }
      privkey = StringUtil.FromBytes(bytes);
      Initialize(privkey, "", true);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="privkeyHex">privkey hex string</param>
    public Privkey(string privkeyHex)
    {
      if ((privkeyHex == null) || (privkeyHex.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to privkey size.");
      }
      privkey = privkeyHex;
      Initialize(privkey, "", true);
    }

    /// <summary>
    /// Constructor. (wif)
    /// </summary>
    /// <param name="wif">pubkey wif string</param>
    /// <param name="isCompressed">compressed flag</param>
    public Privkey(string wif, bool isCompressed)
    {
      privkeyWif = wif;
      Initialize("", wif, isCompressed);
    }

    /// <summary>
    /// get privkey hex string. (not wif)
    /// </summary>
    /// <returns>privkey hex string</returns>
    public string ToHexString()
    {
      return privkey;
    }

    /// <summary>
    /// get privkey byte array. (not wif)
    /// </summary>
    /// <returns>privkey byte array</returns>
    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(privkey);
    }

    /// <summary>
    /// get privkey wif string. (not hex)
    /// </summary>
    /// <returns>privkey wif string</returns>
    public string GetWif()
    {
      return privkeyWif;
    }

    /// <summary>
    /// get pubkey.
    /// </summary>
    /// <returns>pubkey</returns>
    public Pubkey GetPubkey()
    {
      return pubkey;
    }

    private void Initialize(string privkeyHex, string wif, bool isCompressed)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CKey.CfdGetPubkeyFromPrivkey(
          handle.GetHandle(), privkeyHex, wif, isCompressed, out IntPtr pubkeyHex);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        pubkey = new Pubkey(CCommon.ConvertToString(pubkeyHex));
      }
    }
  }
}
