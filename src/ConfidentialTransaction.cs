using System;
using System.Collections.Generic;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// asset and amountValue data class.
  /// </summary>
  public struct AssetValueData
  {
    public string Asset { get; }
    public long SatoshiValue { get; }
    public BlindFactor AssetBlindFactor { get; }
    public BlindFactor AmountBlindFactor { get; }

    /// <summary>
    /// Constructor. (use blinded utxo)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    /// <param name="assetBlindFactor">asset blinder</param>
    /// <param name="amountBlindFactor">amount blinder</param>
    public AssetValueData(string asset, long satoshiValue, BlindFactor assetBlindFactor, BlindFactor amountBlindFactor)
    {
      this.Asset = asset;
      this.SatoshiValue = satoshiValue;
      this.AssetBlindFactor = assetBlindFactor;
      this.AmountBlindFactor = amountBlindFactor;
    }

    /// <summary>
    /// Constructor. (use unblinded utxo)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    public AssetValueData(string asset, long satoshiValue)
    {
      this.Asset = asset;
      this.SatoshiValue = satoshiValue;
      this.AssetBlindFactor = new BlindFactor();
      this.AmountBlindFactor = new BlindFactor();
    }

    /// <summary>
    /// Constructor. (use issue/reissue response only)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    /// <param name="amountBlindFactor">amount blinder</param>
    public AssetValueData(string asset, long satoshiValue, BlindFactor amountBlindFactor)
    {
      this.Asset = asset;
      this.SatoshiValue = satoshiValue;
      this.AssetBlindFactor = new BlindFactor();
      this.AmountBlindFactor = amountBlindFactor;
    }
  }

  /// <summary>
  /// unblinding issuance data. (asset and token)
  /// </summary>
  public struct UnblindIssuanceData
  {
    public AssetValueData AssetData { get; }
    public AssetValueData TokenData { get; }

    public UnblindIssuanceData(AssetValueData asset, AssetValueData token)
    {
      AssetData = asset;
      TokenData = token;
    }
  }

  /// <summary>
  /// issuance blinding key pairs.
  /// </summary>
  public struct IssuanceKeys
  {
    public Privkey AssetKey { get; }
    public Privkey TokenKey { get; }

    /// <summary>
    /// Constructor. (use issueasset)
    /// </summary>
    /// <param name="assetKey">asset blinding key</param>
    /// <param name="tokenKey">token blinding key</param>
    public IssuanceKeys(Privkey assetKey, Privkey tokenKey)
    {
      this.AssetKey = assetKey;
      this.TokenKey = tokenKey;
    }

    /// <summary>
    /// Constructor. (use reissueasset)
    /// </summary>
    /// <param name="assetKey">asset blinding key</param>
    public IssuanceKeys(Privkey assetKey)
    {
      this.AssetKey = assetKey;
      this.TokenKey = new Privkey();
    }
  }

  /// <summary>
  /// Confidential transaction class.
  /// </summary>
  public class ConfidentialTransaction
  {
    private string tx;
    private string lastGetTx = "";
    private string txid = "";
    private string wtxid = "";
    private string witHash = "";
    private uint txSize = 0;
    private uint txVsize = 0;
    private uint txWeight = 0;
    private uint txVersion = 0;
    private uint txLocktime = 0;

    public ConfidentialTransaction(uint version, uint locktime)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdInitializeConfidentialTx(
          handle.GetHandle(),
          version,
          locktime,
          out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="txHex">transaction hex</param>
    public ConfidentialTransaction(string txHex)
    {
      tx = txHex;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">transaction byte array</param>
    public ConfidentialTransaction(byte[] bytes)
    {
      tx = StringUtil.FromBytes(bytes);
    }

    public void AddTxIn(Txid txid, uint vout, uint sequence)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxIn(
          handle.GetHandle(), tx, txid.ToHexString(), vout, sequence, out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddTxOut(string asset, long satoshiValue, string address)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            satoshiValue,
            "",
            address,
            "",
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddTxOut(string asset, string valueCommitment, string address)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            (long)0,
            valueCommitment,
            address,
            "",
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddFeeTxOut(long satoshiValue)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            "",
            satoshiValue,
            "",
            "",
            "",
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddDestroyAmountTxOut(string asset, long satoshiValue)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            satoshiValue,
            "",
            "",
            "6a",  // OP_RETURN
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    /// <summary>
    /// Blind transction txout only.
    /// </summary>
    /// <param name="utxos">txin utxo list</param>
    /// <param name="confidentialKeys">txout confidential key list</param>
    public void BlindTxOut(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<uint, Pubkey> confidentialKeys)
    {
      BlindTransaction(utxos, new Dictionary<OutPoint, IssuanceKeys>(),
          confidentialKeys);
    }

    /// <summary>
    /// Blind transction.
    /// </summary>
    /// <param name="utxos">txin utxo list</param>
    /// <param name="issuanceKeys">issuance blinding key list</param>
    /// <param name="confidentialKeys">txout confidential key list</param>
    public void BlindTransaction(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<OutPoint, IssuanceKeys> issuanceKeys,
        IDictionary<uint, Pubkey> confidentialKeys)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdInitializeBlindTx(
          handle.GetHandle(), out IntPtr blindHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          foreach (var outpoint in utxos.Keys)
          {
            var data = utxos[outpoint];
            string assetKey = "";
            string tokenKey = "";
            if (issuanceKeys.ContainsKey(outpoint))
            {
              IssuanceKeys keys = issuanceKeys[outpoint];
              assetKey = keys.AssetKey.ToHexString();
              tokenKey = keys.TokenKey.ToHexString();
            }

            ret = CElementsTransaction.CfdAddBlindTxInData(
              handle.GetHandle(), blindHandle,
              outpoint.GetTxid().ToHexString(), outpoint.GetVout(),
              data.Asset,
              data.AssetBlindFactor.ToHexString(),
              data.AmountBlindFactor.ToHexString(),
              data.SatoshiValue,
              assetKey,
              tokenKey);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          foreach (var index in confidentialKeys.Keys)
          {
            ret = CElementsTransaction.CfdAddBlindTxOutData(
              handle.GetHandle(), blindHandle,
              index, confidentialKeys[index].ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = CElementsTransaction.CfdFinalizeBlindTx(
            handle.GetHandle(), blindHandle, tx,
            out IntPtr txHexString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          tx = CCommon.ConvertToString(txHexString);
        }
        finally
        {
          CElementsTransaction.CfdFreeBlindHandle(handle.GetHandle(), blindHandle);
        }
      }
    }

    /// <summary>
    /// Unblind transction output.
    /// </summary>
    /// <param name="txoutIndex">txout index</param>
    /// <param name="blindingKey">blinding key</param>
    /// <returns>asset and amount data</returns>
    public AssetValueData UnblindTxOut(uint txoutIndex, Privkey blindingKey)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdUnblindTxOut(
            handle.GetHandle(), tx,
            txoutIndex,
            blindingKey.ToHexString(),
            out IntPtr asset,
            out long value,
            out IntPtr assetBlindFactor,
            out IntPtr amountBlindFactor);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        var abf = CCommon.ConvertToString(assetBlindFactor);
        var vbf = CCommon.ConvertToString(amountBlindFactor);
        var assetId = CCommon.ConvertToString(asset);
        return new AssetValueData(assetId, value, new BlindFactor(abf), new BlindFactor(vbf));
      }
    }

    /// <summary>
    /// Unblind transction output.
    /// </summary>
    /// <param name="txinIndex">txin index</param>
    /// <param name="assetBlindingKey">asset blinding key(issue/reissue)</param>
    /// <param name="tokenBlindingKey">token blinding key(issue only)</param>
    /// <returns>issuance asset data</returns>
    public UnblindIssuanceData UnblindIssuance(uint txinIndex, Privkey assetBlindingKey, Privkey tokenBlindingKey)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdUnblindIssuance(
            handle.GetHandle(), tx,
            txinIndex,
            assetBlindingKey.ToHexString(),
            tokenBlindingKey.ToHexString(),
            out IntPtr asset,
            out long assetValue,
            out IntPtr assetBlindFactor,
            out IntPtr assetAmountBlindFactor,
            out IntPtr token,
            out long tokenValue,
            out IntPtr tokenBlindFactor,
            out IntPtr tokenAmountBlindFactor);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        var assetAbf = CCommon.ConvertToString(assetBlindFactor);
        var assetVbf = CCommon.ConvertToString(assetAmountBlindFactor);
        var tokenAbf = CCommon.ConvertToString(tokenBlindFactor);
        var tokenVbf = CCommon.ConvertToString(tokenBlindFactor);
        var assetId = CCommon.ConvertToString(asset);
        var tokenId = CCommon.ConvertToString(token);

        return new UnblindIssuanceData(
            new AssetValueData(assetId, assetValue,
                new BlindFactor(assetAbf), new BlindFactor(assetVbf)),
            new AssetValueData(tokenId, tokenValue,
                new BlindFactor(tokenAbf), new BlindFactor(tokenVbf)));
      }
    }

    public void GetSignatureHash()
    {
      throw new NotImplementedException();  // FIXME not implements
    }

    public void AddSign()
    {
      throw new NotImplementedException();  // FIXME not implements
    }

    public void AddMultisigSign()
    {
      throw new NotImplementedException();  // FIXME not implements
    }

    public void VerifySignature()
    {
      throw new NotImplementedException();  // FIXME not implements
    }

    /*
    public TxIn GetTxIn() {
      // witness, issuanceも
    }

    public TxIn GetTxIns() {
      // witness, issuanceも
    }

    public TxOut GetTxOut() {
    }

    public TxOut GetTxOuts() {
    }
    */

    public string ToHexString()
    {
      return tx;
    }

    public byte[] GetBytes()
    {
      return StringUtil.ToBytes(tx);
    }

    public Txid GetTxid()
    {
      UpdateTxInfoCache();
      return new Txid(txid);
    }

    public Txid GetWtxid()
    {
      UpdateTxInfoCache();
      return new Txid(wtxid);
    }

    public string GetWitHash()
    {
      UpdateTxInfoCache();
      return witHash;
    }

    public uint GetSize()
    {
      UpdateTxInfoCache();
      return txSize;
    }

    public uint GetVsize()
    {
      UpdateTxInfoCache();
      return txVsize;
    }

    public uint GetWeight()
    {
      UpdateTxInfoCache();
      return txWeight;
    }

    public uint GetVersion()
    {
      UpdateTxInfoCache();
      return txVersion;
    }

    public uint GetLockTime()
    {
      UpdateTxInfoCache();
      return txLocktime;
    }

    public static Privkey GetIssuanceBlindingKey(
        Privkey masterBlindingKey, Txid txid, uint vout)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdGetIssuanceBlindingKey(
            handle.GetHandle(),
            masterBlindingKey.ToHexString(),
            txid.ToHexString(),
            vout,
            out IntPtr blindingKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var key = CCommon.ConvertToString(blindingKey);
        return new Privkey(key);
      }
    }

    private void UpdateTxInfoCache()
    {
      if (tx == lastGetTx)
      {
        return;
      }
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdGetConfidentialTxInfo(
            handle.GetHandle(),
            tx,
            out IntPtr outputTxid,
            out IntPtr outputWtxid,
            out IntPtr outputWitHash,
            out uint outputSize,
            out uint outputVsize,
            out uint outputWeight,
            out uint outputVersion,
            out uint outputLocktime);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        txid = CCommon.ConvertToString(outputTxid);
        wtxid = CCommon.ConvertToString(outputWtxid);
        witHash = CCommon.ConvertToString(outputWitHash);
        txSize = outputSize;
        txVsize = outputVsize;
        txWeight = outputWeight;
        txVersion = outputVersion;
        txLocktime = outputLocktime;
        lastGetTx = tx;
      }
    }
  }
}
