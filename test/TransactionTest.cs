using Xunit;
using Xunit.Abstractions;

namespace Cfd.xTests
{
  public class TransactionTest
  {
    private readonly ITestOutputHelper output;

    public TransactionTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Fact]
    public void ConstructorTest()
    {
      Transaction tx1 = new Transaction(2, 0);
      Transaction tx2 = new Transaction(tx1.ToHexString());
      Transaction tx3 = new Transaction(tx1.GetBytes());
      Assert.Equal("02000000000000000000", tx1.ToHexString());
      Assert.Equal(tx1.ToHexString(), tx2.ToHexString());
      Assert.Equal(tx1.ToHexString(), tx3.ToHexString());
    }

    [Fact]
    public void CreateRawTransactionTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      Address addr1 = new Address(privkey.DerivePubkey(1).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Regtest);
      Address addr2 = new Address(privkey.DerivePubkey(2).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Regtest);
      Address addr3 = new Address(privkey.DerivePubkey(3).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Regtest);

      OutPoint outpoint1 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 2);
      OutPoint outpoint2 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 3);
      Transaction tx = new Transaction("02000000000000000000", new[] {
        new TxIn(outpoint1),
        new TxIn(outpoint2),
      }, new[] {
        new TxOut(10000, addr1.GetLockingScript()),
        new TxOut(10000, addr2.GetLockingScript()),
      });
      tx.AddTxOut(50000, addr3);
      output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("020000000201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c00000000",
        tx.ToHexString());

      Privkey privkey1 = privkey.DerivePrivkey(11).GetPrivkey();
      Pubkey pubkey1 = privkey1.GetPubkey();
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint1, CfdHashType.P2wpkh, pubkey1, 50000, sighashType);
      SignParameter signature = privkey1.CalculateEcSignature(sighash);
      signature.SetDerEncode(sighashType);
      tx.AddSign(outpoint1, CfdHashType.P2wpkh, signature, true);
      tx.AddSign(outpoint1, CfdHashType.P2wpkh, new SignParameter(pubkey1.ToHexString()), false);
      output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("0200000000010201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c02473044022034db802aad655cd9be589075fc8ef325b6ffb8c24e5b27eb87bde8ad38f5fd7a0220364c916c8e8fc0adf714d7148cd1c6dc6f3e67d55471e57233b1870c65ec2727012103782f0ea892d7000e5f0f82b6ff283382a76500137a542bb0a616530094a8f54c0000000000",
        tx.ToHexString());

      Address addr11 = new Address(pubkey1, CfdAddressType.P2wpkh, CfdNetworkType.Regtest);
      bool isVerify = tx.VerifySign(outpoint1, addr11, addr11.GetAddressType(), 50000);
      Assert.True(isVerify);

      string json = Transaction.DecodeRawTransaction(tx);
      output.WriteLine(json);
    }

    [Fact]
    public void GetTxInfoTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      Address addr1 = new Address(privkey.DerivePubkey(1).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Regtest);
      Address addr2 = new Address(privkey.DerivePubkey(2).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Regtest);
      Address addr3 = new Address(privkey.DerivePubkey(3).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Regtest);

      OutPoint outpoint1 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 2);
      OutPoint outpoint2 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 3);
      var txins = new[] {
        new TxIn(outpoint1),
        new TxIn(outpoint2),
      };
      var txouts = new[] {
        new TxOut(10000, addr1.GetLockingScript()),
        new TxOut(10000, addr2.GetLockingScript()),
      };
      Transaction tx = new Transaction("02000000000000000000", txins, txouts);
      tx.AddTxOut(50000, addr3);
      output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("020000000201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c00000000",
        tx.ToHexString());

      Privkey privkey1 = privkey.DerivePrivkey(11).GetPrivkey();
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      tx.AddSignWithPrivkeySimple(outpoint1, CfdHashType.P2wpkh, privkey1, 50000, sighashType);
      // output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("0200000000010201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c02473044022034db802aad655cd9be589075fc8ef325b6ffb8c24e5b27eb87bde8ad38f5fd7a0220364c916c8e8fc0adf714d7148cd1c6dc6f3e67d55471e57233b1870c65ec2727012103782f0ea892d7000e5f0f82b6ff283382a76500137a542bb0a616530094a8f54c0000000000",
        tx.ToHexString());

      Txid txid = tx.GetTxid();
      output.WriteLine("txid: " + txid.ToHexString());
      Assert.Equal("67e1878d1621e77e166bed9d726bff27b2afcde9eb3dbb1ae3088d0387f40be4",
        txid.ToHexString());
      Txid wtxid = tx.GetWtxid();
      output.WriteLine("wtxid: " + wtxid.ToHexString());
      Assert.Equal("24c66461b4b38c750fa4528d0cf3aea9a13d3156c0a73cfd6fca6958523b97f7",
        wtxid.ToHexString());
      Assert.Equal((uint)295, tx.GetSize());
      Assert.Equal((uint)213, tx.GetVsize());
      Assert.Equal((uint)850, tx.GetWeight());
      Assert.Equal((uint)2, tx.GetVersion());
      Assert.Equal((uint)0, tx.GetLockTime());

      Assert.Equal((uint)2, tx.GetTxInCount());
      Assert.Equal((uint)3, tx.GetTxOutCount());
      Assert.Equal((uint)1, tx.GetTxInIndex(outpoint2));
      Assert.Equal((uint)2, tx.GetTxOutIndex(addr3));
      Assert.Equal((uint)1, tx.GetTxOutIndex(addr2.GetLockingScript()));

      Assert.True(outpoint2.Equals(tx.GetTxIn(outpoint2).OutPoint));
      Assert.True(outpoint2.Equals(tx.GetTxIn(1).OutPoint));
      Assert.True(outpoint2.Equals(tx.GetTxInList()[1].OutPoint));

      Assert.True(addr2.GetLockingScript().Equals(tx.GetTxOut(addr2).ScriptPubkey));
      Assert.True(addr2.GetLockingScript().Equals(tx.GetTxOut(addr2.GetLockingScript()).ScriptPubkey));
      Assert.True(addr2.GetLockingScript().Equals(tx.GetTxOut(1).ScriptPubkey));
      Assert.True(addr2.GetLockingScript().Equals(tx.GetTxOutList()[1].ScriptPubkey));
    }

    [Fact]
    public void PrivkeySignTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      Transaction tx = new Transaction("020000000201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c00000000");
      OutPoint outpoint1 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 2);

      Privkey privkey1 = privkey.DerivePrivkey(11).GetPrivkey();
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      tx.AddSignWithPrivkeySimple(outpoint1, CfdHashType.P2wpkh, privkey1, 50000, sighashType);
      // output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("0200000000010201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c02473044022034db802aad655cd9be589075fc8ef325b6ffb8c24e5b27eb87bde8ad38f5fd7a0220364c916c8e8fc0adf714d7148cd1c6dc6f3e67d55471e57233b1870c65ec2727012103782f0ea892d7000e5f0f82b6ff283382a76500137a542bb0a616530094a8f54c0000000000",
        tx.ToHexString());
    }

    [Fact]
    public void PubkeySignTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      Transaction tx = new Transaction("020000000201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c00000000");
      OutPoint outpoint1 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 2);

      Privkey privkey1 = privkey.DerivePrivkey(11).GetPrivkey();
      Pubkey pubkey1 = privkey1.GetPubkey();
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint1, CfdHashType.P2wpkh, pubkey1, 50000, sighashType);
      SignParameter signature = privkey1.CalculateEcSignature(sighash);
      signature.SetDerEncode(sighashType);
      tx.AddPubkeySign(outpoint1, CfdHashType.P2wpkh, pubkey1, signature);
      // output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("0200000000010201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c02473044022034db802aad655cd9be589075fc8ef325b6ffb8c24e5b27eb87bde8ad38f5fd7a0220364c916c8e8fc0adf714d7148cd1c6dc6f3e67d55471e57233b1870c65ec2727012103782f0ea892d7000e5f0f82b6ff283382a76500137a542bb0a616530094a8f54c0000000000",
        tx.ToHexString());

      bool isVerify = tx.VerifySignature(outpoint1, CfdHashType.P2wpkh, signature, pubkey1, sighashType, 50000);
      Assert.True(isVerify);
    }

    [Fact]
    public void MultisigSignTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      Transaction tx = new Transaction("020000000201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c00000000");
      OutPoint outpoint2 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 3);
      long amount = 25000;

      Privkey privkey21 = privkey.DerivePrivkey(21).GetPrivkey();
      Privkey privkey22 = privkey.DerivePrivkey(22).GetPrivkey();
      Privkey privkey23 = privkey.DerivePrivkey(23).GetPrivkey();
      Pubkey pubkey21 = privkey21.GetPubkey();
      Pubkey pubkey22 = privkey22.GetPubkey();
      Pubkey pubkey23 = privkey23.GetPubkey();
      Script multisigScript = Script.CreateMultisigScript(2, new[] {
        pubkey21, pubkey22, pubkey23,
      });
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint2, CfdHashType.P2shP2wsh, multisigScript, amount, sighashType);
      SignParameter sig22 = privkey22.CalculateEcSignature(sighash);
      SignParameter sig23 = privkey23.CalculateEcSignature(sighash);
      sig22.SetDerEncode(sighashType);
      sig22.SetRelatedPubkey(pubkey22);
      sig23.SetDerEncode(sighashType);
      sig23.SetRelatedPubkey(pubkey23);
      tx.AddMultisigSign(outpoint2, CfdHashType.P2shP2wsh, new[] { sig22, sig23 }, multisigScript);
      output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("0200000000010201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff0100000000000000000000000000000000000000000000000000000000000000030000002322002064a0e02e723ce71d8f18441a39bedd5cefc9c5411c3045614c34bba1a8fbd94fffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c0004004730440220749cbe5080a3ce49c2a89f897be537b2b5449b75c64b57030dea1859b22c183f02200573f5be5170bfe4ca617edec0eb021638dd78b90209bbd8eede8a9e8138a32c01473044022019105df75884ff34111282f32c22986db295596983a87bf0df1d16905b4f9a50022075f8a2c8e3335a4265265b428df185fb045d9614ed1b08929bfa9f3f9d294a72016952210334bd4f1bab7f3e6f6bfc4a4aeaa890b858a9a146c6bd6bc5a3fbc00a12524ca72103ff743075c59596729d74b79694ca99b2c57bed6a77a06871b123b6e0d729823021036759d0dc7623e781de940a9bc9162f69c6ad68cc5be1c748e960ae4613e658e053ae00000000",
        tx.ToHexString());

      bool isVerify = tx.VerifySignature(outpoint2, CfdHashType.P2shP2wsh, sig22, pubkey22, multisigScript, sighashType, amount);
      Assert.True(isVerify);
    }

    [Fact]
    public void ScriptSignTest()
    {
      ExtPrivkey privkey = new ExtPrivkey("xprv9zt1onyw8BdEf7SQ6wUVH3bQQdGD9iy9QzXveQQRhX7i5iUN7jZgLbqFEe491LfjozztYa6bJAGZ65GmDCNcbjMdjZcgmdisPJwVjcfcDhV");
      Transaction tx = new Transaction("020000000201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff01000000000000000000000000000000000000000000000000000000000000000300000000ffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c00000000");
      OutPoint outpoint2 = new OutPoint("0000000000000000000000000000000000000000000000000000000000000001", 3);
      long amount = 25000;

      Privkey privkey21 = privkey.DerivePrivkey(21).GetPrivkey();
      Privkey privkey22 = privkey.DerivePrivkey(22).GetPrivkey();
      Privkey privkey23 = privkey.DerivePrivkey(23).GetPrivkey();
      Pubkey pubkey21 = privkey21.GetPubkey();
      Pubkey pubkey22 = privkey22.GetPubkey();
      Pubkey pubkey23 = privkey23.GetPubkey();
      Script multisigScript = Script.CreateMultisigScript(2, new[] {
        pubkey21, pubkey22, pubkey23,
      });
      SignatureHashType sighashType = new SignatureHashType(CfdSighashType.All, false);
      ByteData sighash = tx.GetSignatureHash(outpoint2, CfdHashType.P2shP2wsh, multisigScript, amount, sighashType);
      SignParameter sig22 = privkey22.CalculateEcSignature(sighash);
      SignParameter sig23 = privkey23.CalculateEcSignature(sighash);
      sig22.SetDerEncode(sighashType);
      sig22.SetRelatedPubkey(pubkey22);
      sig23.SetDerEncode(sighashType);
      sig23.SetRelatedPubkey(pubkey23);
      tx.AddScriptSign(outpoint2, CfdHashType.P2shP2wsh, new[] { new SignParameter(), sig22, sig23 }, multisigScript);
      output.WriteLine("tx:\n" + tx.ToHexString());
      Assert.Equal("0200000000010201000000000000000000000000000000000000000000000000000000000000000200000000ffffffff0100000000000000000000000000000000000000000000000000000000000000030000002322002064a0e02e723ce71d8f18441a39bedd5cefc9c5411c3045614c34bba1a8fbd94fffffffff0310270000000000001600148b756cbd98f4f55e985f80437a619d47f0732a941027000000000000160014c0a3dd0b7c1b3281be91112e16ce931dbac2a97950c3000000000000160014ad3abd3c325e40e20d89aa054dd980b97494f16c0004004730440220749cbe5080a3ce49c2a89f897be537b2b5449b75c64b57030dea1859b22c183f02200573f5be5170bfe4ca617edec0eb021638dd78b90209bbd8eede8a9e8138a32c01473044022019105df75884ff34111282f32c22986db295596983a87bf0df1d16905b4f9a50022075f8a2c8e3335a4265265b428df185fb045d9614ed1b08929bfa9f3f9d294a72016952210334bd4f1bab7f3e6f6bfc4a4aeaa890b858a9a146c6bd6bc5a3fbc00a12524ca72103ff743075c59596729d74b79694ca99b2c57bed6a77a06871b123b6e0d729823021036759d0dc7623e781de940a9bc9162f69c6ad68cc5be1c748e960ae4613e658e053ae00000000",
        tx.ToHexString());
    }

    [Fact]
    public void EstimateFeeTest()
    {
      // p2sh-p2wpkh
      UtxoData[] utxos = GetBitcoinBnbUtxoList(CfdNetworkType.Mainnet);
      ExtPubkey key = new ExtPubkey("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy");
      Address setAddr1 = new Address(key.DerivePubkey(11).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);
      Address setAddr2 = new Address(key.DerivePubkey(12).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);

      // amount = 85062500 + 39062500 = 124125000
      long amount = utxos[1].GetAmount() + utxos[2].GetAmount();
      long feeAmount = 5000;
      long amount1 = 100000000;
      long amount2 = amount - amount1 - feeAmount;
      Transaction tx = new Transaction(2, 0, new[] {
        new TxIn(utxos[1].GetOutPoint()),
        new TxIn(utxos[2].GetOutPoint()),
      }, new[] {
        new TxOut(amount1, setAddr1.GetLockingScript()),
        new TxOut(amount2, setAddr2.GetLockingScript()),
      });

      FeeData feeData = tx.EstimateFee(new[] { utxos[1], utxos[2] }, 10.0);
      Assert.Equal(720, feeData.TxFee);
      Assert.Equal(1800, feeData.InputFee);
    }

    [Fact]
    public void FundRawTransactionTest()
    {
      UtxoData[] utxos = GetBitcoinBnbUtxoList(CfdNetworkType.Mainnet);
      ExtPubkey key = new ExtPubkey("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy");
      Address setAddr1 = new Address(key.DerivePubkey(11).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);
      Address setAddr2 = new Address(key.DerivePubkey(12).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);

      Transaction tx = new Transaction(2, 0, null, new[] {
        new TxOut(10000000, setAddr1.GetLockingScript()),
        new TxOut(4000000, setAddr2.GetLockingScript()),
      });

      Address addr1 = new Address(key.DerivePubkey(1).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);
      string usedAddr = tx.FundRawTransaction(null, utxos, addr1.ToAddressString(), 20.0);
      output.WriteLine("tx: " + tx.ToHexString());

      Assert.Equal("02000000010af4768e14f820cb9063f55833b5999119e53390ecf4bf181842909b11d0974d0000000000ffffffff0380969800000000001600144352a1a6e86311f22274f7ebb2746de21b09b15d00093d00000000001600148beaaac4654cf4ebd8e46ca5062b0e7fb3e7470ce47f19000000000016001478eb9fc2c9e1cdf633ecb646858ba862b21384ab00000000",
        tx.ToHexString());
      output.WriteLine(Transaction.DecodeRawTransaction(tx));
      Assert.Equal(addr1.ToAddressString(), usedAddr);
      Assert.Equal(3860, tx.GetLastTxFee());
    }

    [Fact]
    public void FundRawTransactionExistTxInTest()
    {
      // p2sh-p2wpkh
      UtxoData[] utxos = GetBitcoinBnbUtxoList(CfdNetworkType.Mainnet);
      ExtPubkey key = new ExtPubkey("xpub661MyMwAqRbcGB88KaFbLGiYAat55APKhtWg4uYMkXAmfuSTbq2QYsn9sKJCj1YqZPafsboef4h4YbXXhNhPwMbkHTpkf3zLhx7HvFw1NDy");
      Address setAddr1 = new Address(key.DerivePubkey(11).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);
      Address setAddr2 = new Address(key.DerivePubkey(12).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);

      // amount = 85062500 + 39062500 = 124125000
      long amount1 = 100000000;
      long amount2 = 100000000;
      Transaction tx = new Transaction(2, 0, new[] {
        new TxIn(utxos[1].GetOutPoint()),
        new TxIn(utxos[2].GetOutPoint()),
      }, new[] {
        new TxOut(amount1, setAddr1.GetLockingScript()),
        new TxOut(amount2, setAddr2.GetLockingScript()),
      });

      UtxoData[] inputUtxos = new[] {
        utxos[1],
        utxos[2],
      };
      Address addr1 = new Address(key.DerivePubkey(1).GetPubkey(), CfdAddressType.P2wpkh, CfdNetworkType.Mainnet);
      double feeRate = 20.0;
      string usedAddr = tx.FundRawTransaction(inputUtxos, utxos, addr1.ToAddressString(), feeRate);
      output.WriteLine("tx: " + tx.ToHexString());

      Assert.Equal("02000000030a9a33750a810cd384ca5d93b09513f1eb5d93c669091b29eef710d2391ff7300000000000ffffffff0a9bf51e0ac499391efd9426e2c909901edd74a97d2378b49c8832c491ad1e9e0000000000ffffffff0a503dbd4f8f2b064c70e048b21f93fe4584174478abf5f44747932cd21da87c0000000000ffffffff0300e1f505000000001600144352a1a6e86311f22274f7ebb2746de21b09b15d00e1f505000000001600148beaaac4654cf4ebd8e46ca5062b0e7fb3e7470c0831b8040000000016001478eb9fc2c9e1cdf633ecb646858ba862b21384ab00000000",
        tx.ToHexString());
      output.WriteLine(Transaction.DecodeRawTransaction(tx));
      Assert.Equal(addr1.ToAddressString(), usedAddr);
      Assert.Equal(7460, tx.GetLastTxFee());

      UtxoData[] feeUtxos = new[]{
        utxos[1],
        utxos[2],
        utxos[0],
      };
      FeeData feeData = tx.EstimateFee(feeUtxos, feeRate);
      Assert.Equal(7460, feeData.TxFee + feeData.InputFee);
      Assert.Equal(2060, feeData.TxFee);
      Assert.Equal(5400, feeData.InputFee);
    }

    static UtxoData[] GetBitcoinBnbUtxoList(CfdNetworkType netType)
    {
      string desc = "sh(wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1))#4z2vy08x";
      UtxoData[] utxos = new[] {
        new UtxoData(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0), 155062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0), 85062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("9e1ead91c432889cb478237da974dd1e9009c9e22694fd1e3999c40a1ef59b0a", 0), 39062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("8f4af7ee42e62a3d32f25ca56f618fb2f5df3d4c3a9c59e2c3646c5535a3d40a", 0), 61062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("4d97d0119b90421818bff4ec9033e5199199b53358f56390cb20f8148e76f40a", 0), 15675000,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("b9720ed2265a4ced42425bffdb4ef90a473b4106811a802fce53f7c57487fa0b", 0), 14938590,
          new Descriptor(desc, netType)),
      };
      return utxos;
    }
  }
}
