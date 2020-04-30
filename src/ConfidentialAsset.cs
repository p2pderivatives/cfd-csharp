using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public class ConfidentialAsset
  {
    const UInt32 Size = 32;
    const UInt32 CommitmentSize = 33;
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
      this.commitment = asset;
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
        this.commitment = StringUtil.FromBytes(assetBytes);
      }
      else
      {
        this.commitment = StringUtil.FromBytes(asset);
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
  }
}
