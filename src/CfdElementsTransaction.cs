using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// blind factor data class.
  /// </summary>
  public class BlindFactor
  {
    private string hex_data;

    /// <summary>
    /// Constructor. (empty blinder)
    /// </summary>
    public BlindFactor()
    {
      hex_data = "0000000000000000000000000000000000000000000000000000000000000000";
    }

    /// <summary>
    /// Constructor. (valid blind factor)
    /// </summary>
    /// <param name="blind_factor_hex">blinder hex</param>
    public BlindFactor(string blind_factor_hex)
    {
      hex_data = blind_factor_hex;
    }

    /// <summary>
    /// blinder hex string.
    /// </summary>
    /// <returns>blinder hex string</returns>
    public string ToHexString()
    {
      return hex_data;
    }
  }

  /// <summary>
  /// asset and amountValue data class.
  /// </summary>
  public struct AssetValueData
  {
    public string asset { get; }
    public long satoshi_value { get; }
    public BlindFactor asset_blind_factor { get; }
    public BlindFactor amount_blind_factor { get; }

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
    public AssetValueData asset_data { get; }
    public AssetValueData token_data { get; }

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
    public Privkey asset_key { get; }
    public Privkey token_key { get; }

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
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdInitializeConfidentialTx(
          handle.GetHandle(),
          version,
          locktime,
          out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          CUtil.ThrowError(handle, ret);
        }
        tx = CUtil.ConvertToString(tx_string);
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
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdAddConfidentialTxIn(
          handle.GetHandle(), tx, txid.ToHexString(), vout, sequence, out IntPtr tx_string);
        if (ret != CfdErrorCode.Success)
        {
          CUtil.ThrowError(handle, ret);
        }
        tx = CUtil.ConvertToString(tx_string);
      }
    }

    public void AddTxOut(string asset, long satoshi_value, string address)
    {
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdAddConfidentialTxOut(
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
          CUtil.ThrowError(handle, ret);
        }
        tx = CUtil.ConvertToString(tx_string);
      }
    }

    public void AddTxOut(string asset, string value_commitment, string address)
    {
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdAddConfidentialTxOut(
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
          CUtil.ThrowError(handle, ret);
        }
        tx = CUtil.ConvertToString(tx_string);
      }
    }

    public void AddFeeTxOut(long satoshi_value)
    {
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdAddConfidentialTxOut(
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
          CUtil.ThrowError(handle, ret);
        }
        tx = CUtil.ConvertToString(tx_string);
      }
    }

    public void AddDestroyAmountTxOut(string asset, long satoshi_value)
    {
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdAddConfidentialTxOut(
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
          CUtil.ThrowError(handle, ret);
        }
        tx = CUtil.ConvertToString(tx_string);
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
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdInitializeBlindTx(
          handle.GetHandle(), out IntPtr blind_handle);
        if (ret != CfdErrorCode.Success)
        {
          CUtil.ThrowError(handle, ret);
        }
        try
        {
          foreach (var outpoint in utxos.Keys)
          {
            AssetValueData data = utxos[outpoint];
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
              CUtil.ThrowError(handle, ret);
            }
          }

          foreach (var index in confidential_keys.Keys)
          {
            ret = CElementsTransaction.CfdAddBlindTxOutData(
              handle.GetHandle(), blind_handle,
              index, confidential_keys[index].ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              CUtil.ThrowError(handle, ret);
            }
          }

          ret = CElementsTransaction.CfdFinalizeBlindTx(
            handle.GetHandle(), blind_handle, tx,
            out IntPtr tx_hex_string);
          if (ret != CfdErrorCode.Success)
          {
            CUtil.ThrowError(handle, ret);
          }
          tx = CUtil.ConvertToString(tx_hex_string);
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
      AssetValueData result;
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdUnblindTxOut(
            handle.GetHandle(), tx,
            txout_index,
            blinding_key.ToHexString(),
            out IntPtr asset,
            out long value,
            out IntPtr asset_blind_factor,
            out IntPtr amount_blind_factor);
        if (ret != CfdErrorCode.Success)
        {
          CUtil.ThrowError(handle, ret);
        }

        string abf = CUtil.ConvertToString(asset_blind_factor);
        string vbf = CUtil.ConvertToString(amount_blind_factor);
        string asset_id = CUtil.ConvertToString(asset);
        result = new AssetValueData(asset_id, value, new BlindFactor(abf), new BlindFactor(vbf));
      }
      return result;
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
      UnblindIssuanceData result;
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdUnblindIssuance(
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
          CUtil.ThrowError(handle, ret);
        }

        string asset_abf = CUtil.ConvertToString(asset_blind_factor);
        string asset_vbf = CUtil.ConvertToString(asset_amount_blind_factor);
        string token_abf = CUtil.ConvertToString(token_blind_factor);
        string token_vbf = CUtil.ConvertToString(token_blind_factor);
        string asset_id = CUtil.ConvertToString(asset);
        string token_id = CUtil.ConvertToString(token);

        result = new UnblindIssuanceData(
            new AssetValueData(asset_id, asset_value,
                new BlindFactor(asset_abf), new BlindFactor(asset_vbf)),
            new AssetValueData(token_id, token_value,
                new BlindFactor(token_abf), new BlindFactor(token_vbf)));
      }
      return result;
    }

    public void GetSignatureHash()
    {
      // FIXME implement
    }

    public void AddSign()
    {
      // FIXME implement
    }

    public void AddMultisigSign()
    {
      // FIXME implement
    }

    public void VerifySignature()
    {
      // FIXME implement
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
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdGetIssuanceBlindingKey(
            handle.GetHandle(),
            master_blinding_key.ToHexString(),
            txid.ToHexString(),
            vout,
            out IntPtr blinding_key);
        if (ret != CfdErrorCode.Success)
        {
          CUtil.ThrowError(handle, ret);
        }
        string key = CUtil.ConvertToString(blinding_key);
        return new Privkey(key);
      }
    }

    private void UpdateTxInfoCache()
    {
      if (tx == last_getinfo_tx)
      {
        return;
      }
      using (ErrorHandle handle = new ErrorHandle())
      {
        CfdErrorCode ret = CElementsTransaction.CfdGetConfidentialTxInfo(
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
          CUtil.ThrowError(handle, ret);
        }
        txid = CUtil.ConvertToString(out_txid);
        wtxid = CUtil.ConvertToString(out_wtxid);
        wit_hash = CUtil.ConvertToString(out_wit_hash);
        tx_size = out_size;
        tx_vsize = out_vsize;
        tx_weight = out_weight;
        tx_version = out_version;
        tx_locktime = out_locktime;
        last_getinfo_tx = tx;
      }
    }
  }

  /// <summary>
  /// cfd library (elements transaction) access class.
  /// </summary>
  internal class CElementsTransaction
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeConfidentialTx(
        [In] IntPtr handle,
        [In] uint version,
        [In] uint locktime,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxIn(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] string txid,
        [In] uint vout,
        [In] uint sequence,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxOut(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] string asset_string,
        [In] long value_satoshi,
        [In] string value_commitment,
        [In] string address,
        [In] string direct_locking_script,
        [In] string nonce,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdUpdateConfidentialTxOut(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint index,
        [In] string asset_string,
        [In] long value_satoshi,
        [In] string value_commitment,
        [In] string address,
        [In] string direct_locking_script,
        [In] string nonce,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInfo(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [Out] out IntPtr txid,
        [Out] out IntPtr wtxid,
        [Out] out IntPtr wit_hash,
        [Out] out uint size,
        [Out] out uint vsize,
        [Out] out uint weight,
        [Out] out uint version,
        [Out] out uint locktime);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxIn(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint index,
        [Out] out IntPtr txid,
        [Out] out uint vout,
        [Out] out uint sequence,
        [Out] out IntPtr script_sig);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInWitness(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint txin_index,
        [In] uint stack_index,
        [Out] out IntPtr stack_data);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetTxInIssuanceInfo(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint index,
        [Out] out IntPtr entropy,
        [Out] out IntPtr nonce,
        [Out] out long asset_amount,
        [Out] out IntPtr asset_value,
        [Out] out long token_amount,
        [Out] out IntPtr token_value,
        [Out] out IntPtr asset_rangeproof,
        [Out] out IntPtr token_rangeproof);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxOut(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint index,
        [Out] out IntPtr asset_string,
        [Out] out long value_satoshi,
        [Out] out IntPtr value_commitment,
        [Out] out IntPtr nonce,
        [Out] out IntPtr locking_script,
        [Out] out IntPtr surjection_proof,
        [Out] out IntPtr rangeproof);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInCount(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInWitnessCount(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint txin_index,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxOutCount(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdSetRawReissueAsset(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] string txid,
        [In] uint vout,
        [In] long asset_amount,
        [In] string blinding_nonce,
        [In] string entropy,
        [In] string address,
        [In] string direct_locking_script,
        [Out] out IntPtr asset_string,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetIssuanceBlindingKey(
        [In] IntPtr handle,
        [In] string master_blinding_key,
        [In] string txid,
        [In] uint vout,
        [Out] out IntPtr blinding_key);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeBlindTx(
        [In] IntPtr handle,
        [Out] out IntPtr blind_handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddBlindTxInData(
        [In] IntPtr handle,
        [In] IntPtr blind_handle,
        [In] string txid,
        [In] uint vout,
        [In] string asset_string,
        [In] string asset_blind_factor,
        [In] string amount_blind_factor,
        [In] long value_satoshi,
        [In] string asset_key,
        [In] string token_key);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddBlindTxOutData(
        [In] IntPtr handle,
        [In] IntPtr blind_handle,
        [In] uint index,
        [In] string confidential_key);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeBlindTx(
        [In] IntPtr handle,
        [In] IntPtr blind_handle,
        [In] string tx_hex_string,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeBlindHandle(
        [In] IntPtr handle,
        [In] IntPtr blind_handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxSign(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] string txid,
        [In] uint vout,
        [In] bool is_witness,
        [In] string sign_data_hex,
        [In] bool clear_stack,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxDerSign(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] string txid,
        [In] uint vout,
        [In] bool is_witness,
        [In] string signature,
        [In] int sighash_type,
        [In] bool sighash_anyone_can_pay,
        [In] bool clear_stack,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeElementsMultisigSign(
        [In] IntPtr handle,
        [In] IntPtr multisign_handle,
        [In] string tx_hex_string,
        [In] string txid,
        [In] uint vout,
        [In] int hash_type,
        [In] string witness_script,
        [In] string redeem_script,
        [In] bool clear_stack,
        [Out] out IntPtr tx_string);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdCreateConfidentialSighash(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] string txid,
        [In] uint vout,
        [In] int hash_type,
        [In] string pubkey,
        [In] string redeem_script,
        [In] long value_satoshi,
        [In] string value_commitment,
        [In] int sighash_type,
        [In] bool sighash_anyone_can_pay,
        [Out] out IntPtr sighash);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdUnblindTxOut(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint tx_out_index,
        [In] string blinding_key,
        [Out] out IntPtr asset,
        [Out] out long value,
        [Out] out IntPtr asset_blind_factor,
        [Out] out IntPtr amount_blind_factor);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdUnblindIssuance(
        [In] IntPtr handle,
        [In] string tx_hex_string,
        [In] uint tx_in_index,
        [In] string asset_blinding_key,
        [In] string token_blinding_key,
        [Out] out IntPtr asset,
        [Out] out long asset_value,
        [Out] out IntPtr asset_blind_factor,
        [Out] out IntPtr asset_amount_blind_factor,
        [Out] out IntPtr token,
        [Out] out long token_value,
        [Out] out IntPtr token_blind_factor,
        [Out] out IntPtr token_amount_blind_factor);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdVerifyConfidentialTxSignature(
        [In] IntPtr handle,
        [In] string tx_hex,
        [In] string signature,
        [In] string pubkey,
        [In] string script,
        [In] string txid,
        [In] uint vout,
        [In] int sighash_type,
        [In] bool sighash_anyone_can_pay,
        [In] long value_satoshi,
        [In] string value_commitment,
        [In] int witness_version);
  }
}
