// using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
// using System.Runtime.InteropServices;
using Cfd;
using Xunit;
using Xunit.Abstractions;

namespace Cfd.xTests
{
  public class CfdPrivkeyTest
  {
    private readonly ITestOutputHelper output;

    public CfdPrivkeyTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Fact]
    public void PrivkeyTest()
    {
      Privkey emptyKey = new Privkey("");
      Assert.False(emptyKey.IsValid());

      Privkey key = new Privkey("305e293b010d29bf3c888b617763a438fee9054c8cab66eb12ad078f819d9f27");
      Assert.Equal("305e293b010d29bf3c888b617763a438fee9054c8cab66eb12ad078f819d9f27", key.ToHexString());
      Assert.True(key.IsValid());

      Privkey wif = new Privkey("5JBb5A38fjjeBnngkvRmCsXN6EY4w8jWvckik3hDvYQMcddGY23");
      Assert.Equal("305e293b010d29bf3c888b617763a438fee9054c8cab66eb12ad078f819d9f27", wif.ToHexString());
      Assert.Equal("5JBb5A38fjjeBnngkvRmCsXN6EY4w8jWvckik3hDvYQMcddGY23", wif.GetWif());
      Assert.Equal(CfdNetworkType.Mainnet, wif.GetNetworkType());
      Assert.False(wif.IsCompressPubkey());
      Assert.True(wif.IsValid());
    }

    [Fact]
    public void GenerateTest()
    {
      Privkey key = Privkey.Generate();
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", key.ToHexString());
      Assert.True(key.IsValid());
    }

    [Fact]
    public void GetWifTest()
    {
      Privkey key = new Privkey("305e293b010d29bf3c888b617763a438fee9054c8cab66eb12ad078f819d9f27");
      string wif = key.GetWif(CfdNetworkType.Mainnet, false);
      output.WriteLine(wif);
      Assert.Equal("5JBb5A38fjjeBnngkvRmCsXN6EY4w8jWvckik3hDvYQMcddGY23", wif);

      wif = key.GetWif(CfdNetworkType.Mainnet, true);
      output.WriteLine(wif);
      Assert.Equal("KxqjPLtQqydD8d6eUrpJ7Q1266k8Mw8f5eoyEztY3Kc5z4f2RQTG", wif);

      wif = key.GetWif(CfdNetworkType.Testnet, false);
      output.WriteLine(wif);
      Assert.Equal("91xDetrgFxon9rHyPGKg5U5Kjttn6JGiGZcfpg3jGH9QPd4tmrm", wif);

      wif = key.GetWif(CfdNetworkType.Testnet, true);
      output.WriteLine(wif);
      Assert.Equal("cPCirFtGH3KUJ4ZusGdRUiW5iL3Y2PEM9gxSMRM3YSG6Eon9heJj", wif);
    }

    [Fact]
    public void GetPubkeyTest()
    {
      Privkey key = new Privkey("cQNmd1D8MqzijUuXHb2yS5oRSm2F3TSTTMvcHC3V7CiKxArpg1bg");
      output.WriteLine(key.GetPubkey(false).ToHexString());
      Assert.Equal("02e3cf2c4dca39b502a6f8ba37e5d63a9757492c2155bf99418d9532728cd23d93",
        key.GetPubkey(true).ToHexString());
      Assert.Equal("04e3cf2c4dca39b502a6f8ba37e5d63a9757492c2155bf99418d9532728cd23d935c5e615b970a67025cfce674d98f29b6fce161d99b74059a428c1164811ba7d4",
        key.GetPubkey(false).ToHexString());
    }

    [Fact]
    public void TweakTest()
    {
      Privkey key = new Privkey("036b13c5a0dd9935fe175b2b9ff86585c231e734b2148149d788a941f1f4f566");
      ByteData tweak = new ByteData("98430d10471cf697e2661e31ceb8720750b59a85374290e175799ba5dd06508e");
      Assert.Equal("9bae20d5e7fa8fcde07d795d6eb0d78d12e781b9e957122b4d0244e7cefb45f4",
        key.TweakAdd(tweak).ToHexString());
      Assert.Equal("aa71b12accba23b49761a7521e661f07a7e5742ac48cf708b8f9497b3a72a957",
        key.TweakMul(tweak).ToHexString());
    }

    [Fact]
    public void NegateTest()
    {
      Privkey key = new Privkey("036b13c5a0dd9935fe175b2b9ff86585c231e734b2148149d788a941f1f4f566");
      Privkey negate = key.Negate();
      output.WriteLine(negate.ToHexString());
      Assert.Equal("fc94ec3a5f2266ca01e8a4d460079a78f87cf5b1fd341ef1e849b54ade414bdb",
        negate.ToHexString());
      Assert.Equal(key.ToHexString(), negate.Negate().ToHexString());
    }

    [Fact]
    public void CalculateEcSignatureTest()
    {
      Privkey key = new Privkey("305e293b010d29bf3c888b617763a438fee9054c8cab66eb12ad078f819d9f27");
      ByteData sighash = new ByteData("2a67f03e63a6a422125878b40b82da593be8d4efaafe88ee528af6e5a9955c6e");

      SignParameter signData = key.CalculateEcSignature(sighash);
      Assert.Equal("0e68b55347fe37338beb3c28920267c5915a0c474d1dcafc65b087b9b3819cae6ae5e8fb12d669a63127abb4724070f8bd232a9efe3704e6544296a843a64f2c",
        signData.ToHexString());
      signData.SetSignatureHashType(new SignatureHashType(CfdSighashType.Single, false));
      Assert.True(signData.IsDerEncode());

      SignParameter signData2 = key.CalculateEcSignature(sighash, false);
      Assert.Equal("0e68b55347fe37338beb3c28920267c5915a0c474d1dcafc65b087b9b3819cae6ae5e8fb12d669a63127abb4724070f8bd232a9efe3704e6544296a843a64f2c",
        signData2.ToHexString());
      Assert.True(signData.IsDerEncode());
    }

  }
}
