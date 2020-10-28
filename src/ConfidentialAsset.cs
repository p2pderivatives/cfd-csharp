using System;

namespace Cfd
{
  /// <summary>
  /// elements asset class.
  /// </summary>
  public class ConfidentialAsset : IEquatable<ConfidentialAsset>
  {
    const uint Size = 32;
    const uint CommitmentSize = 33;
    private readonly string commitment;

    /// <summary>
    /// empty constructor.
    /// </summary>
    public ConfidentialAsset()
    {
      commitment = "";
    }

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="asset">asset string.</param>
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

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="asset">asset bytes.</param>
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

    /// <summary>
    /// has blinding.
    /// </summary>
    /// <returns>true: blinded, false: unblinded.</returns>
    public bool HasBlinding()
    {
      return commitment.Length == (CommitmentSize * 2);
    }

    /// <summary>
    /// get hex string.
    /// </summary>
    /// <returns>hex string. blinded is 66 chars, else is 64 chars.</returns>
    public string ToHexString()
    {
      return commitment;
    }

    /// <summary>
    /// get asset bytes.
    /// </summary>
    /// <returns>asset bytes. blinded is 33 bytes, else is 32 bytes.</returns>
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
