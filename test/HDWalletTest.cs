using Xunit;

namespace Cfd.xTests
{
  public class HDWalletTest
  {
    [Fact]
    public void GetMnemonicWordlistTest()
    {
      string[] enWords = HDWallet.GetMnemonicWordlist();
      Assert.NotNull(enWords);
      Assert.Equal(2048, enWords.Length);
      Assert.Equal("ability", enWords[1]);

      string[] jpWords = HDWallet.GetMnemonicWordlist("jp");
      Assert.NotNull(jpWords);
      Assert.Equal(2048, jpWords.Length);
      Assert.Equal("あいさつ", jpWords[1]);
    }

    [Fact]
    public void ConvertMnemonicToSeedTest()
    {
      string mnemonicEn = "horn tenant knee talent sponsor spell gate clip pulse soap slush warm silver nephew swap uncle crack brave";
      HDWallet enWallet = HDWallet.ConvertMnemonicToSeed(mnemonicEn, "TREZOR", "en");
      Assert.Equal("fd579828af3da1d32544ce4db5c73d53fc8acc4ddb1e3b251a31179cdb71e853c56d2fcb11aed39898ce6c34b10b5382772db8796e52837b54468aeb312cfc3d",
        enWallet.GetSeed().ToHexString());

      // https://iancoleman.io/bip39/#japanese
      string mnemonicJp = "まぜる　むかし　じぶん　そえん　くつした　このよ　とおる　えもじ　おじさん　ねむたい　しいん　せすじ　のれん　ゆうめい　ときおり";
      HDWallet jpWallet = HDWallet.ConvertMnemonicToSeed(mnemonicJp, "", "jp");
      Assert.Equal("4d2cd52f3ad39dc913d5a90e91163d7af4c9f11d727b273be269a404d2a23546243f8adb5009200d037900c76a3a8fc69c13afaa8084c4c85d3515232785fd54",
        jpWallet.GetSeed().ToHexString());
    }

    [Fact]
    public void ConvertMnemonicToEntropyTest()
    {
      string mnemonicEn = "horn tenant knee talent sponsor spell gate clip pulse soap slush warm silver nephew swap uncle crack brave";
      ByteData entropy = HDWallet.ConvertMnemonicToEntropy(mnemonicEn, "en");
      Assert.Equal("6d9be1ee6ebd27a258115aad99b7317b9c8d28b6d76431c3", entropy.ToHexString());

      string[] mnemonicWords = HDWallet.ConvertEntropyToMnemonic(entropy, "en");
      string mnemonic = string.Join(' ', mnemonicWords);
      Assert.Equal(mnemonicEn, mnemonic);
    }

    [Fact]
    public void ConstructorTest()
    {
      ByteData entropy = new ByteData("6d9be1ee6ebd27a258115aad99b7317b9c8d28b6d76431c3");
      HDWallet hdWallet1 = new HDWallet(entropy, "TREZOR", "en");
      Assert.Equal("fd579828af3da1d32544ce4db5c73d53fc8acc4ddb1e3b251a31179cdb71e853c56d2fcb11aed39898ce6c34b10b5382772db8796e52837b54468aeb312cfc3d",
        hdWallet1.GetSeed().ToHexString());

      string mnemonicEn = "horn tenant knee talent sponsor spell gate clip pulse soap slush warm silver nephew swap uncle crack brave";
      HDWallet hdWallet2 = new HDWallet(mnemonicEn, "TREZOR");
      Assert.Equal(hdWallet1.GetSeed().ToHexString(),
        hdWallet2.GetSeed().ToHexString());
      Assert.True(hdWallet1.Equals(hdWallet2));

      string[] mnemonicWords = mnemonicEn.Split(' ');
      HDWallet hdWallet3 = new HDWallet(mnemonicWords, "TREZOR");
      Assert.Equal(hdWallet2.GetSeed().ToHexString(),
        hdWallet3.GetSeed().ToHexString());
    }

    [Fact]
    public void GeneratePrivkeyTest()
    {
      string[] mnemonicEn = { "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "about" };
      HDWallet hdWallet = new HDWallet(mnemonicEn, "TREZOR");

      ExtPrivkey privkey = hdWallet.GeneratePrivkey(CfdNetworkType.Mainnet);
      Assert.Equal("xprv9s21ZrQH143K3h3fDYiay8mocZ3afhfULfb5GX8kCBdno77K4HiA15Tg23wpbeF1pLfs1c5SPmYHrEpTuuRhxMwvKDwqdKiGJS9XFKzUsAF",
        privkey.ToString());

      privkey = hdWallet.GeneratePrivkey(CfdNetworkType.Testnet);
      Assert.Equal("tprv8ZgxMBicQKsPeWHBt7a68nPnvgTnuDhUgDWC8wZCgA8GahrQ3f3uWpq7wE7Uc1dLBnCe1hhCZ886K6ND37memRDWqsA9HgSKDXtwh2Qxo6J",
        privkey.ToString());

      ExtPrivkey child = hdWallet.GeneratePrivkey(CfdNetworkType.Mainnet, "0/44");
      Assert.Equal("xprv9wiYQ21HNxnQ8FxBjbYjJy5ckuEZ6CAFsKdHEnfkRcw5pZbXAFSturoZugNE6ZpVSu6kdrYw752chFPAbPMXZ62ZLfYwLMHdzMVXqwnfRFn",
        child.ToString());

      ExtPrivkey child11 = hdWallet.GeneratePrivkey(CfdNetworkType.Mainnet, 0);
      ExtPrivkey child12 = child11.DerivePrivkey(44);
      Assert.Equal(child.ToString(), child12.ToString());

      ExtPrivkey child2 = hdWallet.GeneratePrivkey(CfdNetworkType.Mainnet, new uint[] { 0, 44 });
      Assert.Equal(child.ToString(), child2.ToString());

      child = hdWallet.GeneratePrivkey(CfdNetworkType.Mainnet, "0'/44'");
      Assert.Equal("xprv9xcgxExFiq8qWLdxFHXpEZF8VH7Qr9YDZb8c7vMsqygWk2YGTBgSnDtm1LESskfAJqGMWkWWGagNCSbHdVgA8EFxSbfAQTKSD1z4iJ8qHtq",
        child.ToString());

      child11 = hdWallet.GeneratePrivkey(CfdNetworkType.Mainnet, 0x80000000);
      child12 = child11.DerivePrivkey(44 + 0x80000000);
      Assert.Equal(child.ToString(), child12.ToString());

      child2 = hdWallet.GeneratePrivkey(CfdNetworkType.Mainnet, new[] { 0x80000000, 44 + 0x80000000 });
      Assert.Equal(child.ToString(), child2.ToString());
    }

    [Fact]
    public void GeneratePubkeyTest()
    {
      string[] mnemonicEn = { "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "abandon", "about" };
      HDWallet hdWallet = new HDWallet(mnemonicEn, "TREZOR");

      ExtPubkey pubkey = hdWallet.GeneratePubkey(CfdNetworkType.Mainnet);
      Assert.Equal("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy",
        pubkey.ToString());

      pubkey = hdWallet.GeneratePubkey(CfdNetworkType.Testnet);
      Assert.Equal("tpubD6NzVbkrYhZ4XyJymmEgYC3uVhyj4YtPFX6yRTbW6RvfRC7Ag3sVhKSz7MNzFWW5MJ7aVBKXCAX7En296EYdpo43M4a4LaeaHuhhgHToSJF",
        pubkey.ToString());

      ExtPubkey child = hdWallet.GeneratePubkey(CfdNetworkType.Mainnet, "0/44");
      Assert.Equal("xpub6AhtoXYBDLLhLk2eqd5jg72MJw53Vet7EYYt3B5MyxU4hMvfhnm9Tf83kwN1aV5j6g9smszDdCg8dt4uguGHivB75PvNxPkdmecoAqqn7Hm",
        child.ToString());

      ExtPubkey child11 = hdWallet.GeneratePubkey(CfdNetworkType.Mainnet, 0);
      ExtPubkey child12 = child11.DerivePubkey(44);
      Assert.Equal(child.ToString(), child12.ToString());

      ExtPubkey child2 = hdWallet.GeneratePubkey(CfdNetworkType.Mainnet, new uint[] { 0, 44 });
      Assert.Equal(child.ToString(), child2.ToString());

      child = hdWallet.GeneratePubkey(CfdNetworkType.Mainnet, "0'/44'");
      Assert.Equal("xpub6Bc3MkV9ZCh8ipiRMK4pbhBs3JwuFcG4vp4CvJmVQKDVcpsQzizhL2DErc5DHMQuKwBxTg1jLP6PCqriLmLsJzjB2kD9TE9hvqxQ4yLKtcV",
        child.ToString());

      child2 = hdWallet.GeneratePubkey(CfdNetworkType.Mainnet, new[] { 0x80000000, 44 + 0x80000000 });
      Assert.Equal(child.ToString(), child2.ToString());
    }
  }
}
