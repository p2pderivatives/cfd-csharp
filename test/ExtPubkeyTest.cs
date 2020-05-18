using Xunit;

namespace Cfd.xTests
{
  public class ExtPubkeyTest
  {
    [Fact]
    public void ExtPubkeyMainnetTest()
    {
      ExtPubkey emptyKey = new ExtPubkey();
      Assert.False(emptyKey.IsValid());

      ExtPubkey key = new ExtPubkey("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy");

      Assert.Equal("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy",
        key.ToString());
      Assert.Equal("02f632717d78bf73e74aa8461e2e782532abae4eed5110241025afb59ebfd3d2fd",
        key.GetPubkey().ToHexString());
      Assert.Equal("a3fa8c983223306de0f0f65e74ebb1e98aba751633bf91d5fb56529aa5c132c1",
        key.GetChainCode().ToHexString());
      Assert.Equal("0488b21e", key.GetVersion().ToHexString());
      Assert.Equal("00000000",
        key.GetFingerprint().ToHexString());
      Assert.Equal((uint)0, key.GetChildNumber());
      Assert.Equal((uint)0, key.GetDepth());
      Assert.Equal(CfdNetworkType.Mainnet, key.GetNetworkType());
      Assert.True(key.IsValid());
    }

    [Fact]
    public void ExtPubkeyTestnetTest()
    {
      ExtPubkey key = new ExtPubkey("tpubDBwZbsX7C1m4tfHxHSFBvvuasqMxzMvSNM5yuAWz6kAfCATAgegvrtGdnxkqfr8wwRZi5d9fJHXqE8EFTSogTXd3xVx3GUFy9Xcg8dufREz");

      Assert.Equal("tpubDBwZbsX7C1m4tfHxHSFBvvuasqMxzMvSNM5yuAWz6kAfCATAgegvrtGdnxkqfr8wwRZi5d9fJHXqE8EFTSogTXd3xVx3GUFy9Xcg8dufREz",
        key.ToString());
      Assert.Equal("030061b08c4c80dc04aaa0b44018d2c4bcdb0d9c0992fb4fddf9d2fb096a5164c0",
        key.GetPubkey().ToHexString());
      Assert.Equal("bdc76da475a6fbdc4f3758939ab2096d4ab53b7d66c0eed66fc0f4be242835fc",
        key.GetChainCode().ToHexString());
      Assert.Equal("043587cf", key.GetVersion().ToHexString());
      Assert.Equal("f4a831a2", key.GetFingerprint().ToHexString());
      Assert.Equal((uint)0, key.GetChildNumber());
      Assert.Equal((uint)2, key.GetDepth());
      Assert.Equal(CfdNetworkType.Testnet, key.GetNetworkType());
      Assert.True(key.IsValid());
    }

    [Fact]
    public void ExtPubkeyParentTest()
    {
      ExtPubkey key = new ExtPubkey(CfdNetworkType.Testnet,
        new Pubkey("02ca30dbb25a2cf96344a04ae2144fb28a17f006c34cfb973b9f21623db27c5cd3"),
        new Pubkey("03f1e767c0555ce0105b2a76d0f8b19b6d33a147f82f75a05c4c09580c39694fd3"),
        new ByteData("839fb0d66f1887db167cdc530ab98e871d8b017ebcb198568874b6c98516364e"),
        4, 44);
      Assert.Equal("tpubDF7yNiHQHdfns9Mc3XM7PYcS2dqrPqcit3FLkebvHxS4atZxifANou2KTvpQQQP82ANDCkPc5MPQZ28pjYGgmDXGy1iyzaiX6MTBv8i4cua",
        key.ToString());
      Assert.True(key.IsValid());

      key = new ExtPubkey(CfdNetworkType.Testnet,
        new ByteData("a53a8ff3"),
        new Pubkey("03f1e767c0555ce0105b2a76d0f8b19b6d33a147f82f75a05c4c09580c39694fd3"),
        new ByteData("839fb0d66f1887db167cdc530ab98e871d8b017ebcb198568874b6c98516364e"),
        4, 44);
      Assert.Equal("tpubDF7yNiHQHdfns9Mc3XM7PYcS2dqrPqcit3FLkebvHxS4atZxifANou2KTvpQQQP82ANDCkPc5MPQZ28pjYGgmDXGy1iyzaiX6MTBv8i4cua",
        key.ToString());
      Assert.True(key.IsValid());
    }

    [Fact]
    public void DerivePubkeyTest()
    {
      ExtPubkey parent = new ExtPubkey("tpubDBwZbsX7C1m4tfHxHSFBvvuasqMxzMvSNM5yuAWz6kAfCATAgegvrtGdnxkqfr8wwRZi5d9fJHXqE8EFTSogTXd3xVx3GUFy9Xcg8dufREz");
      ExtPubkey key = parent.DerivePubkey("0/44");

      Assert.Equal("tpubDF7yNiHQHdfns9Mc3XM7PYcS2dqrPqcit3FLkebvHxS4atZxifANou2KTvpQQQP82ANDCkPc5MPQZ28pjYGgmDXGy1iyzaiX6MTBv8i4cua",
        key.ToString());
      Assert.Equal("03f1e767c0555ce0105b2a76d0f8b19b6d33a147f82f75a05c4c09580c39694fd3",
        key.GetPubkey().ToHexString());
      Assert.Equal("839fb0d66f1887db167cdc530ab98e871d8b017ebcb198568874b6c98516364e",
        key.GetChainCode().ToHexString());
      Assert.Equal("043587cf", key.GetVersion().ToHexString());
      Assert.Equal("a53a8ff3", key.GetFingerprint().ToHexString());
      Assert.Equal((uint)44, key.GetChildNumber());
      Assert.Equal((uint)4, key.GetDepth());
      Assert.Equal(CfdNetworkType.Testnet, key.GetNetworkType());
      Assert.True(key.IsValid());

      ExtPubkey key1 = parent.DerivePubkey(0).DerivePubkey(44);
      ExtPubkey key2 = parent.DerivePubkey(new uint[] { 0, 44 });
      Assert.Equal(key.ToString(), key1.ToString());
      Assert.Equal(key.ToString(), key2.ToString());
      Assert.True(key.Equals(key2));
    }
  }
}
