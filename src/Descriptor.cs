using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /**
   * @brief descriptor script type.
   */
  public enum CfdDescriptorScriptType
  {
    Null = 0,     //!< null
    Sh,           //!< script hash
    Wsh,          //!< segwit script hash
    Pk,           //!< pubkey
    Pkh,          //!< pubkey hash
    Wpkh,         //!< segwit pubkey hash
    Combo,        //!< combo
    Multi,        //!< multisig
    SortedMulti,  //!< sorted multisig
    Addr,         //!< address
    Raw           //!< raw script
  };

  /**
   * @brief descriptor key type.
   */
  public enum CfdDescriptorKeyType
  {
    Null = 0,  //!< null
    Public,    //!< pubkey
    Bip32,     //!< bip32 extpubkey
    Bip32Priv  //!< bip32 extprivkey
  };

  public class Descriptor
  {
    private string descriptor;
    // Address_type
    // key_type

    public Descriptor(string descriptor_str, CfdNetworkType network)
    {
      ParseDescriptor(descriptor_str, "", network);
    }

    public Descriptor(string descriptor_str, string derive_path, CfdNetworkType network)
    {
      ParseDescriptor(descriptor_str, derive_path, network);
    }

    public void ParseDescriptor(string descriptor_str, string derive_path, CfdNetworkType network)
    {
      descriptor = descriptor_str;
      throw new NotImplementedException();  // FIXME not implementss
      /*
          internal static extern CfdErrorCode CfdParseDescriptor(
              [In] IntPtr handle,
              [In] string descriptor,
              [In] int network_type,
              [In] string bip32_derivation_path,
              [Out] out IntPtr descriptor_handle,
              [Out] out uint max_index);

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
      */
    }

  }
}
