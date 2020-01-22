using System;
using System.Runtime.InteropServices;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// cfd library (elements transaction) access class.
  /// </summary>
  internal static class CElementsTransaction
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
