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
    public void AddressTaprootTest()
    {
      var sk = new Privkey("305e293b010d29bf3c888b617763a438fee9054c8cab66eb12ad078f819d9f27");
      var spk = SchnorrPubkey.GetPubkeyFromPrivkey(sk, out bool _);
      Assert.Equal("1777701648fa4dd93c74edd9d58cfcc7bdc2fa30a2f6fa908b6fd70c92833cfb", spk.ToHexString());

      Address addr = new Address(spk, CfdAddressType.Taproot, CfdNetworkType.Testnet);
      Assert.Equal("tb1pzamhq9jglfxaj0r5ahvatr8uc77u973s5tm04yytdltsey5r8naskf8ee6", addr.ToAddressString());
      Assert.Equal("51201777701648fa4dd93c74edd9d58cfcc7bdc2fa30a2f6fa908b6fd70c92833cfb", addr.GetLockingScript().ToHexString());

      Address addr2 = Address.GetAddressByLockingScript(addr.GetLockingScript(), CfdNetworkType.Testnet);
      Assert.Equal(CfdAddressType.Taproot, addr2.GetAddressType());
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

    [Fact]
    public void PeginAddressTest()
    {
      Script fedpegScript = new Script("522102baae8e066e4f2a1da4b731017697bb8fcacc60e4569f3ec27bc31cf3fb13246221026bccd050e8ecf7a702bc9fb63205cfdf278a22ba8b1f1d3ca3d8e5b38465a9702103430d354b89d1fbe43eb54ea138a4aee1076e4c54f4c805f62f9cee965351a1d053ae");
      Pubkey pubkey = new Pubkey("027592aab5d43618dda13fba71e3993cd7517a712d3da49664c06ee1bd3d1f70af");
      var peginAddrData = Address.GetPeginAddress(fedpegScript, pubkey, CfdHashType.P2shP2wsh, CfdNetworkType.Mainnet);
      Assert.Equal("39cTKhjjh9YWDQT5hhSRkQwjvmpc4d1C7k", peginAddrData.Address.ToAddressString());
      Assert.Equal("0014925d4028880bd0c9d68fbc7fc7dfee976698629c", peginAddrData.ClaimScript.ToHexString());
      Assert.Equal("522103e3b215b75e015a5948efb043079d325a90e68b19112211ae3c1ff62366d441732102779396d5c2348c33bcbdcfd87bf59646ccbebc94bacf4750a9c5245dd297213021036416a1c936d3dc84747d5e544c200578cccfb6ec62dda48df79a0a6a8c7e63fa53ae",
        peginAddrData.TweakedFedpegScript.ToHexString());

      Script redeemScript = new Script("522103a7bd50beb3aff9238336285c0a790169eca90b7ad807abc4b64897ca1f6dedb621039cbaf938d050dd2582e4c2f56d1f75cfc9d165f2f3270532363d9871fb7be14252ae");
      var peginAddrData2 = Address.GetPeginAddress(fedpegScript, redeemScript, CfdHashType.P2shP2wsh, CfdNetworkType.Mainnet);
      Assert.Equal("3DZHAW3TmdwfGuJTGKatD7XpCNJvnX6GiE", peginAddrData2.Address.ToAddressString());
      Assert.Equal("0020c45384fa00fe363ed60968fff46541c89bc1766686c279ffdf0a335b80cad728", peginAddrData2.ClaimScript.ToHexString());
      Assert.Equal("52210272d86fcc18fc129a3fe72ed268356735a176f01ba1bb6b5a6e5181735570fca021021909156e0a206a5a8f47bee2418eebd6db0ecae9b4810d761117fa7891f86f7021026e90023fe74aff9f5a26c76ca88eb19fd4477ae43cebb9d2e81e197961b263b753ae",
        peginAddrData2.TweakedFedpegScript.ToHexString());
    }

    [Fact]
    public void PegoutAddressTest()
    {
      Descriptor desc1 = new Descriptor("wpkh(tpubDASgDECJvTMzUgS7GkSCxQAAWPveW7BeTPSvbi1wpUe1Mq1v743FRw1i7vTavjAb3D3Y8geCTYw2ezgiVS7SFXDXS6NpZmvr6XPjPvg632y)", CfdNetworkType.Regtest);
      var pegoutData1 = Address.GetPegoutAddress(
        desc1, 0, CfdAddressType.P2wpkh, CfdNetworkType.Regtest);
      Assert.Equal("bcrt1qa77w63m523kq82z4fn3d5f7qxqxfm4pmdthkdf", pegoutData1.Address.ToAddressString());
      Assert.Equal("wpkh(tpubDASgDECJvTMzUgS7GkSCxQAAWPveW7BeTPSvbi1wpUe1Mq1v743FRw1i7vTavjAb3D3Y8geCTYw2ezgiVS7SFXDXS6NpZmvr6XPjPvg632y)",
        pegoutData1.BaseDescriptor.ToString());

      ExtPubkey pubkey2 = new ExtPubkey("xpub67v4wfueMiZVkc7UbutFgPiptQw4kkNs89ooNMrwht8xEjnZZim1rNZHhEdrLejB99fiBdnWNNAB8hmUK7tCo5Ua6UtHzwVLj2Bzpch7vB2");
      var pegoutData2 = Address.GetPegoutAddress(
        pubkey2, 0, CfdAddressType.P2pkh, CfdNetworkType.Mainnet);
      Assert.Equal("1MMxsm4QG8NRHqaFZaUTFQQ9c9dEHUPWnD", pegoutData2.Address.ToAddressString());
      Assert.Equal("pkh(xpub67v4wfueMiZVkc7UbutFgPiptQw4kkNs89ooNMrwht8xEjnZZim1rNZHhEdrLejB99fiBdnWNNAB8hmUK7tCo5Ua6UtHzwVLj2Bzpch7vB2)",
        pegoutData2.BaseDescriptor.ToString());
    }
  }
}
