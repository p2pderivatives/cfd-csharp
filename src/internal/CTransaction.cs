using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /// <summary>
  /// cfd library (transaction) access class.
  /// </summary>
  internal static class CTransaction
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeMultisigSign(
        [In] IntPtr handle,
        [Out] out IntPtr multiSignHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddMultisigSignData(
        [In] IntPtr handle,
        [In] IntPtr multiSignHandle,
        [In] string signature,
        [In] string relatedPubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddMultisigSignDataToDer(
        [In] IntPtr handle,
        [In] IntPtr multiSignHandle,
        [In] string signature,
        [In] int sighashType,
        [In] bool sighashAnyoneCanPay,
        [In] string relatedPubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
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
  }
}
