using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /// <summary>
  /// cfd library (transaction) access class.
  /// </summary>
  internal static class NativeMethods
  {
    // Address
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateAddress(
        [In] IntPtr handle,
        [In] int hashType,
        [In] string pubkey,
        [In] string redeemScript,
        [In] int networkType,
        [Out] out IntPtr address,
        [Out] out IntPtr lockingScript,
        [Out] out IntPtr p2shSegwitLockingScript);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeMultisigScript(
        [In] IntPtr handle,
        [In] int networkType,
        [In] int hashType,
        [Out] out IntPtr multisigHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddMultisigScriptData(
        [In] IntPtr handle,
        [In] IntPtr multisigHandle,
        [In] string pubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdFinalizeMultisigScript(
        [In] IntPtr handle,
        [In] IntPtr multisigHandle,
        [In] uint requireNum,
        [Out] out IntPtr address,
        [Out] out IntPtr redeemScript,
        [Out] out IntPtr witnessScript);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeMultisigScriptHandle(
        [In] IntPtr handle,
        [In] IntPtr multisigHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdParseDescriptor(
        [In] IntPtr handle,
        [In] string descriptor,
        [In] int networkType,
        [In] string bip32DerivationPath,
        [Out] out IntPtr descriptorHandle,
        [Out] out uint maxIndex);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetDescriptorData(
        [In] IntPtr handle,
        [In] IntPtr descriptorHandle,
        [In] uint index,
        [Out] out uint maxIndex,
        [Out] out uint depth,
        [Out] out int scriptType,
        [Out] out IntPtr lockingScript,
        [Out] out IntPtr address,
        [Out] out int hashType,
        [Out] out IntPtr redeemScript,
        [Out] out int keyType,
        [Out] out IntPtr pubkey,
        [Out] out IntPtr extPubkey,
        [Out] out IntPtr extPrivkey,
        [Out] out bool isMultisig,
        [Out] out uint maxKeyNum,
        [Out] out uint reqSigNum);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetDescriptorMultisigKey(
        [In] IntPtr handle,
        [In] IntPtr descriptorHandle,
        [In] uint keyIndex,
        [Out] out int keyType,
        [Out] out IntPtr pubkey,
        [Out] out IntPtr extPubkey,
        [Out] out IntPtr extPrivkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeDescriptorHandle(
        [In] IntPtr handle,
        [In] IntPtr descriptorHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetDescriptorChecksum(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string descriptor,
        [Out] out IntPtr descriptorAddedChecksum);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetAddressesFromMultisig(
        [In] IntPtr handle,
        [In] string redeemScript,
        [In] int networkType,
        [In] int hashType,
        [Out] out IntPtr addrMultisigKeysHandle,
        [Out] out uint maxKeyNum);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetAddressFromMultisigKey(
        [In] IntPtr handle,
        [In] IntPtr addrMultisigKeysHandle,
        [In] uint index,
        [Out] out IntPtr address,
        [Out] out IntPtr pubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeAddressesMultisigHandle(
        [In] IntPtr handle,
        [In] IntPtr addrMultisigKeysHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetAddressFromLockingScript(
        [In] IntPtr handle,
        [In] string lockingScript,
        [In] int networkType,
        [Out] out IntPtr address);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetAddressInfo(
        [In] IntPtr handle,
        [In] string address,
        [Out] out int networkType,
        [Out] out int hashType,
        [Out] out int witnessVersion,
        [Out] out IntPtr lockingScript,
        [Out] out IntPtr hash);

    // Coin
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdInitializeCoinSelection(
        [In] IntPtr handle,
        [In] uint utxoCount,
        [In] uint targetAssetCount,
        [In] string feeAsset,
        [In] long tx_feeAmount,
        [In] double effectiveFeeRate,
        [In] double longTermFeeRate,
        [In] double dustFeeRate,
        [In] long knapsackMinChange,
        [Out] out IntPtr coinSelectHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddCoinSelectionUtxoTemplate(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint utxoIndex,
        [In] string txid,
        [In] uint vout,
        [In] long amount,
        [In] string asset,
        [In] string descriptor,
        [In] string scriptsigTemplate);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddCoinSelectionAmount(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint assetIndex,
        [In] long amount,
        [In] string asset);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdSetOptionCoinSelection(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] int key,
        [In] long int64Value,
        [In] double doubleValue,
        [In] bool boolValue);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeCoinSelection(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [Out] out long utxoFeeAmount);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetSelectedCoinIndex(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint index,
        [Out] out int utxoIndex);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetSelectedCoinAssetAmount(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint assetIndex,
        [Out] out long amount);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeCoinSelectionHandle(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeEstimateFee(
        [In] IntPtr handle,
        [Out] out IntPtr feeHandle,
        [In] bool isElements);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTxInForEstimateFee(
        [In] IntPtr handle,
        [In] IntPtr feeHandle,
        [In] string txid,
        [In] uint vout,
        [In] string descriptor,
        [In] string asset,
        [In] bool isIssuance,
        [In] bool isBlindIssuance,
        [In] bool isPegin,
        [In] uint peginBtcTxSize,
        [In] string fedpegScript);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTxInTemplateForEstimateFee(
        [In] IntPtr handle,
        [In] IntPtr feeHandle,
        [In] string txid,
        [In] uint vout,
        [In] string descriptor,
        [In] string asset,
        [In] bool isIssuance,
        [In] bool isBlindIssuance,
        [In] bool isPegin,
        [In] uint peginBtcTxSize,
        [In] string fedpegScript,
        [In] string scriptsigTemplate);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdSetOptionEstimateFee(
        [In] IntPtr handle,
        [In] IntPtr feeHandle,
        [In] int key,
        [In] long int64Value,
        [In] double doubleValue,
        [In] bool boolValue);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdFinalizeEstimateFee(
        [In] IntPtr handle,
        [In] IntPtr feeHandle,
        [In] string txHex,
        [In] string feeAsset,
        [Out] out long txFee,
        [Out] out long utxoFee,
        [In] bool isBlind,
        [In] double effectiveFeeRate);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeEstimateFeeHandle(
        [In] IntPtr handle,
        [In] IntPtr feeHandle);

    // Common
    // TODO: unuse CfdGetSupportedFunction([Out] ulong supportFlag);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitialize();

    // TODO: unuse CfdFinalize([In] bool isFinishProcess);

    // TODO: unuse CfdCreateHandle([Out] out IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdCreateSimpleHandle([Out] out IntPtr handle);

    // TODO: unuse CfdCloneHandle([In] IntPtr source, [Out] IntPtr handle);

    // TODO: unuse CfdCopyErrorState([In] IntPtr source, [In] IntPtr destination);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeHandle([In] IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CfdErrorCode CfdFreeBuffer([In] IntPtr address);

    // TODO: unuse CfdGetLastErrorCode([In] IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetLastErrorMessage(
        [In] IntPtr handle,
        [Out] out IntPtr message);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdRequestExecuteJson(
        [In] IntPtr handle,
        [In] string requestName,
        [In] string requestJsonString,
        [Out] out IntPtr responseJsonString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdSerializeByteData(
        [In] IntPtr handle,
        [In] string buffer,
        [Out] out IntPtr output);

    // ElementsAddress
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateConfidentialAddress(
        [In] IntPtr handle,
        [In] string address,
        [In] string confidentialKey,
        [Out] out IntPtr confidentialAddress);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdParseConfidentialAddress(
        [In] IntPtr handle,
        [In] string confidentialAddress,
        [Out] out IntPtr address,
        [Out] out IntPtr confidentialKey,
        [Out] out int networkType);

    // ElementsTransaction
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeConfidentialTx(
        [In] IntPtr handle,
        [In] uint version,
        [In] uint locktime,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddConfidentialTxIn(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] uint sequence,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxIn(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint index,
        [Out] out IntPtr txid,
        [Out] out uint vout,
        [Out] out uint sequence,
        [Out] out IntPtr scriptSig);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInWitness(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [In] uint stackIndex,
        [Out] out IntPtr stackData);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInPeginWitness(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [In] uint stackIndex,
        [Out] out IntPtr stackData);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInWitnessCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInPeginWitnessCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txinIndex,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxOutCount(
        [In] IntPtr handle,
        [In] string txHexString,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxInIndex(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [Out] out uint index);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetConfidentialTxOutIndex(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string address,
        [In] string directLockingScript,
        [Out] out uint index);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetIssuanceBlindingKey(
        [In] IntPtr handle,
        [In] string masterBlindingKey,
        [In] string txid,
        [In] uint vout,
        [Out] out IntPtr blindingKey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetDefaultBlindingKey(
        [In] IntPtr handle,
        [In] string masterBlindingKey,
        [In] string lockingScript,
        [Out] out IntPtr blindingKey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeBlindTx(
        [In] IntPtr handle,
        [Out] out IntPtr blindHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdSetBlindTxOption(
        [In] IntPtr handle,
        [In] IntPtr blindHandle,
        [In] int key,
        [In] long value);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddBlindTxOutData(
        [In] IntPtr handle,
        [In] IntPtr blindHandle,
        [In] uint index,
        [In] string confidentialKey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddBlindTxOutByAddress(
        [In] IntPtr handle,
        [In] IntPtr blindHandle,
        [In] string confidentialAddress);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdFinalizeBlindTx(
        [In] IntPtr handle,
        [In] IntPtr blindHandle,
        [In] string txHexString,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeBlindHandle(
        [In] IntPtr handle,
        [In] IntPtr blindHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddConfidentialTxSign(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] bool isWitness,
        [In] string signDataHex,
        [In] bool clearStack,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    // TODO: unuse CfdFinalizeElementsMultisigSign

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddConfidentialTxSignWithPrivkeySimple(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string pubkey,
        [In] string privkey,
        [In] long valueSatoshi,
        [In] string valueCommitment,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] bool hasGrindR,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdUnblindTxOut(
        [In] IntPtr handle,
        [In] string txHexString,
        [In] uint txoutIndex,
        [In] string blindingKey,
        [Out] out IntPtr asset,
        [Out] out long value,
        [Out] out IntPtr assetBlindFactor,
        [Out] out IntPtr amountBlindFactor);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
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


    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetConfidentialValueHex(
        [In] IntPtr handle,
        [In] long valueSatoshi,
        [In] bool ignoreVersionInfo,
        [Out] out IntPtr valueHex);

    // Key
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCalculateEcSignature(
          [In] IntPtr handle,
          [In] string sighash,
          [In] string privkey,
          [In] string wif,
          [In] int networkType,
          [In] bool hasGrindR,
          [Out] out IntPtr signature);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdVerifyEcSignature(
          [In] IntPtr handle,
          [In] string sighash,
          [In] string pubkey,
          [In] string signature);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCalculateSchnorrSignature(
          [In] IntPtr handle,
          [In] string oraclePrivkey,
          [In] string kValue,
          [In] string message,
          [Out] out IntPtr signature);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdVerifySchnorrSignatureWithNonce(
          [In] IntPtr handle,
          [In] string oraclePrivkey,
          [In] string kValue,
          [In] string message,
          [Out] out IntPtr signature);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdVerifySchnorrSignature(
          [In] IntPtr handle,
          [In] string pubkey,
          [In] string signature,
          [In] string message);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdVerifySchnorrSignatureWithNonce(
          [In] IntPtr handle,
          [In] string pubkey,
          [In] string nonce,
          [In] string signature,
          [In] string message);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdEncodeSignatureByDer(
          [In] IntPtr handle,
          [In] string signature,
          [In] int sighashType,
          [In] bool sighashAnyoneCanPay,
          [Out] out IntPtr derSignature);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdDecodeSignatureFromDer(
          [In] IntPtr handle,
          [In] string derSignature,
          [Out] out IntPtr signature,
          [Out] out int sighashType,
          [Out] out bool sighashAnyoneCanPay);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdNormalizeSignature(
          [In] IntPtr handle,
          [In] string signature,
          [Out] out IntPtr normalizedSignature);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdCreateKeyPair(
          [In] IntPtr handle,
          [In] bool isCompressed,
          [In] int networkType,
          [Out] out IntPtr pubkey,
          [Out] out IntPtr privkey,
          [Out] out IntPtr wif);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetPrivkeyFromWif(
          [In] IntPtr handle,
          [In] string wif,
          [In] int networkType,
          [Out] out IntPtr privkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetPrivkeyWif(
          [In] IntPtr handle,
          [In] string privkey,
          [In] int networkType,
          [In] bool isCompressed,
          [Out] out IntPtr wif);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdParsePrivkeyWif(
          [In] IntPtr handle,
          [In] string wif,
          [Out] out IntPtr privkey,
          [Out] out int networkType,
          [Out] out bool isCompressed);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetPubkeyFromPrivkey(
          [In] IntPtr handle,
          [In] string privkey,
          [In] string wif,
          [In] bool isCompressed,
          [Out] out IntPtr pubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCompressPubkey(
          [In] IntPtr handle,
          [In] string pubkey,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdUncompressPubkey(
          [In] IntPtr handle,
          [In] string pubkey,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeCombinePubkey(
          [In] IntPtr handle,
          [Out] out IntPtr combineHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddCombinePubkey(
          [In] IntPtr handle,
          [In] IntPtr combineHandle,
          [In] string pubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeCombinePubkey(
          [In] IntPtr handle,
          [In] IntPtr combineHandle,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeCombinePubkeyHandle(
          [In] IntPtr handle,
          [In] IntPtr combineHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdPubkeyTweakAdd(
          [In] IntPtr handle,
          [In] string pubkey,
          [In] string tweak,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdPubkeyTweakMul(
          [In] IntPtr handle,
          [In] string pubkey,
          [In] string tweak,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdNegatePubkey(
          [In] IntPtr handle,
          [In] string pubkey,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdPrivkeyTweakAdd(
          [In] IntPtr handle,
          [In] string privkey,
          [In] string tweak,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdPrivkeyTweakMul(
          [In] IntPtr handle,
          [In] string privkey,
          [In] string tweak,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdNegatePrivkey(
          [In] IntPtr handle,
          [In] string privkey,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetSchnorrPubkey(
          [In] IntPtr handle,
          [In] string oraclePubkey,
          [In] string oracleRPoint,
          [In] string message,
          [Out] out IntPtr output);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetSchnorrPublicNonce(
          [In] IntPtr handle,
          [In] string privkey,
          [Out] out IntPtr nonce);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateExtkeyFromSeed(
          [In] IntPtr handle,
          [In] string seedHex,
          [In] int networkType,
          [In] int keyType,
          [Out] out IntPtr extkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateExtkey(
          [In] IntPtr handle,
          [In] int networkType,
          [In] int keyType,
          [In] string parentKey,
          [In] string fingerprint,
          [In] string key,
          [In] string chainCode,
          [In] byte depth,
          [In] uint childNumber,
          [Out] out IntPtr extkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateExtkeyFromParent(
          [In] IntPtr handle,
          [In] string extkey,
          [In] uint childNumber,
          [In] bool hardened,
          [In] int networkType,
          [In] int keyType,
          [Out] out IntPtr childExtkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateExtkeyFromParentPath(
          [In] IntPtr handle,
          [In] string extkey,
          [In] string path,
          [In] int networkType,
          [In] int keyType,
          [Out] out IntPtr childExtkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateExtPubkey(
          [In] IntPtr handle,
          [In] string extkey,
          [In] int networkType,
          [Out] out IntPtr extPubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetPrivkeyFromExtkey(
          [In] IntPtr handle,
          [In] string extkey,
          [In] int networkType,
          [Out] out IntPtr privkey,
          [Out] out IntPtr wif);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetPubkeyFromExtkey(
          [In] IntPtr handle,
          [In] string extkey,
          [In] int networkType,
          [Out] out IntPtr pubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetParentExtkeyPathData(
          [In] IntPtr handle,
          [In] string parentExtkey,
          [In] string path,
          [In] int childKeyType,
          [Out] out IntPtr keyPathData,
          [Out] out IntPtr childKey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetExtkeyInformation(
          [In] IntPtr handle,
          [In] string extkey,
          [Out] out IntPtr version,
          [Out] out IntPtr fingerprint,
          [Out] out IntPtr chainCode,
          [Out] out uint depth,
          [Out] out uint childNumber);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdInitializeMnemonicWordList(
          [In] IntPtr handle,
          [In] string language,
          [Out] out IntPtr mnemonicHandle,
          [Out] out uint maxIndex);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetMnemonicWord(
          [In] IntPtr handle,
          [In] IntPtr mnemonicHandle,
          [In] uint index,
          [Out] out IntPtr mnemonicWord);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeMnemonicWordList(
          [In] IntPtr handle,
          [In] IntPtr mnemonicHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdConvertMnemonicToSeed(
          [In] IntPtr handle,
          [In] byte[] mnemonic,
          [In] string passphrase,
          [In] bool strictCheck,
          [In] string language,
          [In] bool useIdeographicSpace,
          [Out] out IntPtr seed,
          [Out] out IntPtr entropy);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdConvertEntropyToMnemonic(
          [In] IntPtr handle,
          [In] string entropy,
          [In] string language,
          [Out] out IntPtr mnemonic);

    // Script
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdParseScript(
        [In] IntPtr handle,
        [In] string script,
        [Out] out IntPtr scriptItemHandle,
        [Out] out uint scriptItemNum);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetScriptItem(
        [In] IntPtr handle,
        [In] IntPtr scriptItemHandle,
        [In] uint index,
        [Out] out IntPtr scriptItem);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeScriptItemHandle(
        [In] IntPtr handle, [In] IntPtr scriptItemHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdConvertScriptAsmToHex(
        [In] IntPtr handle,
        [In] string scriptAsm,
        [Out] out IntPtr scriptHex);

    // Transaction
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdInitializeTransaction(
        [In] IntPtr handle,
        [In] int networkType,
        [In] uint version,
        [In] uint locktime,
        [In] string txHexString,
        [Out] out IntPtr createHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTransactionInput(
        [In] IntPtr handle,
        [In] IntPtr createHandle,
        [In] string txid,
        [In] uint vout,
        [In] uint sequence);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTransactionOutput(
        [In] IntPtr handle,
        [In] IntPtr createHandle,
        [In] long satoshiValue,
        [In] string address,
        [In] string directLockingScript,
        [In] string asset);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeTransaction(
        [In] IntPtr handle,
        [In] IntPtr createHandle,
        [Out] out IntPtr txHexString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeTransactionHandle(
        [In] IntPtr handle,
        [In] IntPtr createHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTxSign(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string signDataHex,
        [In] bool useDerEncode,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] bool clearStack,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddPubkeyHashSign(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string pubkey,
        [In] string signature,
        [In] bool useDerEncode,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddScriptHashSign(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string redeemScript,
        [In] bool clearStack,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddSignWithPrivkeySimple(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string pubkey,
        [In] string privkey,
        [In] long satoshiValue,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] bool hasGrindR,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeMultisigSign(
        [In] IntPtr handle,
        [Out] out IntPtr multiSignHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddMultisigSignData(
        [In] IntPtr handle,
        [In] IntPtr multiSignHandle,
        [In] string signature,
        [In] string relatedPubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddMultisigSignDataToDer(
        [In] IntPtr handle,
        [In] IntPtr multiSignHandle,
        [In] string signature,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] string relatedPubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdFinalizeMultisigSign(
        [In] IntPtr handle,
        [In] IntPtr multiSignHandle,
        [In] int networkType,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string redeemScript,
        [Out] out IntPtr txString);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeMultisigSignHandle(
        [In] IntPtr handle,
        [In] IntPtr multiSignHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdVerifySignature(
        [In] IntPtr handle,
        [In] int networktype,
        [In] string txhex,
        [In] string signature,
        [In] int hashType,
        [In] string pubkey,
        [In] string redeemScript,
        [In] string txid,
        [In] uint vout,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] long valueSatoshi,
        [In] string valueBytedata);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdVerifyTxSign(
        [In] IntPtr handle,
        [In] int networktype,
        [In] string txhex,
        [In] string txid,
        [In] uint vout,
        [In] string address,
        [In] int addressType,
        [In] string directLockingScript,
        [In] long valueSatoshi,
        [In] string valueBytedata);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateSighash(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [In] int hashType,
        [In] string pubkey,
        [In] string redeemScript,
        [In] long satoshiValue,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [Out] out IntPtr sighash);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxInfo(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [Out] out IntPtr txid,
        [Out] out IntPtr wtxid,
        [Out] out uint size,
        [Out] out uint vsize,
        [Out] out uint weight,
        [Out] out uint version,
        [Out] out uint locktime);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxIn(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] uint index,
        [Out] out IntPtr txid,
        [Out] out uint vout,
        [Out] out uint sequence,
        [Out] out IntPtr scriptSig);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxInWitness(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] uint txinIndex,
        [In] uint stackIndex,
        [Out] out IntPtr stackData);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxOut(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] uint index,
        [Out] out long satoshiValue,
        [Out] out IntPtr lockingScript);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxInCount(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxInWitnessCount(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] uint index,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxOutCount(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [Out] out uint count);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxInIndex(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] string txid,
        [In] uint vout,
        [Out] out uint index);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetTxOutIndex(
        [In] IntPtr handle,
        [In] int networkType,
        [In] string txHexString,
        [In] string address,
        [In] string directLockingScript,
        [Out] out uint index);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdInitializeFundRawTx(
        [In] IntPtr handle,
        [In] int networkType,
        [In] uint targetAssetCount,
        [In] string feeAsset,
        [Out] out IntPtr fundHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTxInForFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] string txid,
        [In] uint vout,
        [In] long amount,
        [In] string descriptor,
        [In] string asset,
        [In] bool isIssuance,
        [In] bool isBlindIssuance,
        [In] bool isPegin,
        [In] uint peginBtcTxSize,
        [In] string fedpegScript);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTxInTemplateForFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] string txid,
        [In] uint vout,
        [In] long amount,
        [In] string descriptor,
        [In] string asset,
        [In] bool isIssuance,
        [In] bool isBlindIssuance,
        [In] bool isPegin,
        [In] uint peginBtcTxSize,
        [In] string fedpegScript,
        [In] string scriptsigTemplate);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddUtxoForFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] string txid,
        [In] uint vout,
        [In] long amount,
        [In] string descriptor,
        [In] string asset);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddUtxoTemplateForFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] string txid,
        [In] uint vout,
        [In] long amount,
        [In] string descriptor,
        [In] string asset,
        [In] string scriptsigTemplate);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTargetAmountForFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] uint assetIndex,
        [In] long amount,
        [In] string asset,
        [In] string reservedAddress);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdSetOptionFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] int key,
        [In] long int64Value,
        [In] double doubleValue,
        [In] bool boolValue);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdFinalizeFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] string txHex,
        [In] double effectiveFeeRate,
        [Out] out long txFee,
        [Out] out uint appendTxOutCount,
        [Out] out IntPtr outputTxHex);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetAppendTxOutFundRawTx(
        [In] IntPtr handle,
        [In] IntPtr fundHandle,
        [In] uint index,
        [Out] out IntPtr appendAddress);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeFundRawTxHandle(
        [In] IntPtr handle,
        [In] IntPtr fundHandle);
  }
}
