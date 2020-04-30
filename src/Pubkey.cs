using System;

namespace Cfd
{
  /// <summary>
  /// public key data class.
  /// </summary>
  public class Pubkey
  {
    public static readonly uint UncompressLength = 65;
    public static readonly uint CompressLength = 33;
    private readonly string pubkey;

    public static Pubkey Combine(Pubkey[] pubkeyList)
    {
      if (pubkeyList is null)
      {
        throw new ArgumentNullException(nameof(pubkeyList));
      }
      else if (pubkeyList.Length < 2)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError,
          "Failed to pubkey list length. minimum length is 2.");
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeCombinePubkey(
            handle.GetHandle(),
            out IntPtr combineHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          foreach (Pubkey pubkeyItem in pubkeyList)
          {
            ret = NativeMethods.CfdAddCombinePubkey(
              handle.GetHandle(), combineHandle, pubkeyItem.ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdFinalizeCombinePubkey(
            handle.GetHandle(), combineHandle, out IntPtr combinedKey);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          return new Pubkey(CCommon.ConvertToString(combinedKey));
        }
        finally
        {
          NativeMethods.CfdFreeCombinePubkeyHandle(handle.GetHandle(), combineHandle);
        }
      }
    }

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
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      else if (bytes.Length > UncompressLength)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to pubkey size.");
      }
      pubkey = StringUtil.FromBytes(bytes);
      // check format
      Validate(pubkey);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="pubkeyHex">pubkey hex string</param>
    public Pubkey(string pubkeyHex)
    {
      if (pubkeyHex is null)
      {
        throw new ArgumentNullException(nameof(pubkeyHex));
      }
      else if (pubkeyHex.Length > UncompressLength * 2)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to pubkey size.");
      }
      pubkey = pubkeyHex;
      // check format
      Validate(pubkey);
    }

    public bool IsCompressed()
    {
      return pubkey.Length == CompressLength * 2;
    }

    public Pubkey Compress()
    {
      if (IsCompressed())
      {
        return this;
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCompressPubkey(
            handle.GetHandle(), pubkey,
            out IntPtr compressedPubkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Pubkey(CCommon.ConvertToString(compressedPubkey));
      }
    }

    public Pubkey Uncompress()
    {
      if (!IsCompressed())
      {
        return this;
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdUncompressPubkey(
            handle.GetHandle(), pubkey,
            out IntPtr decompressedPubkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Pubkey(CCommon.ConvertToString(decompressedPubkey));
      }
    }

    public Pubkey TweakAdd(ByteData tweak)
    {
      if (tweak is null)
      {
        throw new ArgumentNullException(nameof(tweak));
      }
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        ret = NativeMethods.CfdPubkeyTweakAdd(
          handle.GetHandle(), pubkey, tweak.ToHexString(),
          out IntPtr tweakedKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Pubkey(CCommon.ConvertToString(tweakedKey));
      }
    }

    public Pubkey TweakMul(ByteData tweak)
    {
      if (tweak is null)
      {
        throw new ArgumentNullException(nameof(tweak));
      }
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        ret = NativeMethods.CfdPubkeyTweakMul(
          handle.GetHandle(), pubkey, tweak.ToHexString(),
          out IntPtr tweakedKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Pubkey(CCommon.ConvertToString(tweakedKey));
      }
    }

    public Pubkey Negate()
    {
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        ret = NativeMethods.CfdNegatePubkey(
          handle.GetHandle(), pubkey, out IntPtr negatedKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Pubkey(CCommon.ConvertToString(negatedKey));
      }
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

    public bool IsValid()
    {
      if ((pubkey.Length == CompressLength * 2) || (pubkey.Length == UncompressLength * 2))
      {
        return true;
      }
      return false;
    }

    private static void Validate(string pubkeyHex)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCompressPubkey(
            handle.GetHandle(), pubkeyHex,
            out IntPtr compressedPubkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(compressedPubkey);
      }
    }
  }
}
