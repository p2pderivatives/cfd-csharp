using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /// <summary>
  /// extend key type.
  /// </summary>
  public enum CfdExtKeyType
  {
    Privkey = 0,  //!< extended privkey
    Pubkey        //!< extended pubkey
  };

  /// <summary>
  /// public key data class.
  /// </summary>
  public class Pubkey
  {
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
      pubkey = StringUtil.FromBytes(bytes);
      // FIXME check format
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="pubkey_hex">pubkey hex string</param>
    public Pubkey(string pubkey_hex)
    {
      pubkey = pubkey_hex;
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

  /// <summary>
  /// private key data class.
  /// </summary>
  public class Privkey
  {
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
      privkey = StringUtil.FromBytes(bytes);
      Initialize(privkey, "", true);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="privkey_hex">privkey hex string</param>
    public Privkey(string privkey_hex)
    {
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
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CKey.CfdGetPubkeyFromPrivkey(
          handle.GetHandle(), privkey_hex, wif, is_compressed, out IntPtr out_pubkey);
        if (ret != CfdErrorCode.Success)
        {
          CUtil.ThrowError(handle, ret);
        }
        string pubkey_str = CUtil.ConvertToString(out_pubkey);
        pubkey = new Pubkey(pubkey_str);
      }
    }
  }

  internal class CKey
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetPubkeyFromPrivkey(
          [In] IntPtr handle,
          [In] string privkey,
          [In] string wif,
          [In] bool is_compressed,
          [Out] out IntPtr pubkey);

    /*
        CFDC_API int CfdCalculateEcSignature(
            void* handle, const char* sighash, const char* privkey, const char* wif,
            int network_type, bool has_grind_r, char** signature);

        CFDC_API int CfdEncodeSignatureByDer(
            void* handle, const char* signature, int sighash_type,
            bool sighash_anyone_can_pay, char** der_signature);

        CFDC_API int CfdNormalizeSignature(
            void* handle, const char* signature, char** normalized_signature);

        CFDC_API int CfdCreateKeyPair(
            void* handle, bool is_compressed, int network_type, char** pubkey,
            char** privkey, char** wif);

        CFDC_API int CfdGetPrivkeyFromWif(
            void* handle, const char* wif, int network_type, char** privkey);


        CFDC_API int CfdCreateExtkeyFromSeed(
            void* handle, const char* seed_hex, int network_type, int key_type,
            char** extkey);

        CFDC_API int CfdCreateExtkeyFromParentPath(
            void* handle, const char* extkey, const char* path, int network_type,
            int key_type, char** child_extkey);

        CFDC_API int CfdCreateExtPubkey(
            void* handle, const char* extkey, int network_type, char** ext_pubkey);

        CFDC_API int CfdGetPrivkeyFromExtkey(
            void* handle, const char* extkey, int network_type, char** privkey,
            char** wif);

        CFDC_API int CfdGetPubkeyFromExtkey(
            void* handle, const char* extkey, int network_type, char** pubkey);
    */
  }
}
