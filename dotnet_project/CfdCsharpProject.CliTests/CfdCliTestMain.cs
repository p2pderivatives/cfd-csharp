using System;
using System.Collections.Generic;

namespace Cfd.Tests.Cli
{
  // [TestClass()]
  public static class CfdCliTestMain
  {
    // [TestMethod()]
    public static void TestConfidentialTx()
    {
      Console.WriteLine("ConfidentialTransaction");
      ConfidentialTransaction tx = new ConfidentialTransaction("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000");

      Console.WriteLine("- tx    = " + tx.ToHexString());
      Console.WriteLine("- bytes = " + StringUtil.FromBytes(tx.GetBytes()));
      Console.WriteLine("- txid  = " + tx.GetTxid().ToHexString());
      Console.WriteLine("- size  = " + tx.GetSize());
      Console.WriteLine("- vsize = " + tx.GetVsize());
      Console.WriteLine();
    }

    // [TestMethod()]
    public static void TestBlindTx()
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

      Console.WriteLine("call BlindTransaction");
      tx.BlindTransaction(utxos, issuanceKeys, confidentialKeys);
      Console.WriteLine("- Blind tx data:");
      Console.WriteLine("  - txid     = " + tx.GetTxid().ToHexString());
      Console.WriteLine("  - wtxid    = " + tx.GetWtxid().ToHexString());
      Console.WriteLine("  - witHash  = " + tx.GetWitHash());
      Console.WriteLine("  - size     = " + tx.GetSize());
      Console.WriteLine("  - vsize    = " + tx.GetVsize());
      Console.WriteLine("  - weight   = " + tx.GetWeight());
      Console.WriteLine("  - version  = " + tx.GetVersion());
      Console.WriteLine("  - lockTime = " + tx.GetLockTime());
      // Console.WriteLine("  - tx       = " + tx.ToHexString());
      if (tx.GetSize() != 12589)
      {
        throw new InvalidOperationException("**** size fail. ****");
      }
      if (tx.GetVsize() != 3604)
      {
        throw new InvalidOperationException("**** vsize fail. ****");
      }
      if (tx.GetWeight() != 14413)
      {
        throw new InvalidOperationException("**** weight fail. ****");
      }
      if (tx.GetVersion() != 2)
      {
        throw new InvalidOperationException("**** Version fail. ****");
      }
      if (tx.GetLockTime() != 0)
      {
        throw new InvalidOperationException("**** LockTime fail. ****");
      }

      Console.WriteLine("UnblindTransaction");
      UnblindIssuanceData reissueData = tx.UnblindIssuance(1, issuanceBlindingKey, issuanceBlindingKey);
      AssetValueData vout0 = tx.UnblindTxOut(0,
          new Privkey("6a64f506be6e60b948987aa4d180d2ab05034a6a214146e06e28d4efe101d006"));
      AssetValueData vout1 = tx.UnblindTxOut(1,
          new Privkey("94c85164605f589c4c572874f36b8301989c7fabfd44131297e95824d473681f"));
      AssetValueData vout3 = tx.UnblindTxOut(3,
          new Privkey("0473d39aa6542e0c1bb6a2343b2319c3e92063dd019af4d47dbf50c460204f32"));

      Console.WriteLine("- ReIssuance unblind data:");
      Console.WriteLine("  - asset          = " + reissueData.AssetData.Asset);
      Console.WriteLine("  - satoshiAmount  = " + reissueData.AssetData.SatoshiValue);
      Console.WriteLine("  - asset blinder  = " + reissueData.AssetData.AssetBlindFactor.ToHexString());
      Console.WriteLine("  - amount blinder = " + reissueData.AssetData.AmountBlindFactor.ToHexString());
      if (reissueData.AssetData.Asset != "accb7354c07974e00b32e4e5eef55078490141675592ac3610e6101831edb0cd")
      {
        throw new InvalidOperationException("**** reissue asset fail. ****");
      }
      if (reissueData.AssetData.SatoshiValue != 600000000)
      {
        throw new InvalidOperationException("**** reissue satoshiValue fail. ****");
      }
      if (reissueData.AssetData.AssetBlindFactor.ToHexString() != "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** reissue asset blinder fail. ****");
      }
      if (reissueData.AssetData.AmountBlindFactor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** reissue  amount blinder fail. ****");
      }

      Console.WriteLine("- Vout[0] unblind data:");
      Console.WriteLine("  - asset          = " + vout0.Asset);
      Console.WriteLine("  - satoshiAmount  = " + vout0.SatoshiValue);
      Console.WriteLine("  - asset blinder  = " + vout0.AssetBlindFactor.ToHexString());
      Console.WriteLine("  - amount blinder = " + vout0.AmountBlindFactor.ToHexString());
      if (vout0.Asset != "186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179")
      {
        throw new InvalidOperationException("**** vout[0] asset fail. ****");
      }
      if (vout0.SatoshiValue != 999587680)
      {
        throw new InvalidOperationException("**** vout[0] satoshiValue fail. ****");
      }
      if (vout0.AssetBlindFactor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[0] asset blinder fail. ****");
      }
      if (vout0.AmountBlindFactor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[0]  amount blinder fail. ****");
      }

      Console.WriteLine("- Vout[1] unblind data:");
      Console.WriteLine("  - asset          = " + vout1.Asset);
      Console.WriteLine("  - satoshiAmount  = " + vout1.SatoshiValue);
      Console.WriteLine("  - asset blinder  = " + vout1.AssetBlindFactor.ToHexString());
      Console.WriteLine("  - amount blinder = " + vout1.AmountBlindFactor.ToHexString());
      if (vout1.Asset != "ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b")
      {
        throw new InvalidOperationException("**** vout[1] asset fail. ****");
      }
      if (vout1.SatoshiValue != 700000000)
      {
        throw new InvalidOperationException("**** vout[1] satoshiValue fail. ****");
      }
      if (vout1.AssetBlindFactor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[1] asset blinder fail. ****");
      }
      if (vout1.AmountBlindFactor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[1]  amount blinder fail. ****");
      }

      Console.WriteLine("- Vout[3] unblind data:");
      Console.WriteLine("  - asset          = " + vout3.Asset);
      Console.WriteLine("  - satoshiAmount  = " + vout3.SatoshiValue);
      Console.WriteLine("  - asset blinder  = " + vout3.AssetBlindFactor.ToHexString());
      Console.WriteLine("  - amount blinder = " + vout3.AmountBlindFactor.ToHexString());
      if (vout3.Asset != "accb7354c07974e00b32e4e5eef55078490141675592ac3610e6101831edb0cd")
      {
        throw new InvalidOperationException("**** vout[3] asset fail. ****");
      }
      if (vout3.SatoshiValue != 600000000)
      {
        throw new InvalidOperationException("**** vout[3] satoshiValue fail. ****");
      }
      if (vout3.AssetBlindFactor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[3] asset blinder fail. ****");
      }
      if (vout3.AmountBlindFactor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[3]  amount blinder fail. ****");
      }

      Console.WriteLine();
    }

    // [TestMethod()]
    public static void TestAddress1()
    {
      Console.WriteLine("Address from text");
      Address addr = new Address("bcrt1q576jgpgewxwu205cpjq4s4j5tprxlq38l7kd85");
      Console.WriteLine("- Address = " + addr.ToAddressString());
      Console.WriteLine("- LockingScript = " + addr.GetLockingScript());
      Console.WriteLine("- Hash = " + addr.GetHash());
      Console.WriteLine();

      Console.WriteLine("Address from Pubkey");
      Pubkey pubkey = new Pubkey("031d7463018f867de51a27db866f869ceaf52abab71827a6051bab8a0fd020f4c1");
      addr = new Address(pubkey, CfdAddressType.P2wpkh, CfdNetworkType.ElementsRegtest);
      Console.WriteLine("- Address = " + addr.ToAddressString());
      Console.WriteLine("- LockingScript = " + addr.GetLockingScript());
      Console.WriteLine("- Hash = " + addr.GetHash());
      Console.WriteLine();
    }

    // [TestMethod()]
    public static void TestConfidentialAddress1()
    {
      Console.WriteLine("ConfidentialAddress from text");
      ConfidentialAddress caddr = new ConfidentialAddress("VTpvKKc1SNmLG4H8CnR1fGJdHdyWGEQEvdP9gfeneJR7n81S5kiwNtgF7vrZjC8mp63HvwxM81nEbTxU");
      Console.WriteLine("- ConfidentialAddress = " + caddr.ToAddressString());
      Console.WriteLine("- Address = " + caddr.GetAddress().ToAddressString());
      Console.WriteLine("- ConfidentialKey = " + caddr.GetConfidentialKey().ToHexString());
      Console.WriteLine();

      Console.WriteLine("ConfidentialAddress from address");
      Address addr = new Address("Q7wegLt2qMGhm28vch6VTzvpzs8KXvs4X7");
      Pubkey pubkey = new Pubkey("025476c2e83188368da1ff3e292e7acafcdb3566bb0ad253f62fc70f07aeee6357");
      caddr = new ConfidentialAddress(addr, pubkey);
      Console.WriteLine("- ConfidentialAddress = " + caddr.ToAddressString());
      Console.WriteLine("- Address = " + caddr.GetAddress().ToAddressString());
      Console.WriteLine("- ConfidentialKey = " + caddr.GetConfidentialKey().ToHexString());
      Console.WriteLine();
    }

    // [TestMethod()]
    public static void TestTxidAndOutPoint1()
    {
      Txid txid = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");

      Console.WriteLine("Txid");
      Console.WriteLine("- hexStr => " + txid.ToHexString());
      Console.WriteLine("- bytes  => " + StringUtil.FromBytes(txid.GetBytes()));
      Txid txid2 = new Txid(txid.GetBytes());
      Console.WriteLine("- copy   => " + txid2.ToHexString());
      Console.WriteLine();
    }

    // [TestMethod()]
    public static void TestTxidAndOutPoint2()
    {
      Txid txid1 = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      Txid txid2 = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      Txid txid3 = new Txid("99a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      Txid txid4 = null;
      Txid txid5 = null;
      Console.WriteLine("txid1 == txid2 => " + (txid1 == txid2));
      Console.WriteLine("txid1 == txid3 => " + (txid1 == txid3));
      Console.WriteLine("txid1 == txid4 => " + (txid1 == txid4));
      Console.WriteLine("txid4 == txid5 => " + (txid4 == txid5));
      Console.WriteLine();

      OutPoint p1 = new OutPoint(txid1, 0);
      OutPoint p2 = new OutPoint(txid2, 0);
      OutPoint p3 = new OutPoint(txid3, 0);
      OutPoint p4 = null;
      OutPoint p5 = null;
      Console.WriteLine("p1 == p2 => " + (p1 == p2));
      Console.WriteLine("p1 == p3 => " + (p1 == p3));
      Console.WriteLine("p1 == p4 => " + (p1 == p4));
      Console.WriteLine("p4 == p5 => " + (p4 == p5));
      Console.WriteLine();
    }

    private static string ConvertRangeProof(ByteData data)
    {
      if (data.GetSize() < 256)
      {
        return StringUtil.FromBytes(data);
      }
      else
      {
        return "( byte[] length: " + data.GetSize() + " )";
      }
    }

    public static void DumpDecodeTxData(string txHex)
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
          Console.WriteLine("      - nonce      = " + StringUtil.FromBytes(txin.Issuance.BlindingNonce));
          Console.WriteLine("      - entropy    = " + StringUtil.FromBytes(txin.Issuance.AssetEntropy));
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
      }

      string jsonStr = ConfidentialTransaction.DecodeRawTransaction(tx);
      Console.WriteLine("  - jsonStr => " + jsonStr);
    }

    // [TestMethod()]
    public static void TestDecodeTxData()
    {
      Console.WriteLine("TestDecodeTxData - normal elements transaction.");
      DumpDecodeTxData("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000");
      Console.WriteLine();

      Console.WriteLine("TestDecodeTxData - blind transaction.");
      DumpDecodeTxData("020000000102b5e7e11dd2ae7ed6dfa754d406d240fe8cd0ab1e329cee6edbeffad5e54a4ac7000000006a47304402203d0d7240234aa446a08c1d6107789405c0f3499f4f5dd61fd7318ba58bb21bae02203d2e5a37c704c95af5801618edfb2184d80871b79a160f9dbc8a8e0a90467b380121030ab052e1482e9715c05301b07cf531d6a7e343bb508f0f2ba9126118c15be5bffdffffffd007d56e9e984c52b4e077487a711ff0c7126da52f254ea4d532dafd78748d2c0000000000fdffffff030b48263bdde648e0ba73cb63b44410ad1941fc1304bcba6665be398db23a702a300979f67b8612d871d2dbe646debe1c07717b0429f5afcc7889d84572e9657176d803430e3f6e47f856ef7a1b2928783e2ddcb8acff8e402e1d8c4c22b078e8ea36ea1976a91410eb66140b970b99b072d25fd4f07b4e88db32c088ac0bc322eb24c971bfd454fd61577b70eafab7a7c42f3b973cf57b3e56a002c4adb00802025d289a81f637f62c55500d6e439a9e13743ef7753d728866acc58b459b6d028a3e9de7bddcb400f3c1534270d9063dc465bd06a3da50278013a0ff4a5823b617a914862432e4a10eb1ca46c2e97525ab27a13abaffc987017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c18010000000000009da8000002000000000000000000024730440220761eb444887bd22ed0a3fc05caf4b9e74fa879db2b6cba70747f9aeae40848c00220070205b4817123234536efe00ec778240a87e5d8b5b9f9e155e892767ee922f20121026e3ab12d8a898ac99e71bbca0843cf749009025381a2a109cf0d1c2bfd5f86b300630200036507e368fd17b9db49f8f108b7dc78af4cbbdf67227d77658e2da045ea665cb0a34bcbf77b32c0b3dec83385b8d14a641e926951cf08099dc5e22205db641a8e5ca94ef7ea313435be5541e2b6a5b9b199750921f65dd1030fa4abde717a29bffd4d0b60230000000000000001ecb2010aa97ea3544b1ac0c9a3321a3f05c6accd4436f7944d670b15c32d3f0541ae8779ae3f6105b01085df24aa249b7d12238fa8a775f06815e2818eb4af3ef8f075d1f0d3fe89fd5567cc8c6b4cf4f48d11fea10809ca06f7bf47290c5182516a0d797fb43a80af28f221dd09628f21cb0b98e7b82567d8e28dc2b494c1dd248cc56a9307c39974da90050d2312cb2858e86de0d393ccfda7ba3b368729d4e9b3972393e05e6ccd623e8dd035205ba554c099948bd8992d20030e145b95e64c3d91f6e3217d099ba5a0a64fabe2f2172102097160ec40baee5f5db764abc2f666cc22c20258797d623f413e399a0377561633b68f2057ee74b1d2f1b040e49b5e3df38b612439d25ab332ef9aeec15e6292d84acf5a3faa4d4a7df5d9bea923340363ad0d6ced0e274adbc82c7cfb4801e37a16700925f6f2ff626f43c99817ffcf320070927037508c0372747ef66ba2021e0ac876c431e16a6919a77b5754ed3e321d30cd5e9df6b035d5842cde9c5e02d595ae299d2570b4236a53a0e5a55e5cd95d1f2bc1258ad2db2aeef8d36d16b8f7371cb38b13d6c28f2c8f0152b7e5a49282b8d95ac94d9a592ff0dd5a3fc1ec93fcb742c7cd4b67fa12ab3f69c7c4dcac75eb30a27a12bb82cdb714b6413386fad53609fc1cd455c33e127ce8451b690e62efcb09cc0735f4544dd288835db5d1dd731f0904a33817fa464fd3de8c09e235d36892b502703ab18e4037faab71cbf4ce02728194c4b296d9d1a1a6ba5702a0cb3741a19bb507cd2373f7ef865aa9a68159cf7963bed7ac92403ad7c8dbab0dd3ae8be3cdfc9be71f598195feb8bc332164c2207ef8b7cb6e42fc6501cf41768f39b849aa1b0c9e0f404a913867597cc7df9f88afe55c5e37d53aafde632449165feaf04a147ea3d18b3733e9d4b69e5478f42177486c881553c0c46df164b50814a5151df7b467cd366f9f5bf8171d5fb20e02e94b6f78f91b1bf6bb560cc761f4113759e1de2bc4e5a28085d9ead77ef95eaa481995162143125be8a497dd0d7e45b230c18cb0fe1c13de6849a37efba8a71b7cfab7a3b366de29f038654996c46b97f26cc04cd79b80e845a9f0fd9680c699c61eee46cca800507824d4b5aa053f02fb01ffc7bfa536510ff6964098a3de2bb57e07d7ee1ce1d03966821c06e7f85a0f517ec19eb64059d298ccea429d5bb88fb87aa26b9d97efe810be69e149a426f38fd151e37dabac83c7d42a64068c6d3772193d3cd5b139bc5002b20a046808c01bf506f82255f630aac431ae21b508d839018a6379ca53f27662b525699e9bc316984648961103006eda1de37e71e18078fd79acc7b161297712acb9a552f5f299161248fc328b965251501f44de37acd8ea968a5d38583a9a26b2fa7ee48553bbe24a4ba7ef730eaa741c06ea91367e9eef3840d4dfee1538031249975e83652a1533479b591106e5c2607161149c1b1f1ba7839a105753aeb4b899efa2064c9c9c971025d8e5572529da6b42ea615246b9910fa6560323bf56a6cbf652991145451c77a819e141594fdaa9125cf01e0623a22bc8015bb866e3c311e170bc5f8ee86b15e6a9b20c9bdd240ff75a95981fd03947f11f2c2baf01cf5697b3329c88c896eb508f6826ae1df45bb426156fcb20129f33af4880a28b0d7e894dd293a21527743ee21ff6981df4c875f827aae158c105e6f30c66958e10a5d8f8255ef5a958474fa67b711735deec717472667c1cfa068f77c7f7b2a7adfa38f4e465bae2658486fcfa608e1d03737c9213f68ee04305b64312ac43a1a9cbd16cd20daaf13b1d58587f075f72cf9bfb121693a8e27993e334f5a1627435eae39c5a23c7b0d880c85e672374b1eb480e6bcd8c0b505c94751a36a26fdd6f9f032681ecd5bc75b618cd11489863efcad774a0dab5dcc7c4c47583f373222e02bf6e21921d7cc641685ae021f049e5bf61ee9f5c98d30d3fc0f9fa112e378950e2f33195f2ec245ff614676b09dc932625a989089a6ce0ca6f36d49093d7a544b3255658ef88d49a6eb200c39caf55548a0680ee551239cf693fc2fc1be571bc6080ddba18f75ef318e329ef3e721001772af8f0b869d3b83339917c73d383f2f0fcade5e8f936bf09112c5664e57dcf622fe69f467a342cbfe692882fffc31d22ccae655fa2bc161718fd06c89395ff69c2933ba72ccac3505396a69ff899da2250f6cdf0031f6e7c6e394a0f3a57d078244f11c70bcc5a93aa4756749b12e188183c82aafc6da92236105334013afd315a6603713a43075b9d9fff3280a5a9de58a40adea4958edcc404ee4714c45a82ba16dcb199739003986c0c06176ab0569797f3be6eb7ccead4be4905cee9eede3f4ad83723f207e5e467718bd940b80424d79e3781958f22f778f90504dd5d374c898520170470e9fb4789d3af82f8e61ea5b13829138ece20451a35d8b876694674c8d891bcc1fb44f188eda12bbaf1861480afa931dde5c1017d8e75d8c732df05d4859d4d0d72cdd8361bcaa8e5e13a2dcac0cc6a93bd94523ad054de6fe90de0b69f5410b3c8e44fcc32a9c7f475e42ece13bbd94c1d86986fb5db804ffbc72a51ad6fe058a1fa50690cd4ba4f93fbde3bedb3e98ff3803e3947ef3f442fb59932e5bf3e8ad1db8066a947213e9f956c4b615633668453dd5ad6a3db9b5d9a60ba86881c2c414da9e566a2198c2b1041432e21f1098fd904eb6cd06e24adfe31c8151d238598a7f399f6cb08090fa786d76dbc16c5f06ea6bc10e44ab042aa1507fc290b9185dc17cf78f2eb836e89eb12ca42d30fb96c097e0d362e91c3414606c5f29ac1683ae90b2ef28cf0124186135b46780caee8589d1b733f7e5ac74488b273451012d46c72e85163b428a056b4812595e046b650df7a08cc343503cc1f5bf828c8859854b5c61629d212a06acda00ce4d88b4fbf2fc0e3948d16974a9aadc38c61fb04352896e2926963c60551fc4e5c91db4887551039718321fb4f2df41cfe5f868ee0884eccd2c8854f5eea49e5fac2be54fdd2908fb1c24c4362482a44c82086d72907ff6a80cb8f7be17ce86735681002dd0031d6d011157696dad161a129f7984da3f97ee43718e9c67499d2cdc8bec6f255bd1841ccea5870d3d20d3b69b507e3b364d1d33b4d86dbce407c1e2b4bde37724ed022dc9fffc4d85a80aaf7d0ffcc0d82783f9238b46e17d66f4532a9c29f25df0fcc404963065d776f5d8f6773806c79331cf2031fcd6ab49c28a1b6a10be13a8bbaa2b4d8fb2d14d74ff87b06e989a7a141b9775cb22707cdc5bed26f269f0054245533055c365340e162fb7c2fe38e91afba7b0c2cb222816ab0a5d68437b882e997e85ceb1c7049722643f55857a23452a3cf228f00393bffb2bf100b63bd987f550df8df9a2a5d66d7f642d1e31e81058719469959aefd7f00726da2e911b17f2a892d27a2e2ea3b4c18aee0317565406c4456cc11cc70b4303b04fec0193a3324a8f310f2004fc7b0676ba75e31bbd728bfc248acb1fb242a8b2ac6b349efc38d4e17480da1f45b3eca80cbf80d6f543dbaf59d69a0c5a0bccce0e15532252b00a5e11be765d15bd6308aedfade1c82e9066ed0a4a985d332b81bbdfeca78faf31c96ddf4651218b40bd81e6e13fe1b088ca76ee9a2bd7676f8792c94fbeba1d6dc7b98d880044d3c424cc5e724db685d0804695675129b08a708051c98dae98fb3248bd382ec48ce499a69e45b4eabf2abddea3099c006179207152fc7e63c11edb5d8c9c50c232484636f3240042420b6380d397645c6a2e1d58954947f11863f59eb30a57cbb9917eb6d92c0a93e4ea3f4a0884aff0ee08b93a6603b39de99beccbea94c273786f253904b74abf4103ae099a95154e25d23159420dd3e836c5cebe2772ea740fc0ebbd7a1ca45314e06fd85d9cd98235116c7a091120a2020c9f5d9f3952c44921f934a589985242aa9658b9cea5cbd4550cff46b952480cd822eb0a94029570c59262ca0a6b2f819c9734355d40919f3a96b443f40170f09954598c36cb9fff3356c97829963020003964663a99750c551dd2229ea4fc24702909f4ca1d258e58165b97a086261f553e2d5dc9a23f4231ee4b1c7f3575c8142e394b6d4f4cb0810ba207f400f3aa3d8156632f18da696843ad6ed74dfce3f56feaa97a35ec49b3a460cee5083bf8025fd4d0b6023000000000000000171df008e5ce1b189dbd7161c603db628726b84dddf1083d23c43a376511634ea404fc5a6d1eb5a95b767426e72066d99cdf533b4b075ea6dbea840796c632fb01d2eb2d2fee92b909e269552c521dbf4fa4e8f123ed119513edd066ad7ab0dfcd87bd3b2cc64a665eda4278f922011f799ba6353f85daf9020e4b95b7ad4717a233f474c7e27433ac20ba1066c66296819a069d909d1ce015851286193993d499e0ed4404136dc18b54ac9bee46c34f4a2c26cc9fc3bc159d172a65ec4589546f70d51f0025c91321b54bd80bace8a363370caca7dce096d811f8e496526a370acf590797384d0da382249e6024fe2c0494007689254e9a4c299758c9b1fc6e6865f98b4e04630fba0aa25598f0a0fb339559296043243aedd672b60325820f2b4d88e5ff134f735e0e4fd2abd0fb258b4004025eca31502cfce7c6d879b7faaea94552e31d49d32df37aa0881f423242d472d29e8971d6db88cba7f92fc08e27d3bf742ae270a12eedbf73fc43a9361c94807874495308de00e3c1720fafaeb553ec8eaec65c41a61cd9110894269258f216ae8d23af94141eba5b92211f7daaaae0a8c2ef5a6d59c003ceee7c28414fb5c142070da9930e404bb0a33dbeec1e06168aadf715c5426197966a2d56e172e4fc6f7fdecaa1ed3b1e397d3e83c3d0013b15a78ec697e635b80b5cbd88e2c78867fc4cfa274f09725865edb109058e114502a6d9952c2e8429287e509bdb57e728d4d7beb5c8e73cf9eb45c2930ced482dbed0a8adf3e47bbfb0ee5ec9c1242f254c02b5ff4f54a4b0bbec240814b38b1e20f24e1505d22eef07d6fd25fecb2ba3067ffca727d00d70b070cb0411690479b65f61eb6b357b5f08075a53340caaed328a5af007f7acc2ead770fba7a06bbaab5584eb1c8606e1e6366c640c202c22c34d0e74cc4b14993f11ab04e82291f8f6ce7a2c1adb00e4bbdc7e20a19a39184f0f53726c61d931223ab8b0ca81ca5592a4d44e28b41b00bdcd37cb02adf31c0536f6fd48aee848f1adb27c3141d21a5bba74af0241ffeff0548fbf29e278aa3a0827179393b3a0860557aac767fda675022efffdfd075c07b96ca27f05eb4b4a1f2173b8a0595b1917e30fe37d82725dfb403cbbf9cf84352209cfe70d4792967cfda5e1a7fbc05112048a760a215f2a965b8cb9850bd8544320c3adc30f8dbb53cefe0280d9b3781c1bfbfe7285d6fb91d0d8c8518a7cea21da117e3fbd8570f2371658cd0db77519ed550e700e5c362ffe688d2185b878f6a378005c174eed420b69be5aace92b738579f1d218496f789f4a935e522b3879d8ff23b755c1f40702b11107e76a8a7b57ecb1b36a90c84183fd6c69c35e52493a077305359c9572cb54dd9c3ebc0db510987f4591ac28bab490d34c4e40aeed78c5c8ce2f77119f833a5c882cf7c5d197dd8900ec1520443f2154a1ca4ea2f8056182d7c6839971910fcdfe4053fec4674514be84256d69d3f41c94d343a1fc3778e47f29fda71688ac6db278eddd1b887e0c1e2754bc5e0061452de03ac38f0fce3297246ada974a2abdee4becc12a7d0245439201d5ead049e6a5796da02d3ef79741e372c697f42c6b26d8fe06a8bbae8dd7071d3fffc79b947bde32f0a70de6688820c1f9c240b9d775299cdaabb14f0c3bf9cad1d0a7f76b7a839ef3a54cdeb9de47f07a51e84beb0ad052f66fed105b3acf6cc7f51b19a519de8ea759bb786d50f6df5a99cfe838c7564ce137929e925b9d4a2a515aad8d31ee48cfe1b73bdb9e08020dff9f229387acaffdde47f9dbf1463007d169f81aed7cbf87649fa8cd8224fbc815032d968157693380f9edc784758a14df25d14e6f80f7e273d5c9843ad9cf9c81796c0c9361a82ccf1a06ce1f880aa9586412a947eb58e6f4e3545cf180c84b0aaff2e4e3f947a831d85f1873a9b1f2e40079df0e98579a6b293690f8dd2c66569f6e55a1b8fb85482696839b53772bf2eebc05a5346198b191fcc820f20bf6da8602a65287ba0c6c0206170588707238d148a829692b60b8ba3142c8a24da7771bcbf02ea9b765ec0259f9d2504a25cf9ff1f35d02ea6fc43b4c7330271200a52591e4367c86b44710167dab01558861ec2b7d5da8c990d9be1590fef5afc606db732633ac8890d00787181b5f38441bebdddee361997c9a06499b72818bda1c20a7c4fc666600a86ff06be0e8e87ba143fe6a3871be9433869ff33b3c67b99c5abab03a21036636a3e14df121c476753d6dbf6b45bc9609e440cab81452a1bea7c8e1441b3bcf3e443afedb7679aa09d9870dff0bb72d41ad5372c94ff6ab9f28a58576936b61fc9cd23aa1b3191bf5f590e86d2595012fb82dd4dcf6366d60c3c9380a5ceb60c525e9235b08f00c09ec06c0f760e64d703cfc4afe222d44372109021da9ed278837adc6eb82183e686081d21ce496a83c015543c032bc2bdaaeb796ba89c92f2bc66742cdae9fca7828eb9b27a95457d1f8f225b3bf0a8c52de25859ca45c8e97a04540f4164e07e7117d8d877c7b162c146aaaf32bb7426257c26faa35187d7073d1d06272215700ac6e419d985fbf26d58161f5424f1f57b28607ab1cb87d5340195de5b957124fae287f361f0b1cf4fed091620ab3ae70fe7fa0f83ab09add12bfe4e89d7955e66e2785024ccb1e179da83fd9c2b020afe73dfb60e5454d3ed87dd85c663f6f92a3e84bc4f8bc20c9ca755477260b51247e453541f69faffa864403a9acd5ec3f7e9eb7c700a09d1c25d58e03b25f8dcf9dc15a1153a2b0218d4b64d2bb56cf57fa62c4d1ea1a3e5cb9564a23f27d1b56301003dd62cada5312b15914a5086a8e9168dc0d493cfa6777cf7bfafcaadf47f575966c38ab7ae2149d08ec6c703161938ed75fde6432052f224545e5729229fb13f70e57d6965c1a5f2a191ba8b60ab934a7c6928d76173fa1d9804ffa2b7384c229f51c1405f34f1a089625eed55ee36a2ac83a6d58e4c7795fbaac004e60eacd5c8a5fc7e775cfe5528bfcafbf3c2a69091e58a74a0e1ed19031332caceee7e60a1955734155764d13bc457bd659485f6e21f06db6bbba3ec13e1cf7f3dde73b07896101740905e2c745019417915279f130115bba798bfb08acbbde629796849418e16a62b2cb51eced7e87ee9b3a083faf4011730f964aca5632a08e2aa8fb662f986ddff057d677ba1f2f1dc2e2085b561c8b24a2e65e47270babe6e7350a9e58e2a03b43f544c13c00d8b956ba65e3c4c3071df806d69c3ae198ef4f229c8499fd77a020aa9d36715835249daa8f539acb704f6a1d489137b3af0fa8991606d4b530cdfd85788ab8e5c899ff0abdd02d2a7fc9e74d7e9d2ef2fcda34b810a8b819c00c599aebb6f14efa489b7c965f439c12acd805c7d734a30210a3dc25ed132aede74c0c043cd76dfc6c632385fdbd817c4329dca712740bcd6dd68b164af78c7b048fef6fa7ae0d1da489591abbbbe7b81e02c054f7a0a7a9ebeb769fd494167d0b3b8698842f84e406204bc2ccc373f71ea7a83912e6826db5a0371d80b38c6d536ea88a3aaef71b01721c9817a93ba6d95c4c239ee37c75f746680febfce1cdb5a523cbf5c6b0e2734cb7cfa1133c918aab211daf63bd7f706e69cbffce4603262be927aee1d8c662f3dd4735f7551f1c1b7382b0602b6f49724371d6ea54bf8651ce2b0b76d0621c420cae8306facd7b213e36ad89ccd6c9f3eb5a233cb9391cfa6443f38b489c70460dc513d0a6422668ed9437905b9c7eeb9b1c5d84e9f5bab02252087ed3d05dccc7eb0d429cd3a0c173c5418cdf621b276b3770453b32800dab24b33efe07991802ab0746f9170295b608eaf6c76450207648b1cdeb38864cae39da3b55079d6b8ebe8cdb774e419a19728495a0da0ca039416d0b16e52cfafbc1e07412ad232b749f42404dfd8784f5f692a5b48eacc40da56e551809a2a7f1b5e3dd7de298d16a986ae4d476e104433f840468f16efe2a3b78fb5418c9e738ec13911b2ed98c7f751bd7710f363d89eb69ef911212d3477104c4a05336fc29cc0371fd7b30000");
      Console.WriteLine();

      Console.WriteLine("TestDecodeTxData - blind issuance transaction.");
      DumpDecodeTxData("0200000001016902d7cfeaed59a61432674060dbacf89aa50dc4fcb5980c55034ce75fed5eb40100008017160014bf7f6fbc324728d857801e7f3eff88cce289cf25ffffffff00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000092ef327afc62b0749826df12d34412566ec1743f75ee421b4e4440f0083bdff8c0883321b72665e9844cdb8859a743583ac18623cf6b78e512fa291912320419104040ae7be1f8a5aa41a98a0fd5ad39efdcc3f36a63adf5849836b08aa622ea65d25db0880596e89690a32299914da0a85fd57db46575663968243b3ba5b1ad15120c402034a6d652db056e0364a0eb997821d62266da79409d995f9d39d011ba0daa5998217a91472a7834df62071c0bcae9e00b00cebef505b0cf6870125b251070e29ca19043cf33ccd7324e2ddab03ecc4ae0b5e77c4fc0e5cf6c95a01000000000000271000000a364d73c6e121fdc4ea87a0228180a521be1e03c3b287435dbd06536ac6bf4770080e7d14989746f3c5d3cc4ea1c651731e9c375e71d487665a13ec8ba104b14f44025204161081bf96756531f5b8b00d85f3b831041be2b8bd108a48ffb44d9f014f17a91417514a45caadd530f301c13faae6003c4ba1cfc8870a9033c37f8d1151b8b87c003dd2b2ead5f85e3ca32340e9768a80ba5a840f9f4c0839ece197daa1568947778ed05f8d499e2ecdf326155a93a8052bbcb591356edc03395b4c5090e3fcbb0c5c2b258af7d078a9d4683d33d7905fceae7f0f7033511517a914b721bfa4e90777607c95ad886362e4870ef6072c8700000000fd450b4023677000a52b903bbd128b03b1e29e4d82ed0b43567843cc5ce1f4f979c7e631a1027d48f72a782f9870c1d5eba05b5e582de397255349313c54f5766f27262640c2f657d49a2faa9de02633ae9a627fa2a837a85252fa4f74b6a59f2bbc8b8bccb6094705ffe112c6b0437ea4a6877b53e53a6547c9aaa46e5a1f4fb8c1001d5f738f70d29551b6949bc4a2de85b842d6abf38aca5815c4ac5a6f5fab3a0df52ae5abf354cebf6f15f6dca2a855039ab6697199203bc7a1599990d424dd9bb6c9712af2a49c3abe609aa8d31a9e214fafeabe9393d9dc2dfd92b4f24b19a4fdbf376bb501936b9b875e822b7b117aa6c114c21761f4240162dfa6b98bc5f22c524ebb54130b0e7795663ef7f27ac1fa3daf65d85a8ac75dd98e7d35f14967c8a9a120e9715c8f2399cd41172478fbd1692c961153e16c677520269309687803f070d8e7c890b6f5f2ab79582a862f4ca4a49cede94d981f121643f4710835c1e30d560afb3df7b1020e1530eec31094e346cbe070b3b0e59478a962c3e6126969906c1b39a17667825a93c5882e63eb31e9003b5bfcea0c8a80ffa1ad1b0cca2b1d37382f6307937a6fe41f1a49599d508733b87524cc15ede76d3252aa571541fae83ef75e93639db72a6e54bf27fba09d1ecb8055c962475b996058e6ca85daa7a80d23a3903dd71b93d986ee6be8e27a14f4cfba3e2d953c065880fd715c739628618fb02cce6c4698a0ffe9e04aa116294e9c626e2e63cbb044991e3f29b7215a894cfe8ede4c5ab52ec47b6f64b3e69576798f65a4c3a3943b10cf07723af40bdd3ba6a382154605fe761a6ce0115c6a02e639ace3b44feb378262fb1c18ac2d70892195c6453159b9eef9aa86f8b300e0ef3408605229afba6cb985486d88b880f53f9019430b763d8fd3ade45424394e3a66e9c4d7752e63a1f295574e06bff6974fce8b8385fa597d9c731e972741e80dd4bf7bf6ab2fd0ec7d9aa1c9488b08572650967b373d08a75367ef47e348b5ba9ab90fab85095fae2b0daff44cf83883084dc8d0ef0adcdc95e74976218fbd7d7373fb0646786f76228efeea9a9e9e547e0b11f26b933cb41e5858932a5bcb992fa8b471a061a8379ca053573ff525c741eb0a51e7756820dcaaf7547afd71f30dc06495b6bf3310a9b4277743e9a17ffb5012d20d01365b7090d763c5511f9562437df7b7ede340004f15321cf0ec2994882f3e02114fe44f1805f58e0af310f22ab5a68907041f8f734b1f33e3e8753e365120bf2fb4f2a2a597d3cf2c8839462f35eff7ab62bbb0a4433876bffbf5532c2277ca48cd0a3eb4b773cefb46d0d539b177a694c85e02d70e99f14774faea2aefb65e639e8702ef2434ad66453c1a9f5b00760aa372cd432748b11ba1757b3071e6169cdac6beac4a160b55ea02d1d50159095e593e45683e0ca1bace4581a09b086cb74187de61ab1175c0d42a9601227be8422803d34789219b150a4de6a4432d28127195b8b46ef721f965aa0dfa5a939f85bb2fdb95efe882b877bcaa676ae964ef37d8dab7d5d6e2b6f4faf4f811366576a9d1c65c36c77a8daa6954f1395b1009c859044df8809f5dadcbf1dc16ec5955f0671cd3c499296bd494c6c0e1f0ab0012018e90bbf41d760c88940254258f92d6dfb6998b4afdc4c72487721f0e97ddcc3b28d941c7f2458d51916b9b2094e3c2913df3c227f8f957e5f595264b0da17469ba1e0794490cd5bb1b35cacb5c12152c8303ec038802ed334cef24a28e6aa38708dc72100879f721494a40659c41b63cd2c22d9a55b07aefc71a5532bc03564fad6767a41d9b52bbb1a323d280041e44eb87674ee964c9428e9ef96ba1dac685f8cb8fb950e34528fff02368be76cbddd9215ddd5e0635eae652a0ce642bfdf7777207644f1ba503b409faf68c48a9a31142af5396e4bca8e67cc183b30895a1893b2901e91b4e784dcb137b7d643ffadb6abfd8821ab1b4f6fbe20275d42ff44da02b32e00f238ba7b326f457f3ce70a2b784d4f165e594f0a27d21ef187376a1349401bc6d23ec2e356ce23e8e0be87f599d335e41c43985ece05f0dfe56d945aa5c3410d54a9cadfad6f29042bd37dcc069dfa27b2c7862a01b2535862d96cc80a0bbaebe1f673ae2daebfa133a3e1d7dc959fe062313ef9813f4341a5aec78c76a36d95dba906719ac50973cf7b2ba7037f489732bbc46b8da67f14a8e0ac225858c09f182c6c0b0540f9b69bd03a00c055016afec4c271cba5266255858c87516915e70db30ef8821f6336b184d8afee9c81ba51aabb5af9af9b473eceb9e2cd9076eeed17a5d423302008c63420d49e21147f48de7da5b69185e9733993a3504903a9df6c9e48f327ede7a9337c4e40c24739afe1e0b22377c62a98f2bf9968833af3f7208c7363e3702801c1145a1f1469875f0496a14ef401bf6b1f0aa75f77f031d30f415b514a73905bc7d3e2127ca88ad7969b08f021ec08fcfb3d7b23f1f22e42a5f77aef6319fd4eb4ff802bf0aa8b770789b94eee957636d23a303b07e5837cd9db6744eee86ca00a8ab63406203b10029ec27cf2551a05ebf7382d64aae1011c0ff6f4c98e3ca2162d7a7ac332d54908be1ff91b017b38786809c1a6007d2c6e4fee8b8162f2329ff2340c2302efb08fb8fadb5d84b6c2394988bdb69c08e51c2d7e003c97330bb651ed787584385a8cac09b16882c0a65fcf0c0812e9060a3b4ea443d583cbfefb0aad73d9d6b63eb75de3b5a7e04f26639ddd29e9047b6c0efae2a92b104157ec6164bc9c01249f073b5e42ee4618216f955ecbf0b7f8e4613159c1ff7009ba36571423557aa7807fdc4a07427ee3efbd67b645b3fadea1786acde3c73d65d3ea2743cb965fce112508f096fe44e6cf6d225b74e2f4a583bc8754f482eab453b7ba1472f75dc8892d2f0641cf5a84fa881890b2820c731fce87c6222d3e2014115ea567d0ec4945279bae6813403a46c77eb80eeefe958b765c849e4253fef2e8b474fab5b9df7ebf5ed8fff15063734e283a05e11638f38e6b0c375a989b3866a7122a1e9c2847dac65fce97b240c0bf0748a11aeaeae38d5a7527918c01357e06ff880ee491e96f3dbe75aa2b7513209dbd0b628dfcad3b9ad93d0146ef27515c1b50d43702b47c12b7bde5bd837fbcba97c814f616f7ae97053548afb0c75d1e6fee57bf6baccf9c894dc028c83beac85e1a99304cd76becdd3ac18b5995ea913211440665540dfb4d84706a49cd7159afd36d1d75cf2f2f317a5846d5a54be5d0c5a103a35ec2a370b4b81b11c61247e3178413c18dbf0532c28409b963a6230cb1290d0355d767f0c81a39662a65de72a002cb39e57950eadf93744a345cbf663e5e0d21495605178085091166782188839b462322e95a9f06275f9fdffc51fb7df79637ac388d9d28ce5fe590f99b16f7300756053ed3a3d990db0dc6f2549a3354cab2894b14aad86d2505ef574af0e12b2dd32418c2d5fc8106524b8b85e1953c4459ad7cd36e42202874b9854a0b366b809bbb4ee65757e3876877b552315cb1f0977e63b196b17b7709cdf937733f0330f19fcc192ac87d8b3f76494b38f9d7da166ce8f068a11a679ffb0f58857b4aa9d951f958db0e2001c62b0412de553cacf95ecea11a94b599188cc0fae8c0f404c4591072a08e015e235409716e048c05c208e651580606cf842110065b25064aa3829f916f1a770f5b7c07691612548df3b39dc76d1185879ee541bc2089f50c9bd116c0eb9bce88c55b3d99f5eb5344ec9b437e164b5c72dcd6daf765782dc45c48e572db8c551519e3f0ecb62761d80a7ce1fa319074c101710d4015af4017722d95dc2d468dfcccd8d9398a17152f054fa01730ced76faf3f12c1e5ae062682c74e823abfe59710391c8c7350f303f0c34fbb3bfbdd671f640b4a0eb483e64fcbf560f45b8d5bfe860bd6ce448229febdc3689a66761936283147495e4f7c75df412eb91bd66e81dfbc9f04852f10429b99f03429964c0b83d9d3b9a2597933be5786a00b06759bd95cbb0e52243d37194fd450b40239f26013d1f1952c24c7ca9878a6042683813daa766a6f6887578f1a2f7be418273a0b7e89aefdf3b9bf0cdc28f2e71444825e9978d34beb40e3bfb98497418c3a8386aa8a1f2dbace4ff72ea1e11c00b4b1474ecd934ac6098716d11d7fc83364f91908c12be70081186786503f85a9f7654f7a04e927c3df7bdbb9ffb2e8f7d2174ae0c18bb80adc1248bfcc65ef5b7599172b6428684a8f66fa73fc1f70b4f3f12e6644e27194b0fececc73d32ecab930d51555ec814ada14d1f5d76b2d3b03daf93d5b390314288dce055ee4207b4b2271af3f7351f8bed0de0796ed9c370fc78031b9c60937f786f5fd7f3d3bcd9e0adcc8aa7c0d3af6ccf733210222dacd7422f4ecb5540752862fbb710d83955069b5e91219256f3def7d6bbe7225be32a5e4a9161f7a317c1aa159c18ebc3e88527677e4b293db135732954bbe051681ac2eb96c9ef53f7055b483c4e9a88c5598037e233343986eb9a6ff98d611d8bb9bbb5445e784e781f8b3078bd795b8c24046cf95edea799286eb0faf952408ece194159c361509d2eb7dd0eb08f6e0cff96d6fff59fd9bde2825a510bb41a637c3dc961d268a84d7d3610eb5900a48b9d2808fdcf7570372372aa9f5d0af426d6340a8b422e18dfb9ff92439db30d548f727afd75555cdf28dd4a3f8ebafb680936c544bd2d8ab5c42be214e6e095678d7c5c6c4b56d8e1ac5025313466992eded93fcc365de54fcba323ce273afa9d6355747d8f03bd9ef16510a241f09a94c19eb3e315ee52049c4f2bbd3938e1922a6307711e2133ed442d94755cb4f9fff3b12c705bcfebba75c42afbf09f34cb2124c09462ee81805ee5c4d1f50145ebfb6761edac73cd3180d470c6afc11be09c4d82a95cb4a292ae548d911770a484e56f6caa7a8f411e61d3a11918a748c29953823839813c3fb6b7b5d7a19657fd015619dbfff91688a5510ad3a66d23b44f3182f2bcca1f0a85aa295f1b8bbce15dc8411a7b7ba01a20651924b052efdf3d686b232ba54520a38982cce05c748fe7138655c4273e6a2c2a1a615549635eb9bbd8e3df75c4fc406f4321b170dc29f18b6218ac79f9f9f83e9b126231679ee5863768b6873142017b3e4e17f18be79d988adfb5d4018481d3b07da9b65f95d114a69fda0702c881a0b61b3698daf0670ec79ab22e6862d4f2f369eef35255d6a58dc6d2e0751cbd4eb99cad1ae62aa92c0c80b971ba668bf6d089f79a3105fa2e7099942bace83b769594d9cad7725cc5b8c2cbf29c5be91b5f00e20763eacc1cc7dea5875731de50b13922ec00b3b8d31e5f26c19e937062e71b0fd65aec7ef49e174ffb76edaeb9e4db804e8a508a0dab33f6265de4384d726aa2ad52be1567b442d2e0ed0baef2d3250d8b60e72c3649aa165764b299c694b1cd27b80fbdf8fa25b64e18b6769099e3d3e4dbe952311bf75b92cb3d1fe2354387cd0b732a8e88a251662de9fb161c7742a0eede16c4335faec57b3c2933177f79b66cadc847fb8d5b01680c6ed80705d26a039f1af52675803ce97a8301ad319d3cb4b85ea4ea8220f8e6d2bf3547e3b6d9cc3c79db4a2be3af48c3ca0377f85e38c21807962a6137686f54710ea5baf71a345baec6737650fc26349bc967aa5cb4c046ec5286410c57753501e616684f99a31f5d883536952996335333fd23ed4c247239cb51e7ffb922c659f2939c8fa6f4934fdc99623bd3eaee99f4dcbc4d865b3b6b3f8286f0ec64e870a1f996eb5d69330928ed32de00921f68bed94f2e8d9dc05773b85f8c4cd1440d027e09cfa91d2b4a870f6ee089421de67da45b673b3c70e086ba378017a351e16098d2dda6f5728f8e4f6e1868ea055bc7788e9654bc03bbbcdb75e6c9005bfe861403d33e697c3c1d401b505f1ba7af879f02e2489114de9bd7a77304929c97b8a205ab92dca9ada5287f361133aff1cc4ddb49fe6af3f619cf7fb851a087beabb60916c64f574ffe95327ce3f60173b4a27e9ccbcf3654ec108198f5671e6c504347d9225e9d0e6298475d73c4521a1f8c8b54ecd5a2642ed1c35d348c035c49f851f98a55b756b6af511960c06fbf9ab51776339909727bf7cd7f899461b74313f75debaaa83b7bdded023672e6084b0848e26ff0bc4d2319d4f0eed92a365078c12b440963b7d92db90bf9ceb37ace8f558c446b5de704733d76d4ed88268f6c3685fc791dd46a05b74641d1a087f51eaac43e38de412926161e0e6cbf25b7c6e5971707d40d685b9d77bc58bc7a7e5ae22a07852b788729c32b22545a8fb5bc3d6d20df85df3ffa4bd7f6ab3812decf7362d0db8b60b4a2e9f62d80b46fd6c42138b7d2d5fd5d132f6a122ea88f110d7df5966a82a2fc6e1a9a1c56966c10466045bd395a0f5539b783029b02f3a261df32084b26f38712974fd76f7444fcd6fdafd66c74e43d84701b3280a26ba55e7e417ad8e512cf7ad568c25f37582f5de62539ac659437d525a0a6d547d404d22b4f4dac4dcae9579b6ca96a48db0b1abd489d4f89213cd4249ab042b6cde9776b0db7db098b3710286a83df75778350137972fb74624b242c509c9810f2d3302981c3dda1c1a9e676e52e1e72bfc9efbd0f84c7b48875e0a9f4387a8aaa0eec9ccce2c8db2b37a5b8826de88c2fb4fdf29947079a78f2661a3b92f25011ee6e0607d5db1d1a4d95661e678b689f4f44635d62df8aa82364bb2a879638f255c426d35b0ecd07a32a3a3fc94f6a489aa26448b61baedc1481de28661ddaedab0ee17f8a44b52f7c098c29e42785be645d885f2b8ff9ce666ad6a0a9ecc678075ba1d03306c1f80524fc699933adf76354c5f0d65ce827e54026706bd9b3825cbf85eb427f3e2305a224756eb46ad12109153181ad0e394ad12de3005c1767f2e1fa5e48b369f8455714df4181a8f8f6b6fb1bbc5653274ce648a54313e30583412a695e4eaeeefef038185bce2a1b2a90c3dd4d93e781e4ea1cc2afd4498880c118bcf104d1fb2c250e8263d15f1a82ce6248fe08aebe958c1245d5d82e58e6aba34aa70b384f051363d0f6eb6beae046461381351f18427c8bac041f0b682a02caa868e6f85409238e81d10de12fbe4b5f73941ed5416ccc3a52635978b15d0f90e63e0e25b2eac8a2d0acbcb7627abd13ade4080ca1d22d91661c64b35fc4cf158ae903c5a2bea91f61254878a8cd468025b947436c51d463b090b982f3f5a2af6127ca2c6e236a1d2e9848100bc96f6b2acd6cb7f9ec6c53eba286c26a8b657a3aa426440b1e7700b08e428729ebd5377a558dfcabc6ef8545ed161aa288fc25904b0b6f4306ad7ce51c65f4bfa0e31ee236b952026b8abfce9db79fcbfacc991163e4b8b9dda11fca65a7248997d3844a6bb6ffcc9a82b4d08f699ea527a14a9dc953dddeb2d9db33fbeb52955eb133c60eed85a20f4f764cd84e7955d46a73ab56baf5567efdd92ba6bd908a7a8fd067177170644e9e018ca9352aa641c787e07c78c137b62de0035d9b6ac8e0858ded0a24d9eb5a19d22dfb01fed8aeaa1512c6ed8e50b68332504bf7f58652dff5120b0fcda5dd7c6cbddd161f4ceec26675491fb9e17e6298f2b227f29b46155c9e55b600ad0a635921f0ec15e9b3b02d85fe1f00c05848d8478f97851aa67de15d48d9120fbef82abb1635c417245b2e330c4c79498b92eb471946d4fc2639a900f5b2e5f71d7890b1928d84da975a443d9098198ab5b10769462f6f307ac63d999a8cefa8bef60d4db39d3ae850e9751f1a4375554c54c41d1a1f48c96b0b4eb99935d21c7885b777012076efacd7fa7d69eb24640a37bb3a348faef5bb7462f2762397566a43fdf8df0fa1afbc6736bd46f75b19de822ff5b1b339b55a909b15d793499dd5432296a33efc186a5613842c47cefe1f1945dfbcf7e3deef292c5526391c624ab436adf674da17e8cfce18097b3083d170d0135dcb0fa0afe01c5e2f8e211032345288467258e7561389b6aee9271f645960d81073bcaf70ed72584406cc10d6411fe5f10a86be04f3e37a5f455b743da37130a49cd6347e861402a6536d04891302473044022036503a4f4c4a2e65483c0e762b561e6b267db0c498bfcf2e51a8cd8844015e88022031e9b44468cf9431f1fe74f60b755bb1e273ae462bc4024c097c01aeda7847bb012102ab849ed2b1e6f6c558d74e5c5d49d160c00afb6d7abd5b6c76024b3a85a6a5dd0083030007028daee23a0e51b165be98fc15150972f7f95a09ff050b9e95ba3c2f813293beb1e7fc051fa28f5a8a34cafc3d7e3b5086e3f256a70e1ddc6d9c6ef9c927d3a4e2b2b81f73f27137d0f77b72987d8d9771627abe02b47006b9eb95fe534ffa574798940f1ab429d1199fdacfff19ca08473685e70b10c7e643918d8c2924752dfd4d0b602300000000000000011cb6011e59d40a3b71d40f6080bc390592c5e07086dfb5d8008c41f35a9e51566fa565c66f41f3bfbb5afbbc67e6925b4310763f20f0315bccd30448c7fbdff7f2a467ae8081f7684d86e85ce77326a819d6e43a0b17701bc11e5b4a27e36b957348612f0dbc036358681031a1872e4c6993b9b8e1934d41b68e9123bfc788167fcfed34910378d1de93ec57b61183ea73643621bf9bff9291afa7b40851f4525c5721832d3f7d87c9f3674dd03f9a4e920978e6cb1d8388df138d15fd40cad1a506eb24a8ba41d49d93b61d4fc3afa9a0dfecca2dd882c0094154913c17b182abc5bd112eaed535ad9c210c51e9f82872214426bbfba664f71a887f2c643505bed39e31150c30722f8dcf070433472ffd67d965ac9df345f72ab5c513b155e0f0f52483c760ee5948d5801f481461e16f2d5f57fb52c0fd3a385ce4dbae07e0134729efe57c95bb89191d7fcd0dd4b59bab3c6d88425051dd92f32de5a7fccecc43847347a1cc73600b649ce4485591655bf5a59db89c8c128b792478dd10a0f3cefdfdfb5c2b9cd033cd9bcc2dc6e8155aecab3a35072b1231bf4e30178b963e0312f18e0dbc001ee8bf00f8e183c85cab7a263467af19dd2dbe1f4e96307c6d075c02c8c98a485078dba413558e0db49f5e678726bed1c841c2ceb8ffc8536c21cd152588f5497d5463e1982ebd6f85bfad3051313cfa57981510dba05674fcecc38d3b3a31c236edda9dacc4069ac931a48e9dd3df15ea0d2e72af07f4a1f535c626dc43bc742771a506aefbb6c24d1d713e88230aa67353d3d0762d0c246a11911aa1f41ea75440d2da9ef16391202c7fe9cc8c3b668907f23c131706e971c68403b923806b9f03615a9304531c8fd1fa070710713df0174f6fd95f9c8f368b1aa9d993d933f3d52eb9a01be330b9c631b496e20c72e0de91f2aff1e00a4a41c3fe0c82f6c6351e180b9911e21b3b86146ad7784bb06a378e216f931fef67515d991525b18fe9d04f9cff45559f846487694fab0d24333b0e583488709153a154df7399262dfcbe461609b7f21dffc99130f03ecbbdc6dd3417fce83791ee87464fab964f95500ef2b30e7cea2bbe6e3fdb193db3b203c99a232ac868372b81ac69c3f157bdd0173b999162000ba3e7b5ad4bee099867ed53d3efc714a82da98e608237fd0bb14daef5f65fae9196d8c09416256c8177d060935e4a52945879727d4accbf59fceed6f14ecaac73af2905e94d0922b9e799c9cf332ad5ec0016ec6e21bffde7a188b0a36718b3360a3c5af4f2c44470b0fa3fca45f1d5c95a3727a1573074ce1d73281d97f083b083ed5f6f0b4ce772b5acf9d29ccbddc863a6fd8d624ddb2cd15f86335ac77e7d898e4ce929e4000ba36b49f40f32da6d194dcb56961d94420d2a81c42d02aaccd70989c5906374600b632159dea91f8932449fe5669e8eb7d67456f66855d43da50edd9609ed1e9a76314d09437d118ee3cffbe2fbbca35541dd8091ca8884577706ca647006c4d0d28633567829f3c5bb11017d4339b59d67094b9890a88177cb796914f5dedccc4c668e723027ba987c3e2dd9a45769a5aa94c1643b2609ce8c22fcc223dc2e78bed6d30088747776641c17c0a289fe301db8c5c9c25b9cc2146c5db0e0c999244fa08a816ddf73f96bd685687ec78199bb1bc1e614d1e864c2d140251388b8904e4fb7f3c84a080ae53c120666f58f20e9d966e5c17cc93cc581b76559d5b3695a18318aa98c9e4509e4fe310e05cc8cbe999c70a937eede33cc2cf642b53be72cfb0f4eef91ddbe5b5311fa38200c796ca1dd68b416ab3628a9853b78d43f6316a122a141ede09869dc39f9fda053c643008b753711872b196d845711882be2349883b06b331584e029a8eac92bbb639c1ae9c1572b5bacfffcab6a21d4cfa28cddcf1d310fe5254b8217a20e6deb6b3c462d4de34999d4c17023e69d9497a81145c4e361788778f3ecc9fcc98a839c2bb6654b55316085948c55f2ce173886b065c3ac051e113ef0195e8c1aebd9e708621727e5b264024058721a30b8d035c44e23b1e0d3d9577e37f2707df30fa71bba820475146133b1008669dcaf62784026b49c4d0283973f512feecdd619f4143fb08c2b6ba4313937afb85a7e2bb3115430b79a62b8cb8a76a02c56623e28b2c94c1a1562fa5905c81a8f72af0051f33b4b99dfbe19eab47a24f2b0580d49a0e766bff356dfbfdd8e08e9ba24b48c7d84a7695a8d59fa2a683b417b7b844b2ca09466701ddc488df87fe97372a87957e9d6d8b1d693f1771f411b30c4bb60c33b8721bd53f0d74435c2ae2048be64dfad5d5a3de566e52b6334e34ca613fc187aa873c69b5bbe964566ab03e3047c92a8df13b2fc9dd5ca75fc1530c20a61ad739a6adb6d8c792f0001d0adb3a8065123fa3b41bbc7e52f7fd46196b5b7f50cddb461a892813b404bb6800805f1c647642e3e6faa9fb238c058fbd42bf7ba75d52b2fdc5184d0c2495a3512c636336900f26c6912bda1fb519f8107ca4017b07a7199f287fac6057acbf45f6cb315b6252bc469c589204ef2354ad586888c0e2781dce6fba6619dc37d506a62c6a93fa25ef4255842cb4af74fcc2be7db2fa991521467772f105336f76645b0eb7be20cf9b37244623bf93d0ffa9d1381978d4582fedee14c0d26dbac978c0098260ee25c07b59bf9970ae6eba0003c7defad6383822c83481ca876fc758e0ed8926cfeb6745dbd6d1f93ba05d6f7f5d70c308bfee6f2e2304528c10e9a15d51915f9d3e6c4516c15565f867667df3d77385b5cdeb47f78f7ca7d71199e281a52a1adfcfb511a74f9164bc78ee5d962458926a76fbf4ae8b8db6038fb902883646cbc263fec145d44f0aab3d96969e3332a6973ed303b916abd0178aca760890811b56b18b8803600dd54c23bd0ecce71691a3d7eec24aab9dfac3c47a2cd9601212f94ddc3f96b697eb240cb12de6ccd7dfee1542676da9d6cff66d09c7ee4b558c4e72dfc6a41bcc6a336b7d20de6d354b894b3a74df83352bc1fd657680420e5981a8a3f54e2ce839e98edaaf959c01e87187556aa3958a81743791b6b4c9b3669de92fcdc80b7416faac2a5f9cf2aaa8d25787e1051ffcef11899a50b596bcb799c454290f2763772fee06db9e7293f3df5ba90053017788c15fde945ec60ce87493083264ec8af8f595b3b610e75233011989179f10ea0180bf7ef40412c3c285ae45fe2b19a03366f6f1e346ad617fa9569cafb66a6fa08804d86893e07c3674a49c0426dba1d7e8c56b032245389b91e156645e14fa5997d12c8feb67966b48e45f0b8786ea3fe3541b92ef1492076eb8ee0be0d95a1138e3826674b56b06d317036934249c07ca2620063f1f39059f4a254b3c9c384e9be7d560ce52c39a6e61848bb0fd2ddaf1d9093c7a25d461cb348b4c7d6be5b428a2b703329ea735d10f710a253dfc20cd8e948aa034f2e0b73e47d8e9a957dc5c93a9eaafc3065f93b9076500cd9c057eb4d5ae0444175b15523b6dbd64ee5390b20b0c74db33de03ab60692e066a10e9f10b4db592c2628229dda16ca9f4a79197447392e317a0ec08d267b703625ffe07bf90cabd1976faa1a89aa8b8afc3984dd02454364c1f99d6be9b7df28e03db1fd6140ab7f64777845b0b701134d5184b304c7819e8d500fb846378c7c08b34c25881f664f4bdd56211207fd7607396d0d5a0dff2699b8bf94b30660943ad2615adbb73eab4feadd7807c45db6f4513fd5fbc4fcc054e72099eb5746ff55e472430ff48e22df642886b9f095aebbcb1aa1a9d3106eea8f23f2aab32a598c8d9cafead4bd1599c7da4f1353c5bc41c0a0100493413a7e8427ae5efdbde19496d5bde3881f0adcc16cafd4374a1a22b83ac119a4cb8834c49bd1b6235c87c95243ea8629b08d82645ad1594fd188a07e03d3988bcca00c0b2bd6d526172dbfb21b51a8b408b7059c5b58ffe39e8e8494711a40876bce375fc01fad457de52c2d4467411e38a2c17d0ac3b78906efe2caf5696344c8fb3a8fdd4ad0e65384435a42960000830300077c4ead37561142ead7e8edeb59d2ed16c73828160dc7791fc4884b8031a61602aa5357a7001552652f554ebcf1c8b787500c9ac1e4d5f92aedd99f9b5c3b436ad3315123677372b6e3f080c5aa82b70350ae7976d203f7bc33de1e9429de8f4ec53d16b98779d4b10a437d9d44a66e14e6c1fbefdee87b88bddd428b8231f449fd4d0b602300000000000000016aa500ef0290b474589300ac5bb39f6632f8249c57256ca87309da9c18f745ec2f4fba32d3c0954940d8f00aa6e7ba51e388ccf9a853a9bd9a9de665c679209274b86ff64902b25bf8f34817f983ae92636044bbda21a5c6913e94636d8b5d2ffd8c07c6f4124692dc46bfef54381b76a5566108c554a2688db21aec4e5642bbf97b00c5d05251e74eea4e1f96a9c07c896556dc518f4f61069e001ce372e1448384359998fb12a8c656cf492a5bb598037b0a2aba001e7f94c4db09dd0a0d604c87ab1b1aaca23c4aee9fca3d6a195d887b00bb652dfd02af707eaab2385d86ae97d5067c1eec087ba705f6a4a1bdac83b7c78dcd135b2b4b72dbb367b7ad4c46f0cd8e2a0213d163ef46928584f5f26793a9930f898e364b7af18f4470e86ff4e01a39375682d45138e3eb636551ea675f749bf8a2b23676b4e13c85fef80e06830825b39300203241be31cfbc5c714590e01853c350a91dfdd96acf0f8b94e01ebd6ec487106dab6e69f558b3c6534aef85e7587384e939d666dd2018e0fcec38b6d65528ff4211129f275eb0f228b71eb0cfb2010508f3dab390e66b4a9c6992288470e777a3bf47b0f9f337f8630b4548c3777088f8797497a003324f440cd419e2c5c192e3ab8dafcbe4335c832a950b12cfa211fb7e46760846a120960a562c5df449ced2cb7cc92501fc9a771acee337f1cd3645867177e0b15f1178e0fb697442d308ce600a467866ca9a8aa33135481819189287ecff489b477af7337ae475c8cc3eb6dddb42f00b4fef384d4040c2fa7dfe902410b075afd05e87f6ae76744b581985c33d99364f56f29fb898142f6671b68d779908f6b3c8de1e096375e54384d10d69641989c70cdd44f08ddddde43b8e0da38d1e418d371a5f5851c716de8aa4d48418055dc3deedfa0a94aee55c7b253c47417b3c7397fa8b6004173f19f481629e4b762818eaf0cd89f4fd098e2ceb89227349f030189a5e01ee47b86a952f4a50597a94881f0ba8d4d41d646ad1e55c696c1d2e2d6fb8dc35d27a9c4e58445fb8347300881a5fd3bb7b39a3110e7de09764dd6f43602a1685ca3412cfb87b72cb75eed61dd46095621d30d7a0a17e246a5562790c47dccc1040a2aa5037572f29fd5811708841a13adfd21e603931cb5c1c86f5701426f708132daa25065ed5c82bfcfc623e796807980626c3e62017fc46f9fdcae3f4988e29ffc92a56db9324c07dd06b43556dd2fbf552b3240032f966cdf7227cd9e0557d4b577583a8dc2b87b490f65bb375532858062552a2587e0a6decad9377723a68478f1c4ce89c820967bec05e565e2af9e073f7992634da1818ab27b3927d0bf6fbf3dffd476a9f60e2a48955ad271b4dbe1364a87b3bbdffdeb28a79bfb089041ee85d5758fbf2e33e95ed34e31ad83031c78c9711a12d25a7e180fabeb5bf71d6bbfe092cb55559391fbe0886f9880cf38e0aae451992d73ab0707ca14130bbd4009b84944d4a31f721aa821288c0cb515d776817927877bb9ae919389879b24b122692eda38283955d7e7e23798fc0140a64a6118fec05175960aade3211442127d66f3537d774d552819dcda4ac0a543e8ed5c2963dedbd6e523b0e07ed86e451c172dea331f25a2b69a0f4eca6e9af0766290b3243916e6cb14cd642815b82ab1a3c4ed3e8e0e6f0ea81b4339df432535c198802c014b39ced12d205a425402469ecb65377ae8ecf6e07d06a432eb085e49d0842b2b4130482bdbd80750adcde3b7ecfe0a038f443735bd3fd9faf860958b70c140aafc91bdb267528293fad46db2cf529a0313f76993e63daf4c30c8b7514c54bfeae128e45e4d07485758e8ad130e2021f4569ef085ad771e0eb39696694e4234c871fd8026b9140ea8c8b7afa75c165384ce1eca47b8619095e749905cb8755fec7f5d0cfff187fc3fe4cfb2f355fc7399c905d82a5f8fde8a351e896ac05c1e17bb1c8f18a9e715f46d64fc13ee46c623a40bdaf230d466f6ee051f5698b67aafb8eb5803021c720fc2b6270317c4ef72208d95be0eaa108ba00b94b852fd3c72550bb816c9aad9f19b9d94f06954907207edb5703d5f542c5a5a5b49547e424517d2c5f2971a712f463662ed0e1b842b8c3524420df54885e6fde39c0dddc700992ec7b3caca01d9b584086e0e1bd3c49bb196f35bf50bf422ab1f7e6137ab30c21dd9ffb12d1f357bf3799bffa0ff10e40191c1f46fe272a06aaa7e6c113ac5b12cbf81f5484e1320ad8a0fa9b353a9be9fcee4a15d7040367f3fa1956c453e124c564836921e5c2009e8d270fdae2a75679ef38363bdafb7518f9bc199f565eea2375f479c79d800bfd19ae4a6e871e8d36c265bbfd67e4c7e1be91db81dde7ddd80b6e979dad6058164085d09280c846470b3dd04337bd6c571b6e3a009140b2ee7eb426db59d400d502e8eae1789e3d501d300d40e0c2a1537f34e84f7ec466b7816494068e806ea04120dacc28c30ebee2155574655f045ef5c143f05787ffc8d3a38955fc8d1e98128bac1524af08c3f843371b7861341f9e601764efc890c1e16ff8d86b96ddde4e6de03b1d1508057597b76ef276a5533accbb9f36ef618b508a5a022e2e28bce9fc4633ca11c512a7eaee6dd3abd49c982d9ace67798ad2f56cef370da796c61f02c89dfbae87e8fe095cbc4355cc18127f0bcdf97bd9d36e2503d562d5cae29106eb2936b28f293c93972678537dbeb47b9ddd7335d9de68a09fe4db30b111c9bfce0543f990fd9eab5efbb54be32a9958c4d1c4df5678b6445304988baafce19abc43964b1fc215e76ac03d737883a722523ea43b6de6d13fc45637064186f32d7797be95fdecce91b0b93d5e68b2420d3d477296af0e328aa031d6258c14365bd8061b1d1b82a86e9c6f298689b83736ad29bfb6436f6561dc5953f5377545c47033c1e91b4caea84beb1777315d00326edd5e3cdd346be3dcad3013507c5aee3214ee9eb7ee2322e95866cba3f6d64eefc1e1026adc4ad117182a450ce0eaf19b51457bb0e7638aeb26ff39cfbb33a27d4bd8b0ed715d8431816eac381a86d2856c834d7b5c68849156377aea5e96e6189a2ef459fb746713f2c5c43199d3b9481576929b3f4a87a210063ac45be2cd47154b1dbcaae6d021f356c6156f527e74e01beebe4e36d4e340917f21ca8a0314468df0292009898b6e5650c8f7397d75ce9a41344b6b23698055da8187ab7260bb2d660dbaf24eee20b70bc62db90e3dac7eaed30bb5e1d9ce8b9c638c08c8b032ccd569c71ec0c3e4a4135c4e5d0b1cd0f75c8909e3718a3728048a39dc7bcf6d638dc1d4bb2041f2fef90c680d294273b52c59b8ac70c4b54e691b2e772e409d71ff5a513de477f68e161863ba3b3ac2eff04952b9383799141251a094daafad2544631907e31b1bd50bfc9b4c09d1b86ca13ff23bc3e185ae6990baca356d36a6e3645c85568fe1f8bafbf31409bbcee0a601f5cf9e45148863bc36238e48538715419981b8d09400f71eba893ebbb8ef99c888147d739201d5afb21de99e77f76928a55d32c3a2c98a377ca1dc02432fd45a63d8637fae1b658fded2e96f91e458e6273b7ffd3593de87c9bfaba60347e610924bcd90f44c488f1fb0e6962d2530294c354ffdb4f56b6f568285e676e7fcd30d7482934f1645dabaf276e3340f0be1b3b0329fc53f64b6240e036e10a54af437b4c453c8fe20b5c972ab09f63924a6263d2fd464a2cdb2a5404536a4eee536c4b4ef9c5a18f7156b6fca427a09a8229a170d4063353105e8b9a1014c998ee65405fbac8b20463fc2a55aaf025ae962f5e5140f7157259619ace3aef2b11f4ed654055f4fd3b9a8829e86b8e4f8454563b843db50ba6d7e7e127152642a93fd358e8e6a0402f639fc7b3f70c3b5de2130b81c1655db80501ab9cfb7c0a34ad67cbfe87abecc24433ed90956583db8a7b1178c5fd689df39a0faed32578cd67fa59e958451dcea04a2e2e7f6c7f9109c3fb31037228a51825f02275fbe34a7c92af7e8ddf0de890b1a3290158d0e50a7ed4d672aba2e5a80830300079d7342edc8e4f367c464c673082612688fa916f3d71496950e595ff7c2ff54c04478a8cc525c6d9137c9c05a38253793b3bb1c78b1a8d7f4d08cb8bfeca7f60a266157782ec8b03327c180ebd90d1def2f7ffe88d4ccd008ef3f78c3e32fa33cce6ba6531ec884059d611d7f1366f113ccd90f2f3603f691531c4b4fe90aa930fd4d0b60230000000000000001639f0085e8000b3aefa7f690e47785875d9c8e3f6b18b022ede2c86338401206e88bcde9eaa7524a7663cb52cd50f2f81a00c852d1073e5489be1688183a3309a22b9c1ab0c8dfe1c3b754ad8d0a261ce2296c56eba270902a6e60abdd21dce7486a211e76cca1812b2e9347f95e1dbeaae4d56eda8264e4c8b24802e73e43f17e9ad45f0e3156384659da11b2ec87b523df35e1079e3399ab0ff33e04d0008ac8c019d2eb678b2e219680120a1e9c0b01017f12ce88597abe4bb9719ae9fb3dfd06f03f010cb540b022353f209ca8046e76d506cdfa7c99f01a05ff233a139bc34c45575c193da15ca683a5fe8e77fadafbaf6e5bf07100e426b1e142e42f9fd8f4e5344d052311d85ba0eef2f2df69e74f478487a25b894b017c47b421a917020fc9013c458aefd2b7bfd6d5f5e94a3403a82d90e3634e28a5c1610dd038f0b60ed9f237d7dbc41cb5d4c580b9f3e8997e04c29c57a52851d1ceb45f38fcedd08d219cfa165e1e74d96a39109d1cddc29183bc8c5c9ed7678e42408d8c72ffbb217534d28833e84277a1a6943721bf8629279050fc1a9aea75a730191906b0c05691b7d8dd9f87763d5df7606e9992742487e28e67f58765b8fa0ef4034bf7f8653934604c5650851f6bb10b2b379146ed6e83d1e020d4b2d7f670c16727f5bd091619cbd90ddf0669bc6ea01a3770fddb8f7f218a518defb531e110fcb5d28599c6816bfd7e261b32f7792a08f5ee6a4f308f95092ae55a3a1bd96872a5b06f9bd198e449c8cb5fe6d88211d512ff9d491a8fbd3d6aafaedebcf6d2c9d945c566347380342d48ac26afad49c0eadb3dbaecc1dd61ee13ba42059b40367fbeed476460f9686ce92089c734e648df3bc13a6bf58c6e062f244575bee5821c42b17b68c9bd3c1f74012cbbb8f702530fc1facafd092458dfaa760781f2761943266fb7eed5318d3dc48a6384e2ecab5290244b8b09f14147f734839fc352da11f0b38d619353e948485b4fd9112a1e13e09c6b3316a89280e803d1e3ec0787b8d16c8ff24a9267ee9ac73e8471b43ee01e4b848b608393ff8a3d6a352d8b5b56858b13ae07a83121046873b0adc50379464f0ae8b02f297282920222bb2b56fb0be1d2bbf44d2de42b8d0200f1add66d07ee672ab821dd1b274bf3b645d2b06e1043d5ef44237188278a76cce834729f9d66cc5ba8859735063d4c2f0bdfc569a2c90c0aea8a5c00f5c3df13a8cbae947b2acc5357209e3adc34c04cef2c11ad9f869289a31478c23a4737468d4bee26e5b45cca9d6a454fce837c75c3834082d657480df08143da06395a0180aa73d5a8f6c217aac5ca22b9b1a7a3e64c5a54e8807c41d6f0049728f8a86ce51be606f1da6dd777241e8474515ce6529ff755c7c7d7fa0054d1d4c3492dcac5da8dcb1150d7379734601c3d8d28863bc3d50cf64699dabd264dfe036f7200b348bb1cecb55ef0916b0c17373ff1205e2fa7bbcfee219fe27e4000c613b804dfff10963e48b0b27d9a5c7a6897ee86c4c77cd417ac1ca98415e647feae14115cacb9862a6693bb2c3b31b026b73950308bfb986b8e5f7e860d027df6c9fce9d7c39fc23cac49e4c8d3e07ad2b67a7e965a2b7acec4f771b8b46cd49a7971f6588958ad576afcf34420bfc7bf2827fb130f0d0ae3804f0768fac870cda7cb902638f4fd782f3161537b9e33b9845812ab2a8a0b1abd5daa7aaa218c32520f82a31e9ce6f645642a7309bfaac28a945ddda6fef5484412bbfd07a87f813c5c4ee344cddd81de0b7bfcd1d8e7c2762ad6f415d61dd51e7abe6b17f8138abbb743f5c3029d2b793a14391758a002456bfed2cd68a47822bd16ff7473a82440f4308bccdeacee047f1a429ec7ea8c0a237b35c737d8ed4526f726040ff8fa84d4439095061686500ff9559f16469fd1a66de17c9b8481d0b3db6a9811045b5e5757fcf9e128f03449e6018ee11e6c5a41a9cceeecd1ab9d8a81b34db02aec7f4831804dc1bd0b13d0cf62f2134dff3755ac741e9d789a6f38469fcd00967104e31a1aeceeaf49d63909623b7160fdc4bafcbe1536bf6f43e17b6ffee3dcc864a8c1ef7b437ed39fe2e54253c26d236da82837937042f36f68ef7bf0970cbeac3689a2ff2b2acc57f6d76d634173ba4a636b9723c5ee3852ac4f153e752b0aa17ee71d1365e45b1177f77647a02b1d6f92e386899c64ca1de46d0b34a8fbe1ac3ad8ee75c3dafeb3374f13253eb2a5fb7649a0e9ee3af265301043f22658529437c59e3216e8999ee1298597c26b06360e86b36a28d6fd07b6f837e610530e78c51e413d949c1b8bf687900d51b82fc4d386ec3a9c73aa6276954ac8d7f7273b5a945f7f59cd6ade841d5d68f7743e95ce1a5e351f46bf09b5fcd08409e315888478bd3c4788611185dc81c867ac679f006adc4b060c4a9b17d0af66068ca9a75dcc898ab3655d1a90c9fb92ad54823a7c4c15baa601ca6fc89811b1c619dc61a2a574ce9d45dac75af2600dd042f0679b54effc4c3796b59741ca0765d6d3edd62c1eca4dcc969cf4231273814b8beee07b0664983fe8b9c31b8efaeeefe04b03d7ed608c69cd4b0ef53556bc273f9dc7cd99421c2bca9b8e8a337dcc0aff7a8bb73a29ba2fc77f31476e42b08274c0f37a149a85cff9a49adc3d889812d60c7b8620a929aded40c6a3a0823e23904838de49cecae28088da02c3eb15a020d418f79c5f021484362accefe0dd992969bf4f378a7a512bfef8aee10adbcf3a5d91564a1ef4e0c3161ff2c75ec42a88635b1846ee36fde89744b3182b63794a7a19694605b16f8c5b048015bd9728f46ba9987637f0e8e9a68cac418a35fa9beb6a4e17c30dbed9f6b2453b3384dd2e008ed54e661e2181946b2d7a0bcee5bd9c447465e397fe7fc3373a9e8e0ff735f1eed5130f3c0211701223214e26559f05fd638fec803fa8658cac11fc5fba6ce4710e5281fd2df7f7179774744ed22ffde7d9b966343d34f927a85d885755a6d2676e76a7556401af667654b27481522c9509f76ecc474909c16cf90dff9c40f800461b616e8d1eda757a719a6afd9b54c3e2f8008118e7df8de8b384122a22e0479e24545305b640cd4421f45731fb31809624e123102866b2f93dbfb72e0a80e944ba1bd09af52e4cdf3e49c7e2d8b99a701b8616ec9d89735e928f61de7405da07707eadf4aa82fd5331dbe82c54e99aa502b71dfa578dd9c7b84ce06f80721c349c822654ea5e5ce62bddef81c0844ea2b33eedb1ae564f5144e7abebb2ff914325f251d9eb83ab88194769094c950f205bbc6018c4e9c608d43874abc6b6e36c75e309ecbb0bcc7a904d0d05726a4bbecfb9b3da6518cb2b40bb5f18888812f1f0c3158ae634298c3376f010c1e0bad7dd242328e7660105c633a1e6e5b0aedc4a3630d40b4c55a12ca2cad0c4fbe4be8f93af7575f2cf104a5f7d2d5996af90c7c643dfd8fa36b4fe497096c5eea26e16ff6364b364fc78bfd86726c56717ccbfaaeeba44e6e8527b92d73d9a132c507c13c73bb93335dec31a09d4d74d652c340ffd80860572a48f1735334b3672fc5ad30a6a05e1411cadc442bc8aa79ec48f1c6a7b64c4fb1d519df7819b1e753856baa70d806da054b4cee7369320621405b0a9613cf3d6448210b6388b696e36cd7603b9c57256d22578c88d4ceb07f580c195bcf93791ba3ef41f8b2821f49589c5db205563f87c27331b1b759f1120fd2bc2d035a471e2cdf49cabe20fa55b9343e3a6548d2b552d3c70d02bb596abebe23022d33acf94f03c38cdc4e00b9c59bdbacc92daaf8c14497d84cc6339cf46768e4133c7db68427df2b6bff030854b650f180261f0011b90ce0749ae32d485f978141a5f247b13a34e78f77365fba462db7ac7cf08b9866110c92f4df64779fbd42c12dd56506098a092be6507d53239f25547585c66c2871b77b8181f00c4d0dff8d98ddccc39e74038800516cf707aca21c1bdb1c0586b986911a519ce33cf0e2b75276d65b447c2c3c7fb00a13f9915d22616cf6d812dc98744d536f8dc9d9c650e5b82");
      Console.WriteLine();
    }

    // [TestMethod()]
    public static void TestAddPubkeySign()
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
      if (sighash.ToHexString() != "c90939ef311f105806b401bcfa494921b8df297195fc125ebbd91a018c4066b9")
      {
        Console.WriteLine("  - sighash = " + sighash.ToHexString());
        throw new InvalidOperationException("**** sighash fail. ****");
      }

      SignParameter signature = privkey.CalculateEcSignature(sighash);
      if (signature.ToHexString() != "0268633a57723c6612ef217c49bdf804c632a14be2967c76afec4fd5781ad4c2131f358b2381a039c8c502959c64fbfeccf287be7dae710b4446968553aefbea")
      {
        Console.WriteLine("  - signature = " + signature.ToHexString());
        throw new InvalidOperationException("**** signature fail. ****");
      }
      signature.SetDerEncode(sighashType);

      tx.AddPubkeySign(txid, vout, hashType, pubkey, signature);
      string expTxHex = "0200000001020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac0000000000000247304402200268633a57723c6612ef217c49bdf804c632a14be2967c76afec4fd5781ad4c20220131f358b2381a039c8c502959c64fbfeccf287be7dae710b4446968553aefbea012103f942716865bb9b62678d99aa34de4632249d066d99de2b5a2e542e54908450d600000000000000000000000000";
      Console.WriteLine("  - signed tx = " + tx.ToHexString());
      if (tx.ToHexString() != expTxHex)
      {
        throw new InvalidOperationException("**** tx fail. ****");
      }

      Address addr = new Address(pubkey, CfdAddressType.P2wpkh, CfdNetworkType.ElementsRegtest);
      bool isVerifySign = tx.VerifySign(txid, vout, addr, addr.GetAddressType(), value);
      if (!isVerifySign)
      {
        throw new InvalidOperationException("**** sign fail. ****");
      }
    }

    // [TestMethod()]
    public static void TestAddSignWithPrivkeySimple()
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
      tx.AddSignWithPrivkeySimple(txid, vout, hashType, privkey, value, sighashType);
      string expTxHex = "0200000001020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac0000000000000247304402200268633a57723c6612ef217c49bdf804c632a14be2967c76afec4fd5781ad4c20220131f358b2381a039c8c502959c64fbfeccf287be7dae710b4446968553aefbea012103f942716865bb9b62678d99aa34de4632249d066d99de2b5a2e542e54908450d600000000000000000000000000";
      Console.WriteLine("  - signed tx = " + tx.ToHexString());
      if (tx.ToHexString() != expTxHex)
      {
        throw new InvalidOperationException("**** tx fail. ****");
      }
      Address addr = new Address(pubkey, CfdAddressType.P2wpkh, CfdNetworkType.ElementsRegtest);
      bool isVerifySign = tx.VerifySign(txid, vout, addr, addr.GetAddressType(), value);
      if (!isVerifySign)
      {
        throw new InvalidOperationException("**** sign fail. ****");
      }
    }

    // [TestMethod()]
    public static void TestAddMultisigSign()
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
      if (sighash.ToHexString() != "d17f091203341a0d1f0101c6d010a40ce0f3cef8a09b2b605b77bb6cfc23359f")
      {
        Console.WriteLine("  - sighash = " + sighash.ToHexString());
        throw new InvalidOperationException("**** sighash fail. ****");
      }

      SignParameter signature1 = privkey1.CalculateEcSignature(sighash);
      if (signature1.ToHexString() != "2ce4acde192e4109832d46970b510158d42fc156c92afff137157ebfc2a03e2a0b7dfd3a92770d79d29b3c55fb6325b22bce0e1362de74b2dac80d9689b5a89b")
      {
        Console.WriteLine("  - signature1 = " + signature1.ToHexString());
        throw new InvalidOperationException("**** signature1 fail. ****");
      }
      signature1.SetDerEncode(sighashType);
      signature1.SetRelatedPubkey(pubkey1);

      SignParameter signature2 = privkey2.CalculateEcSignature(sighash);
      if (signature2.ToHexString() != "795dbf165d3197fe27e2b73d57cacfb8d742029c972b109040c7785aee4e75ea65f7a985efe82eba1d0e0cafd7cf711bb8c65485bddc4e495315dd92bd7e4a79")
      {
        Console.WriteLine("  - signature2 = " + signature2.ToHexString());
        throw new InvalidOperationException("**** signature2 fail. ****");
      }
      signature2.SetDerEncode(sighashType);
      signature2.SetRelatedPubkey(pubkey2);

      SignParameter[] signList = { signature1, signature2 };
      tx.AddMultisigSign(txid, vout, hashType, signList, redeemScript);

      string expTxHex = "0200000001020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000000004004730440220795dbf165d3197fe27e2b73d57cacfb8d742029c972b109040c7785aee4e75ea022065f7a985efe82eba1d0e0cafd7cf711bb8c65485bddc4e495315dd92bd7e4a790147304402202ce4acde192e4109832d46970b510158d42fc156c92afff137157ebfc2a03e2a02200b7dfd3a92770d79d29b3c55fb6325b22bce0e1362de74b2dac80d9689b5a89b0147522102bfd7daa5d113fcbd8c2f374ae58cbb89cbed9570e898f1af5ff989457e2d4d712102715ed9a5f16153c5216a6751b7d84eba32076f0b607550a58b209077ab7c30ad52ae00000000000000000000000000";
      Console.WriteLine("  - signed tx = " + tx.ToHexString());
      if (tx.ToHexString() != expTxHex)
      {
        throw new InvalidOperationException("**** tx fail. ****");
      }
      // Address addr = new Address(redeemScript, CfdAddressType.P2wsh, CfdNetworkType.ElementsRegtest);
      // bool isVerifySign = tx.VerifySign(txid, vout, address, addr.GetAddressType(), value);
      // if (!isVerifySign) {
      //   throw new InvalidOperationException("**** sign fail. ****");
      // }
    }

    public static void DescriptorTest()
    {
      string desc = "sh(wpkh(03fff97bd5755eeea420453a14355235d382f6472f8568a18b2f057a1460297556))";
      Console.WriteLine("call descriptor.");
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      Console.WriteLine("descriptor: " + descriptor.ToString());
      Console.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      Console.WriteLine("hashType: " + descriptor.GetHashType());
      Console.WriteLine("scriptListLen: " + descriptor.GetList().Length);
      Console.WriteLine("scriptList[1].Pubkey: " + descriptor.GetList()[1].KeyData.Pubkey.ToHexString());
      Console.WriteLine("HasKeyHash: " + descriptor.HasKeyHash());
      Console.WriteLine("GetKeyData.Pubkey: " + descriptor.GetKeyData().Pubkey.ToHexString());
    }

    public static void Main()
    {
      TestConfidentialTx();

      TestBlindTx();

      TestAddress1();
      TestConfidentialAddress1();

      TestTxidAndOutPoint1();

      TestTxidAndOutPoint2();

      TestDecodeTxData();

      TestAddPubkeySign();
      TestAddSignWithPrivkeySimple();
      TestAddMultisigSign();
      DescriptorTest();

      Console.WriteLine("Call Finish!");
    }
  }
}
