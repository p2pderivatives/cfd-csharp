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
    public static readonly uint Size = 32;
    private readonly string privkey;
    private readonly string privkeyWif;
    private readonly bool isCompressed;
    private readonly CfdNetworkType networkType;

    public static Privkey Generate()
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateKeyPair(
            handle.GetHandle(), true, (int)CfdNetworkType.Mainnet,
            out IntPtr pubkey, out IntPtr privkey, out IntPtr wif);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(pubkey);
        CCommon.ConvertToString(wif);
        return new Privkey(CCommon.ConvertToString(privkey));
      }
    }

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public Privkey()
    {
      privkey = "";
      networkType = CfdNetworkType.Mainnet;
      privkeyWif = "";
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
      isCompressed = true;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="privkey">privkey(wif or hex)</param>
    public Privkey(string privkey)
    {
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      if (string.IsNullOrEmpty(privkey))
      {
        this.privkey = "";
        privkeyWif = "";
        isCompressed = true;
      }
      else if (privkey.Length == Size * 2)
      {
        // hex
        this.privkey = privkey;
        networkType = CfdNetworkType.Mainnet;
        privkeyWif = "";
        isCompressed = true;
      }
      else
      {
        // wif
        privkeyWif = privkey;
        using (var handle = new ErrorHandle())
        {
          CfdErrorCode ret;
          ret = NativeMethods.CfdParsePrivkeyWif(
            handle.GetHandle(), privkeyWif,
            out IntPtr tempPrivkeyHex,
            out int tempNetworkType,
            out isCompressed);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          this.privkey = CCommon.ConvertToString(tempPrivkeyHex);
          networkType = (CfdNetworkType)tempNetworkType;
        }
      }
    }

    public Privkey TweakAdd(ByteData tweak)
    {
      if (tweak is null)
      {
        throw new ArgumentNullException(nameof(tweak));
      }
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        ret = NativeMethods.CfdPrivkeyTweakAdd(
          handle.GetHandle(), privkey, tweak.ToHexString(),
          out IntPtr tweakedKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Privkey(CCommon.ConvertToString(tweakedKey));
      }
    }

    public Privkey TweakMul(ByteData tweak)
    {
      if (tweak is null)
      {
        throw new ArgumentNullException(nameof(tweak));
      }
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        ret = NativeMethods.CfdPrivkeyTweakMul(
          handle.GetHandle(), privkey, tweak.ToHexString(),
          out IntPtr tweakedKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Privkey(CCommon.ConvertToString(tweakedKey));
      }
    }

    public Privkey Negate()
    {
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        ret = NativeMethods.CfdNegatePrivkey(
          handle.GetHandle(), privkey,
          out IntPtr negatedKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Privkey(CCommon.ConvertToString(negatedKey));
      }
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
      if (privkeyWif.Length == 0)
      {
        return GetWif(networkType, isCompressed);
      }
      else
      {
        return privkeyWif;
      }
    }

    /// <summary>
    /// get privkey wif string. (not hex)
    /// </summary>
    /// <returns>privkey wif string</returns>
    public string GetWif(CfdNetworkType networkType, bool isCompressedPubkey)
    {
      if ((privkeyWif.Length != 0) && (networkType == this.networkType) &&
        (isCompressedPubkey == isCompressed))
      {
        return privkeyWif;
      }
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        ret = NativeMethods.CfdGetPrivkeyWif(
          handle.GetHandle(), privkey, (int)networkType, isCompressedPubkey,
          out IntPtr wif);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return CCommon.ConvertToString(wif);
      }
    }

    /// <summary>
    /// get pubkey.
    /// </summary>
    /// <returns>pubkey</returns>
    public Pubkey GetPubkey()
    {
      if (privkeyWif.Length == 0)
      {
        return GetPubkey(true);
      }
      else
      {
        using (var handle = new ErrorHandle())
        {
          var ret = NativeMethods.CfdGetPubkeyFromPrivkey(
          handle.GetHandle(), privkey, privkeyWif, isCompressed,
          out IntPtr pubkeyHex);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          return new Pubkey(CCommon.ConvertToString(pubkeyHex));
        }
      }
    }

    public Pubkey GetPubkey(bool isCompress)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetPubkeyFromPrivkey(
        handle.GetHandle(), privkey, "", isCompress,
        out IntPtr pubkeyHex);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Pubkey(CCommon.ConvertToString(pubkeyHex));
      }
    }

    /// <summary>
    /// Calculate ec-signature.
    /// </summary>
    /// <param name="sighash">signature hash.</param>
    /// <returns></returns>
    public SignParameter CalculateEcSignature(ByteData sighash)
    {
      return CalculateEcSignature(sighash, true);
    }

    /// <summary>
    /// Calculate ec-signature.
    /// </summary>
    /// <param name="sighash">signature hash.</param>
    /// <param name="hasGrindR">use grind-R.</param>
    /// <returns></returns>
    public SignParameter CalculateEcSignature(ByteData sighash, bool hasGrindR)
    {
      if (sighash is null)
      {
        throw new ArgumentNullException(nameof(sighash));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCalculateEcSignature(
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

    public bool IsValid()
    {
      return (privkey.Length == Size * 2);
    }

    public CfdNetworkType GetNetworkType()
    {
      return networkType;
    }

    public bool IsCompressPubkey()
    {
      return isCompressed;
    }
  }
}
