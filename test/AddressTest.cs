using Xunit;

namespace Cfd.xTests
{
  public class AddressTest
  {
    [Fact]
    public void ConstructorTest()
    {
      string addrStr = "bcrt1q576jgpgewxwu205cpjq4s4j5tprxlq38l7kd85";
      Address addr = new Address(addrStr);
      Assert.Equal(addrStr, addr.ToAddressString());
      Assert.Equal("0014a7b5240519719dc53e980c8158565458466f8227", addr.GetLockingScript().ToHexString());
      Assert.Equal("a7b5240519719dc53e980c8158565458466f8227", addr.GetHash());

      Pubkey pubkey = new Pubkey("031d7463018f867de51a27db866f869ceaf52abab71827a6051bab8a0fd020f4c1");
      addr = new Address(pubkey, CfdAddressType.P2wpkh, CfdNetworkType.ElementsRegtest);
      Assert.Equal("ert1q7jm5vw5cunpy3lkvwdl3sr3qfm794xd4zr6z3k", addr.ToAddressString());
      Assert.Equal("0014f4b7463a98e4c248fecc737f180e204efc5a99b5", addr.GetLockingScript().ToHexString());
      Assert.True(addr.Equals(new Address("ert1q7jm5vw5cunpy3lkvwdl3sr3qfm794xd4zr6z3k")));
    }

    [Fact]
    public void AddressByLockingScriptTest()
    {
      string addrStr = "bcrt1q576jgpgewxwu205cpjq4s4j5tprxlq38l7kd85";
      Address addr = new Address(addrStr);
      Assert.Equal(addrStr, addr.ToAddressString());
      Assert.Equal("0014a7b5240519719dc53e980c8158565458466f8227", addr.GetLockingScript().ToHexString());
      Address addr2 = Address.GetAddressByLockingScript(addr.GetLockingScript(), CfdNetworkType.Regtest);
      Assert.Equal(addrStr, addr2.ToAddressString());
      Assert.True(addr.Equals(addr2));
      Assert.Equal("a7b5240519719dc53e980c8158565458466f8227", addr.GetHash());
      Assert.Equal("a7b5240519719dc53e980c8158565458466f8227", addr2.GetHash());
      Assert.Equal(addr2.GetHash(), addr2.GetHash());
    }

    [Fact]
    public void AddressP2shSegwitTest()
    {
      Pubkey pubkey = new Pubkey("031d7463018f867de51a27db866f869ceaf52abab71827a6051bab8a0fd020f4c1");
      Address addr = new Address(pubkey, CfdAddressType.P2shP2wpkh, CfdNetworkType.ElementsRegtest);
      Assert.Equal("XBsZoa2ueqj8TJA52KzNrHGtjzAeqTf6DS", addr.ToAddressString());
      Assert.Equal("a91405bc4d5d12925f008cef06ba387ade16a49d7a3187", addr.GetLockingScript().ToHexString());
      Assert.Equal("0014f4b7463a98e4c248fecc737f180e204efc5a99b5", addr.GetP2shLockingScript());

      Address addr2 = Address.GetAddressByLockingScript(addr.GetLockingScript(), CfdNetworkType.ElementsRegtest);
      Assert.Equal(CfdAddressType.P2sh, addr2.GetAddressType());
      addr2.SetAddressType(CfdAddressType.P2shP2wpkh);
      Assert.Equal(CfdAddressType.P2shP2wpkh, addr2.GetAddressType());
    }

    [Fact]
    public void ConfidentialAddressTest()
    {
      string ctAddr = "VTpvKKc1SNmLG4H8CnR1fGJdHdyWGEQEvdP9gfeneJR7n81S5kiwNtgF7vrZjC8mp63HvwxM81nEbTxU";
      string address = "Q7wegLt2qMGhm28vch6VTzvpzs8KXvs4X7";
      string ctKey = "025476c2e83188368da1ff3e292e7acafcdb3566bb0ad253f62fc70f07aeee6357";
      ConfidentialAddress caddr = new ConfidentialAddress(ctAddr);
      Assert.Equal(ctAddr, caddr.ToAddressString());
      Assert.Equal(address, caddr.GetAddress().ToAddressString());
      Assert.Equal("76a914751e76e8199196d454941c45d1b3a323f1433bd688ac", caddr.GetAddress().GetLockingScript().ToHexString());
      Assert.Equal(ctKey, caddr.GetConfidentialKey().ToHexString());
      Assert.True(caddr.Equals(new ConfidentialAddress(ctAddr)));

      Address addr = new Address(address);
      Pubkey pubkey = new Pubkey(ctKey);
      caddr = new ConfidentialAddress(addr, pubkey);
      Assert.Equal(ctAddr, caddr.ToAddressString());
      Assert.Equal(address, caddr.GetAddress().ToAddressString());
      Assert.Equal("76a914751e76e8199196d454941c45d1b3a323f1433bd688ac", caddr.GetAddress().GetLockingScript().ToHexString());
      Assert.Equal(ctKey, caddr.GetConfidentialKey().ToHexString());
    }
  }
}
