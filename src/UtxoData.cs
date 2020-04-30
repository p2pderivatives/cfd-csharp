using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// txid data class.
  /// </summary>
  public class UtxoData : IEquatable<UtxoData>
  {
    public static readonly uint Size = 32;
    private readonly string txid;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public UtxoData()
    {
      txid = "0000000000000000000000000000000000000000000000000000000000000000";
    }

    public UtxoData(string txid)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (txid.Length != Size * 2)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to txid size.");
      }
      this.txid = txid;
    }

    public UtxoData(byte[] bytes)
    {
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      if (bytes.Length != Size)
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

    public bool Equals(UtxoData other)
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
      if ((obj as UtxoData) != null)
      {
        return this.Equals((UtxoData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(txid);
    }

    public static bool operator ==(UtxoData lhs, UtxoData rhs)
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

    public static bool operator !=(UtxoData lhs, UtxoData rhs)
    {
      return !(lhs == rhs);
    }
  }
}
