using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// OutPoint data class.
  /// </summary>
  public class OutPoint : IEquatable<OutPoint>
  {
    private readonly Txid txid;
    private readonly uint vout;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public OutPoint()
    {
    }

    public OutPoint(string txid, uint vout)
    {
      this.txid = new Txid(txid);
      this.vout = vout;
    }

    public OutPoint(byte[] txid, uint vout)
    {
      this.txid = new Txid(txid);
      this.vout = vout;
    }

    public OutPoint(Txid txid, uint vout)
    {
      this.txid = txid;
      this.vout = vout;
    }

    public Txid GetTxid()
    {
      return txid;
    }

    public uint GetVout()
    {
      return vout;
    }

    public bool Equals(OutPoint other)
    {
      if (other is null)
      {
        return false;
      }
      if (Object.ReferenceEquals(this, other))
      {
        return true;
      }

      return txid.Equals(other.txid) && (vout == other.vout);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as OutPoint) != null)
      {
        return this.Equals((OutPoint)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return txid.GetHashCode() + vout.GetHashCode();
    }

    public static bool operator ==(OutPoint lhs, OutPoint rhs)
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

    public static bool operator !=(OutPoint lhs, OutPoint rhs)
    {
      return !(lhs == rhs);
    }
  };
}
