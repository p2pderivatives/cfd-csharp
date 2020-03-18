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
    public const uint Size = 32;
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
      if ((txid == null) || (txid.Length != Size * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to txid size.");
      }
      this.txid = txid;
    }

    public UtxoData(byte[] bytes)
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

    public bool Equals(UtxoData obj)
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
      return this.Equals(obj as UtxoData);
    }

    public override int GetHashCode()
    {
      return txid.GetHashCode();
    }

    public static bool operator ==(UtxoData lhs, UtxoData rhs)
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

    public static bool operator !=(UtxoData lhs, UtxoData rhs)
    {
      return !(lhs == rhs);
    }
  }
}
