using System;

namespace Cfd
{
  /// <summary>
  /// Schnorr signature class.
  /// </summary>
  public class SchnorrSignature : IEquatable<SchnorrSignature>
  {
    public static readonly uint Size = 64;
    public static readonly uint AddedSigHashTypeSize = 65;
    private readonly string data;
    private readonly SchnorrPubkey nonce;
    private readonly Privkey key;
    private readonly SignatureHashType sighashType;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public SchnorrSignature()
    {
      data = "";
      nonce = new SchnorrPubkey();
      key = new Privkey();
      sighashType = new SignatureHashType(CfdSighashType.Default, false);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">byte array</param>
    public SchnorrSignature(byte[] bytes)
    {
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      if ((bytes.Length != Size) && (bytes.Length != AddedSigHashTypeSize))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to signature size.");
      }
      data = StringUtil.FromBytes(bytes);
      string[] list = Verify(data);
      nonce = new SchnorrPubkey(list[0]);
      key = new Privkey(list[1]);
      if (bytes.Length == AddedSigHashTypeSize)
      {
        sighashType = CollectSigHashType(data);
      }
      else
      {
        sighashType = new SignatureHashType(CfdSighashType.Default, false);
      }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hex">hex string</param>
    public SchnorrSignature(string hex)
    {
      if (hex is null)
      {
        throw new ArgumentNullException(nameof(hex));
      }
      if ((hex.Length != Size * 2) && (hex.Length != AddedSigHashTypeSize * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to signature size.");
      }
      data = hex;
      string[] list = Verify(data);
      nonce = new SchnorrPubkey(list[0]);
      key = new Privkey(list[1]);
      if (hex.Length == AddedSigHashTypeSize * 2)
      {
        sighashType = CollectSigHashType(data);
      }
      else
      {
        sighashType = new SignatureHashType(CfdSighashType.Default, false);
      }
    }

    static string[] Verify(string signature)
    {
      using (var handle = new ErrorHandle())
      {
        if (signature.Length == AddedSigHashTypeSize * 2)
        {
          signature = signature.Substring(0, (int)(Size * 2));
        }
        var ret = NativeMethods.CfdSplitSchnorrSignature(
            handle.GetHandle(), signature,
            out IntPtr schnorrNonce,
            out IntPtr privkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string nonceStr = CCommon.ConvertToString(schnorrNonce);
        string keyStr = CCommon.ConvertToString(privkey);
        return new string[] { nonceStr, keyStr };
      }
    }

    static SignatureHashType CollectSigHashType(string signature)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetSighashTypeFromSchnorrSignature(
            handle.GetHandle(), signature,
            out int sighashType,
            out bool _);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new SignatureHashType(sighashType);
      }
    }

    /// <summary>
    /// Get sign parameter object.
    /// </summary>
    /// <param name="signaturehashType">sighash type</param>
    /// <returns>sign parameter object.</returns>
    public SignParameter GetSignData(SignatureHashType signaturehashType)
    {
      var sig = data;
      var sigType = sighashType;
      if ((data.Length == Size * 2) && (signaturehashType.SighashType != CfdSighashType.Default))
      {
        using (var handle = new ErrorHandle())
        {
          var ret = NativeMethods.CfdAddSighashTypeInSchnorrSignature(
              handle.GetHandle(), data, signaturehashType.GetValue(),
              signaturehashType.IsSighashAnyoneCanPay,
              out IntPtr addedSignature);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          sig = CCommon.ConvertToString(addedSignature);
        }
        sigType = signaturehashType;
      }

      var signData = new SignParameter(sig);
      signData.SetSignatureHashType(sigType);
      return signData;
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

    /// <summary>
    /// get nonce data.
    /// </summary>
    /// <returns>nonce data</returns>
    public SchnorrPubkey GetNonce()
    {
      return nonce;
    }

    /// <summary>
    /// get key data.
    /// </summary>
    /// <returns>key data</returns>
    public Privkey GetKey()
    {
      return key;
    }

    public bool IsValid()
    {
      return data.Length == Size;
    }

    public bool Equals(SchnorrSignature other)
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
      if ((obj as SchnorrSignature) != null)
      {
        return Equals((SchnorrSignature)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return data.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(SchnorrSignature lhs, SchnorrSignature rhs)
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

    public static bool operator !=(SchnorrSignature lhs, SchnorrSignature rhs)
    {
      return !(lhs == rhs);
    }
  }
}
