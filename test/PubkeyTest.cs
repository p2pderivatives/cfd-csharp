using Xunit;

namespace Cfd.xTests
{
  public class PubkeyTest
  {
    [Fact]
    public void ConstructorTest()
    {
      Pubkey pubkey = new Pubkey("031d7463018f867de51a27db866f869ceaf52abab71827a6051bab8a0fd020f4c1");
      Assert.True(pubkey.IsValid());

      byte[] byteData = pubkey.ToBytes();
      Pubkey pubkeyBytes = new Pubkey(byteData);
      Assert.Equal(pubkeyBytes.ToHexString(), pubkey.ToHexString());

      Pubkey emptyPubkey = new Pubkey();
      Assert.False(emptyPubkey.IsValid());
    }

    [Fact]
    public void CombineTest()
    {
      Pubkey[] list = new Pubkey[2];
      list[0] = new Pubkey("03662a01c232918c9deb3b330272483c3e4ec0c6b5da86df59252835afeb4ab5f9");
      list[1] = new Pubkey("0261e37f277f02a977b4f11eb5055abab4990bbf8dee701119d88df382fcc1fafe");
      Pubkey extPubkey = new Pubkey("022a66efd1ea9b1ad3acfcc62a5ce8c756fa6fc3917fce3d4952a8701244ed1049");

      Pubkey combinedPubkey = Pubkey.Combine(list);
      Assert.True(combinedPubkey.IsValid());
      Assert.Equal(combinedPubkey.ToHexString(), extPubkey.ToHexString());
    }

    [Fact]
    public void CompressTest()
    {
      Pubkey uncompressed = new Pubkey("076468efc14b8512007bb720d6e7d4217a6686095a79b57e50dd48355110422955400e1a8f159b5dcea116049d09eb756b80d52aeaabb195b343cf713f62f01a73");
      Pubkey extUncompressed = new Pubkey("046468efc14b8512007bb720d6e7d4217a6686095a79b57e50dd48355110422955400e1a8f159b5dcea116049d09eb756b80d52aeaabb195b343cf713f62f01a73");
      Pubkey extCompressed = new Pubkey("036468efc14b8512007bb720d6e7d4217a6686095a79b57e50dd48355110422955");
      Pubkey compressPubkey = uncompressed.Compress();
      Assert.True(compressPubkey.IsCompressed());
      Assert.False(uncompressed.IsCompressed());
      Assert.Equal(compressPubkey.ToHexString(), extCompressed.ToHexString());

      Pubkey uncompPubkey = compressPubkey.Uncompress();
      Assert.False(uncompPubkey.IsCompressed());
      Assert.Equal(uncompPubkey.ToHexString(), extUncompressed.ToHexString());
    }

    [Fact]
    public void TweakTest()
    {
      Pubkey pubkey = new Pubkey("03662a01c232918c9deb3b330272483c3e4ec0c6b5da86df59252835afeb4ab5f9");
      ByteData tweak = new ByteData("98430d10471cf697e2661e31ceb8720750b59a85374290e175799ba5dd06508e");

      Pubkey tweakedAdd = pubkey.TweakAdd(tweak);
      Assert.Equal("02b05cf99a2f556177a38f5108445472316e87eb4f5b243d79d7e5829d3d53babc", tweakedAdd.ToHexString());
      Pubkey tweakedMul = pubkey.TweakMul(tweak);
      Assert.Equal("0305d10e760a529d0523e98892d2deff59b91593a0d670bd82271cfa627c9e7e18", tweakedMul.ToHexString());

      Pubkey negate = pubkey.Negate();
      Assert.Equal("02662a01c232918c9deb3b330272483c3e4ec0c6b5da86df59252835afeb4ab5f9", negate.ToHexString());
      Assert.Equal(pubkey.ToHexString(), negate.Negate().ToHexString());
      Assert.True(pubkey.Equals(negate.Negate()));
    }
  }
}
