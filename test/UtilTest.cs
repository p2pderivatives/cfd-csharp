using Xunit;

namespace Cfd.xTests
{
  public class UtilTest
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

    [Fact]
    public void EmptyConstructorTest()
    {
      Txid txid = new Txid();
      Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", txid.ToHexString());

      OutPoint outpoint1 = new OutPoint();
      OutPoint outpoint2 = new OutPoint(txid.GetBytes(), 0);
      Assert.False((outpoint1 != outpoint2));
    }

    [Fact]
    public void ByteDataTest()
    {
      ByteData emptyData = new ByteData();
      Assert.True(emptyData.IsEmpty());

      ByteData data = new ByteData("773420c0ded41a55b1f1205cfb632f08f3f911a53e7338a0dac73ec6cbe3ca471907434d046185abedc5afddc2761a642bccc70af6d22b46394f1d04a8b24226");
      ByteData serializedData = data.Serialize();
      Assert.Equal("40773420c0ded41a55b1f1205cfb632f08f3f911a53e7338a0dac73ec6cbe3ca471907434d046185abedc5afddc2761a642bccc70af6d22b46394f1d04a8b24226", serializedData.ToHexString());
      Assert.Equal((uint)65, serializedData.GetLength());
    }

    [Fact]
    public void BlindFactorTest()
    {
      BlindFactor data = new BlindFactor("aa00000000000000000000000000000000000000000000000000000000000001");
      Assert.Equal("aa00000000000000000000000000000000000000000000000000000000000001", data.ToHexString());
      BlindFactor data2 = new BlindFactor(data.GetBytes());
      Assert.Equal(data.ToHexString(), data2.ToHexString());
    }

    [Fact]
    public void ConfidentialAssetTest()
    {
      ConfidentialAsset data = new ConfidentialAsset("aa00000000000000000000000000000000000000000000000000000000000001");
      Assert.False(data.HasBlinding());
      ConfidentialAsset data2 = new ConfidentialAsset(data.ToBytes());
      Assert.Equal(data.ToHexString(), data2.ToHexString());
      Assert.True(data.Equals(data2));
    }

    [Fact]
    public void ConfidentialValueTest()
    {
      ConfidentialValue data = new ConfidentialValue(2000);
      Assert.Equal("0100000000000007d0", data.ToHexString());
      ConfidentialValue data2 = new ConfidentialValue(data.ToBytes());
      Assert.Equal(data.ToHexString(), data2.ToHexString());
    }

    [Fact]
    public void SignParameterTest()
    {
      SignParameter data = new SignParameter("0e68b55347fe37338beb3c28920267c5915a0c474d1dcafc65b087b9b3819cae6ae5e8fb12d669a63127abb4724070f8bd232a9efe3704e6544296a843a64f2c");
      Assert.Equal("0e68b55347fe37338beb3c28920267c5915a0c474d1dcafc65b087b9b3819cae6ae5e8fb12d669a63127abb4724070f8bd232a9efe3704e6544296a843a64f2c", data.ToHexString());
      SignParameter data2 = new SignParameter(data.GetBytes());
      Assert.Equal(data.ToHexString(), data2.ToHexString());

      ByteData normalize = SignParameter.NormalizeSignature(data.GetData());
      Assert.Equal(data.ToHexString(), normalize.ToHexString());
    }

    [Fact]
    public void NotEqualTest()
    {
      Txid txid1 = new Txid();
      Txid txid2 = new Txid("0000000000000000000000000000000000000000000000000000000000000001");
      Assert.True((txid1 != txid2));

      ByteData data = new ByteData("773420c0ded41a55b1f1205cfb632f08f3f911a53e7338a0dac73ec6cbe3ca471907434d046185abedc5afddc2761a642bccc70af6d22b46394f1d04a8b24226");
      Assert.False(txid1.Equals(data));

      OutPoint outpoint1 = new OutPoint();
      Assert.False(outpoint1.Equals(data));
    }

    [Fact]
    public void UtxoEqualsTest()
    {
      OutPoint outpoint1 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 0);
      OutPoint outpoint2 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000002", 0);
      OutPoint outpoint3 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 1);
      UtxoData utxo = new UtxoData(outpoint1);
      UtxoData utxo1 = new UtxoData(outpoint1);
      UtxoData utxo2 = new UtxoData(outpoint2);
      UtxoData utxo3 = new UtxoData(outpoint3);
      Assert.True(utxo.Equals(utxo1));
      Assert.False(utxo.Equals(utxo2));
      Assert.False(utxo.Equals(utxo3));

      ElementsUtxoData eutxo = new ElementsUtxoData(outpoint1);
      ElementsUtxoData eutxo1 = new ElementsUtxoData(outpoint1);
      ElementsUtxoData eutxo2 = new ElementsUtxoData(outpoint2);
      ElementsUtxoData eutxo3 = new ElementsUtxoData(outpoint3);
      Assert.True(eutxo.Equals(eutxo1));
      Assert.False(eutxo.Equals(eutxo2));
      Assert.False(eutxo.Equals(eutxo3));
    }
  }
}
