using System;

/// <summary>
/// cfd library namespace.
/// </summary>
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
    private CfdNetworkType networkType;
    private Pubkey pubkey;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public Privkey()
    {
      privkey = "";
      networkType = CfdNetworkType.Mainnet;
      privkeyWif = "";
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
      networkType = CfdNetworkType.Mainnet;
      privkeyWif = "";
      Initialize(privkey, "", true);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="privkeyHex">privkey hex string</param>
    public Privkey(string privkeyHex)
    {
      if (string.IsNullOrEmpty(privkeyHex))
      {
        privkeyHex = "";
      }
      else if ((privkeyHex == null) || (privkeyHex.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to privkey size.");
      }
      privkey = privkeyHex;
      networkType = CfdNetworkType.Mainnet;
      privkeyWif = "";
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
      networkType = CfdNetworkType.Mainnet;
      privkey = "";
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

    public SignParameter CalculateEcSignature(ByteData sighash, bool hasGrindR = true)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CKey.CfdCalculateEcSignature(
            handle.GetHandle(), sighash.ToHexString(),
            privkey, privkeyWif, (int)networkType, hasGrindR,
            out IntPtr signatureHex);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        SignParameter signature = new SignParameter(CCommon.ConvertToString(signatureHex));
        SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
        signature.SetDerEncode(sighashType);
        return signature;
      }
    }

    private void Initialize(string privkeyHex, string wif, bool isCompressed)
    {
      bool isCompressPubkey = isCompressed;
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        if (!String.IsNullOrEmpty(wif))
        {
          ret = CKey.CfdParsePrivkeyWif(
            handle.GetHandle(), wif,
            out IntPtr tempPrivkeyHex,
            out int tempNetworkType,
            out isCompressPubkey);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          privkey = CCommon.ConvertToString(tempPrivkeyHex);
          networkType = (CfdNetworkType)tempNetworkType;
        }

        ret = CKey.CfdGetPubkeyFromPrivkey(
          handle.GetHandle(), privkeyHex, wif, isCompressPubkey,
          out IntPtr pubkeyHex);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        pubkey = new Pubkey(CCommon.ConvertToString(pubkeyHex));
      }
    }
  }
}
