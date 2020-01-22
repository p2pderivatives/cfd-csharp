using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  internal static class CAddress
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdCreateAddress(
        [In] IntPtr handle,
        [In] int hash_type,
        [In] string pubkey,
        [In] string redeem_script,
        [In] int network_type,
        [Out] out IntPtr address,
        [Out] out IntPtr locking_script,
        [Out] out IntPtr p2sh_segwit_locking_script);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdInitializeMultisigScript(
        [In] IntPtr handle,
        [In] int network_type,
        [In] int hash_type,
        [Out] out IntPtr multisig_handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdAddMultisigScriptData(
        [In] IntPtr handle,
        [In] IntPtr multisig_handle,
        [In] string pubkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeMultisigScript(
        [In] IntPtr handle,
        [In] IntPtr multisig_handle,
        [In] uint require_num,
        [Out] out IntPtr address,
        [Out] out IntPtr redeem_script,
        [Out] out IntPtr witness_script);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeMultisigScriptHandle(
        [In] IntPtr handle,
        [In] IntPtr multisig_handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdParseDescriptor(
        [In] IntPtr handle,
        [In] string descriptor,
        [In] int network_type,
        [In] string bip32_derivation_path,
        [Out] out IntPtr descriptor_handle,
        [Out] out uint max_index);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetDescriptorData(
        [In] IntPtr handle,
        [In] IntPtr descriptor_handle,
        [In] uint index,
        [Out] out uint max_index,
        [Out] out uint depth,
        [Out] out int script_type,
        [Out] out IntPtr locking_script,
        [Out] out IntPtr address,
        [Out] out int hash_type,
        [Out] out IntPtr redeem_script,
        [Out] out int key_type,
        [Out] out IntPtr pubkey,
        [Out] out IntPtr ext_pubkey,
        [Out] out IntPtr ext_privkey,
        [Out] out bool is_multisig,
        [Out] out uint max_key_num,
        [Out] out uint req_sig_num);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetDescriptorMultisigKey(
        [In] IntPtr handle,
        [In] IntPtr descriptor_handle,
        [In] uint key_index,
        [Out] out int key_type,
        [Out] out IntPtr pubkey,
        [Out] out IntPtr ext_pubkey,
        [Out] out IntPtr ext_privkey);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeDescriptorHandle(
        [In] IntPtr handle,
        [In] IntPtr descriptor_handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetAddressFromLockingScript(
        [In] IntPtr handle,
        [In] string locking_script,
        [In] int network_type,
        [Out] out IntPtr address);

    /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdGetAddressInfo(
        [In] IntPtr handle,
        [In] string address,
        [Out] out int network_type,
        [Out] out int hash_type,
        [Out] out int witness_version,
        [Out] out IntPtr locking_script,
        [Out] out IntPtr hash);
*/
  }
}
