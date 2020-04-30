// using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
// using System.Runtime.InteropServices;
using Cfd;
using Xunit;

namespace Cfd.xTests
{
  public class CfdUtilTest
  {
    [Fact]
    public void TxidAndOutPointTest()
    {
      string txidStr = "57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f";
      string txidStrReverse = "0f231181a6d8fa2c5f7020948464110fbcc925f94d673d5752ce66d00250a157";
      Txid txid = new Txid(txidStr);
      Assert.Equal(txidStr, txid.ToHexString());
      Assert.Equal(txidStrReverse, StringUtil.FromBytes(txid.GetBytes()));

      Txid copyTxid = new Txid(txid.GetBytes());
      Assert.Equal(txidStr, copyTxid.ToHexString());

      Txid txid1 = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      Txid txid2 = new Txid("57a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      Txid txid3 = new Txid("99a15002d066ce52573d674df925c9bc0f1164849420705f2cfad8a68111230f");
      Txid txid4 = null;
      Txid txid5 = null;
      Assert.True((txid1 == txid2));
      Assert.False((txid1 == txid3));
      Assert.False((txid1 == txid4));
      Assert.True((txid4 == txid5));

      OutPoint p1 = new OutPoint(txid1, 0);
      OutPoint p2 = new OutPoint(txid2, 0);
      OutPoint p3 = new OutPoint(txid3, 0);
      OutPoint p4 = null;
      OutPoint p5 = null;
      Assert.True((p1 == p2));
      Assert.False((p1 == p3));
      Assert.False((p1 == p4));
      Assert.True((p4 == p5));
    }

    [Fact]
    public void EncodeToDerTest()
    {
      ByteData sig = new ByteData("773420c0ded41a55b1f1205cfb632f08f3f911a53e7338a0dac73ec6cbe3ca471907434d046185abedc5afddc2761a642bccc70af6d22b46394f1d04a8b24226");
      ByteData derencData = SignParameter.EncodeToDer(sig, new SignatureHashType(CfdSighashType.All, false));
      Assert.Equal("30440220773420c0ded41a55b1f1205cfb632f08f3f911a53e7338a0dac73ec6cbe3ca4702201907434d046185abedc5afddc2761a642bccc70af6d22b46394f1d04a8b2422601", derencData.ToHexString());
    }
  }
}
