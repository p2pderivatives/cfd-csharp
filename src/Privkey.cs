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
    private string privkey_wif;
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
    /// <param name="privkey_hex">privkey hex string</param>
    public Privkey(string privkey_hex)
    {
      if ((privkey_hex == null) || (privkey_hex.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to privkey size.");
      }
      privkey = privkey_hex;
      Initialize(privkey, "", true);
    }

    /// <summary>
    /// Constructor. (wif)
    /// </summary>
    /// <param name="wif">pubkey wif string</param>
    /// <param name="is_compressed">compressed flag</param>
    public Privkey(string wif, bool is_compressed)
    {
      privkey_wif = wif;
      Initialize("", wif, is_compressed);
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
      return privkey_wif;
    }

    /// <summary>
    /// get pubkey.
    /// </summary>
    /// <returns>pubkey</returns>
    public Pubkey GetPubkey()
    {
      return pubkey;
    }

    private void Initialize(string privkey_hex, string wif, bool is_compressed)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CKey.CfdGetPubkeyFromPrivkey(
          handle.GetHandle(), privkey_hex, wif, is_compressed, out IntPtr out_pubkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var pubkey_str = CCommon.ConvertToString(out_pubkey);
        pubkey = new Pubkey(pubkey_str);
      }
    }
  }
}
