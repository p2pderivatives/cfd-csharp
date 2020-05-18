using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Cfd.xTests
{
  public class ConfidentialTransactionTest
  {
    private readonly ITestOutputHelper output;

    public ConfidentialTransactionTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Fact]
    public void ConfidentialTxTest()
    {
      string txHex = "0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000";
      ConfidentialTransaction tx = new ConfidentialTransaction(txHex);

      Assert.Equal(txHex, tx.ToHexString());
      Assert.Equal(txHex, StringUtil.FromBytes(tx.GetBytes()));
      Assert.Equal("cf7783b2b1de646e35186df988a219a17f0317b5c3f3c47fa4ab2d7463ea3992", tx.GetTxid().ToHexString());
      Assert.Equal("cf7783b2b1de646e35186df988a219a17f0317b5c3f3c47fa4ab2d7463ea3992", tx.GetWtxid().ToHexString());
      Assert.Equal("938e3a9b5bac410e812d08db74c4ef2bc58d1ed99d94b637cab0ac2e9eb59df8", tx.GetWitHash());
      Assert.Equal<uint>(2, tx.GetVersion());
      Assert.Equal<uint>(0, tx.GetLockTime());
      Assert.Equal<uint>(512, tx.GetSize());
      Assert.Equal<uint>(512, tx.GetVsize());
      Assert.Equal<uint>(2048, tx.GetWeight());
    }

    [Fact]
    public void CreateRawTransactionTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      ExtPubkey key = privkey.GetExtPubkey();
      Address setAddr1 = new Address(key.DerivePubkey(11).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address setAddr2 = new Address(key.DerivePubkey(12).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address setAddr3 = new Address(key.DerivePubkey(13).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Pubkey confidentialKey3 = key.DerivePubkey(103).GetPubkey();
      ConfidentialAddress ctAddr3 = new ConfidentialAddress(setAddr3, confidentialKey3);

      ConfidentialTransaction tx = new ConfidentialTransaction(2, 0);
      tx.AddTxInList(new[] {
        new ConfidentialTxIn(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0)),
        new ConfidentialTxIn(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0)),
      });
      tx.AddTxOutList(new[] {
        new ConfidentialTxOut(new ConfidentialAsset(assetA), 1000),
        new ConfidentialTxOut(new ConfidentialAsset(assetA), new ConfidentialValue(10000000), setAddr1.GetLockingScript()),
        new ConfidentialTxOut(new ConfidentialAsset(assetB), new ConfidentialValue(5000000), setAddr2.GetLockingScript()),
      });
      tx.AddTxOut(new ConfidentialAsset(assetB), new ConfidentialValue(3000000), ctAddr3);
      tx.UpdateFee(2000, new ConfidentialAsset(assetA));

      output.WriteLine("tx: " + tx.ToHexString());
      Assert.Equal("0200000000020a503dbd4f8f2b064c70e048b21f93fe4584174478abf5f44747932cd21da87c0000000000ffffffff0a9a33750a810cd384ca5d93b09513f1eb5d93c669091b29eef710d2391ff7300000000000ffffffff040100000000000000000000000000000000000000000000000000000000000000aa0100000000000007d000000100000000000000000000000000000000000000000000000000000000000000aa010000000000989680001600142c9337c6875705dc8ead1d42b961ffeb2e9ac1790100000000000000000000000000000000000000000000000000000000000000bb0100000000004c4b4000160014265cb61e5c017c02270d7fe385ba08836616c1180100000000000000000000000000000000000000000000000000000000000000bb0100000000002dc6c000160014964d1721318c8bc7b016d0eb9374a620e8b88a9800000000",
        tx.ToHexString());

      // AddScriptSign
      Pubkey pubkey1 = key.DerivePubkey(1).GetPubkey();
      Privkey privkey1 = privkey.DerivePrivkey(1).GetPrivkey();
      string desc = string.Format("wsh(pkh({0}))", pubkey1.ToHexString());
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Liquidv1);
      OutPoint outpoint1 = new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0);
      ConfidentialValue value1 = new ConfidentialValue(10000000);
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint1, descriptor.GetHashType(), descriptor.GetRedeemScript(), value1,
        sighashType);
      SignParameter signParam = privkey1.CalculateEcSignature(sighash);

      tx.AddScriptSign(outpoint1,
        descriptor.GetHashType(), new[] {
          signParam,
        }, descriptor.GetRedeemScript());

      output.WriteLine("tx: " + tx.ToHexString());
      Assert.Equal("0200000001020a503dbd4f8f2b064c70e048b21f93fe4584174478abf5f44747932cd21da87c0000000000ffffffff0a9a33750a810cd384ca5d93b09513f1eb5d93c669091b29eef710d2391ff7300000000000ffffffff040100000000000000000000000000000000000000000000000000000000000000aa0100000000000007d000000100000000000000000000000000000000000000000000000000000000000000aa010000000000989680001600142c9337c6875705dc8ead1d42b961ffeb2e9ac1790100000000000000000000000000000000000000000000000000000000000000bb0100000000004c4b4000160014265cb61e5c017c02270d7fe385ba08836616c1180100000000000000000000000000000000000000000000000000000000000000bb0100000000002dc6c000160014964d1721318c8bc7b016d0eb9374a620e8b88a98000000000000024730440220205c9d0314ddd2a60e63e525e2452d360d9a485325a76ce359ab015bfbffa63102201da43d23a1cb397252d354cd07f0954ed5406b1cef74b5f2dc06c20897c8046a011976a9148b756cbd98f4f55e985f80437a619d47f0732a9488ac00000000000000000000000000",
        tx.ToHexString());

      OutPoint outpoint2 = new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0);
      tx.AddSignWithPrivkeySimple(outpoint2.GetTxid(), outpoint2.GetVout(), CfdHashType.P2wpkh, pubkey1, privkey1, value1, sighashType);

      uint outIndex1 = tx.GetTxOutFeeIndex();
      Assert.Equal((uint)0, outIndex1);
      uint outIndex2 = tx.GetTxOutIndex(setAddr1);
      Assert.Equal((uint)1, outIndex2);
      uint outIndex3 = tx.GetTxOutIndex(setAddr2.GetLockingScript());
      Assert.Equal((uint)2, outIndex3);
      uint outIndex4 = tx.GetTxOutIndex(ctAddr3);
      Assert.Equal((uint)3, outIndex4);

      ConfidentialTxIn txin1 = tx.GetTxIn(0);
      Assert.Equal(descriptor.GetRedeemScript().ToHexString(), txin1.WitnessStack.ToHexString(1));
      ConfidentialTxIn txin2 = tx.GetTxIn(outpoint2);
      Assert.Equal(pubkey1.ToHexString(), txin2.WitnessStack.ToHexString(1));
      Assert.Equal(pubkey1.ToHexString(), txin2.WitnessStack.GetHexStringArray()[1]);
      Assert.Equal(StringUtil.FromBytes(pubkey1.ToBytes()), StringUtil.FromBytes(txin2.WitnessStack.GetBytes(1)));
      ConfidentialTxOut txout2 = tx.GetTxOut(1);
      Assert.Equal(setAddr1.GetLockingScript().ToHexString(), txout2.ScriptPubkey.ToHexString());
    }

    [Fact]
    public void CreateRawTransactionTest2()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      ExtPubkey key = privkey.GetExtPubkey();
      Address setAddr1 = new Address(key.DerivePubkey(11).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address setAddr2 = new Address(key.DerivePubkey(12).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address setAddr3 = new Address(key.DerivePubkey(13).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Pubkey confidentialKey3 = key.DerivePubkey(103).GetPubkey();
      ConfidentialAddress ctAddr3 = new ConfidentialAddress(setAddr3, confidentialKey3);

      ConfidentialTransaction tx = new ConfidentialTransaction(2, 0);
      tx.AddTxIn(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0));
      tx.AddTxIn(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0));
      tx.AddTxIn(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0));
      tx.AddFeeTxOut(assetA, 1000);
      tx.AddTxOut(new ConfidentialAsset(assetA), new ConfidentialValue(10000000), setAddr1);
      tx.AddTxOut(new ConfidentialAsset(assetB), new ConfidentialValue(5000000), setAddr2);
      tx.AddTxOut(new ConfidentialAsset(assetB), new ConfidentialValue(3000000), ctAddr3);
      tx.AddDestroyAmountTxOut(assetB, 10000);
      tx.UpdateFee(2000, new ConfidentialAsset(assetA));

      output.WriteLine("tx: " + tx.ToHexString());
      Assert.Equal("0200000000030a503dbd4f8f2b064c70e048b21f93fe4584174478abf5f44747932cd21da87c0000000000ffffffff0a9a33750a810cd384ca5d93b09513f1eb5d93c669091b29eef710d2391ff7300000000000ffffffff0a9a33750a810cd384ca5d93b09513f1eb5d93c669091b29eef710d2391ff7300000000000ffffffff050100000000000000000000000000000000000000000000000000000000000000aa0100000000000007d000000100000000000000000000000000000000000000000000000000000000000000aa010000000000989680001600142c9337c6875705dc8ead1d42b961ffeb2e9ac1790100000000000000000000000000000000000000000000000000000000000000bb0100000000004c4b4000160014265cb61e5c017c02270d7fe385ba08836616c1180100000000000000000000000000000000000000000000000000000000000000bb0100000000002dc6c000160014964d1721318c8bc7b016d0eb9374a620e8b88a980100000000000000000000000000000000000000000000000000000000000000bb01000000000000271000016a00000000",
        tx.ToHexString());
    }

    [Fact]
    public void AddSignTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      ExtPubkey key = privkey.GetExtPubkey();

      ConfidentialTransaction tx = new ConfidentialTransaction("0200000000020a503dbd4f8f2b064c70e048b21f93fe4584174478abf5f44747932cd21da87c0000000000ffffffff0a9a33750a810cd384ca5d93b09513f1eb5d93c669091b29eef710d2391ff7300000000000ffffffff040100000000000000000000000000000000000000000000000000000000000000aa0100000000000007d000000100000000000000000000000000000000000000000000000000000000000000aa010000000000989680001600142c9337c6875705dc8ead1d42b961ffeb2e9ac1790100000000000000000000000000000000000000000000000000000000000000bb0100000000004c4b4000160014265cb61e5c017c02270d7fe385ba08836616c1180100000000000000000000000000000000000000000000000000000000000000bb0100000000002dc6c000160014964d1721318c8bc7b016d0eb9374a620e8b88a9800000000");

      // AddScriptSign
      Pubkey pubkey1 = key.DerivePubkey(1).GetPubkey();
      Privkey privkey1 = privkey.DerivePrivkey(1).GetPrivkey();
      string desc = string.Format("wsh(pkh({0}))", pubkey1.ToHexString());
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Liquidv1);
      OutPoint outpoint1 = new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0);
      ConfidentialValue value1 = new ConfidentialValue(10000000);
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint1, descriptor.GetHashType(), descriptor.GetRedeemScript(), value1,
        sighashType);
      SignParameter signParam = privkey1.CalculateEcSignature(sighash);

      tx.AddSign(outpoint1.GetTxid(), outpoint1.GetVout(), descriptor.GetHashType(), signParam, true);
      SignParameter script = new SignParameter(descriptor.GetRedeemScript().ToHexString());
      tx.AddSign(outpoint1.GetTxid(), outpoint1.GetVout(), descriptor.GetHashType(), script, false);

      output.WriteLine("tx: " + tx.ToHexString());
      Assert.Equal("0200000001020a503dbd4f8f2b064c70e048b21f93fe4584174478abf5f44747932cd21da87c0000000000ffffffff0a9a33750a810cd384ca5d93b09513f1eb5d93c669091b29eef710d2391ff7300000000000ffffffff040100000000000000000000000000000000000000000000000000000000000000aa0100000000000007d000000100000000000000000000000000000000000000000000000000000000000000aa010000000000989680001600142c9337c6875705dc8ead1d42b961ffeb2e9ac1790100000000000000000000000000000000000000000000000000000000000000bb0100000000004c4b4000160014265cb61e5c017c02270d7fe385ba08836616c1180100000000000000000000000000000000000000000000000000000000000000bb0100000000002dc6c000160014964d1721318c8bc7b016d0eb9374a620e8b88a98000000000000024730440220205c9d0314ddd2a60e63e525e2452d360d9a485325a76ce359ab015bfbffa63102201da43d23a1cb397252d354cd07f0954ed5406b1cef74b5f2dc06c20897c8046a011976a9148b756cbd98f4f55e985f80437a619d47f0732a9488ac00000000000000000000000000",
        tx.ToHexString());
    }

    [Fact]
    public void GetIssuanceBlindingKeyTest()
    {
      Privkey masterBlindingKey = new Privkey("a0cd219833629db30c5210716c7b2e22fb8dd10aa231692f1f53acd8e662de01");
      OutPoint outpoint = new OutPoint("21fe3fc8e38256ec747fa8d8ea96319fd00cbcf318c2586b9b8e9e27a5afe9aa", 0);
      Privkey blindingKey = ConfidentialTransaction.GetIssuanceBlindingKey(masterBlindingKey, outpoint);
      Assert.Equal("24c10c3b128f220e3b4253cec39ea3b75d2f6000033508c8deeadc1b6eefc4a1", blindingKey.ToHexString());
    }

    [Fact]
    public void GetDefaultBlindingKeyTest()
    {
      Privkey masterBlindingKey = new Privkey("881a1ab07e99ab0626b4d93b3dddfd16cbc04342ee71aab4da7093e7b853fd80");
      Script lockingScript = new Script("001478bf37fbe762374026b2b884f7eb47fa61e3420d");
      Address address = new Address("ert1q0zln07l8vgm5qf4jhzz00668lfs7xssdlxlysh");

      Privkey blindingKey1 = ConfidentialTransaction.GetDefaultBlindingKey(masterBlindingKey, lockingScript);
      Assert.Equal("95af1be4f929e182442c9f3aa55a3cacde69d1182677f3afd618cdfb4a588742", blindingKey1.ToHexString());

      Privkey blindingKey2 = ConfidentialTransaction.GetDefaultBlindingKey(masterBlindingKey, address);
      Assert.Equal("95af1be4f929e182442c9f3aa55a3cacde69d1182677f3afd618cdfb4a588742", blindingKey2.ToHexString());
    }

    [Fact]
    public void BlindTxTest()
    {
      Console.WriteLine("BlindTransaction");
      ConfidentialTransaction tx = new ConfidentialTransaction("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000");

      IDictionary<OutPoint, AssetValueData> utxos = new Dictionary<OutPoint, AssetValueData>();
      IDictionary<OutPoint, IssuanceKeys> issuanceKeys = new Dictionary<OutPoint, IssuanceKeys>();
      IDictionary<uint, Pubkey> confidentialKeys = new Dictionary<uint, Pubkey>();

      // set utxo data
      utxos.Add(
        new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 0),
        new AssetValueData(
          "186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179",
          999637680,
          new BlindFactor("a10ecbe1be7a5f883d5d45d966e30dbc1beff5f21c55cec76cc21a2229116a9f"),
          new BlindFactor("ae0f46d1940f297c2dc3bbd82bf8ef6931a2431fbb05b3d3bc5df41af86ae808")
        ));
      utxos.Add(
        new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
        new AssetValueData(
          "ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b",
          700000000,
          new BlindFactor("0b8954757234fd3ec9cf0dd6ef0a89d825ec56a9532e7da4b6cb90c51be3bbd8"),
          new BlindFactor("62e36e1f0fa4916b031648a6b6903083069fa587572a88b729250cde528cfd3b")
        ));

      // set issuance blinding key (only issue/reissue)
      Privkey issuanceBlindingKey = new Privkey("7d65c7970d836a878a1080399a3c11de39a8e82493e12b1ad154e383661fb77f");
      issuanceKeys.Add(
        new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
        new IssuanceKeys(
          issuanceBlindingKey,
          issuanceBlindingKey
        ));

      // set txout blinding info
      confidentialKeys.Add(
        0, new Pubkey("02200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d"));
      confidentialKeys.Add(
        1, new Pubkey("02cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a"));
      // 2: fee
      confidentialKeys.Add(
        3, new Pubkey("03ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed879"));

      tx.BlindTransaction(utxos, issuanceKeys, confidentialKeys);
      // Console.WriteLine("  - txid     = " + tx.GetTxid().ToHexString());
      // Console.WriteLine("  - wtxid    = " + tx.GetWtxid().ToHexString());
      // Console.WriteLine("  - witHash  = " + tx.GetWitHash());
      // Console.WriteLine("  - size     = " + tx.GetSize());
      // Console.WriteLine("  - vsize    = " + tx.GetVsize());
      // Console.WriteLine("  - weight   = " + tx.GetWeight());
      // Console.WriteLine("  - tx       = " + tx.ToHexString());
      Assert.Equal<uint>(17713, tx.GetSize());  // at randomize size
      Assert.Equal<uint>(4885, tx.GetVsize());  // at randomize size
      Assert.Equal<uint>(19537, tx.GetWeight());  // at randomize size
      Assert.Equal<uint>(2, tx.GetVersion());
      Assert.Equal<uint>(0, tx.GetLockTime());

      Console.WriteLine("UnblindTransaction");
      UnblindIssuanceData reissueData = tx.UnblindIssuance(1, issuanceBlindingKey, issuanceBlindingKey);
      AssetValueData vout0 = tx.UnblindTxOut(0,
          new Privkey("6a64f506be6e60b948987aa4d180d2ab05034a6a214146e06e28d4efe101d006"));
      AssetValueData vout1 = tx.UnblindTxOut(1,
          new Privkey("94c85164605f589c4c572874f36b8301989c7fabfd44131297e95824d473681f"));
      AssetValueData vout3 = tx.UnblindTxOut(3,
          new Privkey("0473d39aa6542e0c1bb6a2343b2319c3e92063dd019af4d47dbf50c460204f32"));

      Assert.Equal("accb7354c07974e00b32e4e5eef55078490141675592ac3610e6101831edb0cd", reissueData.AssetData.Asset);
      Assert.Equal(600000000, reissueData.AssetData.SatoshiValue);
      Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", reissueData.AssetData.AssetBlindFactor.ToHexString());
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", reissueData.AssetData.AmountBlindFactor.ToHexString());

      Assert.Equal("186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179", vout0.Asset);
      Assert.Equal(999587680, vout0.SatoshiValue);
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", vout0.AssetBlindFactor.ToHexString());
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", vout0.AmountBlindFactor.ToHexString());

      Assert.Equal("ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b", vout1.Asset);
      Assert.Equal(700000000, vout1.SatoshiValue);
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", vout1.AssetBlindFactor.ToHexString());
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", vout1.AmountBlindFactor.ToHexString());

      Assert.Equal("accb7354c07974e00b32e4e5eef55078490141675592ac3610e6101831edb0cd", vout3.Asset);
      Assert.Equal(600000000, vout3.SatoshiValue);
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", vout3.AssetBlindFactor.ToHexString());
      Assert.NotEqual("0000000000000000000000000000000000000000000000000000000000000000", vout3.AmountBlindFactor.ToHexString());
    }

    [Fact]
    public void BlindTxAndOptionTest()
    {
      Console.WriteLine("BlindTransaction");
      ConfidentialTransaction tx = new ConfidentialTransaction("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000");

      IDictionary<OutPoint, IssuanceKeys> issuanceKeys = new Dictionary<OutPoint, IssuanceKeys>();

      // set utxo data
      ElementsUtxoData[] utxos = new[]
      {
        new ElementsUtxoData(
          new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 0),
          "186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179",
          999637680,
          new ConfidentialAsset("0a1100000000000000000000000000000000000000000000000000000000000000"),
          new ConfidentialValue("081100000000000000000000000000000000000000000000000000000000000000"),
          new BlindFactor("a10ecbe1be7a5f883d5d45d966e30dbc1beff5f21c55cec76cc21a2229116a9f"),
          new BlindFactor("ae0f46d1940f297c2dc3bbd82bf8ef6931a2431fbb05b3d3bc5df41af86ae808")),
        new ElementsUtxoData(
          new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
          "ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b",
          700000000,
          new ConfidentialAsset("0a1100000000000000000000000000000000000000000000000000000000000000"),
          new ConfidentialValue("081100000000000000000000000000000000000000000000000000000000000000"),
          new BlindFactor("0b8954757234fd3ec9cf0dd6ef0a89d825ec56a9532e7da4b6cb90c51be3bbd8"),
          new BlindFactor("62e36e1f0fa4916b031648a6b6903083069fa587572a88b729250cde528cfd3b")),
      };
      // set issuance blinding key (only issue/reissue)
      Privkey issuanceBlindingKey = new Privkey("7d65c7970d836a878a1080399a3c11de39a8e82493e12b1ad154e383661fb77f");
      issuanceKeys.Add(
        new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
        new IssuanceKeys(
          issuanceBlindingKey,
          issuanceBlindingKey
        ));

      // set txout blinding info
      ConfidentialAddress ctAddr1 = new ConfidentialAddress(
        new Address("XZAEvLpasCN7rb5c6Cq8HfaQRSyJihQAcH"),
        new Pubkey("02200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d"));
      ConfidentialAddress ctAddr2 = new ConfidentialAddress(
        new Address("2djHX9wtrtdyGw9cer1u6zB6Yq4SRD8V5zw"),
        new Pubkey("02cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a"));
      ConfidentialAddress ctAddr4 = new ConfidentialAddress(
        new Address("2dodsWJgP3pTWWidK5hDxuYHqC1U4CEnT3n"),
        new Pubkey("03ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed879"));

      tx.BlindTransaction(utxos, issuanceKeys, new[] { ctAddr1, ctAddr2, ctAddr4 });
      // Console.WriteLine("  - txid     = " + tx.GetTxid().ToHexString());
      // Console.WriteLine("  - wtxid    = " + tx.GetWtxid().ToHexString());
      // Console.WriteLine("  - witHash  = " + tx.GetWitHash());
      // Console.WriteLine("  - size     = " + tx.GetSize());
      // Console.WriteLine("  - vsize    = " + tx.GetVsize());
      // Console.WriteLine("  - weight   = " + tx.GetWeight());
      // Console.WriteLine("  - tx       = " + tx.ToHexString());
      Assert.Equal<uint>(17713, tx.GetSize());  // at randomize size
      Assert.Equal<uint>(4885, tx.GetVsize());  // at randomize size
      Assert.Equal<uint>(19537, tx.GetWeight());  // at randomize size
      Assert.Equal<uint>(2, tx.GetVersion());
      Assert.Equal<uint>(0, tx.GetLockTime());
    }

    private string ConvertRangeProof(byte[] data)
    {
      if (data.Length < 256)
      {
        return StringUtil.FromBytes(data);
      }
      else
      {
        return "( byte[] length: " + data.Length + " )";
      }
    }

    private string ConvertRangeProof(ByteData data)
    {
      return ConvertRangeProof(data.ToBytes());
    }

    private void DumpDecodeTxData(string txHex)
    {
      ConfidentialTransaction tx = new ConfidentialTransaction(txHex);

      Console.WriteLine("  - txin cout      = " + tx.GetTxInCount());
      Console.WriteLine("  - txout cout     = " + tx.GetTxOutCount());

      ConfidentialTxIn[] txins = tx.GetTxInList();
      ConfidentialTxOut[] txouts = tx.GetTxOutList();
      if (tx.GetTxInCount() != txins.Length)
      {
        throw new InvalidOperationException("**** txins fail. ****");
      }
      if (tx.GetTxOutCount() != txouts.Length)
      {
        throw new InvalidOperationException("**** txouts fail. ****");
      }

      for (uint index = 0; index < txins.Length; ++index)
      {
        ConfidentialTxIn txin = txins[index];
        Console.WriteLine("  - vin[" + index + "]");
        Console.WriteLine("    - txid         = " + txin.OutPoint.GetTxid().ToHexString());
        Console.WriteLine("    - vout         = " + txin.OutPoint.GetVout());
        Console.WriteLine("    - scriptsig    = " + txin.ScriptSig.ToHexString());
        Console.WriteLine("      - asm        = " + txin.ScriptSig.GetAsm());
        Console.WriteLine("    - WitnessStack[" + txin.WitnessStack.GetCount() + "]");
        for (uint windex = 0; windex < txin.WitnessStack.GetCount(); ++windex)
        {
          Console.WriteLine("      - " + txin.WitnessStack.ToHexString(windex));
        }
        if (!txin.Issuance.IssuanceAmount.IsEmpty() || !txin.Issuance.InflationKeys.IsEmpty())
        {
          Console.WriteLine("    - Issuance");
          Console.WriteLine("      - nonce      = " + StringUtil.FromBytes(txin.Issuance.BlindingNonce.ToBytes()));
          Console.WriteLine("      - entropy    = " + StringUtil.FromBytes(txin.Issuance.AssetEntropy.ToBytes()));
          if (!txin.Issuance.IssuanceAmount.IsEmpty())
          {
            Console.WriteLine("      - issueAmt   = " + txin.Issuance.IssuanceAmount.ToString());
          }
          if (!txin.Issuance.InflationKeys.IsEmpty())
          {
            Console.WriteLine("      - tokenAmt   = " + txin.Issuance.InflationKeys.ToString());
          }
          if (!txin.Issuance.IssuanceAmount.IsEmpty())
          {
            Console.WriteLine("      - issueProof = " + ConvertRangeProof(txin.Issuance.IssuanceAmountRangeproof));
          }
          if (!txin.Issuance.InflationKeys.IsEmpty())
          {
            Console.WriteLine("      - tokenProof = " + ConvertRangeProof(txin.Issuance.InflationKeysRangeproof));
          }
        }
        Console.WriteLine("    - PeginWitness[" + txin.PeginWitness.GetCount() + "]");
        uint txinIndex = tx.GetTxInIndex(txin.OutPoint);
        Assert.Equal(index, txinIndex);
      }

      for (uint index = 0; index < txouts.Length; ++index)
      {
        ConfidentialTxOut txout = txouts[index];
        Console.WriteLine("  - vout[" + index + "]");
        Console.WriteLine("    - asset        = " + txout.Asset.ToHexString());
        Console.WriteLine("    - value        = " + txout.Value.ToString());
        Console.WriteLine("    - nonce        = " + StringUtil.FromBytes(txout.Nonce));
        if (txout.ScriptPubkey.IsEmpty())
        {
          Console.WriteLine("    - scriptPubkey = (fee)");
        }
        else
        {
          Console.WriteLine("    - scriptPubkey = " + txout.ScriptPubkey.ToHexString());
          Console.WriteLine("      - asm        = " + txout.ScriptPubkey.GetAsm());
        }
        if ((txout.SurjectionProof.GetSize() != 0) || (txout.RangeProof.GetSize() != 0))
        {
          Console.WriteLine("    - SurjProof    = " + StringUtil.FromBytes(txout.SurjectionProof));
          Console.WriteLine("    - RangeProof   = " + ConvertRangeProof(txout.RangeProof));
        }
        uint txoutIndex = tx.GetTxOutIndex(txout.ScriptPubkey);
        Assert.Equal(index, txoutIndex);
      }

      string jsonStr = ConfidentialTransaction.DecodeRawTransaction(tx);
      Console.WriteLine("  - jsonStr => " + jsonStr);
    }

    [Fact]
    public void TestDecodeTxData()
    {
      string txStr = "0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000";
      DumpDecodeTxData(txStr);

      string blindTxStr1 = "020000000102b5e7e11dd2ae7ed6dfa754d406d240fe8cd0ab1e329cee6edbeffad5e54a4ac7000000006a47304402203d0d7240234aa446a08c1d6107789405c0f3499f4f5dd61fd7318ba58bb21bae02203d2e5a37c704c95af5801618edfb2184d80871b79a160f9dbc8a8e0a90467b380121030ab052e1482e9715c05301b07cf531d6a7e343bb508f0f2ba9126118c15be5bffdffffffd007d56e9e984c52b4e077487a711ff0c7126da52f254ea4d532dafd78748d2c0000000000fdffffff030b48263bdde648e0ba73cb63b44410ad1941fc1304bcba6665be398db23a702a300979f67b8612d871d2dbe646debe1c07717b0429f5afcc7889d84572e9657176d803430e3f6e47f856ef7a1b2928783e2ddcb8acff8e402e1d8c4c22b078e8ea36ea1976a91410eb66140b970b99b072d25fd4f07b4e88db32c088ac0bc322eb24c971bfd454fd61577b70eafab7a7c42f3b973cf57b3e56a002c4adb00802025d289a81f637f62c55500d6e439a9e13743ef7753d728866acc58b459b6d028a3e9de7bddcb400f3c1534270d9063dc465bd06a3da50278013a0ff4a5823b617a914862432e4a10eb1ca46c2e97525ab27a13abaffc987017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c18010000000000009da8000002000000000000000000024730440220761eb444887bd22ed0a3fc05caf4b9e74fa879db2b6cba70747f9aeae40848c00220070205b4817123234536efe00ec778240a87e5d8b5b9f9e155e892767ee922f20121026e3ab12d8a898ac99e71bbca0843cf749009025381a2a109cf0d1c2bfd5f86b300630200036507e368fd17b9db49f8f108b7dc78af4cbbdf67227d77658e2da045ea665cb0a34bcbf77b32c0b3dec83385b8d14a641e926951cf08099dc5e22205db641a8e5ca94ef7ea313435be5541e2b6a5b9b199750921f65dd1030fa4abde717a29bffd4d0b60230000000000000001ecb2010aa97ea3544b1ac0c9a3321a3f05c6accd4436f7944d670b15c32d3f0541ae8779ae3f6105b01085df24aa249b7d12238fa8a775f06815e2818eb4af3ef8f075d1f0d3fe89fd5567cc8c6b4cf4f48d11fea10809ca06f7bf47290c5182516a0d797fb43a80af28f221dd09628f21cb0b98e7b82567d8e28dc2b494c1dd248cc56a9307c39974da90050d2312cb2858e86de0d393ccfda7ba3b368729d4e9b3972393e05e6ccd623e8dd035205ba554c099948bd8992d20030e145b95e64c3d91f6e3217d099ba5a0a64fabe2f2172102097160ec40baee5f5db764abc2f666cc22c20258797d623f413e399a0377561633b68f2057ee74b1d2f1b040e49b5e3df38b612439d25ab332ef9aeec15e6292d84acf5a3faa4d4a7df5d9bea923340363ad0d6ced0e274adbc82c7cfb4801e37a16700925f6f2ff626f43c99817ffcf320070927037508c0372747ef66ba2021e0ac876c431e16a6919a77b5754ed3e321d30cd5e9df6b035d5842cde9c5e02d595ae299d2570b4236a53a0e5a55e5cd95d1f2bc1258ad2db2aeef8d36d16b8f7371cb38b13d6c28f2c8f0152b7e5a49282b8d95ac94d9a592ff0dd5a3fc1ec93fcb742c7cd4b67fa12ab3f69c7c4dcac75eb30a27a12bb82cdb714b6413386fad53609fc1cd455c33e127ce8451b690e62efcb09cc0735f4544dd288835db5d1dd731f0904a33817fa464fd3de8c09e235d36892b502703ab18e4037faab71cbf4ce02728194c4b296d9d1a1a6ba5702a0cb3741a19bb507cd2373f7ef865aa9a68159cf7963bed7ac92403ad7c8dbab0dd3ae8be3cdfc9be71f598195feb8bc332164c2207ef8b7cb6e42fc6501cf41768f39b849aa1b0c9e0f404a913867597cc7df9f88afe55c5e37d53aafde632449165feaf04a147ea3d18b3733e9d4b69e5478f42177486c881553c0c46df164b50814a5151df7b467cd366f9f5bf8171d5fb20e02e94b6f78f91b1bf6bb560cc761f4113759e1de2bc4e5a28085d9ead77ef95eaa481995162143125be8a497dd0d7e45b230c18cb0fe1c13de6849a37efba8a71b7cfab7a3b366de29f038654996c46b97f26cc04cd79b80e845a9f0fd9680c699c61eee46cca800507824d4b5aa053f02fb01ffc7bfa536510ff6964098a3de2bb57e07d7ee1ce1d03966821c06e7f85a0f517ec19eb64059d298ccea429d5bb88fb87aa26b9d97efe810be69e149a426f38fd151e37dabac83c7d42a64068c6d3772193d3cd5b139bc5002b20a046808c01bf506f82255f630aac431ae21b508d839018a6379ca53f27662b525699e9bc316984648961103006eda1de37e71e18078fd79acc7b161297712acb9a552f5f299161248fc328b965251501f44de37acd8ea968a5d38583a9a26b2fa7ee48553bbe24a4ba7ef730eaa741c06ea91367e9eef3840d4dfee1538031249975e83652a1533479b591106e5c2607161149c1b1f1ba7839a105753aeb4b899efa2064c9c9c971025d8e5572529da6b42ea615246b9910fa6560323bf56a6cbf652991145451c77a819e141594fdaa9125cf01e0623a22bc8015bb866e3c311e170bc5f8ee86b15e6a9b20c9bdd240ff75a95981fd03947f11f2c2baf01cf5697b3329c88c896eb508f6826ae1df45bb426156fcb20129f33af4880a28b0d7e894dd293a21527743ee21ff6981df4c875f827aae158c105e6f30c66958e10a5d8f8255ef5a958474fa67b711735deec717472667c1cfa068f77c7f7b2a7adfa38f4e465bae2658486fcfa608e1d03737c9213f68ee04305b64312ac43a1a9cbd16cd20daaf13b1d58587f075f72cf9bfb121693a8e27993e334f5a1627435eae39c5a23c7b0d880c85e672374b1eb480e6bcd8c0b505c94751a36a26fdd6f9f032681ecd5bc75b618cd11489863efcad774a0dab5dcc7c4c47583f373222e02bf6e21921d7cc641685ae021f049e5bf61ee9f5c98d30d3fc0f9fa112e378950e2f33195f2ec245ff614676b09dc932625a989089a6ce0ca6f36d49093d7a544b3255658ef88d49a6eb200c39caf55548a0680ee551239cf693fc2fc1be571bc6080ddba18f75ef318e329ef3e721001772af8f0b869d3b83339917c73d383f2f0fcade5e8f936bf09112c5664e57dcf622fe69f467a342cbfe692882fffc31d22ccae655fa2bc161718fd06c89395ff69c2933ba72ccac3505396a69ff899da2250f6cdf0031f6e7c6e394a0f3a57d078244f11c70bcc5a93aa4756749b12e188183c82aafc6da92236105334013afd315a6603713a43075b9d9fff3280a5a9de58a40adea4958edcc404ee4714c45a82ba16dcb199739003986c0c06176ab0569797f3be6eb7ccead4be4905cee9eede3f4ad83723f207e5e467718bd940b80424d79e3781958f22f778f90504dd5d374c898520170470e9fb4789d3af82f8e61ea5b13829138ece20451a35d8b876694674c8d891bcc1fb44f188eda12bbaf1861480afa931dde5c1017d8e75d8c732df05d4859d4d0d72cdd8361bcaa8e5e13a2dcac0cc6a93bd94523ad054de6fe90de0b69f5410b3c8e44fcc32a9c7f475e42ece13bbd94c1d86986fb5db804ffbc72a51ad6fe058a1fa50690cd4ba4f93fbde3bedb3e98ff3803e3947ef3f442fb59932e5bf3e8ad1db8066a947213e9f956c4b615633668453dd5ad6a3db9b5d9a60ba86881c2c414da9e566a2198c2b1041432e21f1098fd904eb6cd06e24adfe31c8151d238598a7f399f6cb08090fa786d76dbc16c5f06ea6bc10e44ab042aa1507fc290b9185dc17cf78f2eb836e89eb12ca42d30fb96c097e0d362e91c3414606c5f29ac1683ae90b2ef28cf0124186135b46780caee8589d1b733f7e5ac74488b273451012d46c72e85163b428a056b4812595e046b650df7a08cc343503cc1f5bf828c8859854b5c61629d212a06acda00ce4d88b4fbf2fc0e3948d16974a9aadc38c61fb04352896e2926963c60551fc4e5c91db4887551039718321fb4f2df41cfe5f868ee0884eccd2c8854f5eea49e5fac2be54fdd2908fb1c24c4362482a44c82086d72907ff6a80cb8f7be17ce86735681002dd0031d6d011157696dad161a129f7984da3f97ee43718e9c67499d2cdc8bec6f255bd1841ccea5870d3d20d3b69b507e3b364d1d33b4d86dbce407c1e2b4bde37724ed022dc9fffc4d85a80aaf7d0ffcc0d82783f9238b46e17d66f4532a9c29f25df0fcc404963065d776f5d8f6773806c79331cf2031fcd6ab49c28a1b6a10be13a8bbaa2b4d8fb2d14d74ff87b06e989a7a141b9775cb22707cdc5bed26f269f0054245533055c365340e162fb7c2fe38e91afba7b0c2cb222816ab0a5d68437b882e997e85ceb1c7049722643f55857a23452a3cf228f00393bffb2bf100b63bd987f550df8df9a2a5d66d7f642d1e31e81058719469959aefd7f00726da2e911b17f2a892d27a2e2ea3b4c18aee0317565406c4456cc11cc70b4303b04fec0193a3324a8f310f2004fc7b0676ba75e31bbd728bfc248acb1fb242a8b2ac6b349efc38d4e17480da1f45b3eca80cbf80d6f543dbaf59d69a0c5a0bccce0e15532252b00a5e11be765d15bd6308aedfade1c82e9066ed0a4a985d332b81bbdfeca78faf31c96ddf4651218b40bd81e6e13fe1b088ca76ee9a2bd7676f8792c94fbeba1d6dc7b98d880044d3c424cc5e724db685d0804695675129b08a708051c98dae98fb3248bd382ec48ce499a69e45b4eabf2abddea3099c006179207152fc7e63c11edb5d8c9c50c232484636f3240042420b6380d397645c6a2e1d58954947f11863f59eb30a57cbb9917eb6d92c0a93e4ea3f4a0884aff0ee08b93a6603b39de99beccbea94c273786f253904b74abf4103ae099a95154e25d23159420dd3e836c5cebe2772ea740fc0ebbd7a1ca45314e06fd85d9cd98235116c7a091120a2020c9f5d9f3952c44921f934a589985242aa9658b9cea5cbd4550cff46b952480cd822eb0a94029570c59262ca0a6b2f819c9734355d40919f3a96b443f40170f09954598c36cb9fff3356c97829963020003964663a99750c551dd2229ea4fc24702909f4ca1d258e58165b97a086261f553e2d5dc9a23f4231ee4b1c7f3575c8142e394b6d4f4cb0810ba207f400f3aa3d8156632f18da696843ad6ed74dfce3f56feaa97a35ec49b3a460cee5083bf8025fd4d0b6023000000000000000171df008e5ce1b189dbd7161c603db628726b84dddf1083d23c43a376511634ea404fc5a6d1eb5a95b767426e72066d99cdf533b4b075ea6dbea840796c632fb01d2eb2d2fee92b909e269552c521dbf4fa4e8f123ed119513edd066ad7ab0dfcd87bd3b2cc64a665eda4278f922011f799ba6353f85daf9020e4b95b7ad4717a233f474c7e27433ac20ba1066c66296819a069d909d1ce015851286193993d499e0ed4404136dc18b54ac9bee46c34f4a2c26cc9fc3bc159d172a65ec4589546f70d51f0025c91321b54bd80bace8a363370caca7dce096d811f8e496526a370acf590797384d0da382249e6024fe2c0494007689254e9a4c299758c9b1fc6e6865f98b4e04630fba0aa25598f0a0fb339559296043243aedd672b60325820f2b4d88e5ff134f735e0e4fd2abd0fb258b4004025eca31502cfce7c6d879b7faaea94552e31d49d32df37aa0881f423242d472d29e8971d6db88cba7f92fc08e27d3bf742ae270a12eedbf73fc43a9361c94807874495308de00e3c1720fafaeb553ec8eaec65c41a61cd9110894269258f216ae8d23af94141eba5b92211f7daaaae0a8c2ef5a6d59c003ceee7c28414fb5c142070da9930e404bb0a33dbeec1e06168aadf715c5426197966a2d56e172e4fc6f7fdecaa1ed3b1e397d3e83c3d0013b15a78ec697e635b80b5cbd88e2c78867fc4cfa274f09725865edb109058e114502a6d9952c2e8429287e509bdb57e728d4d7beb5c8e73cf9eb45c2930ced482dbed0a8adf3e47bbfb0ee5ec9c1242f254c02b5ff4f54a4b0bbec240814b38b1e20f24e1505d22eef07d6fd25fecb2ba3067ffca727d00d70b070cb0411690479b65f61eb6b357b5f08075a53340caaed328a5af007f7acc2ead770fba7a06bbaab5584eb1c8606e1e6366c640c202c22c34d0e74cc4b14993f11ab04e82291f8f6ce7a2c1adb00e4bbdc7e20a19a39184f0f53726c61d931223ab8b0ca81ca5592a4d44e28b41b00bdcd37cb02adf31c0536f6fd48aee848f1adb27c3141d21a5bba74af0241ffeff0548fbf29e278aa3a0827179393b3a0860557aac767fda675022efffdfd075c07b96ca27f05eb4b4a1f2173b8a0595b1917e30fe37d82725dfb403cbbf9cf84352209cfe70d4792967cfda5e1a7fbc05112048a760a215f2a965b8cb9850bd8544320c3adc30f8dbb53cefe0280d9b3781c1bfbfe7285d6fb91d0d8c8518a7cea21da117e3fbd8570f2371658cd0db77519ed550e700e5c362ffe688d2185b878f6a378005c174eed420b69be5aace92b738579f1d218496f789f4a935e522b3879d8ff23b755c1f40702b11107e76a8a7b57ecb1b36a90c84183fd6c69c35e52493a077305359c9572cb54dd9c3ebc0db510987f4591ac28bab490d34c4e40aeed78c5c8ce2f77119f833a5c882cf7c5d197dd8900ec1520443f2154a1ca4ea2f8056182d7c6839971910fcdfe4053fec4674514be84256d69d3f41c94d343a1fc3778e47f29fda71688ac6db278eddd1b887e0c1e2754bc5e0061452de03ac38f0fce3297246ada974a2abdee4becc12a7d0245439201d5ead049e6a5796da02d3ef79741e372c697f42c6b26d8fe06a8bbae8dd7071d3fffc79b947bde32f0a70de6688820c1f9c240b9d775299cdaabb14f0c3bf9cad1d0a7f76b7a839ef3a54cdeb9de47f07a51e84beb0ad052f66fed105b3acf6cc7f51b19a519de8ea759bb786d50f6df5a99cfe838c7564ce137929e925b9d4a2a515aad8d31ee48cfe1b73bdb9e08020dff9f229387acaffdde47f9dbf1463007d169f81aed7cbf87649fa8cd8224fbc815032d968157693380f9edc784758a14df25d14e6f80f7e273d5c9843ad9cf9c81796c0c9361a82ccf1a06ce1f880aa9586412a947eb58e6f4e3545cf180c84b0aaff2e4e3f947a831d85f1873a9b1f2e40079df0e98579a6b293690f8dd2c66569f6e55a1b8fb85482696839b53772bf2eebc05a5346198b191fcc820f20bf6da8602a65287ba0c6c0206170588707238d148a829692b60b8ba3142c8a24da7771bcbf02ea9b765ec0259f9d2504a25cf9ff1f35d02ea6fc43b4c7330271200a52591e4367c86b44710167dab01558861ec2b7d5da8c990d9be1590fef5afc606db732633ac8890d00787181b5f38441bebdddee361997c9a06499b72818bda1c20a7c4fc666600a86ff06be0e8e87ba143fe6a3871be9433869ff33b3c67b99c5abab03a21036636a3e14df121c476753d6dbf6b45bc9609e440cab81452a1bea7c8e1441b3bcf3e443afedb7679aa09d9870dff0bb72d41ad5372c94ff6ab9f28a58576936b61fc9cd23aa1b3191bf5f590e86d2595012fb82dd4dcf6366d60c3c9380a5ceb60c525e9235b08f00c09ec06c0f760e64d703cfc4afe222d44372109021da9ed278837adc6eb82183e686081d21ce496a83c015543c032bc2bdaaeb796ba89c92f2bc66742cdae9fca7828eb9b27a95457d1f8f225b3bf0a8c52de25859ca45c8e97a04540f4164e07e7117d8d877c7b162c146aaaf32bb7426257c26faa35187d7073d1d06272215700ac6e419d985fbf26d58161f5424f1f57b28607ab1cb87d5340195de5b957124fae287f361f0b1cf4fed091620ab3ae70fe7fa0f83ab09add12bfe4e89d7955e66e2785024ccb1e179da83fd9c2b020afe73dfb60e5454d3ed87dd85c663f6f92a3e84bc4f8bc20c9ca755477260b51247e453541f69faffa864403a9acd5ec3f7e9eb7c700a09d1c25d58e03b25f8dcf9dc15a1153a2b0218d4b64d2bb56cf57fa62c4d1ea1a3e5cb9564a23f27d1b56301003dd62cada5312b15914a5086a8e9168dc0d493cfa6777cf7bfafcaadf47f575966c38ab7ae2149d08ec6c703161938ed75fde6432052f224545e5729229fb13f70e57d6965c1a5f2a191ba8b60ab934a7c6928d76173fa1d9804ffa2b7384c229f51c1405f34f1a089625eed55ee36a2ac83a6d58e4c7795fbaac004e60eacd5c8a5fc7e775cfe5528bfcafbf3c2a69091e58a74a0e1ed19031332caceee7e60a1955734155764d13bc457bd659485f6e21f06db6bbba3ec13e1cf7f3dde73b07896101740905e2c745019417915279f130115bba798bfb08acbbde629796849418e16a62b2cb51eced7e87ee9b3a083faf4011730f964aca5632a08e2aa8fb662f986ddff057d677ba1f2f1dc2e2085b561c8b24a2e65e47270babe6e7350a9e58e2a03b43f544c13c00d8b956ba65e3c4c3071df806d69c3ae198ef4f229c8499fd77a020aa9d36715835249daa8f539acb704f6a1d489137b3af0fa8991606d4b530cdfd85788ab8e5c899ff0abdd02d2a7fc9e74d7e9d2ef2fcda34b810a8b819c00c599aebb6f14efa489b7c965f439c12acd805c7d734a30210a3dc25ed132aede74c0c043cd76dfc6c632385fdbd817c4329dca712740bcd6dd68b164af78c7b048fef6fa7ae0d1da489591abbbbe7b81e02c054f7a0a7a9ebeb769fd494167d0b3b8698842f84e406204bc2ccc373f71ea7a83912e6826db5a0371d80b38c6d536ea88a3aaef71b01721c9817a93ba6d95c4c239ee37c75f746680febfce1cdb5a523cbf5c6b0e2734cb7cfa1133c918aab211daf63bd7f706e69cbffce4603262be927aee1d8c662f3dd4735f7551f1c1b7382b0602b6f49724371d6ea54bf8651ce2b0b76d0621c420cae8306facd7b213e36ad89ccd6c9f3eb5a233cb9391cfa6443f38b489c70460dc513d0a6422668ed9437905b9c7eeb9b1c5d84e9f5bab02252087ed3d05dccc7eb0d429cd3a0c173c5418cdf621b276b3770453b32800dab24b33efe07991802ab0746f9170295b608eaf6c76450207648b1cdeb38864cae39da3b55079d6b8ebe8cdb774e419a19728495a0da0ca039416d0b16e52cfafbc1e07412ad232b749f42404dfd8784f5f692a5b48eacc40da56e551809a2a7f1b5e3dd7de298d16a986ae4d476e104433f840468f16efe2a3b78fb5418c9e738ec13911b2ed98c7f751bd7710f363d89eb69ef911212d3477104c4a05336fc29cc0371fd7b30000";
      DumpDecodeTxData(blindTxStr1);

      string blindTxStr2 = "0200000001016902d7cfeaed59a61432674060dbacf89aa50dc4fcb5980c55034ce75fed5eb40100008017160014bf7f6fbc324728d857801e7f3eff88cce289cf25ffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000092ef327afc62b0749826df12d34412566ec1743f75ee421b4e4440f0083bdff8c0883321b72665e9844cdb8859a743583ac18623cf6b78e512fa291912320419104040ae7be1f8a5aa41a98a0fd5ad39efdcc3f36a63adf5849836b08aa622ea65d25db0880596e89690a32299914da0a85fd57db46575663968243b3ba5b1ad15120c402034a6d652db056e0364a0eb997821d62266da79409d995f9d39d011ba0daa5998217a91472a7834df62071c0bcae9e00b00cebef505b0cf6870125b251070e29ca19043cf33ccd7324e2ddab03ecc4ae0b5e77c4fc0e5cf6c95a01000000000000271000000a364d73c6e121fdc4ea87a0228180a521be1e03c3b287435dbd06536ac6bf4770080e7d14989746f3c5d3cc4ea1c651731e9c375e71d487665a13ec8ba104b14f44025204161081bf96756531f5b8b00d85f3b831041be2b8bd108a48ffb44d9f014f17a91417514a45caadd530f301c13faae6003c4ba1cfc8870a9033c37f8d1151b8b87c003dd2b2ead5f85e3ca32340e9768a80ba5a840f9f4c0839ece197daa1568947778ed05f8d499e2ecdf326155a93a8052bbcb591356edc03395b4c5090e3fcbb0c5c2b258af7d078a9d4683d33d7905fceae7f0f7033511517a914b721bfa4e90777607c95ad886362e4870ef6072c8700000000fd450b4023677000a52b903bbd128b03b1e29e4d82ed0b43567843cc5ce1f4f979c7e631a1027d48f72a782f9870c1d5eba05b5e582de397255349313c54f5766f27262640c2f657d49a2faa9de02633ae9a627fa2a837a85252fa4f74b6a59f2bbc8b8bccb6094705ffe112c6b0437ea4a6877b53e53a6547c9aaa46e5a1f4fb8c1001d5f738f70d29551b6949bc4a2de85b842d6abf38aca5815c4ac5a6f5fab3a0df52ae5abf354cebf6f15f6dca2a855039ab6697199203bc7a1599990d424dd9bb6c9712af2a49c3abe609aa8d31a9e214fafeabe9393d9dc2dfd92b4f24b19a4fdbf376bb501936b9b875e822b7b117aa6c114c21761f4240162dfa6b98bc5f22c524ebb54130b0e7795663ef7f27ac1fa3daf65d85a8ac75dd98e7d35f14967c8a9a120e9715c8f2399cd41172478fbd1692c961153e16c677520269309687803f070d8e7c890b6f5f2ab79582a862f4ca4a49cede94d981f121643f4710835c1e30d560afb3df7b1020e1530eec31094e346cbe070b3b0e59478a962c3e6126969906c1b39a17667825a93c5882e63eb31e9003b5bfcea0c8a80ffa1ad1b0cca2b1d37382f6307937a6fe41f1a49599d508733b87524cc15ede76d3252aa571541fae83ef75e93639db72a6e54bf27fba09d1ecb8055c962475b996058e6ca85daa7a80d23a3903dd71b93d986ee6be8e27a14f4cfba3e2d953c065880fd715c739628618fb02cce6c4698a0ffe9e04aa116294e9c626e2e63cbb044991e3f29b7215a894cfe8ede4c5ab52ec47b6f64b3e69576798f65a4c3a3943b10cf07723af40bdd3ba6a382154605fe761a6ce0115c6a02e639ace3b44feb378262fb1c18ac2d70892195c6453159b9eef9aa86f8b300e0ef3408605229afba6cb985486d88b880f53f9019430b763d8fd3ade45424394e3a66e9c4d7752e63a1f295574e06bff6974fce8b8385fa597d9c731e972741e80dd4bf7bf6ab2fd0ec7d9aa1c9488b08572650967b373d08a75367ef47e348b5ba9ab90fab85095fae2b0daff44cf83883084dc8d0ef0adcdc95e74976218fbd7d7373fb0646786f76228efeea9a9e9e547e0b11f26b933cb41e5858932a5bcb992fa8b471a061a8379ca053573ff525c741eb0a51e7756820dcaaf7547afd71f30dc06495b6bf3310a9b4277743e9a17ffb5012d20d01365b7090d763c5511f9562437df7b7ede340004f15321cf0ec2994882f3e02114fe44f1805f58e0af310f22ab5a68907041f8f734b1f33e3e8753e365120bf2fb4f2a2a597d3cf2c8839462f35eff7ab62bbb0a4433876bffbf5532c2277ca48cd0a3eb4b773cefb46d0d539b177a694c85e02d70e99f14774faea2aefb65e639e8702ef2434ad66453c1a9f5b00760aa372cd432748b11ba1757b3071e6169cdac6beac4a160b55ea02d1d50159095e593e45683e0ca1bace4581a09b086cb74187de61ab1175c0d42a9601227be8422803d34789219b150a4de6a4432d28127195b8b46ef721f965aa0dfa5a939f85bb2fdb95efe882b877bcaa676ae964ef37d8dab7d5d6e2b6f4faf4f811366576a9d1c65c36c77a8daa6954f1395b1009c859044df8809f5dadcbf1dc16ec5955f0671cd3c499296bd494c6c0e1f0ab0012018e90bbf41d760c88940254258f92d6dfb6998b4afdc4c72487721f0e97ddcc3b28d941c7f2458d51916b9b2094e3c2913df3c227f8f957e5f595264b0da17469ba1e0794490cd5bb1b35cacb5c12152c8303ec038802ed334cef24a28e6aa38708dc72100879f721494a40659c41b63cd2c22d9a55b07aefc71a5532bc03564fad6767a41d9b52bbb1a323d280041e44eb87674ee964c9428e9ef96ba1dac685f8cb8fb950e34528fff02368be76cbddd9215ddd5e0635eae652a0ce642bfdf7777207644f1ba503b409faf68c48a9a31142af5396e4bca8e67cc183b30895a1893b2901e91b4e784dcb137b7d643ffadb6abfd8821ab1b4f6fbe20275d42ff44da02b32e00f238ba7b326f457f3ce70a2b784d4f165e594f0a27d21ef187376a1349401bc6d23ec2e356ce23e8e0be87f599d335e41c43985ece05f0dfe56d945aa5c3410d54a9cadfad6f29042bd37dcc069dfa27b2c7862a01b2535862d96cc80a0bbaebe1f673ae2daebfa133a3e1d7dc959fe062313ef9813f4341a5aec78c76a36d95dba906719ac50973cf7b2ba7037f489732bbc46b8da67f14a8e0ac225858c09f182c6c0b0540f9b69bd03a00c055016afec4c271cba5266255858c87516915e70db30ef8821f6336b184d8afee9c81ba51aabb5af9af9b473eceb9e2cd9076eeed17a5d423302008c63420d49e21147f48de7da5b69185e9733993a3504903a9df6c9e48f327ede7a9337c4e40c24739afe1e0b22377c62a98f2bf9968833af3f7208c7363e3702801c1145a1f1469875f0496a14ef401bf6b1f0aa75f77f031d30f415b514a73905bc7d3e2127ca88ad7969b08f021ec08fcfb3d7b23f1f22e42a5f77aef6319fd4eb4ff802bf0aa8b770789b94eee957636d23a303b07e5837cd9db6744eee86ca00a8ab63406203b10029ec27cf2551a05ebf7382d64aae1011c0ff6f4c98e3ca2162d7a7ac332d54908be1ff91b017b38786809c1a6007d2c6e4fee8b8162f2329ff2340c2302efb08fb8fadb5d84b6c2394988bdb69c08e51c2d7e003c97330bb651ed787584385a8cac09b16882c0a65fcf0c0812e9060a3b4ea443d583cbfefb0aad73d9d6b63eb75de3b5a7e04f26639ddd29e9047b6c0efae2a92b104157ec6164bc9c01249f073b5e42ee4618216f955ecbf0b7f8e4613159c1ff7009ba36571423557aa7807fdc4a07427ee3efbd67b645b3fadea1786acde3c73d65d3ea2743cb965fce112508f096fe44e6cf6d225b74e2f4a583bc8754f482eab453b7ba1472f75dc8892d2f0641cf5a84fa881890b2820c731fce87c6222d3e2014115ea567d0ec4945279bae6813403a46c77eb80eeefe958b765c849e4253fef2e8b474fab5b9df7ebf5ed8fff15063734e283a05e11638f38e6b0c375a989b3866a7122a1e9c2847dac65fce97b240c0bf0748a11aeaeae38d5a7527918c01357e06ff880ee491e96f3dbe75aa2b7513209dbd0b628dfcad3b9ad93d0146ef27515c1b50d43702b47c12b7bde5bd837fbcba97c814f616f7ae97053548afb0c75d1e6fee57bf6baccf9c894dc028c83beac85e1a99304cd76becdd3ac18b5995ea913211440665540dfb4d84706a49cd7159afd36d1d75cf2f2f317a5846d5a54be5d0c5a103a35ec2a370b4b81b11c61247e3178413c18dbf0532c28409b963a6230cb1290d0355d767f0c81a39662a65de72a002cb39e57950eadf93744a345cbf663e5e0d21495605178085091166782188839b462322e95a9f06275f9fdffc51fb7df79637ac388d9d28ce5fe590f99b16f7300756053ed3a3d990db0dc6f2549a3354cab2894b14aad86d2505ef574af0e12b2dd32418c2d5fc8106524b8b85e1953c4459ad7cd36e42202874b9854a0b366b809bbb4ee65757e3876877b552315cb1f0977e63b196b17b7709cdf937733f0330f19fcc192ac87d8b3f76494b38f9d7da166ce8f068a11a679ffb0f58857b4aa9d951f958db0e2001c62b0412de553cacf95ecea11a94b599188cc0fae8c0f404c4591072a08e015e235409716e048c05c208e651580606cf842110065b25064aa3829f916f1a770f5b7c07691612548df3b39dc76d1185879ee541bc2089f50c9bd116c0eb9bce88c55b3d99f5eb5344ec9b437e164b5c72dcd6daf765782dc45c48e572db8c551519e3f0ecb62761d80a7ce1fa319074c101710d4015af4017722d95dc2d468dfcccd8d9398a17152f054fa01730ced76faf3f12c1e5ae062682c74e823abfe59710391c8c7350f303f0c34fbb3bfbdd671f640b4a0eb483e64fcbf560f45b8d5bfe860bd6ce448229febdc3689a66761936283147495e4f7c75df412eb91bd66e81dfbc9f04852f10429b99f03429964c0b83d9d3b9a2597933be5786a00b06759bd95cbb0e52243d37194fd450b40239f26013d1f1952c24c7ca9878a6042683813daa766a6f6887578f1a2f7be418273a0b7e89aefdf3b9bf0cdc28f2e71444825e9978d34beb40e3bfb98497418c3a8386aa8a1f2dbace4ff72ea1e11c00b4b1474ecd934ac6098716d11d7fc83364f91908c12be70081186786503f85a9f7654f7a04e927c3df7bdbb9ffb2e8f7d2174ae0c18bb80adc1248bfcc65ef5b7599172b6428684a8f66fa73fc1f70b4f3f12e6644e27194b0fececc73d32ecab930d51555ec814ada14d1f5d76b2d3b03daf93d5b390314288dce055ee4207b4b2271af3f7351f8bed0de0796ed9c370fc78031b9c60937f786f5fd7f3d3bcd9e0adcc8aa7c0d3af6ccf733210222dacd7422f4ecb5540752862fbb710d83955069b5e91219256f3def7d6bbe7225be32a5e4a9161f7a317c1aa159c18ebc3e88527677e4b293db135732954bbe051681ac2eb96c9ef53f7055b483c4e9a88c5598037e233343986eb9a6ff98d611d8bb9bbb5445e784e781f8b3078bd795b8c24046cf95edea799286eb0faf952408ece194159c361509d2eb7dd0eb08f6e0cff96d6fff59fd9bde2825a510bb41a637c3dc961d268a84d7d3610eb5900a48b9d2808fdcf7570372372aa9f5d0af426d6340a8b422e18dfb9ff92439db30d548f727afd75555cdf28dd4a3f8ebafb680936c544bd2d8ab5c42be214e6e095678d7c5c6c4b56d8e1ac5025313466992eded93fcc365de54fcba323ce273afa9d6355747d8f03bd9ef16510a241f09a94c19eb3e315ee52049c4f2bbd3938e1922a6307711e2133ed442d94755cb4f9fff3b12c705bcfebba75c42afbf09f34cb2124c09462ee81805ee5c4d1f50145ebfb6761edac73cd3180d470c6afc11be09c4d82a95cb4a292ae548d911770a484e56f6caa7a8f411e61d3a11918a748c29953823839813c3fb6b7b5d7a19657fd015619dbfff91688a5510ad3a66d23b44f3182f2bcca1f0a85aa295f1b8bbce15dc8411a7b7ba01a20651924b052efdf3d686b232ba54520a38982cce05c748fe7138655c4273e6a2c2a1a615549635eb9bbd8e3df75c4fc406f4321b170dc29f18b6218ac79f9f9f83e9b126231679ee5863768b6873142017b3e4e17f18be79d988adfb5d4018481d3b07da9b65f95d114a69fda0702c881a0b61b3698daf0670ec79ab22e6862d4f2f369eef35255d6a58dc6d2e0751cbd4eb99cad1ae62aa92c0c80b971ba668bf6d089f79a3105fa2e7099942bace83b769594d9cad7725cc5b8c2cbf29c5be91b5f00e20763eacc1cc7dea5875731de50b13922ec00b3b8d31e5f26c19e937062e71b0fd65aec7ef49e174ffb76edaeb9e4db804e8a508a0dab33f6265de4384d726aa2ad52be1567b442d2e0ed0baef2d3250d8b60e72c3649aa165764b299c694b1cd27b80fbdf8fa25b64e18b6769099e3d3e4dbe952311bf75b92cb3d1fe2354387cd0b732a8e88a251662de9fb161c7742a0eede16c4335faec57b3c2933177f79b66cadc847fb8d5b01680c6ed80705d26a039f1af52675803ce97a8301ad319d3cb4b85ea4ea8220f8e6d2bf3547e3b6d9cc3c79db4a2be3af48c3ca0377f85e38c21807962a6137686f54710ea5baf71a345baec6737650fc26349bc967aa5cb4c046ec5286410c57753501e616684f99a31f5d883536952996335333fd23ed4c247239cb51e7ffb922c659f2939c8fa6f4934fdc99623bd3eaee99f4dcbc4d865b3b6b3f8286f0ec64e870a1f996eb5d69330928ed32de00921f68bed94f2e8d9dc05773b85f8c4cd1440d027e09cfa91d2b4a870f6ee089421de67da45b673b3c70e086ba378017a351e16098d2dda6f5728f8e4f6e1868ea055bc7788e9654bc03bbbcdb75e6c9005bfe861403d33e697c3c1d401b505f1ba7af879f02e2489114de9bd7a77304929c97b8a205ab92dca9ada5287f361133aff1cc4ddb49fe6af3f619cf7fb851a087beabb60916c64f574ffe95327ce3f60173b4a27e9ccbcf3654ec108198f5671e6c504347d9225e9d0e6298475d73c4521a1f8c8b54ecd5a2642ed1c35d348c035c49f851f98a55b756b6af511960c06fbf9ab51776339909727bf7cd7f899461b74313f75debaaa83b7bdded023672e6084b0848e26ff0bc4d2319d4f0eed92a365078c12b440963b7d92db90bf9ceb37ace8f558c446b5de704733d76d4ed88268f6c3685fc791dd46a05b74641d1a087f51eaac43e38de412926161e0e6cbf25b7c6e5971707d40d685b9d77bc58bc7a7e5ae22a07852b788729c32b22545a8fb5bc3d6d20df85df3ffa4bd7f6ab3812decf7362d0db8b60b4a2e9f62d80b46fd6c42138b7d2d5fd5d132f6a122ea88f110d7df5966a82a2fc6e1a9a1c56966c10466045bd395a0f5539b783029b02f3a261df32084b26f38712974fd76f7444fcd6fdafd66c74e43d84701b3280a26ba55e7e417ad8e512cf7ad568c25f37582f5de62539ac659437d525a0a6d547d404d22b4f4dac4dcae9579b6ca96a48db0b1abd489d4f89213cd4249ab042b6cde9776b0db7db098b3710286a83df75778350137972fb74624b242c509c9810f2d3302981c3dda1c1a9e676e52e1e72bfc9efbd0f84c7b48875e0a9f4387a8aaa0eec9ccce2c8db2b37a5b8826de88c2fb4fdf29947079a78f2661a3b92f25011ee6e0607d5db1d1a4d95661e678b689f4f44635d62df8aa82364bb2a879638f255c426d35b0ecd07a32a3a3fc94f6a489aa26448b61baedc1481de28661ddaedab0ee17f8a44b52f7c098c29e42785be645d885f2b8ff9ce666ad6a0a9ecc678075ba1d03306c1f80524fc699933adf76354c5f0d65ce827e54026706bd9b3825cbf85eb427f3e2305a224756eb46ad12109153181ad0e394ad12de3005c1767f2e1fa5e48b369f8455714df4181a8f8f6b6fb1bbc5653274ce648a54313e30583412a695e4eaeeefef038185bce2a1b2a90c3dd4d93e781e4ea1cc2afd4498880c118bcf104d1fb2c250e8263d15f1a82ce6248fe08aebe958c1245d5d82e58e6aba34aa70b384f051363d0f6eb6beae046461381351f18427c8bac041f0b682a02caa868e6f85409238e81d10de12fbe4b5f73941ed5416ccc3a52635978b15d0f90e63e0e25b2eac8a2d0acbcb7627abd13ade4080ca1d22d91661c64b35fc4cf158ae903c5a2bea91f61254878a8cd468025b947436c51d463b090b982f3f5a2af6127ca2c6e236a1d2e9848100bc96f6b2acd6cb7f9ec6c53eba286c26a8b657a3aa426440b1e7700b08e428729ebd5377a558dfcabc6ef8545ed161aa288fc25904b0b6f4306ad7ce51c65f4bfa0e31ee236b952026b8abfce9db79fcbfacc991163e4b8b9dda11fca65a7248997d3844a6bb6ffcc9a82b4d08f699ea527a14a9dc953dddeb2d9db33fbeb52955eb133c60eed85a20f4f764cd84e7955d46a73ab56baf5567efdd92ba6bd908a7a8fd067177170644e9e018ca9352aa641c787e07c78c137b62de0035d9b6ac8e0858ded0a24d9eb5a19d22dfb01fed8aeaa1512c6ed8e50b68332504bf7f58652dff5120b0fcda5dd7c6cbddd161f4ceec26675491fb9e17e6298f2b227f29b46155c9e55b600ad0a635921f0ec15e9b3b02d85fe1f00c05848d8478f97851aa67de15d48d9120fbef82abb1635c417245b2e330c4c79498b92eb471946d4fc2639a900f5b2e5f71d7890b1928d84da975a443d9098198ab5b10769462f6f307ac63d999a8cefa8bef60d4db39d3ae850e9751f1a4375554c54c41d1a1f48c96b0b4eb99935d21c7885b777012076efacd7fa7d69eb24640a37bb3a348faef5bb7462f2762397566a43fdf8df0fa1afbc6736bd46f75b19de822ff5b1b339b55a909b15d793499dd5432296a33efc186a5613842c47cefe1f1945dfbcf7e3deef292c5526391c624ab436adf674da17e8cfce18097b3083d170d0135dcb0fa0afe01c5e2f8e211032345288467258e7561389b6aee9271f645960d81073bcaf70ed72584406cc10d6411fe5f10a86be04f3e37a5f455b743da37130a49cd6347e861402a6536d04891302473044022036503a4f4c4a2e65483c0e762b561e6b267db0c498bfcf2e51a8cd8844015e88022031e9b44468cf9431f1fe74f60b755bb1e273ae462bc4024c097c01aeda7847bb012102ab849ed2b1e6f6c558d74e5c5d49d160c00afb6d7abd5b6c76024b3a85a6a5dd0083030007028daee23a0e51b165be98fc15150972f7f95a09ff050b9e95ba3c2f813293beb1e7fc051fa28f5a8a34cafc3d7e3b5086e3f256a70e1ddc6d9c6ef9c927d3a4e2b2b81f73f27137d0f77b72987d8d9771627abe02b47006b9eb95fe534ffa574798940f1ab429d1199fdacfff19ca08473685e70b10c7e643918d8c2924752dfd4d0b602300000000000000011cb6011e59d40a3b71d40f6080bc390592c5e07086dfb5d8008c41f35a9e51566fa565c66f41f3bfbb5afbbc67e6925b4310763f20f0315bccd30448c7fbdff7f2a467ae8081f7684d86e85ce77326a819d6e43a0b17701bc11e5b4a27e36b957348612f0dbc036358681031a1872e4c6993b9b8e1934d41b68e9123bfc788167fcfed34910378d1de93ec57b61183ea73643621bf9bff9291afa7b40851f4525c5721832d3f7d87c9f3674dd03f9a4e920978e6cb1d8388df138d15fd40cad1a506eb24a8ba41d49d93b61d4fc3afa9a0dfecca2dd882c0094154913c17b182abc5bd112eaed535ad9c210c51e9f82872214426bbfba664f71a887f2c643505bed39e31150c30722f8dcf070433472ffd67d965ac9df345f72ab5c513b155e0f0f52483c760ee5948d5801f481461e16f2d5f57fb52c0fd3a385ce4dbae07e0134729efe57c95bb89191d7fcd0dd4b59bab3c6d88425051dd92f32de5a7fccecc43847347a1cc73600b649ce4485591655bf5a59db89c8c128b792478dd10a0f3cefdfdfb5c2b9cd033cd9bcc2dc6e8155aecab3a35072b1231bf4e30178b963e0312f18e0dbc001ee8bf00f8e183c85cab7a263467af19dd2dbe1f4e96307c6d075c02c8c98a485078dba413558e0db49f5e678726bed1c841c2ceb8ffc8536c21cd152588f5497d5463e1982ebd6f85bfad3051313cfa57981510dba05674fcecc38d3b3a31c236edda9dacc4069ac931a48e9dd3df15ea0d2e72af07f4a1f535c626dc43bc742771a506aefbb6c24d1d713e88230aa67353d3d0762d0c246a11911aa1f41ea75440d2da9ef16391202c7fe9cc8c3b668907f23c131706e971c68403b923806b9f03615a9304531c8fd1fa070710713df0174f6fd95f9c8f368b1aa9d993d933f3d52eb9a01be330b9c631b496e20c72e0de91f2aff1e00a4a41c3fe0c82f6c6351e180b9911e21b3b86146ad7784bb06a378e216f931fef67515d991525b18fe9d04f9cff45559f846487694fab0d24333b0e583488709153a154df7399262dfcbe461609b7f21dffc99130f03ecbbdc6dd3417fce83791ee87464fab964f95500ef2b30e7cea2bbe6e3fdb193db3b203c99a232ac868372b81ac69c3f157bdd0173b999162000ba3e7b5ad4bee099867ed53d3efc714a82da98e608237fd0bb14daef5f65fae9196d8c09416256c8177d060935e4a52945879727d4accbf59fceed6f14ecaac73af2905e94d0922b9e799c9cf332ad5ec0016ec6e21bffde7a188b0a36718b3360a3c5af4f2c44470b0fa3fca45f1d5c95a3727a1573074ce1d73281d97f083b083ed5f6f0b4ce772b5acf9d29ccbddc863a6fd8d624ddb2cd15f86335ac77e7d898e4ce929e4000ba36b49f40f32da6d194dcb56961d94420d2a81c42d02aaccd70989c5906374600b632159dea91f8932449fe5669e8eb7d67456f66855d43da50edd9609ed1e9a76314d09437d118ee3cffbe2fbbca35541dd8091ca8884577706ca647006c4d0d28633567829f3c5bb11017d4339b59d67094b9890a88177cb796914f5dedccc4c668e723027ba987c3e2dd9a45769a5aa94c1643b2609ce8c22fcc223dc2e78bed6d30088747776641c17c0a289fe301db8c5c9c25b9cc2146c5db0e0c999244fa08a816ddf73f96bd685687ec78199bb1bc1e614d1e864c2d140251388b8904e4fb7f3c84a080ae53c120666f58f20e9d966e5c17cc93cc581b76559d5b3695a18318aa98c9e4509e4fe310e05cc8cbe999c70a937eede33cc2cf642b53be72cfb0f4eef91ddbe5b5311fa38200c796ca1dd68b416ab3628a9853b78d43f6316a122a141ede09869dc39f9fda053c643008b753711872b196d845711882be2349883b06b331584e029a8eac92bbb639c1ae9c1572b5bacfffcab6a21d4cfa28cddcf1d310fe5254b8217a20e6deb6b3c462d4de34999d4c17023e69d9497a81145c4e361788778f3ecc9fcc98a839c2bb6654b55316085948c55f2ce173886b065c3ac051e113ef0195e8c1aebd9e708621727e5b264024058721a30b8d035c44e23b1e0d3d9577e37f2707df30fa71bba820475146133b1008669dcaf62784026b49c4d0283973f512feecdd619f4143fb08c2b6ba4313937afb85a7e2bb3115430b79a62b8cb8a76a02c56623e28b2c94c1a1562fa5905c81a8f72af0051f33b4b99dfbe19eab47a24f2b0580d49a0e766bff356dfbfdd8e08e9ba24b48c7d84a7695a8d59fa2a683b417b7b844b2ca09466701ddc488df87fe97372a87957e9d6d8b1d693f1771f411b30c4bb60c33b8721bd53f0d74435c2ae2048be64dfad5d5a3de566e52b6334e34ca613fc187aa873c69b5bbe964566ab03e3047c92a8df13b2fc9dd5ca75fc1530c20a61ad739a6adb6d8c792f0001d0adb3a8065123fa3b41bbc7e52f7fd46196b5b7f50cddb461a892813b404bb6800805f1c647642e3e6faa9fb238c058fbd42bf7ba75d52b2fdc5184d0c2495a3512c636336900f26c6912bda1fb519f8107ca4017b07a7199f287fac6057acbf45f6cb315b6252bc469c589204ef2354ad586888c0e2781dce6fba6619dc37d506a62c6a93fa25ef4255842cb4af74fcc2be7db2fa991521467772f105336f76645b0eb7be20cf9b37244623bf93d0ffa9d1381978d4582fedee14c0d26dbac978c0098260ee25c07b59bf9970ae6eba0003c7defad6383822c83481ca876fc758e0ed8926cfeb6745dbd6d1f93ba05d6f7f5d70c308bfee6f2e2304528c10e9a15d51915f9d3e6c4516c15565f867667df3d77385b5cdeb47f78f7ca7d71199e281a52a1adfcfb511a74f9164bc78ee5d962458926a76fbf4ae8b8db6038fb902883646cbc263fec145d44f0aab3d96969e3332a6973ed303b916abd0178aca760890811b56b18b8803600dd54c23bd0ecce71691a3d7eec24aab9dfac3c47a2cd9601212f94ddc3f96b697eb240cb12de6ccd7dfee1542676da9d6cff66d09c7ee4b558c4e72dfc6a41bcc6a336b7d20de6d354b894b3a74df83352bc1fd657680420e5981a8a3f54e2ce839e98edaaf959c01e87187556aa3958a81743791b6b4c9b3669de92fcdc80b7416faac2a5f9cf2aaa8d25787e1051ffcef11899a50b596bcb799c454290f2763772fee06db9e7293f3df5ba90053017788c15fde945ec60ce87493083264ec8af8f595b3b610e75233011989179f10ea0180bf7ef40412c3c285ae45fe2b19a03366f6f1e346ad617fa9569cafb66a6fa08804d86893e07c3674a49c0426dba1d7e8c56b032245389b91e156645e14fa5997d12c8feb67966b48e45f0b8786ea3fe3541b92ef1492076eb8ee0be0d95a1138e3826674b56b06d317036934249c07ca2620063f1f39059f4a254b3c9c384e9be7d560ce52c39a6e61848bb0fd2ddaf1d9093c7a25d461cb348b4c7d6be5b428a2b703329ea735d10f710a253dfc20cd8e948aa034f2e0b73e47d8e9a957dc5c93a9eaafc3065f93b9076500cd9c057eb4d5ae0444175b15523b6dbd64ee5390b20b0c74db33de03ab60692e066a10e9f10b4db592c2628229dda16ca9f4a79197447392e317a0ec08d267b703625ffe07bf90cabd1976faa1a89aa8b8afc3984dd02454364c1f99d6be9b7df28e03db1fd6140ab7f64777845b0b701134d5184b304c7819e8d500fb846378c7c08b34c25881f664f4bdd56211207fd7607396d0d5a0dff2699b8bf94b30660943ad2615adbb73eab4feadd7807c45db6f4513fd5fbc4fcc054e72099eb5746ff55e472430ff48e22df642886b9f095aebbcb1aa1a9d3106eea8f23f2aab32a598c8d9cafead4bd1599c7da4f1353c5bc41c0a0100493413a7e8427ae5efdbde19496d5bde3881f0adcc16cafd4374a1a22b83ac119a4cb8834c49bd1b6235c87c95243ea8629b08d82645ad1594fd188a07e03d3988bcca00c0b2bd6d526172dbfb21b51a8b408b7059c5b58ffe39e8e8494711a40876bce375fc01fad457de52c2d4467411e38a2c17d0ac3b78906efe2caf5696344c8fb3a8fdd4ad0e65384435a42960000830300077c4ead37561142ead7e8edeb59d2ed16c73828160dc7791fc4884b8031a61602aa5357a7001552652f554ebcf1c8b787500c9ac1e4d5f92aedd99f9b5c3b436ad3315123677372b6e3f080c5aa82b70350ae7976d203f7bc33de1e9429de8f4ec53d16b98779d4b10a437d9d44a66e14e6c1fbefdee87b88bddd428b8231f449fd4d0b602300000000000000016aa500ef0290b474589300ac5bb39f6632f8249c57256ca87309da9c18f745ec2f4fba32d3c0954940d8f00aa6e7ba51e388ccf9a853a9bd9a9de665c679209274b86ff64902b25bf8f34817f983ae92636044bbda21a5c6913e94636d8b5d2ffd8c07c6f4124692dc46bfef54381b76a5566108c554a2688db21aec4e5642bbf97b00c5d05251e74eea4e1f96a9c07c896556dc518f4f61069e001ce372e1448384359998fb12a8c656cf492a5bb598037b0a2aba001e7f94c4db09dd0a0d604c87ab1b1aaca23c4aee9fca3d6a195d887b00bb652dfd02af707eaab2385d86ae97d5067c1eec087ba705f6a4a1bdac83b7c78dcd135b2b4b72dbb367b7ad4c46f0cd8e2a0213d163ef46928584f5f26793a9930f898e364b7af18f4470e86ff4e01a39375682d45138e3eb636551ea675f749bf8a2b23676b4e13c85fef80e06830825b39300203241be31cfbc5c714590e01853c350a91dfdd96acf0f8b94e01ebd6ec487106dab6e69f558b3c6534aef85e7587384e939d666dd2018e0fcec38b6d65528ff4211129f275eb0f228b71eb0cfb2010508f3dab390e66b4a9c6992288470e777a3bf47b0f9f337f8630b4548c3777088f8797497a003324f440cd419e2c5c192e3ab8dafcbe4335c832a950b12cfa211fb7e46760846a120960a562c5df449ced2cb7cc92501fc9a771acee337f1cd3645867177e0b15f1178e0fb697442d308ce600a467866ca9a8aa33135481819189287ecff489b477af7337ae475c8cc3eb6dddb42f00b4fef384d4040c2fa7dfe902410b075afd05e87f6ae76744b581985c33d99364f56f29fb898142f6671b68d779908f6b3c8de1e096375e54384d10d69641989c70cdd44f08ddddde43b8e0da38d1e418d371a5f5851c716de8aa4d48418055dc3deedfa0a94aee55c7b253c47417b3c7397fa8b6004173f19f481629e4b762818eaf0cd89f4fd098e2ceb89227349f030189a5e01ee47b86a952f4a50597a94881f0ba8d4d41d646ad1e55c696c1d2e2d6fb8dc35d27a9c4e58445fb8347300881a5fd3bb7b39a3110e7de09764dd6f43602a1685ca3412cfb87b72cb75eed61dd46095621d30d7a0a17e246a5562790c47dccc1040a2aa5037572f29fd5811708841a13adfd21e603931cb5c1c86f5701426f708132daa25065ed5c82bfcfc623e796807980626c3e62017fc46f9fdcae3f4988e29ffc92a56db9324c07dd06b43556dd2fbf552b3240032f966cdf7227cd9e0557d4b577583a8dc2b87b490f65bb375532858062552a2587e0a6decad9377723a68478f1c4ce89c820967bec05e565e2af9e073f7992634da1818ab27b3927d0bf6fbf3dffd476a9f60e2a48955ad271b4dbe1364a87b3bbdffdeb28a79bfb089041ee85d5758fbf2e33e95ed34e31ad83031c78c9711a12d25a7e180fabeb5bf71d6bbfe092cb55559391fbe0886f9880cf38e0aae451992d73ab0707ca14130bbd4009b84944d4a31f721aa821288c0cb515d776817927877bb9ae919389879b24b122692eda38283955d7e7e23798fc0140a64a6118fec05175960aade3211442127d66f3537d774d552819dcda4ac0a543e8ed5c2963dedbd6e523b0e07ed86e451c172dea331f25a2b69a0f4eca6e9af0766290b3243916e6cb14cd642815b82ab1a3c4ed3e8e0e6f0ea81b4339df432535c198802c014b39ced12d205a425402469ecb65377ae8ecf6e07d06a432eb085e49d0842b2b4130482bdbd80750adcde3b7ecfe0a038f443735bd3fd9faf860958b70c140aafc91bdb267528293fad46db2cf529a0313f76993e63daf4c30c8b7514c54bfeae128e45e4d07485758e8ad130e2021f4569ef085ad771e0eb39696694e4234c871fd8026b9140ea8c8b7afa75c165384ce1eca47b8619095e749905cb8755fec7f5d0cfff187fc3fe4cfb2f355fc7399c905d82a5f8fde8a351e896ac05c1e17bb1c8f18a9e715f46d64fc13ee46c623a40bdaf230d466f6ee051f5698b67aafb8eb5803021c720fc2b6270317c4ef72208d95be0eaa108ba00b94b852fd3c72550bb816c9aad9f19b9d94f06954907207edb5703d5f542c5a5a5b49547e424517d2c5f2971a712f463662ed0e1b842b8c3524420df54885e6fde39c0dddc700992ec7b3caca01d9b584086e0e1bd3c49bb196f35bf50bf422ab1f7e6137ab30c21dd9ffb12d1f357bf3799bffa0ff10e40191c1f46fe272a06aaa7e6c113ac5b12cbf81f5484e1320ad8a0fa9b353a9be9fcee4a15d7040367f3fa1956c453e124c564836921e5c2009e8d270fdae2a75679ef38363bdafb7518f9bc199f565eea2375f479c79d800bfd19ae4a6e871e8d36c265bbfd67e4c7e1be91db81dde7ddd80b6e979dad6058164085d09280c846470b3dd04337bd6c571b6e3a009140b2ee7eb426db59d400d502e8eae1789e3d501d300d40e0c2a1537f34e84f7ec466b7816494068e806ea04120dacc28c30ebee2155574655f045ef5c143f05787ffc8d3a38955fc8d1e98128bac1524af08c3f843371b7861341f9e601764efc890c1e16ff8d86b96ddde4e6de03b1d1508057597b76ef276a5533accbb9f36ef618b508a5a022e2e28bce9fc4633ca11c512a7eaee6dd3abd49c982d9ace67798ad2f56cef370da796c61f02c89dfbae87e8fe095cbc4355cc18127f0bcdf97bd9d36e2503d562d5cae29106eb2936b28f293c93972678537dbeb47b9ddd7335d9de68a09fe4db30b111c9bfce0543f990fd9eab5efbb54be32a9958c4d1c4df5678b6445304988baafce19abc43964b1fc215e76ac03d737883a722523ea43b6de6d13fc45637064186f32d7797be95fdecce91b0b93d5e68b2420d3d477296af0e328aa031d6258c14365bd8061b1d1b82a86e9c6f298689b83736ad29bfb6436f6561dc5953f5377545c47033c1e91b4caea84beb1777315d00326edd5e3cdd346be3dcad3013507c5aee3214ee9eb7ee2322e95866cba3f6d64eefc1e1026adc4ad117182a450ce0eaf19b51457bb0e7638aeb26ff39cfbb33a27d4bd8b0ed715d8431816eac381a86d2856c834d7b5c68849156377aea5e96e6189a2ef459fb746713f2c5c43199d3b9481576929b3f4a87a210063ac45be2cd47154b1dbcaae6d021f356c6156f527e74e01beebe4e36d4e340917f21ca8a0314468df0292009898b6e5650c8f7397d75ce9a41344b6b23698055da8187ab7260bb2d660dbaf24eee20b70bc62db90e3dac7eaed30bb5e1d9ce8b9c638c08c8b032ccd569c71ec0c3e4a4135c4e5d0b1cd0f75c8909e3718a3728048a39dc7bcf6d638dc1d4bb2041f2fef90c680d294273b52c59b8ac70c4b54e691b2e772e409d71ff5a513de477f68e161863ba3b3ac2eff04952b9383799141251a094daafad2544631907e31b1bd50bfc9b4c09d1b86ca13ff23bc3e185ae6990baca356d36a6e3645c85568fe1f8bafbf31409bbcee0a601f5cf9e45148863bc36238e48538715419981b8d09400f71eba893ebbb8ef99c888147d739201d5afb21de99e77f76928a55d32c3a2c98a377ca1dc02432fd45a63d8637fae1b658fded2e96f91e458e6273b7ffd3593de87c9bfaba60347e610924bcd90f44c488f1fb0e6962d2530294c354ffdb4f56b6f568285e676e7fcd30d7482934f1645dabaf276e3340f0be1b3b0329fc53f64b6240e036e10a54af437b4c453c8fe20b5c972ab09f63924a6263d2fd464a2cdb2a5404536a4eee536c4b4ef9c5a18f7156b6fca427a09a8229a170d4063353105e8b9a1014c998ee65405fbac8b20463fc2a55aaf025ae962f5e5140f7157259619ace3aef2b11f4ed654055f4fd3b9a8829e86b8e4f8454563b843db50ba6d7e7e127152642a93fd358e8e6a0402f639fc7b3f70c3b5de2130b81c1655db80501ab9cfb7c0a34ad67cbfe87abecc24433ed90956583db8a7b1178c5fd689df39a0faed32578cd67fa59e958451dcea04a2e2e7f6c7f9109c3fb31037228a51825f02275fbe34a7c92af7e8ddf0de890b1a3290158d0e50a7ed4d672aba2e5a80830300079d7342edc8e4f367c464c673082612688fa916f3d71496950e595ff7c2ff54c04478a8cc525c6d9137c9c05a38253793b3bb1c78b1a8d7f4d08cb8bfeca7f60a266157782ec8b03327c180ebd90d1def2f7ffe88d4ccd008ef3f78c3e32fa33cce6ba6531ec884059d611d7f1366f113ccd90f2f3603f691531c4b4fe90aa930fd4d0b60230000000000000001639f0085e8000b3aefa7f690e47785875d9c8e3f6b18b022ede2c86338401206e88bcde9eaa7524a7663cb52cd50f2f81a00c852d1073e5489be1688183a3309a22b9c1ab0c8dfe1c3b754ad8d0a261ce2296c56eba270902a6e60abdd21dce7486a211e76cca1812b2e9347f95e1dbeaae4d56eda8264e4c8b24802e73e43f17e9ad45f0e3156384659da11b2ec87b523df35e1079e3399ab0ff33e04d0008ac8c019d2eb678b2e219680120a1e9c0b01017f12ce88597abe4bb9719ae9fb3dfd06f03f010cb540b022353f209ca8046e76d506cdfa7c99f01a05ff233a139bc34c45575c193da15ca683a5fe8e77fadafbaf6e5bf07100e426b1e142e42f9fd8f4e5344d052311d85ba0eef2f2df69e74f478487a25b894b017c47b421a917020fc9013c458aefd2b7bfd6d5f5e94a3403a82d90e3634e28a5c1610dd038f0b60ed9f237d7dbc41cb5d4c580b9f3e8997e04c29c57a52851d1ceb45f38fcedd08d219cfa165e1e74d96a39109d1cddc29183bc8c5c9ed7678e42408d8c72ffbb217534d28833e84277a1a6943721bf8629279050fc1a9aea75a730191906b0c05691b7d8dd9f87763d5df7606e9992742487e28e67f58765b8fa0ef4034bf7f8653934604c5650851f6bb10b2b379146ed6e83d1e020d4b2d7f670c16727f5bd091619cbd90ddf0669bc6ea01a3770fddb8f7f218a518defb531e110fcb5d28599c6816bfd7e261b32f7792a08f5ee6a4f308f95092ae55a3a1bd96872a5b06f9bd198e449c8cb5fe6d88211d512ff9d491a8fbd3d6aafaedebcf6d2c9d945c566347380342d48ac26afad49c0eadb3dbaecc1dd61ee13ba42059b40367fbeed476460f9686ce92089c734e648df3bc13a6bf58c6e062f244575bee5821c42b17b68c9bd3c1f74012cbbb8f702530fc1facafd092458dfaa760781f2761943266fb7eed5318d3dc48a6384e2ecab5290244b8b09f14147f734839fc352da11f0b38d619353e948485b4fd9112a1e13e09c6b3316a89280e803d1e3ec0787b8d16c8ff24a9267ee9ac73e8471b43ee01e4b848b608393ff8a3d6a352d8b5b56858b13ae07a83121046873b0adc50379464f0ae8b02f297282920222bb2b56fb0be1d2bbf44d2de42b8d0200f1add66d07ee672ab821dd1b274bf3b645d2b06e1043d5ef44237188278a76cce834729f9d66cc5ba8859735063d4c2f0bdfc569a2c90c0aea8a5c00f5c3df13a8cbae947b2acc5357209e3adc34c04cef2c11ad9f869289a31478c23a4737468d4bee26e5b45cca9d6a454fce837c75c3834082d657480df08143da06395a0180aa73d5a8f6c217aac5ca22b9b1a7a3e64c5a54e8807c41d6f0049728f8a86ce51be606f1da6dd777241e8474515ce6529ff755c7c7d7fa0054d1d4c3492dcac5da8dcb1150d7379734601c3d8d28863bc3d50cf64699dabd264dfe036f7200b348bb1cecb55ef0916b0c17373ff1205e2fa7bbcfee219fe27e4000c613b804dfff10963e48b0b27d9a5c7a6897ee86c4c77cd417ac1ca98415e647feae14115cacb9862a6693bb2c3b31b026b73950308bfb986b8e5f7e860d027df6c9fce9d7c39fc23cac49e4c8d3e07ad2b67a7e965a2b7acec4f771b8b46cd49a7971f6588958ad576afcf34420bfc7bf2827fb130f0d0ae3804f0768fac870cda7cb902638f4fd782f3161537b9e33b9845812ab2a8a0b1abd5daa7aaa218c32520f82a31e9ce6f645642a7309bfaac28a945ddda6fef5484412bbfd07a87f813c5c4ee344cddd81de0b7bfcd1d8e7c2762ad6f415d61dd51e7abe6b17f8138abbb743f5c3029d2b793a14391758a002456bfed2cd68a47822bd16ff7473a82440f4308bccdeacee047f1a429ec7ea8c0a237b35c737d8ed4526f726040ff8fa84d4439095061686500ff9559f16469fd1a66de17c9b8481d0b3db6a9811045b5e5757fcf9e128f03449e6018ee11e6c5a41a9cceeecd1ab9d8a81b34db02aec7f4831804dc1bd0b13d0cf62f2134dff3755ac741e9d789a6f38469fcd00967104e31a1aeceeaf49d63909623b7160fdc4bafcbe1536bf6f43e17b6ffee3dcc864a8c1ef7b437ed39fe2e54253c26d236da82837937042f36f68ef7bf0970cbeac3689a2ff2b2acc57f6d76d634173ba4a636b9723c5ee3852ac4f153e752b0aa17ee71d1365e45b1177f77647a02b1d6f92e386899c64ca1de46d0b34a8fbe1ac3ad8ee75c3dafeb3374f13253eb2a5fb7649a0e9ee3af265301043f22658529437c59e3216e8999ee1298597c26b06360e86b36a28d6fd07b6f837e610530e78c51e413d949c1b8bf687900d51b82fc4d386ec3a9c73aa6276954ac8d7f7273b5a945f7f59cd6ade841d5d68f7743e95ce1a5e351f46bf09b5fcd08409e315888478bd3c4788611185dc81c867ac679f006adc4b060c4a9b17d0af66068ca9a75dcc898ab3655d1a90c9fb92ad54823a7c4c15baa601ca6fc89811b1c619dc61a2a574ce9d45dac75af2600dd042f0679b54effc4c3796b59741ca0765d6d3edd62c1eca4dcc969cf4231273814b8beee07b0664983fe8b9c31b8efaeeefe04b03d7ed608c69cd4b0ef53556bc273f9dc7cd99421c2bca9b8e8a337dcc0aff7a8bb73a29ba2fc77f31476e42b08274c0f37a149a85cff9a49adc3d889812d60c7b8620a929aded40c6a3a0823e23904838de49cecae28088da02c3eb15a020d418f79c5f021484362accefe0dd992969bf4f378a7a512bfef8aee10adbcf3a5d91564a1ef4e0c3161ff2c75ec42a88635b1846ee36fde89744b3182b63794a7a19694605b16f8c5b048015bd9728f46ba9987637f0e8e9a68cac418a35fa9beb6a4e17c30dbed9f6b2453b3384dd2e008ed54e661e2181946b2d7a0bcee5bd9c447465e397fe7fc3373a9e8e0ff735f1eed5130f3c0211701223214e26559f05fd638fec803fa8658cac11fc5fba6ce4710e5281fd2df7f7179774744ed22ffde7d9b966343d34f927a85d885755a6d2676e76a7556401af667654b27481522c9509f76ecc474909c16cf90dff9c40f800461b616e8d1eda757a719a6afd9b54c3e2f8008118e7df8de8b384122a22e0479e24545305b640cd4421f45731fb31809624e123102866b2f93dbfb72e0a80e944ba1bd09af52e4cdf3e49c7e2d8b99a701b8616ec9d89735e928f61de7405da07707eadf4aa82fd5331dbe82c54e99aa502b71dfa578dd9c7b84ce06f80721c349c822654ea5e5ce62bddef81c0844ea2b33eedb1ae564f5144e7abebb2ff914325f251d9eb83ab88194769094c950f205bbc6018c4e9c608d43874abc6b6e36c75e309ecbb0bcc7a904d0d05726a4bbecfb9b3da6518cb2b40bb5f18888812f1f0c3158ae634298c3376f010c1e0bad7dd242328e7660105c633a1e6e5b0aedc4a3630d40b4c55a12ca2cad0c4fbe4be8f93af7575f2cf104a5f7d2d5996af90c7c643dfd8fa36b4fe497096c5eea26e16ff6364b364fc78bfd86726c56717ccbfaaeeba44e6e8527b92d73d9a132c507c13c73bb93335dec31a09d4d74d652c340ffd80860572a48f1735334b3672fc5ad30a6a05e1411cadc442bc8aa79ec48f1c6a7b64c4fb1d519df7819b1e753856baa70d806da054b4cee7369320621405b0a9613cf3d6448210b6388b696e36cd7603b9c57256d22578c88d4ceb07f580c195bcf93791ba3ef41f8b2821f49589c5db205563f87c27331b1b759f1120fd2bc2d035a471e2cdf49cabe20fa55b9343e3a6548d2b552d3c70d02bb596abebe23022d33acf94f03c38cdc4e00b9c59bdbacc92daaf8c14497d84cc6339cf46768e4133c7db68427df2b6bff030854b650f180261f0011b90ce0749ae32d485f978141a5f247b13a34e78f77365fba462db7ac7cf08b9866110c92f4df64779fbd42c12dd56506098a092be6507d53239f25547585c66c2871b77b8181f00c4d0dff8d98ddccc39e74038800516cf707aca21c1bdb1c0586b986911a519ce33cf0e2b75276d65b447c2c3c7fb00a13f9915d22616cf6d812dc98744d536f8dc9d9c650e5b82";
      DumpDecodeTxData(blindTxStr2);
    }

    [Fact]
    public void TestAddPubkeySign()
    {
      string txHex = "0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000";
      ConfidentialTransaction tx = new ConfidentialTransaction(txHex);

      long satoshiValue = 13000000000000;
      ConfidentialValue value = new ConfidentialValue(satoshiValue);
      Txid txid = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      uint vout = 0;
      Pubkey pubkey = new Pubkey("03f942716865bb9b62678d99aa34de4632249d066d99de2b5a2e542e54908450d6");
      Privkey privkey = new Privkey("cU4KjNUT7GjHm7CkjRjG46SzLrXHXoH3ekXmqa2jTCFPMkQ64sw1");
      CfdHashType hashType = CfdHashType.P2wpkh;
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);

      ByteData sighash = tx.GetSignatureHash(txid, vout, hashType, pubkey, value, sighashType);
      Assert.Equal("c90939ef311f105806b401bcfa494921b8df297195fc125ebbd91a018c4066b9", sighash.ToHexString());

      SignParameter signature = privkey.CalculateEcSignature(sighash);
      Assert.Equal("0268633a57723c6612ef217c49bdf804c632a14be2967c76afec4fd5781ad4c2131f358b2381a039c8c502959c64fbfeccf287be7dae710b4446968553aefbea", signature.ToHexString());
      signature.SetDerEncode(sighashType);

      tx.AddPubkeySign(new OutPoint(txid, vout), hashType, pubkey, signature);
      string expTxHex = "0200000001020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac0000000000000247304402200268633a57723c6612ef217c49bdf804c632a14be2967c76afec4fd5781ad4c20220131f358b2381a039c8c502959c64fbfeccf287be7dae710b4446968553aefbea012103f942716865bb9b62678d99aa34de4632249d066d99de2b5a2e542e54908450d600000000000000000000000000";
      Assert.Equal(expTxHex, tx.ToHexString());

      Address addr = new Address(pubkey, CfdAddressType.P2wpkh, CfdNetworkType.ElementsRegtest);
      bool isVerifySign = tx.VerifySign(new OutPoint(txid, vout), addr, addr.GetAddressType(), value);
      Assert.True(isVerifySign);
    }

    [Fact]
    public void AddSignWithPrivkeySimpleTest()
    {
      string txHex = "0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000";
      ConfidentialTransaction tx = new ConfidentialTransaction(txHex);
      long satoshiValue = 13000000000000;
      ConfidentialValue value = new ConfidentialValue(satoshiValue);
      Txid txid = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      uint vout = 0;
      Pubkey pubkey = new Pubkey("03f942716865bb9b62678d99aa34de4632249d066d99de2b5a2e542e54908450d6");
      Privkey privkey = new Privkey("cU4KjNUT7GjHm7CkjRjG46SzLrXHXoH3ekXmqa2jTCFPMkQ64sw1");
      CfdHashType hashType = CfdHashType.P2wpkh;
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      tx.AddSignWithPrivkeySimple(new OutPoint(txid, vout), hashType, privkey, value, sighashType);
      string expTxHex = "0200000001020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac0000000000000247304402200268633a57723c6612ef217c49bdf804c632a14be2967c76afec4fd5781ad4c20220131f358b2381a039c8c502959c64fbfeccf287be7dae710b4446968553aefbea012103f942716865bb9b62678d99aa34de4632249d066d99de2b5a2e542e54908450d600000000000000000000000000";
      Assert.Equal(expTxHex, tx.ToHexString());

      Address addr = new Address(pubkey, CfdAddressType.P2wpkh, CfdNetworkType.ElementsRegtest);
      bool isVerifySign = tx.VerifySign(txid, vout, addr, addr.GetAddressType(), value);
      Assert.True(isVerifySign);
    }

    [Fact]
    public void AddMultisigSignTest()
    {
      string txHex = "0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000";
      ConfidentialTransaction tx = new ConfidentialTransaction(txHex);

      Pubkey pubkey1 = new Pubkey("02715ed9a5f16153c5216a6751b7d84eba32076f0b607550a58b209077ab7c30ad");
      Privkey privkey1 = new Privkey("cRVLMWHogUo51WECRykTbeLNbm5c57iEpSegjdxco3oef6o5dbFi");
      Pubkey pubkey2 = new Pubkey("02bfd7daa5d113fcbd8c2f374ae58cbb89cbed9570e898f1af5ff989457e2d4d71");
      Privkey privkey2 = new Privkey("cQUTZ8VbWNYBEtrB7xwe41kqiKMQPRZshTvBHmkoJGaUfmS5pxzR");
      Pubkey[] pubkeyList = { pubkey2, pubkey1 };

      long satoshiValue = 13000000000000;
      ConfidentialValue value = new ConfidentialValue(satoshiValue);
      Txid txid = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      uint vout = 0;

      Script redeemScript = Script.CreateMultisigScript(2, pubkeyList);

      CfdHashType hashType = CfdHashType.P2wsh;
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(txid, vout, hashType, redeemScript, value, sighashType);
      Assert.Equal("d17f091203341a0d1f0101c6d010a40ce0f3cef8a09b2b605b77bb6cfc23359f", sighash.ToHexString());

      SignParameter signature1 = privkey1.CalculateEcSignature(sighash);
      Assert.Equal("2ce4acde192e4109832d46970b510158d42fc156c92afff137157ebfc2a03e2a0b7dfd3a92770d79d29b3c55fb6325b22bce0e1362de74b2dac80d9689b5a89b", signature1.ToHexString());
      signature1.SetDerEncode(sighashType);
      signature1.SetRelatedPubkey(pubkey1);

      SignParameter signature2 = privkey2.CalculateEcSignature(sighash);
      Assert.Equal("795dbf165d3197fe27e2b73d57cacfb8d742029c972b109040c7785aee4e75ea65f7a985efe82eba1d0e0cafd7cf711bb8c65485bddc4e495315dd92bd7e4a79", signature2.ToHexString());
      signature2.SetDerEncode(sighashType);
      signature2.SetRelatedPubkey(pubkey2);

      SignParameter[] signList = { signature1, signature2 };
      tx.AddMultisigSign(new OutPoint(txid, vout), hashType, signList, redeemScript);

      string expTxHex = "0200000001020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000000004004730440220795dbf165d3197fe27e2b73d57cacfb8d742029c972b109040c7785aee4e75ea022065f7a985efe82eba1d0e0cafd7cf711bb8c65485bddc4e495315dd92bd7e4a790147304402202ce4acde192e4109832d46970b510158d42fc156c92afff137157ebfc2a03e2a02200b7dfd3a92770d79d29b3c55fb6325b22bce0e1362de74b2dac80d9689b5a89b0147522102bfd7daa5d113fcbd8c2f374ae58cbb89cbed9570e898f1af5ff989457e2d4d712102715ed9a5f16153c5216a6751b7d84eba32076f0b607550a58b209077ab7c30ad52ae00000000000000000000000000";
      Assert.Equal(expTxHex, tx.ToHexString());

      Address addr = new Address(redeemScript, CfdAddressType.P2wsh, CfdNetworkType.ElementsRegtest);
      bool isVerifySign = tx.VerifySign(txid, vout, addr, addr.GetAddressType(), value);
      Assert.True(isVerifySign);
    }

    [Fact]
    public void CreateScriptSigPkhTest()
    {
      string blindTxStr1 = "020000000104a76c51c1a0ad26df3eae8e8391a9da3cecea24edabcda10436cd3e550cd2f9e20100000000fdffffff37ae0d4fdb2e319449897e6defb3826632a5eaaab84ac5b4539979c4b5e042ea0100000017160014077042bf149434cda92751a2ca017eb35b1ae29cfdffffff08c67897576709ee329620a9d63f0e6f662ddaecceac73e5a8807f9c9fcb88430100000017160014a65e7f101ff5f4f4106603b884d2c631717f61d8fdffffff8daed986c59703a36742a38b9c379afa2489532a876da63e9cd1fe61577c0a740000000000fdffffff030a151e78448b0efec64069c8d11ac9d494635c81c68be60dd8dc5d00b61e66937009f2c7d4ec124a14c9b6e4a2311b4081944d7c493da119117fff8118f09b5c20c703bc52778cf3328c1bccc647e890df68294bcd38632d39a10e53d0757453a6c1b81976a914027aa64642c68f9e3d2d06c3141484ef1704fa8c88ac0b8a04b307c5bb5d383137600d5815347f6fb3f712672ff9c104af1eed3f7b2c9909e02b1ef9d285f8bd1b310f30facc9b9b267f18692ebe675fba02b2eda9febd67025eb7264392705673c88a2dea01a2325354ebb60a5cf045af41351077bb0a0e4917a914df01567680d27699b7d84dafa2bdabb5ada034d5870125b251070e29ca19043cf33ccd7324e2ddab03ecc4ae0b5e77c4fc0e5cf6c95a01000000000000b3740000dd070000000000000000000000000000000000008304000b13f3245f12994306c0732649e27622e0460517da75dae7bf3c3976bb7ab9cbed4719e56eb21b7fb65c2b938b61a6a53f368195b128c3178feec7ddaff3a9cc64e59fe0bfad6cb9506211c945fbba0be14a5457dab662094539b82d80ff2f50a492abaa7fe406810879833f2452651e0f6b51faaa5a0e507293330bfa16ea6283fd4d0b60230000000000000001c2b101c36161ea32674a76fc474e5a45d8bf234dc0dadaf96837b9b3117a63c06128b58861827accd9e5f4f64e61b1efdbc4b7757f7baa8d7072757e4bb65c6045cd70ff8b1ab4b9f1e82283dae8c61651351f56695a752f9f5bcc0ea785fdfce0d49f38c541912bcd55df51dc09cc161b35a0b04e0df400b122d4d4a393dbde8933ea7cea3c5be1caff45f3a760cd51315101d27456da3fb01acbf1af4a53a668ea7e5ebfe456ceef332b3878a21acd717a82557d2d353f39789893403987dadec79a934fd4d62418bcf5af3c9fb4250f9ae1456de1b110f25e9c713fc26470bd0acf2095799e3a73d91e4e187b30731d062ac917f6261df0c58e579a075019a8cb1e88f95fe509456510009ef5e0f9502ac426c264806f5bf1374ba826677b8b1b06fd00c060cf130bf1943a0a85d7b9993d937a1321aed730c13cc9a22316e57a526b0f5ec2e4f1355ef1d4820afa35d9d2c441fe128da31ca8f18b27c4e6fc5d73bf68165aaa4ad066aefee3cd342b85ec8b806433e58b49f300340b140bb042c63959f52a0b68155318f38065521c1564bbc80306fdbabd70c0805cebdd50e3f78ff5d55b53be8443bac0013ea25c0d1e5b9cf4edeaee946a266424c5ce45099b5f14ec0ec9ae9d1fafbdbdfcb608e3b3d6bc26ceb1e32d9535817f0ef1e90dbbd6013e94686fd846d50fc9b9312bd3016d81c0513c4c09078b3199f799dc1791f4e02e635de7295ed0e8bcf9971fa0cdb7067f71fe6226e6c5d6c9911fc5865a74a05d9f596c29e9ca2be83663f0ff1e41a5ec8ddc01c667026eb778c6b1f0d4f3d2dc0c937841238d8ded60a3f4a012a859b66d40bcd260acc6068bad56c8c3834ba4d1b4f778213d2d8365fa068b86613e3da1105123efc4b5ba934f532f5b6cb4af2e4828a1a560d3cd720eeef41f0fda6c466a6993c65ab9223cd3f0a09d99a6d7bac4974a586ea0bad4bc646c456e7df4ef84dda95943784cef2710653a9bdeacedbb7a541db754ea6a15ee9d38eac49a1c9e1ad1f38f4ebaa782da4b1e6fde7b66cd81ab60c002080dea754dabb296d4408e926a45f7f1301d551432d8c8e1294fdd307d9ee2a77e395e3684bae5a9b5afe76a389d67ee0913ef8832967955c0434fef85270411f5c60379512231c428bc03cdec09f0334659ebae151e2f7449f538b37c8c758fed826ab10da63a136938f12e8ad9c3cb76489c0610b8c1908820cc8dda284dda57e1e4c1e4f16c904aae0fc491019a273037d036ee0aa88cb43b4682fa8026933200ead8e0ad1792b2a9f2e177a58321f97ca0db36b6e1ce2661608b8d0bb654d43f676678ab2994436cac1f4c593abf0f11bc5dd737e2d645a6e92ecf7597b139268e5c565e24e7e0987358ae46900df03bc971b7a856181d551c149d6750459c56f1905799ef0552e0148418898839f5ed3167ec2fc1e0184514cd243c830e9092c6cb66a51f45d6a12b363adbc669b7aadf4d0b5cc2bf9922d6c1364dfdaca171fb6c3fd001fd1b053f42a24e9776a75daba64701a7248da6c7d4719c069364f86cb2f364b3f4e8fab3452545a24764bad1db29bc271bb8869c4dbf031f3786a12339ee562451c2b66d1af7ddbd7d65f6d06fafa06422be1312d144c1bc649e98fe665d0e5842cca596ea3a14a85d230da00e1f2880dfcd482535a617b4eb9d762d6771a90420bea9fbeb211e8794bbc77d353287a40c0b17ad86964a96318831fc815d92c5e6dd89968931f7487c86415cc1d600aa27bd164e3c4d0a15c01a7377d5a85ca3262f4462b945414bb9e4714f28fa6c8b0e57db2cf29b012a1730e976edb2f27ac24386bae63cd6022425fd8c2453aa6cd98e401ed8cdd6e86836d917c2bdd3e66b7c71c8a1628587b46c710df92fe74e16711179bc8eb19bae77cde2dc680fb2d862b74bba838621de9573c0b9875578441eb65e266b82db937e092e5260a42ac07659f7100aae03ee02977743a6249c685232851768a022fe95b7f35fa080d81f7de31a456d1a01d252b4e8860b48cfda15080616ce9f628c9ca670225102315ce92da545ddabce9943f257ff61f6ffc220840f1b29a64a8eada361386cdebc03d146f7682149e7bfcb3338430da40226714ff35a31aff60bdac47a0816806a1de48eeaaffbab88e82cc643d4ceb527f5c94bce1659bb56301460bb718f3ead9323345a7ecec1b8f0fd6297ef7d3850430fa8b90e75d45db87a265d3457fbefd6951c7d628496f06e75373c96f2eb97809b10c0143809cc28174b0a949f4dc8df6e6eb22896f26e5331e6ab66d7e0bc0347da39f0e7a3bd2c3369538bdea29a7d76461832dee3fe593bd598cb51a8b1966b135e3a84b8f4ec9a174b2be7cd0f6315d4ca03a958d2e88c1be4e19fc517111cd2a90c4fffc76583e6c361f5fabac6f0aac54cccaa02015e5cdb7f99b84b8026f5667c5f9b57a4f57cad899b7be8ce66b817a0483091378a8db14579cf1e9568ce2c4af3ed872193f449fe05bac79a7ea92743ffa72cf212f8da9679f733c89fee4b78dfe66a982c79cdd16e97b3a316f3191542a022a517f49566edb5bdbafe8a63fd844d5f6763e42e46bdc2c7de22705b8644253397c17480dc0c9484909ae99f3c5038ace18bddce9be9cae3b86312af4ec937a5e05e1a119a2dcc57391d291b25ffe3dff7da88451e5b9270ce7bac6086197a8a2796bf953b3b0fe70b1a53035ee560d302a9ab55f2926e7f888086367c9cf469d4b0a276525e567f73a7b55b48f0ddb36e1a7c1b97d2ee008e61c2bc635e6db9585b01dca600d81b044bb1af230c9ed6e658eb8c3c4b35e1d9773295ec57eec12ebf7da3135beb7e7de0ca086f254770ee0df3341271c6df96b10b3569fc479fbfe1cce62504612d38e30fc7b70fb22aab65ab178fbb095e04767a0198e3dcb6dd6aa04e538453d9e933e8c56bb16078ed85490963fb99ec7279b5d46618d2e596c5c057367d6ef3dfde0f6e61d9d0c9ec076c6ee1fe588463cb596921b5f130b278c1ba13ebec24cda89b4d8b25ac475c34de025b4a04a319367a5745a9963095a9f560e57b61c442a3b02c46302d2f8464966b4fdb28caa592da61695f292f76f69c3120ddd9308f6cc13c2008eee3ea874914cb1a8357949829fd8afd68ca1f854393d6aeb26bfc130d1e6c62d45b0ef5eb6700b7c0f5aa066ab985a825b65212affcc901278fe79f3b6cb45015f0010e92fb190758a64729a8b46f299564eb59a2ddb03057d97843723318f6ddf0f7e9254235dc49318dac7c028df6f680491ea1ab28a820a95d35f1937a9f035bb6cfb56050989900ef809fd55961d10832c2abd79e565d80131908a84d53f8a51752ed00abbb9ae41e6ffecc93fabc19373effa546cdf715c26456238f06651bdf4ffda9c0d77ada10354030480301f09ec383db5bca36deb002c482202e574e63cb5156d09bf511fae0a72125bb61d3efd2197f84e029558a3dd1cde325df7f639d4b030373807ca64fb49100ae21c632fca67ce4821132bd5c488517b5ef45a76013dd41d4449fd585bb128ee9eef39ceef57854e52a9cd61fd5122d8a249454e4f6cae75f37f616b20b5b340c789e5599541af4778d85efaa849e25b5224029179026245d5ced747e6871fa5a200898fedbae2954c452e267ab2a001b3ebd5920390e79687af5c012f53479c37a57897e578cf0e1fc3b4f038a84d72b3ca5363257a48919d74c606b1ed4361c5a1f7610ad70b5ee40f248a6c6822880e8ed4901b6ec0bca46aff4623273d256078701af941f4925df504961632c7e427c0dfd8607c5d1acc55c2f30ead6ea485512965f2a1d6015e6c4cf71453df81f70ced854eb0b23839223c5e18d1b63bbef7d79bc49a720e4fc2fe06e459d81340dc36f186c353295eb548838bfdd22876e34e2ea3170bb09104b77640e89dbd1048e6685df2d46bbc8ef682157d91f518d6ea083c649dbfeb23956164407b325424850ee2c0a98e6d2c3f19a1aeb4a5e6c38cf6792022ea6d5116671a1dee15006f2bbcb45b2f335e803c04d04fe089aae6c2151d30cd8e7d58304000ec0e7f42f42e6df0b144f11c0d5a3f6ad7cf84936e9d5a38fa266c3f7a742d4011f95f54553243fb35972ec66b650d781dad6ff672cbd8a935bd477b838595380ffe1586bf1bcee43e0ff18d252263250e952d39b0f215ebb69cf0c23a6b1a1d2b686c01c5d79fc5b486a88afbe466b4d4bc8e559b0b1f6432c98d8e0844cafecfd4d0b6023000000000000000111cb01acd74ab4034d001feb7cd326bb4f078d678ff1f7d281d3b1262864f97e6dbd42fcc90e63b1bfeacb96f2ee0b5889a2c1550467af8f940791eb2af806628252ff52a77cb2a48cc7b8ecfb0cf6db934e90d812bb4d51c73f84b32a7e9c481f89d56f24a2e7d36b2f6932f2ffc8eea9f9798f25f6f03e51729f4164171ce6475a9d0b9da3fa04922745ceaae12327bd7e40ff22f17d01dc4a6148674c4fcff4a3974fc48b8fb924f0545392d1b329301fba3a5e71fb541e8be0a96ad79bd2cd586bc43fecbc705e61240b58a21dbe0d2e0f0dab734ac0be7655e08917fa62fec78cda4cc83c50e8ae5089f94e14bf70f6f1698c23e914003f3633ef257cfe997327cce8bdf7f2adff2070785e651334e5a0ca97d5e8528e0b47e88570e152a18026c99658cc3350174bc83d887192fdf6221c938f25d09d64bddf98f4313c4f67909e9aeef61a633e2c708a1dc5f995882a780eb93f9db0bd97470b77fb61b0263abdc6232b654da092bbfe22f486b90a7c5898f83ec595a003a3ea66c4288cc6e529262d6226675a7009dd1bd762491a509225651c356b60ffdd1f36a6cc7c24ee719b941f02f2a88406e93d0d0b81c75ba33ce4821f7009c21274af21a5f61fd105b25cfa857df61b2cdd29ce0eeeec86faa52ead261dd3b1326d52d96945e8b15974d2c4be711813f7130234dafbb7715cc4dd8ed69d3c4ca62f0bf448bbf8a32e52df9334a094bfa9f577eab017de57bbbe028fc08014a2f3ee55d9df9cadff760dcab6d06257c814c031bb11c836a8363f261982a7ce73392f82fcb6bdd5968835f657831e7dd1b0252a2112a45c403bf2975bebabfaccc365eb09bb25abfc3b80797cb2058268b9899ba642570d17ce05b8d9a304eaeb4a61b6fec6dc189f54f3d337df46918a76bc7f1ce9846ede724eaef34d4f02b6da0a293aa3a60b306b7c937bc2fd11f58d08440287b0c7793649ecb23b69181af9912f83afb283af17f48f05c79fad79b2aafadb64309a29ee3d08521f5b08d075eca726e2731c6cf85209c9341ee65878b7f86d85503b96c115f8cd603dfebfd24d9d6fb1df2cd92a32bfbb5eb926c280f902cb5bff0ca683a64a5b215fc2f377995363f42080bf96d953153a640762f91a4e68fff1c62c382eba70301dea1823345c9b5e52c5e2488881a76f5aa29f30a24fbc720ef24e1811640e44d0e92fafffde3bb7c3c8c8e813569d89604afbd7e09bd99418dbe31457a33d528924fa51291b62be9e8bb4fea91578296d1ffda0eaf0549347010e64181271b52036613153b61b8e83ea1aeff860ecf4af953639ff008a92a8c2e15d846ac75da55c1732277273ba070235f40d60554181f17b17b05158260e62b0f3cb7a11fb92e20dea7eda89ded7a69b16a359cfe9b3f334d369def50b1700d250bf003cfe23af5a2ee44b7ec2109650c1d88b33edc4f9404cdec8916f84366b36a79e05b4083f201ac59e688dd1ba03e2ea48fd6a42f8ae524a48b6ebb4b4326c667ec75f79e88af078ca667caacc06af34ca42eacabf441dff6fd80a8d6daa89e184d80e9516df2544a539723c9ed18cbaecbc94557e3a31b6ca5b0ecd15d1839c0f193405f2a461e7a520f414d056864f0ff6591d6f0bac63746f9887b5cad36b208160fe008d4eae56384019f92de1ecc2ca53be8055c900ad94521440bd99646f365ef944b6caf92e2614454ae032cb52e59bdc4aff6710a978a4bb6c3ff4eaa478f6a40c96fdb26268cbb422b6ee7d6fda354de6ca7cde33456762a04db65389ff0c10f2852158b543abd9c26e0f1bd0ae04f7e40452f9b4d5d5aab0947856ba73202a5ac577b5a9a329299849b4304436289f271b8bb8f7da91f9c9d4d4fb2d707c3b0ccbf1820c4723ad5a6a1e4d79dbd62ab086ef409c3fc8b5e11bc7e8aa187b7d576d50fbfb755f1dbb8fa7658d52da16a8a7575747a841b52ad6d89c53bfc8afaf0f4d3041c774e05b40bbbaa4fccffa93054efa769d1566313c7f840a0ef9b91c45ac79f1b5e12c866157e4203f8860af2fcb26d2ff665d8e4e2baec6d7c05d5ba45cbcdb0c270b7e0b992e54ceeed41397a11d8478ced4f98ed161e335c4133372d1e4bcbcfa87fdb3fb0930cd99e4af6ac732ea164a328ee5445c374b59e9dfcd665399ed8d795b53dfca7059eb1364eac50d4a8e85ea72803069979843ebdb49436a3db93144b0438f0b15cf1d30b171a3352a6d3b2007fd7a4b97c62a20dd801944cd1e1e924a887fdb41fadee7a571a2d824f3c3925a8f32faa7308190c77b7da7516c81d2ba0302c4eb0763442b7873d85530080ec023aebc7191336c420db7b9c066eb6067f66fa2f6b03a510a1ccf301585aba7ba442b7f9be6182bf60db46a3e2ff380b350b7cf1432f530ce7d3503fd34195f74ee3292a6db28943d0362ee7de7f3bfc7103f074ef0bdb5c5741ed54ff27a11f1159fc2ec4334bad8050464df238403fb0b5d814f65fed6a6ed76bb11868abcf36509fafa05b04fbb80185a58022d87badafe4c09cc475566b4b883558f459c97652eb82510b07a1ad530342573d36722c35e3a3fecdb616a72942e1e889f7fbbed4c7f2fa30c1a1bdfb92fa55e047d7d51f89633f21fdd74315731ccf0c1ca647135cbea7db9a62f4b3cb43a92b1b8b03e910d2e709c9c8a0fe25041f72120a51a2c9ab4155c01685132a9cfb62718a483fdcfb87bfb705dd5a85b10c8bba2c4df81a63d10480812f42a2a2d57ef71c6b685f0faa79d08d01ffab71037f0276b5b966756532470d115d63e6bcca2e420b04f628017b022c95cb991663b85be23fa70014bea539bc02c033711fa3ac026813f233941917507b1bd2dad073668f92e8697834ac12fcf6da847535afa019726fbe5acfbb3df32f231322b11069a46d0e883d62ca1766d1f91063be7e2e55d2a875ff5a24bef810bf068edd93863bb21a03d478d8bf78ac902614aeb19a8be6eb82fff7db2588d93cbe55628b1abcfce379b9cebadd718f0cb96b72f859bc7e944777f182c0ad160d912b5e43cf3b7ae27b894a4b4f0c704ee9fbfb87e63b9d812a8277634e8e0960d0c736230d78d91d7268129562a5fc7674321833fc1085e826dec7bb6c7d98172302ea2abe016f0e38d75e0fb895f364378a5d8479879dba16d6248c6decf84e0ed58dcdc13c0b2c96fadd15cbc5bf92a6268bf1672b339581e90259accd515f66eb207ee962bab98179ea5d0438b40bbac52b248ef293d13beb22cdc081a1c7bbbbb4c0060e6ad1240ff46c1d43450839626365e768c3b8510e31c76a6166e023dad31077e9f6afea1dc9686454f25a60eb6cf7b999c51a005171ed8435588451de5f6ab831ede004f7e6be1d807cb45e45b82298764657ea17692c6198b27f5f93ece31297443dbed66bbe878fc7b2cecf11cfdab409c764e77015fb5bf8dc5add2986569e225f1890ef28c519e90f028e453b51a48d7896c0804c3297241d415bccd7e7cee0a859fc648635652e033311e59c70f08d588cb1fd1cd8cf0ed261d47d9cca08b777fe6e4d02ea586369a9f1e3f5dcfece64066eb3ce15f7c1e1fba956ed4aa3f9e9bec7e27f1dac19f4af425030a56ee318f7199959f26ab641de25ba563a2d1159528d4329ce84ce3bdff56306f7437f552e15a90072cd4b5f785fcca79b6b57e5adb7dfb5a957c8c9c42a44770336c0e6f57a584af3ecc0c08526ef33a044f14610decde7aa82741a764290f6cb9940152663aa118bd0fff7341b1e336823ccb6815f9404e3f4874c99e0d335d814516b8cf75a7931c5b344ab27366c08482b17b4374fbd4a61385143fbbec73845157ea9894472b0613f0892087fc7eb2cc92b3e47100ef3b2b404ad9dae0c447c53d7fa1907f2dca4c106ee66a4bd6a5136b1e303e8a50827b8a9a65c551a84bee01de37c30cb3e00073c641da19a3f405e94f22ff7ab713d9f3d94dd7a2831291db9b886d53ba7430ff863df094262ef8fe8cfae9e6dfc1aef29d7ba0ce49060c829b7f85ddb9138ef76b51df1bf51e1eddfc057895d113bc4cc98d9a964b22a2ca0000";
      ConfidentialTransaction tx = new ConfidentialTransaction(blindTxStr1);
      // string jsonStr = ConfidentialTransaction.DecodeRawTransaction(tx);
      // output.WriteLine("decode: " + jsonStr);

      Txid txid = new Txid("ea42e0b5c4799953b4c54ab8aaeaa5326682b3ef6d7e894994312edb4f0dae37");
      uint vout = 1;
      string rootpriv = "tprv8ZgxMBicQKsPeWHBt7a68nPnvgTnuDhUgDWC8wZCgA8GahrQ3f3uWpq7wE7Uc1dLBnCe1hhCZ886K6ND37memRDWqsA9HgSKDXtwh2Qxo6J";
      CfdNetworkType networkType = CfdNetworkType.ElementsRegtest;
      string bip32Path = "m/44h/0h/0h";
      string utxoDescriptor = "pkh(0331e9b0c6b7f3798bb1b5a6b90c5e2e27c2906cbfd063a3c97b6031ee062ef745)";
      string valueCommitment = "09b6e7605917e27f35690dcae922f664c8a3b057e2c6249db6cd304096aa87a226";

      OutPoint outpoint = new OutPoint(txid, vout);
      uint txinIndex = tx.GetTxInIndex(outpoint);
      Assert.Equal((uint)1, txinIndex);

      ExtPrivkey childKey = new ExtPrivkey(rootpriv).DerivePrivkey(bip32Path);
      Privkey privkey = childKey.GetPrivkey();
      Pubkey pubkey = privkey.GetPubkey();
      output.WriteLine("child  : " + childKey.ToString());
      output.WriteLine("privkey: " + privkey.GetWif());
      output.WriteLine("pubkey : " + pubkey.ToHexString());

      Assert.Equal("tprv8gio6qQZzaVsZkjJY62vfoohmCysvZ9HDPNej342qrMxaV87wH7DQahQMvjXzFyGn1HZwGKMCpiGswAMAqJkB1uPamKKYk7FNsQG4SLnWUA",
        childKey.ToString());
      Assert.Equal("cUHGziyU25BGE489FVsaHJgAogBwhcyMDexp1EikUqWTgdQhbpfr",
        privkey.GetWif());
      Assert.Equal("0331e9b0c6b7f3798bb1b5a6b90c5e2e27c2906cbfd063a3c97b6031ee062ef745",
        pubkey.ToHexString());

      Descriptor desc = new Descriptor(utxoDescriptor, networkType);
      Assert.Equal(pubkey.ToHexString(), desc.GetKeyData().Pubkey.ToHexString());

      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint, desc.GetHashType(), pubkey,
        new ConfidentialValue(valueCommitment), sighashType);
      output.WriteLine("sighash: " + sighash.ToHexString());
      Assert.Equal("cc933ceafeb99f570c7000417888c96fead7173b954171c966a19a07f1b5aa0c",
        sighash.ToHexString());

      SignParameter sig = privkey.CalculateEcSignature(sighash);
      ByteData signature = sig.ToDerEncode(sighashType);
      output.WriteLine("derSig: " + signature.ToHexString());
      Assert.Equal("3044022011cff5458ca362dbb540be2226244fd262f3eadaa005eac2a367b70d136413e702201c9981f607a5eb7f1283d26d2860495fafdad53c5699944fe8b01dadb4aa0f7d01",
        signature.ToHexString());

      Script script = Script.CreateFromAsm(new string[] {
        signature.ToHexString(),
        pubkey.ToHexString()
      });
      output.WriteLine("script : " + script.ToHexString());
      output.WriteLine("asm    : " + script.GetAsm());
      Assert.Equal("473044022011cff5458ca362dbb540be2226244fd262f3eadaa005eac2a367b70d136413e702201c9981f607a5eb7f1283d26d2860495fafdad53c5699944fe8b01dadb4aa0f7d01210331e9b0c6b7f3798bb1b5a6b90c5e2e27c2906cbfd063a3c97b6031ee062ef745",
        script.ToHexString());

      bool isVerify = tx.VerifySignature(txid, vout, desc.GetHashType(), signature,
        pubkey, sighashType, new ConfidentialValue(valueCommitment));
      Assert.True(isVerify);
    }

    [Fact]
    public void CreateScriptSigShMultiTest()
    {
      string blindTxStr1 = "020000000104a76c51c1a0ad26df3eae8e8391a9da3cecea24edabcda10436cd3e550cd2f9e20100000000fdffffff37ae0d4fdb2e319449897e6defb3826632a5eaaab84ac5b4539979c4b5e042ea0100000017160014077042bf149434cda92751a2ca017eb35b1ae29cfdffffff08c67897576709ee329620a9d63f0e6f662ddaecceac73e5a8807f9c9fcb88430100000017160014a65e7f101ff5f4f4106603b884d2c631717f61d8fdffffff8daed986c59703a36742a38b9c379afa2489532a876da63e9cd1fe61577c0a740000000000fdffffff030a151e78448b0efec64069c8d11ac9d494635c81c68be60dd8dc5d00b61e66937009f2c7d4ec124a14c9b6e4a2311b4081944d7c493da119117fff8118f09b5c20c703bc52778cf3328c1bccc647e890df68294bcd38632d39a10e53d0757453a6c1b81976a914027aa64642c68f9e3d2d06c3141484ef1704fa8c88ac0b8a04b307c5bb5d383137600d5815347f6fb3f712672ff9c104af1eed3f7b2c9909e02b1ef9d285f8bd1b310f30facc9b9b267f18692ebe675fba02b2eda9febd67025eb7264392705673c88a2dea01a2325354ebb60a5cf045af41351077bb0a0e4917a914df01567680d27699b7d84dafa2bdabb5ada034d5870125b251070e29ca19043cf33ccd7324e2ddab03ecc4ae0b5e77c4fc0e5cf6c95a01000000000000b3740000dd070000000000000000000000000000000000008304000b13f3245f12994306c0732649e27622e0460517da75dae7bf3c3976bb7ab9cbed4719e56eb21b7fb65c2b938b61a6a53f368195b128c3178feec7ddaff3a9cc64e59fe0bfad6cb9506211c945fbba0be14a5457dab662094539b82d80ff2f50a492abaa7fe406810879833f2452651e0f6b51faaa5a0e507293330bfa16ea6283fd4d0b60230000000000000001c2b101c36161ea32674a76fc474e5a45d8bf234dc0dadaf96837b9b3117a63c06128b58861827accd9e5f4f64e61b1efdbc4b7757f7baa8d7072757e4bb65c6045cd70ff8b1ab4b9f1e82283dae8c61651351f56695a752f9f5bcc0ea785fdfce0d49f38c541912bcd55df51dc09cc161b35a0b04e0df400b122d4d4a393dbde8933ea7cea3c5be1caff45f3a760cd51315101d27456da3fb01acbf1af4a53a668ea7e5ebfe456ceef332b3878a21acd717a82557d2d353f39789893403987dadec79a934fd4d62418bcf5af3c9fb4250f9ae1456de1b110f25e9c713fc26470bd0acf2095799e3a73d91e4e187b30731d062ac917f6261df0c58e579a075019a8cb1e88f95fe509456510009ef5e0f9502ac426c264806f5bf1374ba826677b8b1b06fd00c060cf130bf1943a0a85d7b9993d937a1321aed730c13cc9a22316e57a526b0f5ec2e4f1355ef1d4820afa35d9d2c441fe128da31ca8f18b27c4e6fc5d73bf68165aaa4ad066aefee3cd342b85ec8b806433e58b49f300340b140bb042c63959f52a0b68155318f38065521c1564bbc80306fdbabd70c0805cebdd50e3f78ff5d55b53be8443bac0013ea25c0d1e5b9cf4edeaee946a266424c5ce45099b5f14ec0ec9ae9d1fafbdbdfcb608e3b3d6bc26ceb1e32d9535817f0ef1e90dbbd6013e94686fd846d50fc9b9312bd3016d81c0513c4c09078b3199f799dc1791f4e02e635de7295ed0e8bcf9971fa0cdb7067f71fe6226e6c5d6c9911fc5865a74a05d9f596c29e9ca2be83663f0ff1e41a5ec8ddc01c667026eb778c6b1f0d4f3d2dc0c937841238d8ded60a3f4a012a859b66d40bcd260acc6068bad56c8c3834ba4d1b4f778213d2d8365fa068b86613e3da1105123efc4b5ba934f532f5b6cb4af2e4828a1a560d3cd720eeef41f0fda6c466a6993c65ab9223cd3f0a09d99a6d7bac4974a586ea0bad4bc646c456e7df4ef84dda95943784cef2710653a9bdeacedbb7a541db754ea6a15ee9d38eac49a1c9e1ad1f38f4ebaa782da4b1e6fde7b66cd81ab60c002080dea754dabb296d4408e926a45f7f1301d551432d8c8e1294fdd307d9ee2a77e395e3684bae5a9b5afe76a389d67ee0913ef8832967955c0434fef85270411f5c60379512231c428bc03cdec09f0334659ebae151e2f7449f538b37c8c758fed826ab10da63a136938f12e8ad9c3cb76489c0610b8c1908820cc8dda284dda57e1e4c1e4f16c904aae0fc491019a273037d036ee0aa88cb43b4682fa8026933200ead8e0ad1792b2a9f2e177a58321f97ca0db36b6e1ce2661608b8d0bb654d43f676678ab2994436cac1f4c593abf0f11bc5dd737e2d645a6e92ecf7597b139268e5c565e24e7e0987358ae46900df03bc971b7a856181d551c149d6750459c56f1905799ef0552e0148418898839f5ed3167ec2fc1e0184514cd243c830e9092c6cb66a51f45d6a12b363adbc669b7aadf4d0b5cc2bf9922d6c1364dfdaca171fb6c3fd001fd1b053f42a24e9776a75daba64701a7248da6c7d4719c069364f86cb2f364b3f4e8fab3452545a24764bad1db29bc271bb8869c4dbf031f3786a12339ee562451c2b66d1af7ddbd7d65f6d06fafa06422be1312d144c1bc649e98fe665d0e5842cca596ea3a14a85d230da00e1f2880dfcd482535a617b4eb9d762d6771a90420bea9fbeb211e8794bbc77d353287a40c0b17ad86964a96318831fc815d92c5e6dd89968931f7487c86415cc1d600aa27bd164e3c4d0a15c01a7377d5a85ca3262f4462b945414bb9e4714f28fa6c8b0e57db2cf29b012a1730e976edb2f27ac24386bae63cd6022425fd8c2453aa6cd98e401ed8cdd6e86836d917c2bdd3e66b7c71c8a1628587b46c710df92fe74e16711179bc8eb19bae77cde2dc680fb2d862b74bba838621de9573c0b9875578441eb65e266b82db937e092e5260a42ac07659f7100aae03ee02977743a6249c685232851768a022fe95b7f35fa080d81f7de31a456d1a01d252b4e8860b48cfda15080616ce9f628c9ca670225102315ce92da545ddabce9943f257ff61f6ffc220840f1b29a64a8eada361386cdebc03d146f7682149e7bfcb3338430da40226714ff35a31aff60bdac47a0816806a1de48eeaaffbab88e82cc643d4ceb527f5c94bce1659bb56301460bb718f3ead9323345a7ecec1b8f0fd6297ef7d3850430fa8b90e75d45db87a265d3457fbefd6951c7d628496f06e75373c96f2eb97809b10c0143809cc28174b0a949f4dc8df6e6eb22896f26e5331e6ab66d7e0bc0347da39f0e7a3bd2c3369538bdea29a7d76461832dee3fe593bd598cb51a8b1966b135e3a84b8f4ec9a174b2be7cd0f6315d4ca03a958d2e88c1be4e19fc517111cd2a90c4fffc76583e6c361f5fabac6f0aac54cccaa02015e5cdb7f99b84b8026f5667c5f9b57a4f57cad899b7be8ce66b817a0483091378a8db14579cf1e9568ce2c4af3ed872193f449fe05bac79a7ea92743ffa72cf212f8da9679f733c89fee4b78dfe66a982c79cdd16e97b3a316f3191542a022a517f49566edb5bdbafe8a63fd844d5f6763e42e46bdc2c7de22705b8644253397c17480dc0c9484909ae99f3c5038ace18bddce9be9cae3b86312af4ec937a5e05e1a119a2dcc57391d291b25ffe3dff7da88451e5b9270ce7bac6086197a8a2796bf953b3b0fe70b1a53035ee560d302a9ab55f2926e7f888086367c9cf469d4b0a276525e567f73a7b55b48f0ddb36e1a7c1b97d2ee008e61c2bc635e6db9585b01dca600d81b044bb1af230c9ed6e658eb8c3c4b35e1d9773295ec57eec12ebf7da3135beb7e7de0ca086f254770ee0df3341271c6df96b10b3569fc479fbfe1cce62504612d38e30fc7b70fb22aab65ab178fbb095e04767a0198e3dcb6dd6aa04e538453d9e933e8c56bb16078ed85490963fb99ec7279b5d46618d2e596c5c057367d6ef3dfde0f6e61d9d0c9ec076c6ee1fe588463cb596921b5f130b278c1ba13ebec24cda89b4d8b25ac475c34de025b4a04a319367a5745a9963095a9f560e57b61c442a3b02c46302d2f8464966b4fdb28caa592da61695f292f76f69c3120ddd9308f6cc13c2008eee3ea874914cb1a8357949829fd8afd68ca1f854393d6aeb26bfc130d1e6c62d45b0ef5eb6700b7c0f5aa066ab985a825b65212affcc901278fe79f3b6cb45015f0010e92fb190758a64729a8b46f299564eb59a2ddb03057d97843723318f6ddf0f7e9254235dc49318dac7c028df6f680491ea1ab28a820a95d35f1937a9f035bb6cfb56050989900ef809fd55961d10832c2abd79e565d80131908a84d53f8a51752ed00abbb9ae41e6ffecc93fabc19373effa546cdf715c26456238f06651bdf4ffda9c0d77ada10354030480301f09ec383db5bca36deb002c482202e574e63cb5156d09bf511fae0a72125bb61d3efd2197f84e029558a3dd1cde325df7f639d4b030373807ca64fb49100ae21c632fca67ce4821132bd5c488517b5ef45a76013dd41d4449fd585bb128ee9eef39ceef57854e52a9cd61fd5122d8a249454e4f6cae75f37f616b20b5b340c789e5599541af4778d85efaa849e25b5224029179026245d5ced747e6871fa5a200898fedbae2954c452e267ab2a001b3ebd5920390e79687af5c012f53479c37a57897e578cf0e1fc3b4f038a84d72b3ca5363257a48919d74c606b1ed4361c5a1f7610ad70b5ee40f248a6c6822880e8ed4901b6ec0bca46aff4623273d256078701af941f4925df504961632c7e427c0dfd8607c5d1acc55c2f30ead6ea485512965f2a1d6015e6c4cf71453df81f70ced854eb0b23839223c5e18d1b63bbef7d79bc49a720e4fc2fe06e459d81340dc36f186c353295eb548838bfdd22876e34e2ea3170bb09104b77640e89dbd1048e6685df2d46bbc8ef682157d91f518d6ea083c649dbfeb23956164407b325424850ee2c0a98e6d2c3f19a1aeb4a5e6c38cf6792022ea6d5116671a1dee15006f2bbcb45b2f335e803c04d04fe089aae6c2151d30cd8e7d58304000ec0e7f42f42e6df0b144f11c0d5a3f6ad7cf84936e9d5a38fa266c3f7a742d4011f95f54553243fb35972ec66b650d781dad6ff672cbd8a935bd477b838595380ffe1586bf1bcee43e0ff18d252263250e952d39b0f215ebb69cf0c23a6b1a1d2b686c01c5d79fc5b486a88afbe466b4d4bc8e559b0b1f6432c98d8e0844cafecfd4d0b6023000000000000000111cb01acd74ab4034d001feb7cd326bb4f078d678ff1f7d281d3b1262864f97e6dbd42fcc90e63b1bfeacb96f2ee0b5889a2c1550467af8f940791eb2af806628252ff52a77cb2a48cc7b8ecfb0cf6db934e90d812bb4d51c73f84b32a7e9c481f89d56f24a2e7d36b2f6932f2ffc8eea9f9798f25f6f03e51729f4164171ce6475a9d0b9da3fa04922745ceaae12327bd7e40ff22f17d01dc4a6148674c4fcff4a3974fc48b8fb924f0545392d1b329301fba3a5e71fb541e8be0a96ad79bd2cd586bc43fecbc705e61240b58a21dbe0d2e0f0dab734ac0be7655e08917fa62fec78cda4cc83c50e8ae5089f94e14bf70f6f1698c23e914003f3633ef257cfe997327cce8bdf7f2adff2070785e651334e5a0ca97d5e8528e0b47e88570e152a18026c99658cc3350174bc83d887192fdf6221c938f25d09d64bddf98f4313c4f67909e9aeef61a633e2c708a1dc5f995882a780eb93f9db0bd97470b77fb61b0263abdc6232b654da092bbfe22f486b90a7c5898f83ec595a003a3ea66c4288cc6e529262d6226675a7009dd1bd762491a509225651c356b60ffdd1f36a6cc7c24ee719b941f02f2a88406e93d0d0b81c75ba33ce4821f7009c21274af21a5f61fd105b25cfa857df61b2cdd29ce0eeeec86faa52ead261dd3b1326d52d96945e8b15974d2c4be711813f7130234dafbb7715cc4dd8ed69d3c4ca62f0bf448bbf8a32e52df9334a094bfa9f577eab017de57bbbe028fc08014a2f3ee55d9df9cadff760dcab6d06257c814c031bb11c836a8363f261982a7ce73392f82fcb6bdd5968835f657831e7dd1b0252a2112a45c403bf2975bebabfaccc365eb09bb25abfc3b80797cb2058268b9899ba642570d17ce05b8d9a304eaeb4a61b6fec6dc189f54f3d337df46918a76bc7f1ce9846ede724eaef34d4f02b6da0a293aa3a60b306b7c937bc2fd11f58d08440287b0c7793649ecb23b69181af9912f83afb283af17f48f05c79fad79b2aafadb64309a29ee3d08521f5b08d075eca726e2731c6cf85209c9341ee65878b7f86d85503b96c115f8cd603dfebfd24d9d6fb1df2cd92a32bfbb5eb926c280f902cb5bff0ca683a64a5b215fc2f377995363f42080bf96d953153a640762f91a4e68fff1c62c382eba70301dea1823345c9b5e52c5e2488881a76f5aa29f30a24fbc720ef24e1811640e44d0e92fafffde3bb7c3c8c8e813569d89604afbd7e09bd99418dbe31457a33d528924fa51291b62be9e8bb4fea91578296d1ffda0eaf0549347010e64181271b52036613153b61b8e83ea1aeff860ecf4af953639ff008a92a8c2e15d846ac75da55c1732277273ba070235f40d60554181f17b17b05158260e62b0f3cb7a11fb92e20dea7eda89ded7a69b16a359cfe9b3f334d369def50b1700d250bf003cfe23af5a2ee44b7ec2109650c1d88b33edc4f9404cdec8916f84366b36a79e05b4083f201ac59e688dd1ba03e2ea48fd6a42f8ae524a48b6ebb4b4326c667ec75f79e88af078ca667caacc06af34ca42eacabf441dff6fd80a8d6daa89e184d80e9516df2544a539723c9ed18cbaecbc94557e3a31b6ca5b0ecd15d1839c0f193405f2a461e7a520f414d056864f0ff6591d6f0bac63746f9887b5cad36b208160fe008d4eae56384019f92de1ecc2ca53be8055c900ad94521440bd99646f365ef944b6caf92e2614454ae032cb52e59bdc4aff6710a978a4bb6c3ff4eaa478f6a40c96fdb26268cbb422b6ee7d6fda354de6ca7cde33456762a04db65389ff0c10f2852158b543abd9c26e0f1bd0ae04f7e40452f9b4d5d5aab0947856ba73202a5ac577b5a9a329299849b4304436289f271b8bb8f7da91f9c9d4d4fb2d707c3b0ccbf1820c4723ad5a6a1e4d79dbd62ab086ef409c3fc8b5e11bc7e8aa187b7d576d50fbfb755f1dbb8fa7658d52da16a8a7575747a841b52ad6d89c53bfc8afaf0f4d3041c774e05b40bbbaa4fccffa93054efa769d1566313c7f840a0ef9b91c45ac79f1b5e12c866157e4203f8860af2fcb26d2ff665d8e4e2baec6d7c05d5ba45cbcdb0c270b7e0b992e54ceeed41397a11d8478ced4f98ed161e335c4133372d1e4bcbcfa87fdb3fb0930cd99e4af6ac732ea164a328ee5445c374b59e9dfcd665399ed8d795b53dfca7059eb1364eac50d4a8e85ea72803069979843ebdb49436a3db93144b0438f0b15cf1d30b171a3352a6d3b2007fd7a4b97c62a20dd801944cd1e1e924a887fdb41fadee7a571a2d824f3c3925a8f32faa7308190c77b7da7516c81d2ba0302c4eb0763442b7873d85530080ec023aebc7191336c420db7b9c066eb6067f66fa2f6b03a510a1ccf301585aba7ba442b7f9be6182bf60db46a3e2ff380b350b7cf1432f530ce7d3503fd34195f74ee3292a6db28943d0362ee7de7f3bfc7103f074ef0bdb5c5741ed54ff27a11f1159fc2ec4334bad8050464df238403fb0b5d814f65fed6a6ed76bb11868abcf36509fafa05b04fbb80185a58022d87badafe4c09cc475566b4b883558f459c97652eb82510b07a1ad530342573d36722c35e3a3fecdb616a72942e1e889f7fbbed4c7f2fa30c1a1bdfb92fa55e047d7d51f89633f21fdd74315731ccf0c1ca647135cbea7db9a62f4b3cb43a92b1b8b03e910d2e709c9c8a0fe25041f72120a51a2c9ab4155c01685132a9cfb62718a483fdcfb87bfb705dd5a85b10c8bba2c4df81a63d10480812f42a2a2d57ef71c6b685f0faa79d08d01ffab71037f0276b5b966756532470d115d63e6bcca2e420b04f628017b022c95cb991663b85be23fa70014bea539bc02c033711fa3ac026813f233941917507b1bd2dad073668f92e8697834ac12fcf6da847535afa019726fbe5acfbb3df32f231322b11069a46d0e883d62ca1766d1f91063be7e2e55d2a875ff5a24bef810bf068edd93863bb21a03d478d8bf78ac902614aeb19a8be6eb82fff7db2588d93cbe55628b1abcfce379b9cebadd718f0cb96b72f859bc7e944777f182c0ad160d912b5e43cf3b7ae27b894a4b4f0c704ee9fbfb87e63b9d812a8277634e8e0960d0c736230d78d91d7268129562a5fc7674321833fc1085e826dec7bb6c7d98172302ea2abe016f0e38d75e0fb895f364378a5d8479879dba16d6248c6decf84e0ed58dcdc13c0b2c96fadd15cbc5bf92a6268bf1672b339581e90259accd515f66eb207ee962bab98179ea5d0438b40bbac52b248ef293d13beb22cdc081a1c7bbbbb4c0060e6ad1240ff46c1d43450839626365e768c3b8510e31c76a6166e023dad31077e9f6afea1dc9686454f25a60eb6cf7b999c51a005171ed8435588451de5f6ab831ede004f7e6be1d807cb45e45b82298764657ea17692c6198b27f5f93ece31297443dbed66bbe878fc7b2cecf11cfdab409c764e77015fb5bf8dc5add2986569e225f1890ef28c519e90f028e453b51a48d7896c0804c3297241d415bccd7e7cee0a859fc648635652e033311e59c70f08d588cb1fd1cd8cf0ed261d47d9cca08b777fe6e4d02ea586369a9f1e3f5dcfece64066eb3ce15f7c1e1fba956ed4aa3f9e9bec7e27f1dac19f4af425030a56ee318f7199959f26ab641de25ba563a2d1159528d4329ce84ce3bdff56306f7437f552e15a90072cd4b5f785fcca79b6b57e5adb7dfb5a957c8c9c42a44770336c0e6f57a584af3ecc0c08526ef33a044f14610decde7aa82741a764290f6cb9940152663aa118bd0fff7341b1e336823ccb6815f9404e3f4874c99e0d335d814516b8cf75a7931c5b344ab27366c08482b17b4374fbd4a61385143fbbec73845157ea9894472b0613f0892087fc7eb2cc92b3e47100ef3b2b404ad9dae0c447c53d7fa1907f2dca4c106ee66a4bd6a5136b1e303e8a50827b8a9a65c551a84bee01de37c30cb3e00073c641da19a3f405e94f22ff7ab713d9f3d94dd7a2831291db9b886d53ba7430ff863df094262ef8fe8cfae9e6dfc1aef29d7ba0ce49060c829b7f85ddb9138ef76b51df1bf51e1eddfc057895d113bc4cc98d9a964b22a2ca0000";
      ConfidentialTransaction tx = new ConfidentialTransaction(blindTxStr1);
      // string jsonStr = ConfidentialTransaction.DecodeRawTransaction(tx);
      // output.WriteLine("decode: " + jsonStr);

      Txid txid = new Txid("4388cb9f9c7f80a8e573acceecda2d666f0e3fd6a9209632ee0967579778c608");
      uint vout = 1;
      string rootpriv = "tprv8ZgxMBicQKsPeWHBt7a68nPnvgTnuDhUgDWC8wZCgA8GahrQ3f3uWpq7wE7Uc1dLBnCe1hhCZ886K6ND37memRDWqsA9HgSKDXtwh2Qxo6J";
      CfdNetworkType networkType = CfdNetworkType.ElementsRegtest;
      string bip32Path = "m/44h/0h/0h";
      string utxoDescriptor = "sh(multi(2,0331e9b0c6b7f3798bb1b5a6b90c5e2e27c2906cbfd063a3c97b6031ee062ef745,02e3cf2c4dca39b502a6f8ba37e5d63a9757492c2155bf99418d9532728cd23d93))";
      string valueCommitment = "09b6e7605917e27f35690dcae922f664c8a3b057e2c6249db6cd304096bb87a226";

      OutPoint outpoint = new OutPoint(txid, vout);
      uint txinIndex = tx.GetTxInIndex(outpoint);
      Assert.Equal((uint)2, txinIndex);

      ExtPrivkey childKey = new ExtPrivkey(rootpriv).DerivePrivkey(bip32Path);
      Privkey privkey = childKey.GetPrivkey();
      Pubkey pubkey = privkey.GetPubkey();
      output.WriteLine("child  : " + childKey.ToString());
      output.WriteLine("privkey: " + privkey.GetWif());
      output.WriteLine("pubkey : " + pubkey.ToHexString());

      Assert.Equal("tprv8gio6qQZzaVsZkjJY62vfoohmCysvZ9HDPNej342qrMxaV87wH7DQahQMvjXzFyGn1HZwGKMCpiGswAMAqJkB1uPamKKYk7FNsQG4SLnWUA",
        childKey.ToString());
      Assert.Equal("cUHGziyU25BGE489FVsaHJgAogBwhcyMDexp1EikUqWTgdQhbpfr",
        privkey.GetWif());
      Assert.Equal("0331e9b0c6b7f3798bb1b5a6b90c5e2e27c2906cbfd063a3c97b6031ee062ef745",
        pubkey.ToHexString());

      Descriptor desc = new Descriptor(utxoDescriptor, networkType);
      Script redeemScript = desc.GetRedeemScript();
      output.WriteLine("redeemScript : " + redeemScript.ToHexString());
      Assert.Equal("52210331e9b0c6b7f3798bb1b5a6b90c5e2e27c2906cbfd063a3c97b6031ee062ef7452102e3cf2c4dca39b502a6f8ba37e5d63a9757492c2155bf99418d9532728cd23d9352ae",
        redeemScript.ToHexString());

      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint, desc.GetHashType(), redeemScript,
        new ConfidentialValue(valueCommitment), sighashType);
      output.WriteLine("sighash: " + sighash.ToHexString());
      Assert.Equal("108febc4145d32facd83dbfb89d550d9b5a3e75fa5d262a41579d71528df6e25",
        sighash.ToHexString());

      SignParameter sig = privkey.CalculateEcSignature(sighash);
      sig.SetDerEncode(sighashType);
      ByteData signature = sig.ToDerEncode();
      output.WriteLine("derSig: " + signature.ToHexString());
      Assert.Equal("304402207c39dd14480925d6d4babcf8c5393085f55054fa108aaf54fe34a87494132d48022050dbde84e1e955aca56a26af4fc117d95b2541547ea0ac4adc0aa50547a8a3e301",
        signature.ToHexString());

      Script script = Script.CreateFromAsm(new string[] {
        "OP_0",
        signature.ToHexString(),
        redeemScript.ToHexString()
      });
      output.WriteLine("script : " + script.ToHexString());
      output.WriteLine("asm    : " + script.GetAsm());
      Assert.Equal("0047304402207c39dd14480925d6d4babcf8c5393085f55054fa108aaf54fe34a87494132d48022050dbde84e1e955aca56a26af4fc117d95b2541547ea0ac4adc0aa50547a8a3e3014752210331e9b0c6b7f3798bb1b5a6b90c5e2e27c2906cbfd063a3c97b6031ee062ef7452102e3cf2c4dca39b502a6f8ba37e5d63a9757492c2155bf99418d9532728cd23d9352ae",
        script.ToHexString());

      bool isVerify = tx.VerifySignature(txid, vout, desc.GetHashType(), signature,
        pubkey, redeemScript, sighashType, new ConfidentialValue(valueCommitment));
      Assert.True(isVerify);
    }

    [Fact]
    public void EstimateFeeTest()
    {
      string desc = "sh(wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1))#4z2vy08x";
      string descMulti = "wsh(multi(2,03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7,03774ae7f858a9411e5ef4246b70c65aac5649980be5c17891bbec17895da008cb,03d01115d548e7561b15c38f004d734633687cf4419620095bc5b0f47070afe85a))";

      ConfidentialTransaction tx = new ConfidentialTransaction("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000");
      Descriptor desc1 = new Descriptor(desc, CfdNetworkType.ElementsRegtest);
      Descriptor desc2 = new Descriptor(descMulti, CfdNetworkType.ElementsRegtest);

      // set utxo data
      ElementsUtxoData[] utxos = new[]
      {
        new ElementsUtxoData(
          new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 0),
          "186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179",
          999637680,
          new ConfidentialAsset("0a1100000000000000000000000000000000000000000000000000000000000000"),
          new ConfidentialValue("081100000000000000000000000000000000000000000000000000000000000000"),
          desc1,
          new BlindFactor("a10ecbe1be7a5f883d5d45d966e30dbc1beff5f21c55cec76cc21a2229116a9f"),
          new BlindFactor("ae0f46d1940f297c2dc3bbd82bf8ef6931a2431fbb05b3d3bc5df41af86ae808")),
        new ElementsUtxoData(
          new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
          "ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b",
          700000000,
          new ConfidentialAsset("0a1100000000000000000000000000000000000000000000000000000000000000"),
          new ConfidentialValue("081100000000000000000000000000000000000000000000000000000000000000"),
          desc2,
          true,  // isBlindIssuance
          new BlindFactor("0b8954757234fd3ec9cf0dd6ef0a89d825ec56a9532e7da4b6cb90c51be3bbd8"),
          new BlindFactor("62e36e1f0fa4916b031648a6b6903083069fa587572a88b729250cde528cfd3b")),
      };

      // estimate fee on blind tx
      FeeData feeData = tx.EstimateFee(utxos, new ConfidentialAsset("186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179"));
      Assert.Equal(552, feeData.TxFee);
      Assert.Equal(205, feeData.InputFee);
      Assert.Equal(757, feeData.InputFee + feeData.TxFee);

      tx.UpdateFee(feeData.InputFee + feeData.TxFee,
        new ConfidentialAsset("186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179"));
    }

    [Fact]
    public void FundRawTransactionTest()
    {
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);
      ExtPubkey key = new ExtPubkey("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy");
      Address setAddr1 = new Address(key.DerivePubkey(11).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address setAddr2 = new Address(key.DerivePubkey(12).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);

      ConfidentialTransaction tx = new ConfidentialTransaction(2, 0, null, new[] {
        new ConfidentialTxOut(new ConfidentialAsset(assetA), new ConfidentialValue(10000000), setAddr1.GetLockingScript()),
        new ConfidentialTxOut(new ConfidentialAsset(assetB), new ConfidentialValue(500000), setAddr2.GetLockingScript()),
      });

      var feeAsset = new ConfidentialAsset(assetA);
      var targetAssetMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 0 },
        { new ConfidentialAsset(assetB), 0 },
      };
      Address addr1 = new Address(key.DerivePubkey(1).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address addr2 = new Address(key.DerivePubkey(2).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      var reservedAddressMap = new Dictionary<ConfidentialAsset, string> {
        { new ConfidentialAsset(assetA), addr1.ToAddressString() },
        { new ConfidentialAsset(assetB), addr2.ToAddressString() },
      };
      double feeRate = 0.1;
      string[] usedAddr = tx.FundRawTransaction(null, utxos, targetAssetMap, reservedAddressMap, feeAsset, feeRate);
      output.WriteLine("tx: " + tx.ToHexString());

      Assert.Equal("0200000000020bfa8774c5f753ce2f801a8106413b470af94edbff5b4242ed4c5a26d20e72b90000000000ffffffff040b0000000000000000000000000000000000000000000000000000000000000000000000ffffffff050100000000000000000000000000000000000000000000000000000000000000aa010000000000989680001600144352a1a6e86311f22274f7ebb2746de21b09b15d0100000000000000000000000000000000000000000000000000000000000000bb01000000000007a120001600148beaaac4654cf4ebd8e46ca5062b0e7fb3e7470c0100000000000000000000000000000000000000000000000000000000000000aa01000000000000020300000100000000000000000000000000000000000000000000000000000000000000bb010000000001124c1e00160014a53be40113bb50f2b8b2d0bfea1e823e75632b5f0100000000000000000000000000000000000000000000000000000000000000aa0100000000004b595b0016001478eb9fc2c9e1cdf633ecb646858ba862b21384ab00000000",
        tx.ToHexString());
      output.WriteLine(ConfidentialTransaction.DecodeRawTransaction(tx));
      Assert.Equal(2, usedAddr.Length);
      if (usedAddr.Length == 2)
      {
        output.WriteLine("addr1: " + usedAddr[0]);
        output.WriteLine("addr2: " + usedAddr[1]);
        Assert.Equal(addr2.ToAddressString(), usedAddr[0]);
        Assert.Equal(addr1.ToAddressString(), usedAddr[1]);
      }
      Assert.Equal(515, tx.GetLastTxFee());

      // calc fee
      ElementsUtxoData[] feeUtxos = new[]{
        utxos[5],
        utxos[9],
      };
      FeeData feeData = tx.EstimateFee(feeUtxos, feeRate, feeAsset, true);
      Assert.Equal(506, feeData.TxFee + feeData.InputFee);
      Assert.Equal(488, feeData.TxFee);
      Assert.Equal(18, feeData.InputFee);
    }

    [Fact]
    public void FundRawTransactionExistTxInTest()
    {
      // create reissue tx
      string tokenAsset = "ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b";
      string descMulti = "wsh(multi(2,03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7,03774ae7f858a9411e5ef4246b70c65aac5649980be5c17891bbec17895da008cb,03d01115d548e7561b15c38f004d734633687cf4419620095bc5b0f47070afe85a))";
      Descriptor desc2 = new Descriptor(descMulti, CfdNetworkType.ElementsRegtest);
      ElementsUtxoData inputUtxo = new ElementsUtxoData(
          new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
          tokenAsset,
          700000000,
          new ConfidentialAsset("0a1100000000000000000000000000000000000000000000000000000000000000"),
          new ConfidentialValue("081100000000000000000000000000000000000000000000000000000000000000"),
          desc2,
          true,
          new BlindFactor("0b8954757234fd3ec9cf0dd6ef0a89d825ec56a9532e7da4b6cb90c51be3bbd8"),
          new BlindFactor("62e36e1f0fa4916b031648a6b6903083069fa587572a88b729250cde528cfd3b"));
      Address outAddr1 = new Address("2djHX9wtrtdyGw9cer1u6zB6Yq4SRD8V5zw");
      Address outAddr2 = new Address("2dodsWJgP3pTWWidK5hDxuYHqC1U4CEnT3n");
      ConfidentialTransaction tx = new ConfidentialTransaction(2, 0, new[] {
        new ConfidentialTxIn(inputUtxo.GetOutPoint()),
      }, new[] {
        new ConfidentialTxOut(
          new ConfidentialAsset(tokenAsset),
          new ConfidentialValue(700000000), outAddr1.GetLockingScript()),
      });
      ConfidentialAsset issueAsset = tx.SetRawReissueAsset(inputUtxo.GetOutPoint(),
        600000000, inputUtxo.GetAssetBlindFactor().GetData(),
        new ByteData("6f9ccf5949eba5d6a08bff7a015e825c97824e82d57c8a0c77f9a41908fe8306"),
        outAddr2);
      output.WriteLine("issue asset: " + issueAsset.ToHexString());

      ElementsUtxoData[] inputUtxos = new[]
      {
        inputUtxo,
      };

      // add txout
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);

      ExtPubkey key = new ExtPubkey("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy");
      Address setAddr1 = new Address(key.DerivePubkey(11).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address setAddr2 = new Address(key.DerivePubkey(12).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);

      tx.AddTxOutList(new[] {
        new ConfidentialTxOut(new ConfidentialAsset(assetA), new ConfidentialValue(10000000), setAddr1.GetLockingScript()),
        new ConfidentialTxOut(new ConfidentialAsset(assetB), new ConfidentialValue(500000), setAddr2.GetLockingScript()),
      });

      // fundrawtx
      var feeAsset = new ConfidentialAsset(assetA);
      var targetAssetMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 0 },
        { new ConfidentialAsset(assetB), 0 },
      };
      Address addr1 = new Address(key.DerivePubkey(1).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      Address addr2 = new Address(key.DerivePubkey(2).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      // Address addr3 = new Address(key.DerivePubkey(3).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      // Address addr4 = new Address(key.DerivePubkey(3).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Liquidv1);
      var reservedAddressMap = new Dictionary<ConfidentialAsset, string> {
        { new ConfidentialAsset(assetA), addr1.ToAddressString() },
        { new ConfidentialAsset(assetB), addr2.ToAddressString() },
      };
      double feeRate = 0.1;
      string[] usedAddr = tx.FundRawTransaction(inputUtxos, utxos, targetAssetMap, reservedAddressMap, feeAsset, feeRate);
      output.WriteLine("tx: " + tx.ToHexString());

      Assert.Equal("0200000000030f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c34600000bfa8774c5f753ce2f801a8106413b470af94edbff5b4242ed4c5a26d20e72b90000000000ffffffff040b0000000000000000000000000000000000000000000000000000000000000000000000ffffffff07010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b92700001976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac01cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c34600001976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac0100000000000000000000000000000000000000000000000000000000000000aa010000000000989680001600144352a1a6e86311f22274f7ebb2746de21b09b15d0100000000000000000000000000000000000000000000000000000000000000bb01000000000007a120001600148beaaac4654cf4ebd8e46ca5062b0e7fb3e7470c0100000000000000000000000000000000000000000000000000000000000000aa01000000000000037400000100000000000000000000000000000000000000000000000000000000000000bb010000000001124c1e00160014a53be40113bb50f2b8b2d0bfea1e823e75632b5f0100000000000000000000000000000000000000000000000000000000000000aa0100000000004b57ea0016001478eb9fc2c9e1cdf633ecb646858ba862b21384ab00000000",
        tx.ToHexString());
      output.WriteLine(ConfidentialTransaction.DecodeRawTransaction(tx));
      Assert.Equal(2, usedAddr.Length);
      if (usedAddr.Length == 2)
      {
        output.WriteLine("addr1: " + usedAddr[0]);
        output.WriteLine("addr2: " + usedAddr[1]);
        Assert.Equal(addr2.ToAddressString(), usedAddr[0]);
        Assert.Equal(addr1.ToAddressString(), usedAddr[1]);
      }
      Assert.Equal(884, tx.GetLastTxFee());

      // calc fee
      ElementsUtxoData[] feeUtxos = new[]{
        inputUtxo,
        utxos[5],
        utxos[9],
      };
      FeeData feeData = tx.EstimateFee(feeUtxos, feeRate, feeAsset, true);
      Assert.Equal(875, feeData.TxFee + feeData.InputFee);
      Assert.Equal(729, feeData.TxFee);
      Assert.Equal(146, feeData.InputFee);
    }

    [Fact]
    public void RawReissueAssetTest()
    {
      string desc = "sh(wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1))#4z2vy08x";
      string descMulti = "wsh(multi(2,03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7,03774ae7f858a9411e5ef4246b70c65aac5649980be5c17891bbec17895da008cb,03d01115d548e7561b15c38f004d734633687cf4419620095bc5b0f47070afe85a))";

      Descriptor desc1 = new Descriptor(desc, CfdNetworkType.ElementsRegtest);
      Descriptor desc2 = new Descriptor(descMulti, CfdNetworkType.ElementsRegtest);

      // set utxo data
      ElementsUtxoData[] utxos = new[]
      {
        new ElementsUtxoData(
          new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 0),
          "186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179",
          999637680,
          new ConfidentialAsset("0a1100000000000000000000000000000000000000000000000000000000000000"),
          new ConfidentialValue("081100000000000000000000000000000000000000000000000000000000000000"),
          desc1,
          new BlindFactor("a10ecbe1be7a5f883d5d45d966e30dbc1beff5f21c55cec76cc21a2229116a9f"),
          new BlindFactor("ae0f46d1940f297c2dc3bbd82bf8ef6931a2431fbb05b3d3bc5df41af86ae808")),
        new ElementsUtxoData(
          new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
          "ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b",
          700000000,
          new ConfidentialAsset("0a1100000000000000000000000000000000000000000000000000000000000000"),
          new ConfidentialValue("081100000000000000000000000000000000000000000000000000000000000000"),
          desc2,
          new BlindFactor("0b8954757234fd3ec9cf0dd6ef0a89d825ec56a9532e7da4b6cb90c51be3bbd8"),
          new BlindFactor("62e36e1f0fa4916b031648a6b6903083069fa587572a88b729250cde528cfd3b")),
      };

      ConfidentialTransaction tx = new ConfidentialTransaction(2, 0, new[] {
        new ConfidentialTxIn(utxos[0].GetOutPoint()),
        new ConfidentialTxIn(utxos[1].GetOutPoint()),
      }, new[] {
        new ConfidentialTxOut(
          new ConfidentialAsset("186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179"),
          new ConfidentialValue(999587680), new Script("a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87")),
        new ConfidentialTxOut(
          new ConfidentialAsset("ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b"),
          new ConfidentialValue(700000000), new Script("76a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac")),
        new ConfidentialTxOut(
          new ConfidentialAsset("186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179"),
          50000),
      });

      tx.SetRawReissueAsset(utxos[1].GetOutPoint(), 600000000, utxos[1].GetAssetBlindFactor().GetData(),
        new ByteData("6f9ccf5949eba5d6a08bff7a015e825c97824e82d57c8a0c77f9a41908fe8306"),
        new Address("2dodsWJgP3pTWWidK5hDxuYHqC1U4CEnT3n"));

      output.WriteLine("tx: " + tx.ToHexString());
      Assert.Equal("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f600017a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b92700001976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c34600001976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000",
        tx.ToHexString());
    }

    private static readonly string assetA = "aa00000000000000000000000000000000000000000000000000000000000000";
    private static readonly string assetB = "bb00000000000000000000000000000000000000000000000000000000000000";
    private static readonly string assetC = "cc00000000000000000000000000000000000000000000000000000000000000";

    static ElementsUtxoData[] GetElementsBnbUtxoList(CfdNetworkType netType)
    {
      string desc = "sh(wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1))#4z2vy08x";
      ElementsUtxoData[] utxos = new[] {
        new ElementsUtxoData(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(155062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(85062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("9e1ead91c432889cb478237da974dd1e9009c9e22694fd1e3999c40a1ef59b0a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(39062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("8f4af7ee42e62a3d32f25ca56f618fb2f5df3d4c3a9c59e2c3646c5535a3d40a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(61062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("4d97d0119b90421818bff4ec9033e5199199b53358f56390cb20f8148e76f40a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(15675000),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("b9720ed2265a4ced42425bffdb4ef90a473b4106811a802fce53f7c57487fa0b", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(14938590),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b01", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(26918400),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b02", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(750000),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b03", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(346430050),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b04", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(18476350),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000c01", 0),
          new ConfidentialAsset(assetC), new ConfidentialValue(37654200),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000c02", 0),
          new ConfidentialAsset(assetC), new ConfidentialValue(127030000),
          new Descriptor(desc, netType)),
      };
      return utxos;
    }

  }
}
