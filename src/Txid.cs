using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /// <summary>
  /// txid data class.
  /// </summary>
  public class Txid : IEquatable<Txid>
  {
    public const uint Size = 32;
    private readonly string txid;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public Txid()
    {
      txid = "0000000000000000000000000000000000000000000000000000000000000000";
    }

    public Txid(string txid)
    {
      if ((txid == null) || (txid.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to txid size.");
      }
      this.txid = txid;
    }

    public Txid(byte[] bytes)
    {
      if ((bytes == null) || (bytes.Length != Size))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to txid size.");
      }
      var txidBytes = CfdCommon.ReverseBytes(bytes);
      this.txid = StringUtil.FromBytes(txidBytes);
    }

    public string ToHexString()
    {
      return txid;
    }

    public byte[] GetBytes()
    {
      var txidBytes = StringUtil.ToBytes(txid);
      return CfdCommon.ReverseBytes(txidBytes);
    }

    public bool Equals(Txid obj)
    {
      if (Object.ReferenceEquals(obj, null))
      {
        return false;
      }
      if (Object.ReferenceEquals(this, obj))
      {
        return true;
      }

      return (txid == obj.txid);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as Txid);
    }

    public override int GetHashCode()
    {
      return txid.GetHashCode();
    }

    public static bool operator ==(Txid lhs, Txid rhs)
    {
      if (Object.ReferenceEquals(lhs, null))
      {
        if (Object.ReferenceEquals(rhs, null))
        {
          return true;
        }
        return false;
      }
      return lhs.Equals(rhs);
    }

    public static bool operator !=(Txid lhs, Txid rhs)
    {
      return !(lhs == rhs);
    }
  }
}
