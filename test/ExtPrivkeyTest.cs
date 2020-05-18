using Xunit;

namespace Cfd.xTests
{
  public class ExtPrivkeyTest
  {
    [Fact]
    public void ExtPrivkeyMainnetTest()
    {
      ExtPrivkey emptyKey = new ExtPrivkey();
      Assert.False(emptyKey.IsValid());

      ExtPrivkey key = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");

      Assert.Equal("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV",
        key.ToString());
      Assert.Equal("73a2361673d25f998d1e9d94aabdeba8ac1ddd4628bc4f55341397d263bd560c",
        key.GetPrivkey().ToHexString());
      Assert.Equal("xpub6DsNDJWpxZBXsbWsCy1VeBY8xf6hZBgznDTXSnp3FregxWoWfGsvtQ9j5wBJNPebZXD5YmhpQBV7nVjhUsUgkG9R7yE31mh6sVh2w854a1o",
        key.GetExtPubkey().ToString());
      Assert.Equal("28009126a24557d32ff2c5da21850dd06529f34faed53b4a3552b5ed4bda35d5",
        key.GetChainCode().ToHexString());
      Assert.Equal("0488ade4", key.GetVersion().ToHexString());
      Assert.Equal("2da711a5",
        key.GetFingerprint().ToHexString());
      Assert.Equal((uint)0, key.GetChildNumber());
      Assert.Equal((uint)4, key.GetDepth());
      Assert.Equal(CfdNetworkType.Mainnet, key.GetNetworkType());
      Assert.True(key.IsValid());
    }

    [Fact]
    public void ExtPrivkeyTestnetTest()
    {
      ExtPrivkey key = new ExtPrivkey("tprv8ZgxMBicQKsPeWHBt7a68nPnvgTnuDhUgDWC8wZCgA8GahrQ3f3uWpq7wE7Uc1dLBnCe1hhCZ886K6ND37memRDWqsA9HgSKDXtwh2Qxo6J");

      Assert.Equal("tprv8ZgxMBicQKsPeWHBt7a68nPnvgTnuDhUgDWC8wZCgA8GahrQ3f3uWpq7wE7Uc1dLBnCe1hhCZ886K6ND37memRDWqsA9HgSKDXtwh2Qxo6J",
        key.ToString());
      Assert.Equal("cbedc75b0d6412c85c79bc13875112ef912fd1e756631b5a00330866f22ff184",
        key.GetPrivkey().ToHexString());
      Assert.Equal("a3fa8c983223306de0f0f65e74ebb1e98aba751633bf91d5fb56529aa5c132c1",
        key.GetChainCode().ToHexString());
      Assert.Equal("04358394", key.GetVersion().ToHexString());
      Assert.Equal("00000000", key.GetFingerprint().ToHexString());
      Assert.Equal((uint)0, key.GetChildNumber());
      Assert.Equal((uint)0, key.GetDepth());
      Assert.Equal(CfdNetworkType.Testnet, key.GetNetworkType());
      Assert.True(key.IsValid());
    }

    [Fact]
    public void DerivePrivkeyTest()
    {
      ExtPrivkey parent = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      ExtPrivkey key = parent.DerivePrivkey("0/44");

      Assert.Equal("xprvA5P4YtgFjzqM4QpXJZ8Zr7Wkhng7ugTybA3KWMAqDfAamqu5nqJ3zKRhB29cxuqCc8hPagZcN5BsuoXx4Xn7iYHnQvEdyMwZRFgoJXs8CDN",
        key.ToString());
      Assert.Equal((uint)44, key.GetChildNumber());
      Assert.Equal((uint)6, key.GetDepth());
      Assert.Equal(CfdNetworkType.Mainnet, key.GetNetworkType());
      Assert.True(key.IsValid());

      ExtPrivkey key1 = parent.DerivePrivkey(0).DerivePrivkey(44);
      ExtPrivkey key2 = parent.DerivePrivkey(new uint[] { 0, 44 });
      Assert.Equal(key.ToString(), key1.ToString());
      Assert.Equal(key.ToString(), key2.ToString());
      Assert.True(key.Equals(key2));
    }

    [Fact]
    public void DerivePubkeyTest()
    {
      ExtPrivkey parent = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      ExtPubkey key = parent.DerivePrivkey("0/44'").GetExtPubkey();
      uint childNumber = 0x8000002c;

      Assert.Equal("xpub6JNQxQDHv2vcUQiXjggbaGYZg3nmxX6ojMcJPSs4KfLSLnMBCg8VbJUh5n4to2SwLWXdSXnHBkUQx1fVnJ9oKYjPPYAQehjWRpx6ErQyykX",
        key.ToString());
      Assert.Equal(childNumber, key.GetChildNumber());
      Assert.Equal((uint)6, key.GetDepth());
      Assert.Equal(CfdNetworkType.Mainnet, key.GetNetworkType());
      Assert.True(key.IsValid());

      ExtPubkey key1 = parent.DerivePrivkey(0).DerivePrivkey(childNumber).GetExtPubkey();
      ExtPubkey key2 = parent.DerivePrivkey(new uint[] { 0, childNumber }).GetExtPubkey();
      Assert.Equal(key.ToString(), key1.ToString());
      Assert.Equal(key.ToString(), key2.ToString());
      Assert.True(key.Equals(key2));
    }
  }
}
