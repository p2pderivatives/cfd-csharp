using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
    public readonly string asset;
    public readonly long satoshi_value;
    public readonly BlindFactor asset_blind_factor;
    public readonly BlindFactor amount_blind_factor;

    /// <summary>
    /// Constructor. (use blinded utxo)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshi_value">satoshi amount</param>
    /// <param name="asset_blind_factor">asset blinder</param>
    /// <param name="amount_blind_factor">amount blinder</param>
    public AssetValueData(string asset, long satoshi_value, BlindFactor asset_blind_factor, BlindFactor amount_blind_factor)
    {
      this.asset = asset;
      this.satoshi_value = satoshi_value;
      this.asset_blind_factor = asset_blind_factor;
      this.amount_blind_factor = amount_blind_factor;
    }

    /// <summary>
    /// Constructor. (use unblinded utxo)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshi_value">satoshi amount</param>
    public AssetValueData(string asset, long satoshi_value)
    {
      this.asset = asset;
      this.satoshi_value = satoshi_value;
      this.asset_blind_factor = new BlindFactor();
      this.amount_blind_factor = new BlindFactor();
    }

    /// <summary>
    /// Constructor. (use issue/reissue response only)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshi_value">satoshi amount</param>
    /// <param name="amount_blind_factor">amount blinder</param>
    public AssetValueData(string asset, long satoshi_value, BlindFactor amount_blind_factor)
    {
      this.asset = asset;
      this.satoshi_value = satoshi_value;
      this.asset_blind_factor = new BlindFactor();
      this.amount_blind_factor = amount_blind_factor;
    }
  }

  /// <summary>
  /// unblinding issuance data. (asset and token)
  /// </summary>
  public struct UnblindIssuanceData
  {
    public readonly AssetValueData asset_data;
    public readonly AssetValueData token_data;

    public UnblindIssuanceData(AssetValueData asset, AssetValueData token)
    {
      asset_data = asset;
      token_data = token;
    }
  }

  /// <summary>
  /// issuance blinding key pairs.
  /// </summary>
  public struct IssuanceKeys
  {
    public readonly Privkey asset_key;
    public readonly Privkey token_key;

    /// <summary>
    /// Constructor. (use issueasset)
    /// </summary>
    /// <param name="asset_key">asset blinding key</param>
    /// <param name="token_key">token blinding key</param>
    public IssuanceKeys(Privkey asset_key, Privkey token_key)
    {
      this.asset_key = asset_key;
      this.token_key = token_key;
    }

    /// <summary>
    /// Constructor. (use reissueasset)
    /// </summary>
    /// <param name="asset_key">asset blinding key</param>
    public IssuanceKeys(Privkey asset_key)
    {
      this.asset_key = asset_key;
      this.token_key = new Privkey();
    }
  }

  /// <summary>
  /// Confidential transaction class.
  /// </summary>
  public class ConfidentialTransaction
  {
    private string tx;
    private string last_getinfo_tx = "";
    private string txid = "";
    private string wtxid = "";
    private string wit_hash = "";
    private uint tx_size = 0;
    private uint tx_vsize = 0;
    private uint tx_weight = 0;
    private uint tx_version = 0;
    private uint tx_locktime = 0;

    public ConfidentialTransaction(uint version, uint locktime)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdInitializeConfidentialTx(
          handle.GetHandle(),
          version,
          locktime,
          out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(tx_string);
      }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="tx_hex">transaction hex</param>
    public ConfidentialTransaction(string tx_hex)
    {
      tx = tx_hex;
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
          handle.GetHandle(), tx, txid.ToHexString(), vout, sequence, out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(tx_string);
      }
    }

    public void AddTxOut(string asset, long satoshi_value, string address)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            satoshi_value,
            "",
            address,
            "",
            "",
            out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(tx_string);
      }
    }

    public void AddTxOut(string asset, string value_commitment, string address)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            (long)0,
            value_commitment,
            address,
            "",
            "",
            out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(tx_string);
      }
    }

    public void AddFeeTxOut(long satoshi_value)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            "",
            satoshi_value,
            "",
            "",
            "",
            "",
            out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(tx_string);
      }
    }

    public void AddDestroyAmountTxOut(string asset, long satoshi_value)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            satoshi_value,
            "",
            "",
            "6a",  // OP_RETURN
            "",
            out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(tx_string);
      }
    }

    /// <summary>
    /// Blind transction txout only.
    /// </summary>
    /// <param name="utxos">txin utxo list</param>
    /// <param name="confidential_keys">txout confidential key list</param>
    public void BlindTxOut(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<uint, Pubkey> confidential_keys)
    {
      BlindTransaction(utxos, new Dictionary<OutPoint, IssuanceKeys>(),
          confidential_keys);
    }

    /// <summary>
    /// Blind transction.
    /// </summary>
    /// <param name="utxos">txin utxo list</param>
    /// <param name="issuance_keys">issuance blinding key list</param>
    /// <param name="confidential_keys">txout confidential key list</param>
    public void BlindTransaction(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<OutPoint, IssuanceKeys> issuance_keys,
        IDictionary<uint, Pubkey> confidential_keys)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdInitializeBlindTx(
          handle.GetHandle(), out IntPtr blind_handle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          foreach (var outpoint in utxos.Keys)
          {
            var data = utxos[outpoint];
            string asset_key = "";
            string token_key = "";
            if (issuance_keys.ContainsKey(outpoint))
            {
              IssuanceKeys keys = issuance_keys[outpoint];
              asset_key = keys.asset_key.ToHexString();
              token_key = keys.token_key.ToHexString();
            }

            ret = CElementsTransaction.CfdAddBlindTxInData(
              handle.GetHandle(), blind_handle,
              outpoint.GetTxid().ToHexString(), outpoint.GetVout(),
              data.asset,
              data.asset_blind_factor.ToHexString(),
              data.amount_blind_factor.ToHexString(),
              data.satoshi_value,
              asset_key,
              token_key);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          foreach (var index in confidential_keys.Keys)
          {
            ret = CElementsTransaction.CfdAddBlindTxOutData(
              handle.GetHandle(), blind_handle,
              index, confidential_keys[index].ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = CElementsTransaction.CfdFinalizeBlindTx(
            handle.GetHandle(), blind_handle, tx,
            out IntPtr tx_hex_string);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          tx = CCommon.ConvertToString(tx_hex_string);
        }
        finally
        {
          CElementsTransaction.CfdFreeBlindHandle(handle.GetHandle(), blind_handle);
        }
      }
    }

    /// <summary>
    /// Unblind transction output.
    /// </summary>
    /// <param name="txout_index">txout index</param>
    /// <param name="blinding_key">blinding key</param>
    /// <returns>asset and amount data</returns>
    public AssetValueData UnblindTxOut(uint txout_index, Privkey blinding_key)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdUnblindTxOut(
            handle.GetHandle(), tx,
            txout_index,
            blinding_key.ToHexString(),
            out IntPtr asset,
            out long value,
            out IntPtr asset_blind_factor,
            out IntPtr amount_blind_factor);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        var abf = CCommon.ConvertToString(asset_blind_factor);
        var vbf = CCommon.ConvertToString(amount_blind_factor);
        var asset_id = CCommon.ConvertToString(asset);
        return new AssetValueData(asset_id, value, new BlindFactor(abf), new BlindFactor(vbf));
      }
    }

    /// <summary>
    /// Unblind transction output.
    /// </summary>
    /// <param name="txin_index">txin index</param>
    /// <param name="asset_blinding_key">asset blinding key(issue/reissue)</param>
    /// <param name="token_blinding_key">token blinding key(issue only)</param>
    /// <returns>issuance asset data</returns>
    public UnblindIssuanceData UnblindIssuance(uint txin_index, Privkey asset_blinding_key, Privkey token_blinding_key)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdUnblindIssuance(
            handle.GetHandle(), tx,
            txin_index,
            asset_blinding_key.ToHexString(),
            token_blinding_key.ToHexString(),
            out IntPtr asset,
            out long asset_value,
            out IntPtr asset_blind_factor,
            out IntPtr asset_amount_blind_factor,
            out IntPtr token,
            out long token_value,
            out IntPtr token_blind_factor,
            out IntPtr token_amount_blind_factor);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        var asset_abf = CCommon.ConvertToString(asset_blind_factor);
        var asset_vbf = CCommon.ConvertToString(asset_amount_blind_factor);
        var token_abf = CCommon.ConvertToString(token_blind_factor);
        var token_vbf = CCommon.ConvertToString(token_blind_factor);
        var asset_id = CCommon.ConvertToString(asset);
        var token_id = CCommon.ConvertToString(token);

        return new UnblindIssuanceData(
            new AssetValueData(asset_id, asset_value,
                new BlindFactor(asset_abf), new BlindFactor(asset_vbf)),
            new AssetValueData(token_id, token_value,
                new BlindFactor(token_abf), new BlindFactor(token_vbf)));
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
      return wit_hash;
    }

    public uint GetSize()
    {
      UpdateTxInfoCache();
      return tx_size;
    }

    public uint GetVsize()
    {
      UpdateTxInfoCache();
      return tx_vsize;
    }

    public uint GetWeight()
    {
      UpdateTxInfoCache();
      return tx_weight;
    }

    public uint GetVersion()
    {
      UpdateTxInfoCache();
      return tx_version;
    }

    public uint GetLockTime()
    {
      UpdateTxInfoCache();
      return tx_locktime;
    }

    public static Privkey GetIssuanceBlindingKey(
        Privkey master_blinding_key, Txid txid, uint vout)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdGetIssuanceBlindingKey(
            handle.GetHandle(),
            master_blinding_key.ToHexString(),
            txid.ToHexString(),
            vout,
            out IntPtr blinding_key);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var key = CCommon.ConvertToString(blinding_key);
        return new Privkey(key);
      }
    }

    private void UpdateTxInfoCache()
    {
      if (tx == last_getinfo_tx)
      {
        return;
      }
      using (var handle = new ErrorHandle())
      {
        var ret = CElementsTransaction.CfdGetConfidentialTxInfo(
            handle.GetHandle(),
            tx,
            out IntPtr out_txid,
            out IntPtr out_wtxid,
            out IntPtr out_wit_hash,
            out uint out_size,
            out uint out_vsize,
            out uint out_weight,
            out uint out_version,
            out uint out_locktime);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        txid = CCommon.ConvertToString(out_txid);
        wtxid = CCommon.ConvertToString(out_wtxid);
        wit_hash = CCommon.ConvertToString(out_wit_hash);
        tx_size = out_size;
        tx_vsize = out_vsize;
        tx_weight = out_weight;
        tx_version = out_version;
        tx_locktime = out_locktime;
        last_getinfo_tx = tx;
      }
    }
  }
}
