using System;

namespace Cfd
{
  public class ConfidentialAsset : IEquatable<ConfidentialAsset>
  {
    const uint Size = 32;
    const uint CommitmentSize = 33;
    private readonly string commitment;

    public ConfidentialAsset()
    {
      commitment = "";
    }

    public ConfidentialAsset(string asset)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if ((asset.Length != Size * 2) && (asset.Length != CommitmentSize * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to asset size.");
      }
      commitment = asset;
    }

    public ConfidentialAsset(byte[] asset)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if ((asset.Length != Size) && (asset.Length != CommitmentSize))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to asset size.");
      }
      if (asset.Length == Size)
      {
        var assetBytes = CfdCommon.ReverseBytes(asset);
        commitment = StringUtil.FromBytes(assetBytes);
      }
      else
      {
        commitment = StringUtil.FromBytes(asset);
      }
    }

    public bool HasBlinding()
    {
      return commitment.Length == (CommitmentSize * 2);
    }

    public string ToHexString()
    {
      return commitment;
    }

    public byte[] ToBytes()
    {
      if (commitment.Length == (Size * 2))
      {
        var assetBytes = StringUtil.ToBytes(commitment);
        return CfdCommon.ReverseBytes(assetBytes);
      }
      return StringUtil.ToBytes(commitment);
    }

    public bool Equals(ConfidentialAsset other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }
      return commitment == other.commitment;
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as ConfidentialAsset) != null)
      {
        return Equals((ConfidentialAsset)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return commitment.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(ConfidentialAsset lhs, ConfidentialAsset rhs)
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

    public static bool operator !=(ConfidentialAsset lhs, ConfidentialAsset rhs)
    {
      return !(lhs == rhs);
    }
  }
}
