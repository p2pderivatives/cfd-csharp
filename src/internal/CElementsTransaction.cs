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
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxIn(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] uint sequence,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxOut(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string assetString,
        [In] long satoshi,
        [In] string valueCommitment,
        [In] string address,
        [In] string directLockingScript,
        [In] string nonce,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdUpdateConfidentialTxOut(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint index,
        [In] string assetString,
        [In] long satoshi,
        [In] string valueCommitment,
        [In] string address,
        [In] string directLockingScript,
        [In] string nonce,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInfo(
        [In] IntPtr handle,
        [In] string txHexString,
        [Out] out IntPtr txid,
        [Out] out IntPtr wtxid,
        [Out] out IntPtr witHash,
        [Out] out uint size,
        [Out] out uint vsize,
        [Out] out uint weight,
        [Out] out uint version,
        [Out] out uint locktime);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxIn(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint index,
        [Out] out IntPtr txid,
        [Out] out uint vout,
        [Out] out uint sequence,
        [Out] out IntPtr scriptSig);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInWitness(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [In] uint stackIndex,
        [Out] out IntPtr stackData);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInPeginWitness(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [In] uint stackIndex,
        [Out] out IntPtr stackData);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetTxInIssuanceInfo(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint index,
        [Out] out IntPtr entropy,
        [Out] out IntPtr nonce,
        [Out] out long assetAmount,
        [Out] out IntPtr assetValue,
        [Out] out long tokenAmount,
        [Out] out IntPtr tokenValue,
        [Out] out IntPtr assetRangeproof,
        [Out] out IntPtr tokenRangeproof);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxOut(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint index,
        [Out] out IntPtr assetString,
        [Out] out long satoshi,
        [Out] out IntPtr valueCommitment,
        [Out] out IntPtr nonce,
        [Out] out IntPtr lockingScript,
        [Out] out IntPtr surjectionProof,
        [Out] out IntPtr rangeproof);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInWitnessCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInPeginWitnessCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxOutCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInIndex(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [Out] out uint index);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialTxOutIndex(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string address,
        [In] string directLockingScript,
        [Out] out uint index);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdSetRawReissueAsset(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] long assetAmount,
        [In] string blindingNonce,
        [In] string entropy,
        [In] string address,
        [In] string directLockingScript,
        [Out] out IntPtr assetString,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetIssuanceBlindingKey(
        [In] IntPtr handle,
        [In] string masterBlindingKey,
        [In] string txid,
        [In] uint vout,
        [Out] out IntPtr blindingKey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeBlindTx(
        [In] IntPtr handle,
        [Out] out IntPtr blindHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddBlindTxInData(
        [In] IntPtr handle,
        [In] IntPtr blindHandle,
        [In] string txid,
        [In] uint vout,
        [In] string assetString,
        [In] string assetBlindFactor,
        [In] string amountBlindFactor,
        [In] long satoshi,
        [In] string assetKey,
        [In] string tokenKey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddBlindTxOutData(
        [In] IntPtr handle,
        [In] IntPtr blindHandle,
        [In] uint index,
        [In] string confidentialKey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeBlindTx(
        [In] IntPtr handle,
        [In] IntPtr blindHandle,
        [In] string txHexString,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeBlindHandle(
        [In] IntPtr handle,
        [In] IntPtr blindHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxSign(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] bool isWitness,
        [In] string signDataHex,
        [In] bool clearStack,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxDerSign(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] bool isWitness,
        [In] string signature,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] bool clearStack,
        [Out] out IntPtr txString);

    /*
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeElementsMultisigSign(
        [In] IntPtr handle,
        [In] IntPtr multisignHandle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string witnessScript,
        [In] string redeemScript,
        [In] bool clearStack,
        [Out] out IntPtr txString);
    */

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddConfidentialTxSignWithPrivkeySimple(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string pubkey,
        [In] string privkey,
        [In] Int64 valueSatoshi,
        [In] string valueCommitment,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] bool hasGrindR,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdCreateConfidentialSighash(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string pubkey,
        [In] string redeemScript,
        [In] long satoshi,
        [In] string valueCommitment,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [Out] out IntPtr sighash);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdUnblindTxOut(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txoutIndex,
        [In] string blindingKey,
        [Out] out IntPtr asset,
        [Out] out long value,
        [Out] out IntPtr assetBlindFactor,
        [Out] out IntPtr amountBlindFactor);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdUnblindIssuance(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [In] string assetBlindingKey,
        [In] string tokenBlindingKey,
        [Out] out IntPtr asset,
        [Out] out long assetValue,
        [Out] out IntPtr assetBlindFactor,
        [Out] out IntPtr assetAmountBlindFactor,
        [Out] out IntPtr token,
        [Out] out long tokenValue,
        [Out] out IntPtr tokenBlindFactor,
        [Out] out IntPtr tokenAmountBlindFactor);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdVerifyConfidentialTxSignature(
        [In] IntPtr handle,
        [In] string txHex,
        [In] string signature,
        [In] string pubkey,
        [In] string script,
        [In] string txid,
        [In] uint vout,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] long satoshi,
        [In] string valueCommitment,
        [In] int witnessVersion);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdVerifyConfidentialTxSign(
        [In] IntPtr handle,
        [In] string txHex,
        [In] string txid,
        [In] uint vout,
        [In] string address,
        [In] int addressType,
        [In] string directLockingScript,
        [In] long satoshi,
        [In] string valueCommitment);
  }
}
