// using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
// using System.Runtime.InteropServices;
using Cfd;

namespace Cfd.Tests
{
  // [TestClass()]
  public class CfdTests
  {
    // [TestMethod()]
    public void TestConfidentialTx()
    {
      Console.WriteLine("ConfidentialTransaction");
      Cfd.ConfidentialTransaction tx = new Cfd.ConfidentialTransaction("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000");

      Console.WriteLine("- tx    = " + tx.ToHexString());
      Console.WriteLine("- bytes = " + Cfd.StringUtil.FromBytes(tx.GetBytes()));
      Console.WriteLine("- txid  = " + tx.GetTxid().ToHexString());
      Console.WriteLine("- size  = " + tx.GetSize());
      Console.WriteLine("- vsize = " + tx.GetVsize());
      Console.WriteLine();
    }

    // [TestMethod()]
    public void TestBlindTx()
    {
      Console.WriteLine("BlindTransaction");
      Cfd.ConfidentialTransaction tx = new Cfd.ConfidentialTransaction("0200000000020f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570000000000ffffffff0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a1570100008000ffffffffd8bbe31bc590cbb6a47d2e53a956ec25d8890aefd60dcfc93efd34727554890b0683fe0819a4f9770c8a7cd5824e82975c825e017aff8ba0d6a5eb4959cf9c6f010000000023c346000004017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000003b947f6002200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d17a914ef3e40882e17d6e477082fcafeb0f09dc32d377b87010bad521bafdac767421d45b71b29a349c7b2ca2a06b5d8e3b5898c91df2769ed010000000029b9270002cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a1976a9146c22e209d36612e0d9d2a20b814d7d8648cc7a7788ac017981c1f171d7973a1fd922652f559f47d6d1506a4be2394b27a54951957f6c1801000000000000c350000001cdb0ed311810e61036ac9255674101497850f5eee5e4320be07479c05473cbac010000000023c3460003ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed8791976a9149bdcb18911fa9faad6632ca43b81739082b0a19588ac00000000");

      IDictionary<OutPoint, AssetValueData> utxos = new Dictionary<OutPoint, AssetValueData>();
      IDictionary<OutPoint, IssuanceKeys> issuance_keys = new Dictionary<OutPoint, IssuanceKeys>();
      IDictionary<uint, Pubkey> confidential_keys = new Dictionary<uint, Pubkey>();

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
      Privkey issuance_blinding_key = new Privkey("7d65c7970d836a878a1080399a3c11de39a8e82493e12b1ad154e383661fb77f");
      issuance_keys.Add(
        new OutPoint("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f", 1),
        new IssuanceKeys(
          issuance_blinding_key,
          issuance_blinding_key
        ));

      // set txout blinding info
      confidential_keys.Add(
        0, new Pubkey("02200d8510dfcf8e2330c0795c771d1e6064daab2f274ac32a6e2708df9bfa893d"));
      confidential_keys.Add(
        1, new Pubkey("02cc645552109331726c0ffadccab21620dd7a5a33260c6ac7bd1c78b98cb1e35a"));
      // 2: fee
      confidential_keys.Add(
        3, new Pubkey("03ce4c4eac09fe317f365e45c00ffcf2e9639bc0fd792c10f72cdc173c4e5ed879"));

      tx.BlindTransaction(utxos, issuance_keys, confidential_keys);
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
      UnblindIssuanceData reissue_data = tx.UnblindIssuance(1, issuance_blinding_key, issuance_blinding_key);
      AssetValueData vout0 = tx.UnblindTxOut(0,
          new Privkey("6a64f506be6e60b948987aa4d180d2ab05034a6a214146e06e28d4efe101d006"));
      AssetValueData vout1 = tx.UnblindTxOut(1,
          new Privkey("94c85164605f589c4c572874f36b8301989c7fabfd44131297e95824d473681f"));
      AssetValueData vout3 = tx.UnblindTxOut(3,
          new Privkey("0473d39aa6542e0c1bb6a2343b2319c3e92063dd019af4d47dbf50c460204f32"));

      Console.WriteLine("- ReIssuance unblind data:");
      Console.WriteLine("  - asset          = " + reissue_data.asset_data.asset);
      Console.WriteLine("  - satoshiAmount  = " + reissue_data.asset_data.satoshi_value);
      Console.WriteLine("  - asset blinder  = " + reissue_data.asset_data.asset_blind_factor.ToHexString());
      Console.WriteLine("  - amount blinder = " + reissue_data.asset_data.amount_blind_factor.ToHexString());
      if (reissue_data.asset_data.asset != "accb7354c07974e00b32e4e5eef55078490141675592ac3610e6101831edb0cd")
      {
        throw new InvalidOperationException("**** reissue asset fail. ****");
      }
      if (reissue_data.asset_data.satoshi_value != 600000000)
      {
        throw new InvalidOperationException("**** reissue satoshi_value fail. ****");
      }
      if (reissue_data.asset_data.asset_blind_factor.ToHexString() != "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** reissue asset blinder fail. ****");
      }
      if (reissue_data.asset_data.amount_blind_factor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** reissue  amount blinder fail. ****");
      }

      Console.WriteLine("- Vout[0] unblind data:");
      Console.WriteLine("  - asset          = " + vout0.asset);
      Console.WriteLine("  - satoshiAmount  = " + vout0.satoshi_value);
      Console.WriteLine("  - asset blinder  = " + vout0.asset_blind_factor.ToHexString());
      Console.WriteLine("  - amount blinder = " + vout0.amount_blind_factor.ToHexString());
      if (vout0.asset != "186c7f955149a5274b39e24b6a50d1d6479f552f6522d91f3a97d771f1c18179")
      {
        throw new InvalidOperationException("**** vout[0] asset fail. ****");
      }
      if (vout0.satoshi_value != 999587680)
      {
        throw new InvalidOperationException("**** vout[0] satoshi_value fail. ****");
      }
      if (vout0.asset_blind_factor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[0] asset blinder fail. ****");
      }
      if (vout0.amount_blind_factor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[0]  amount blinder fail. ****");
      }

      Console.WriteLine("- Vout[1] unblind data:");
      Console.WriteLine("  - asset          = " + vout1.asset);
      Console.WriteLine("  - satoshiAmount  = " + vout1.satoshi_value);
      Console.WriteLine("  - asset blinder  = " + vout1.asset_blind_factor.ToHexString());
      Console.WriteLine("  - amount blinder = " + vout1.amount_blind_factor.ToHexString());
      if (vout1.asset != "ed6927df918c89b5e3d8b5062acab2c749a3291bb7451d4267c7daaf1b52ad0b")
      {
        throw new InvalidOperationException("**** vout[1] asset fail. ****");
      }
      if (vout1.satoshi_value != 700000000)
      {
        throw new InvalidOperationException("**** vout[1] satoshi_value fail. ****");
      }
      if (vout1.asset_blind_factor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[1] asset blinder fail. ****");
      }
      if (vout1.amount_blind_factor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[1]  amount blinder fail. ****");
      }

      Console.WriteLine("- Vout[3] unblind data:");
      Console.WriteLine("  - asset          = " + vout3.asset);
      Console.WriteLine("  - satoshiAmount  = " + vout3.satoshi_value);
      Console.WriteLine("  - asset blinder  = " + vout3.asset_blind_factor.ToHexString());
      Console.WriteLine("  - amount blinder = " + vout3.amount_blind_factor.ToHexString());
      if (vout3.asset != "accb7354c07974e00b32e4e5eef55078490141675592ac3610e6101831edb0cd")
      {
        throw new InvalidOperationException("**** vout[3] asset fail. ****");
      }
      if (vout3.satoshi_value != 600000000)
      {
        throw new InvalidOperationException("**** vout[3] satoshi_value fail. ****");
      }
      if (vout3.asset_blind_factor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[3] asset blinder fail. ****");
      }
      if (vout3.amount_blind_factor.ToHexString() == "0000000000000000000000000000000000000000000000000000000000000000")
      {
        throw new InvalidOperationException("**** vout[3]  amount blinder fail. ****");
      }

      Console.WriteLine();
    }

    /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
        // [TestMethod()]
        public void TestAddress1()
        {
          Console.WriteLine("Address from text");
          Cfd.Address addr = new Cfd.Address("bcrt1q576jgpgewxwu205cpjq4s4j5tprxlq38l7kd85");
          Console.WriteLine("- Address = " + addr.ToAddressString());
          Console.WriteLine("- LockingScript = " + addr.GetLockingScript());
          Console.WriteLine("- Hash = " + addr.GetHash());
          Console.WriteLine();

          Console.WriteLine("Address from Pubkey");
          Cfd.Pubkey pubkey = new Cfd.Pubkey("031d7463018f867de51a27db866f869ceaf52abab71827a6051bab8a0fd020f4c1");
          Cfd.Address addr = new Cfd.Address(pubkey, Cfd.CfdAddressType.P2wpkh, Cfd.CfdNetworkType.ElementsRegtest);
          Console.WriteLine("- Address = " + addr.ToAddressString());
          Console.WriteLine("- LockingScript = " + addr.GetLockingScript());
          Console.WriteLine("- Hash = " + addr.GetHash());
          Console.WriteLine();
        }
    */

    /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
        // [TestMethod()]
        public void TestConfidentialAddress1()
        {
      Console.WriteLine("ConfidentialAddress from text");
      Cfd.ConfidentialAddress caddr = new Cfd.ConfidentialAddress("VTpvKKc1SNmLG4H8CnR1fGJdHdyWGEQEvdP9gfeneJR7n81S5kiwNtgF7vrZjC8mp63HvwxM81nEbTxU");
      Console.WriteLine("- ConfidentialAddress = " + caddr.ToAddressString());
      Console.WriteLine("- Address = " + caddr.GetAddress().ToAddressString());
      Console.WriteLine("- ConfidentialKey = " + caddr.GetConfidentialKey().ToHexString());
      Console.WriteLine();

            Console.WriteLine("ConfidentialAddress from address");
            addr = new Cfd.Address("Q7wegLt2qMGhm28vch6VTzvpzs8KXvs4X7");
            pubkey = new Cfd.Pubkey("025476c2e83188368da1ff3e292e7acafcdb3566bb0ad253f62fc70f07aeee6357"); caddr = new Cfd.ConfidentialAddress(addr, pubkey);
            Console.WriteLine("- ConfidentialAddress = " + caddr.ToAddressString());
            Console.WriteLine("- Address = " + caddr.GetAddress().ToAddressString());
            Console.WriteLine("- ConfidentialKey = " + caddr.GetConfidentialKey().ToHexString());
            Console.WriteLine();
        }
    */

    // [TestMethod()]
    public void TestTxidAndOutPoint1()
    {
      Txid txid = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");

      Console.WriteLine("Txid");
      Console.WriteLine("- hexStr => " + txid.ToHexString());
      Console.WriteLine("- bytes  => " + Cfd.StringUtil.FromBytes(txid.GetBytes()));
      Txid txid2 = new Txid(txid.GetBytes());
      Console.WriteLine("- copy   => " + txid2.ToHexString());
      Console.WriteLine();
    }

    // [TestMethod()]
    public void TestTxidAndOutPoint2()
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

    public static void Main()
    {
      CfdTests test_obj = new CfdTests();

      test_obj.TestConfidentialTx();

      test_obj.TestBlindTx();

      /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
            test_obj.TestAddress1();
            test_obj.TestConfidentialAddress1();
      */

      test_obj.TestTxidAndOutPoint1();

      test_obj.TestTxidAndOutPoint2();

      Console.WriteLine("Call Finish!");
    }
  }
}
