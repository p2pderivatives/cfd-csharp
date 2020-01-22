using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  internal static class CAddress
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateAddress(
        [In] IntPtr handle,
        [In] int hashType,
        [In] string pubkey,
        [In] string redeemScript,
        [In] int networkType,
        [Out] out IntPtr address,
        [Out] out IntPtr lockingScript,
        [Out] out IntPtr p2shSegwitLockingScript;

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeMultisigScript(
        [In] IntPtr handle,
        [In] int networkType,
        [In] int hashType,
        [Out] out IntPtr multisigHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddMultisigScriptData(
        [In] IntPtr handle,
        [In] IntPtr multisigHandle,
        [In] string pubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeMultisigScript(
        [In] IntPtr handle,
        [In] IntPtr multisigHandle,
        [In] uint requireNum,
        [Out] out IntPtr address,
        [Out] out IntPtr redeemScript,
        [Out] out IntPtr witnessScript;

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeMultisigScriptHandle(
        [In] IntPtr handle,
        [In] IntPtr multisigHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdParseDescriptor(
        [In] IntPtr handle,
        [In] string descriptor,
        [In] int networkType,
        [In] string bip32DerivationPath,
        [Out] out IntPtr descriptorHandle,
        [Out] out uint maxIndex);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
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

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
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
    internal static extern CfdErrorCode CfdGetAddressFromLockingScript(
        [In] IntPtr handle,
        [In] string lockingScript,
        [In] int networkType,
        [Out] out IntPtr address);

    /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetAddressInfo(
        [In] IntPtr handle,
        [In] string address,
        [Out] out int networkType,
        [Out] out int hashType,
        [Out] out int witnessVersion,
        [Out] out IntPtr lockingScript,
        [Out] out IntPtr hash);
*/
  }
}
