using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /**
   * @brief network type
   */
  public enum CfdNetworkType
  {
    Mainnet = 0,      //!< btc mainnet
    Testnet,          //!< btc testnet
    Regtest,          //!< btc regtest
    Liquidv1 = 10,    //!< liquidv1
    ElementsRegtest,  //!< elements regtest
    CustomChain,      //!< elements custom chain
  };

  /**
   * @brief address type
   */
  public enum CfdAddressType
  {
    P2sh = 1,   //!< Legacy address (Script Hash)
    P2pkh,      //!< Legacy address (PublicKey Hash)
    P2wsh,      //!< Native segwit address (Script Hash)
    P2wpkh,     //!< Native segwit address (PublicKey Hash)
    P2shP2wsh,  //!< P2sh wrapped address (Script Hash)
    P2shP2wpkh  //!< P2sh wrapped address (Pubkey Hash)
  };

  /**
   * @brief hash type
   */
  public enum CfdHashType
  {
    P2sh = 1,   //!< Script Hash
    P2pkh,      //!< PublicKey Hash
    P2wsh,      //!< Native segwit Script Hash
    P2wpkh,     //!< Native segwit PublicKey Hash
    P2shP2wsh,  //!< P2sh wrapped segwit Script Hash
    P2shP2wpkh  //!< P2sh wrapped segwit Pubkey Hash
  };

  /**
   * @brief witness version
   */
  public enum CfdWitnessVersion
  {
    VersionNone = -1,  //!< Missing WitnessVersion
    Version0 = 0,      //!< version 0
    Version1,          //!< version 1 (for future use)
    Version2,          //!< version 2 (for future use)
    Version3,          //!< version 3 (for future use)
    Version4,          //!< version 4 (for future use)
    Version5,          //!< version 5 (for future use)
    Version6,          //!< version 6 (for future use)
    Version7,          //!< version 7 (for future use)
    Version8,          //!< version 8 (for future use)
    Version9,          //!< version 9 (for future use)
    Version10,         //!< version 10 (for future use)
    Version11,         //!< version 11 (for future use)
    Version12,         //!< version 12 (for future use)
    Version13,         //!< version 13 (for future use)
    Version14,         //!< version 14 (for future use)
    Version15,         //!< version 15 (for future use)
    Version16          //!< version 16 (for future use)
  };

  /**
   * @brief sighash type
   */
  public enum CfdSighashType
  {
    All = 0x01,    //!< SIGHASH_ALL
    None = 0x02,   //!< SIGHASH_NONE
    Single = 0x03  //!< SIGHASH_SINGLE
  };

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
      // FIXME implements
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

  /* for cfd cg-v0.0.11 or p2pderivatives-v0.0.4
public class Address
{
  private string address;
  private string locking_script;
  private string p2sh_segwit_locking_script;
  private string hash;
  private CfdNetworkType network;
  private CfdAddressType address_type;
  private CfdWitnessVersion witness_version;

  public Address()
  {
  }

      public Address(string address_str)
      {
        using (ErrorHandle handle = new ErrorHandle())
        {
          Initialize(handle, address_str);
          address = address_str;
        }
      }

  public Address(Pubkey pubkey, CfdAddressType type, CfdNetworkType network)
  {
    using (ErrorHandle handle = new ErrorHandle())
    {
      CfdErrorCode ret = CAddress.CfdCreateAddress(
        handle.GetHandle(),
        (int)type,
        pubkey.ToHexString(),
        "",
        (int)network,
        out IntPtr out_address,
        out IntPtr out_locking_script,
        out IntPtr out_p2sh_segwit_locking_script);
      if (ret != CfdErrorCode.Success)
      {
        CUtil.ThrowError(handle, ret);
      }
      address = CUtil.ConvertToString(out_address);
      locking_script = CUtil.ConvertToString(out_locking_script);
      p2sh_segwit_locking_script = CUtil.ConvertToString(out_p2sh_segwit_locking_script);

      Initialize(handle, address);
      address_type = type;
    }
  }

  public Address(Script script, CfdAddressType type, CfdNetworkType network)
  {
    using (ErrorHandle handle = new ErrorHandle())
    {
      CfdErrorCode ret = CAddress.CfdCreateAddress(
        handle.GetHandle(),
        (int)type,
        "",
        script.ToHexString(),
        (int)network,
        out IntPtr out_address,
        out IntPtr out_locking_script,
        out IntPtr out_p2sh_segwit_locking_script);
      if (ret != CfdErrorCode.Success)
      {
        CUtil.ThrowError(handle, ret);
      }
      address = CUtil.ConvertToString(out_address);
      locking_script = CUtil.ConvertToString(out_locking_script);
      p2sh_segwit_locking_script = CUtil.ConvertToString(out_p2sh_segwit_locking_script);

      Initialize(handle, address);
      address_type = type;
    }
  }

  public static Address GetAddressByLockingScript(Script in_locking_script, CfdNetworkType network)
  {
    string address = "";
    using (ErrorHandle handle = new ErrorHandle())
    {
      CfdErrorCode ret = CAddress.CfdGetAddressFromLockingScript(
        handle.GetHandle(),
        in_locking_script.ToHexString(),
        (int)network,
        out IntPtr out_address);
      if (ret != CfdErrorCode.Success)
      {
        CUtil.ThrowError(handle, ret);
      }
      address = CUtil.ConvertToString(out_address);
    }
    return new Address(address);
  }

  public string ToAddressString()
  {
    return address;
  }

  public string GetLockingScript()
  {
    return locking_script;
  }

  public string GetHash()
  {
    return hash;
  }

  public string GetP2shLockingScript()
  {
    return p2sh_segwit_locking_script;
  }

  public CfdNetworkType GetNetwork()
  {
    return network;
  }

  public CfdAddressType GetAddressType()
  {
    return address_type;
  }

  public CfdWitnessVersion GetWitnessVersion()
  {
    return witness_version;
  }

  public void SetAddressType(CfdAddressType addr_type)
  {
    address_type = addr_type;
  }

  private void Initialize(ErrorHandle handle, string address_str)
  {
      CfdErrorCode ret = CAddress.CfdGetAddressInfo(
        handle.GetHandle(),
        address_str,
        out int out_network_type,
        out int out_hash_type,
        out int out_witness_version,
        out IntPtr out_locking_script,
        out IntPtr out_hash);
      if (ret != CfdErrorCode.Success)
      {
        CUtil.ThrowError(handle, ret);
      }
      network = (CfdNetworkType)out_network_type;
      address_type = (CfdAddressType)out_hash_type;
      witness_version = (CfdWitnessVersion)out_witness_version;
      locking_script = CUtil.ConvertToString(out_locking_script);
      hash = CUtil.ConvertToString(out_hash);
  }

}
*/

  internal class CAddress
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
