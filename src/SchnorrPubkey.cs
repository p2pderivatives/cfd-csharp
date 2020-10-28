using System;

namespace Cfd
{
  /// <summary>
  /// Schnorr public key class.
  /// </summary>
  public class SchnorrPubkey : IEquatable<SchnorrPubkey>
  {
    public static readonly uint Size = 32;
    private readonly string data;

    /// <summary>
    /// Get Schnorr public key from private key.
    /// </summary>
    /// <param name="privkey">private key</param>
    /// <param name="parity">parity flag</param>
    /// <returns>schnorr pubkey</returns>
    public static SchnorrPubkey GetPubkeyFromPrivkey(Privkey privkey, out bool parity)
    {
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetSchnorrPubkeyFromPrivkey(
            handle.GetHandle(), privkey.ToHexString(),
            out IntPtr pubkey, out parity);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string data = CCommon.ConvertToString(pubkey);
        return new SchnorrPubkey(data);
      }
    }

    /// <summary>
    /// Get Schnorr public key from public key.
    /// </summary>
    /// <param name="pubkey">public key</param>
    /// <param name="parity">parity flag</param>
    /// <returns>schnorr pubkey</returns>
    public static SchnorrPubkey GetPubkeyFromPubkey(Pubkey pubkey, out bool parity)
    {
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetSchnorrPubkeyFromPubkey(
            handle.GetHandle(), pubkey.ToHexString(),
            out IntPtr schnorrPubkey, out parity);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string data = CCommon.ConvertToString(schnorrPubkey);
        return new SchnorrPubkey(data);
      }
    }

    /// <summary>
    /// Get tweaked Schnorr public key and private key.
    /// </summary>
    /// <param name="privkey">private key</param>
    /// <param name="tweak">tweak</param>
    /// <param name="tweakedPubkey">tweaked public key</param>
    /// <param name="tweakedParity">tweaked parity flag</param>
    /// <param name="tweakedPrivkey">tweaked private key</param>
    public static void GetTweakAddKeyPair(Privkey privkey, ByteData tweak,
      out SchnorrPubkey tweakedPubkey, out bool tweakedParity, out Privkey tweakedPrivkey)
    {
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      if (tweak is null)
      {
        throw new ArgumentNullException(nameof(tweak));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdSchnorrKeyPairTweakAdd(
            handle.GetHandle(), privkey.ToHexString(), tweak.ToHexString(),
            out IntPtr tempPubkey, out tweakedParity, out IntPtr tempPrivkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var pk = CCommon.ConvertToString(tempPubkey);
        var sk = CCommon.ConvertToString(tempPrivkey);
        tweakedPubkey = new SchnorrPubkey(pk);
        tweakedPrivkey = new Privkey(sk);
      }
    }

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public SchnorrPubkey()
    {
      data = "";
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">byte array</param>
    public SchnorrPubkey(byte[] bytes)
    {
      if ((bytes == null) || (bytes.Length != Size))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to pubkey size.");
      }
      data = StringUtil.FromBytes(bytes);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hex">hex string</param>
    public SchnorrPubkey(string hex)
    {
      if ((hex == null) || (hex.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to pubkey size.");
      }
      data = hex;
    }

    /// <summary>
    /// Get tweaked Schnorr public key.
    /// </summary>
    /// <param name="tweak">tweak</param>
    /// <param name="parity">parity flag</param>
    /// <returns>schnorr pubkey</returns>
    public SchnorrPubkey TweakAdd(ByteData tweak, out bool parity)
    {
      if (tweak is null)
      {
        throw new ArgumentNullException(nameof(tweak));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdSchnorrPubkeyTweakAdd(
            handle.GetHandle(), data, tweak.ToHexString(),
            out IntPtr tempPubkey, out parity);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var pk = CCommon.ConvertToString(tempPubkey);
        return new SchnorrPubkey(pk);
      }
    }

    /// <summary>
    /// Check tweaked Schnorr public key from base public key.
    /// </summary>
    /// <param name="parity">parity flag</param>
    /// <param name="basePubkey">base public key</param>
    /// <param name="tweak">tweak</param>
    /// <returns>true or false</returns>
    public bool IsTweaked(bool parity, SchnorrPubkey basePubkey, ByteData tweak)
    {
      if (basePubkey is null)
      {
        throw new ArgumentNullException(nameof(basePubkey));
      }
      if (tweak is null)
      {
        throw new ArgumentNullException(nameof(tweak));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCheckTweakAddFromSchnorrPubkey(
            handle.GetHandle(), data, parity, basePubkey.ToHexString(),
            tweak.ToHexString());
        if (ret == CfdErrorCode.Success)
        {
          return true;
        }
        else if (ret != CfdErrorCode.SignVerificationError)
        {
          handle.ThrowError(ret);
        }
      }
      return false;
    }

    /// <summary>
    /// hex string.
    /// </summary>
    /// <returns>hex string</returns>
    public string ToHexString()
    {
      return data;
    }

    /// <summary>
    /// byte array.
    /// </summary>
    /// <returns>byte array</returns>
    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(data);
    }

    public bool IsValid()
    {
      return data.Length == Size;
    }

    public bool Equals(SchnorrPubkey other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }
      return data.Equals(other.data, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as SchnorrPubkey) != null)
      {
        return Equals((SchnorrPubkey)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return data.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(SchnorrPubkey lhs, SchnorrPubkey rhs)
    {
      if (lhs is null)
      {
        if (rhs is null)
        {
          return true;
        }
        return false;
      }
      return lhs.Equals(rhs);
    }

    public static bool operator !=(SchnorrPubkey lhs, SchnorrPubkey rhs)
    {
      return !(lhs == rhs);
    }
  }
}
