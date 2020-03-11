using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public class ConfidentialAsset
  {
    private readonly string commitment;

    public ConfidentialAsset()
    {
      commitment = "";
    }

    public ConfidentialAsset(string asset)
    {
      if ((asset == null) || ((asset.Length != 64) && (asset.Length != 66)))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to asset size.");
      }
      this.commitment = asset;
    }

    public ConfidentialAsset(byte[] asset)
    {
      if ((asset == null) || ((asset.Length != 32) && (asset.Length != 33)))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to asset size.");
      }
      if (asset.Length == 32)
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
      return commitment.Length == 66;
    }

    public string ToHexString()
    {
      return commitment;
    }

    public byte[] ToBytes()
    {
      if (commitment.Length == 64)
      {
        var assetBytes = StringUtil.ToBytes(commitment);
        return CfdCommon.ReverseBytes(assetBytes);
      }
      return StringUtil.ToBytes(commitment);
    }
  }
}
