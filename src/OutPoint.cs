using System;
using System.Runtime.InteropServices;

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

    public bool Equals(OutPoint obj)
    {
      if (Object.ReferenceEquals(obj, null))
      {
        return false;
      }
      if (Object.ReferenceEquals(this, obj))
      {
        return true;
      }

      return txid.Equals(obj.txid) && (vout == obj.vout);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as OutPoint);
    }

    public override int GetHashCode()
    {
      return txid.GetHashCode() + vout.GetHashCode();
    }

    public static bool operator ==(OutPoint lhs, OutPoint rhs)
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

    public static bool operator !=(OutPoint lhs, OutPoint rhs)
    {
      return !(lhs == rhs);
    }
  };
}
