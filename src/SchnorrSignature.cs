using System;

namespace Cfd
{
  /// <summary>
  /// Schnorr signature class.
  /// </summary>
  public class SchnorrSignature : IEquatable<SchnorrSignature>
  {
    public static readonly uint Size = 64;
    private readonly string data;
    private readonly SchnorrPubkey nonce;
    private readonly Privkey key;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public SchnorrSignature()
    {
      data = "";
      nonce = new SchnorrPubkey();
      key = new Privkey();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">byte array</param>
    public SchnorrSignature(byte[] bytes)
    {
      if ((bytes == null) || (bytes.Length != Size))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to signature size.");
      }
      data = StringUtil.FromBytes(bytes);
      string[] list = Verify(data);
      nonce = new SchnorrPubkey(list[0]);
      key = new Privkey(list[1]);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hex">hex string</param>
    public SchnorrSignature(string hex)
    {
      if ((hex == null) || (hex.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to signature size.");
      }
      data = hex;
      string[] list = Verify(data);
      nonce = new SchnorrPubkey(list[0]);
      key = new Privkey(list[1]);
    }

    string[] Verify(string signature)
    {
      using (var handle = new ErrorHandle())
      {
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
