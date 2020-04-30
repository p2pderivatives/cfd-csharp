using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// txid data class.
  /// </summary>
  public class Txid : IEquatable<Txid>
  {
    public static readonly uint Size = 32;
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

    public bool Equals(Txid other)
    {
      if (other is null)
      {
        return false;
      }
      if (Object.ReferenceEquals(this, other))
      {
        return true;
      }

      return (txid == other.txid);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as Txid) != null)
      {
        return this.Equals((Txid)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(txid);
    }

    public static bool operator ==(Txid lhs, Txid rhs)
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

    public static bool operator !=(Txid lhs, Txid rhs)
    {
      return !(lhs == rhs);
    }
  }
}
