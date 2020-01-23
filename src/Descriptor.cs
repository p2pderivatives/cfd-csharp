using System;

/// <summary>
/// cfd library namespace.
/// </summary>
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
    // AddressType
    // keyType

    public Descriptor(string descriptorString, CfdNetworkType network)
    {
      ParseDescriptor(descriptorString, "", network);
    }

    public Descriptor(string descriptorString, string derivePath, CfdNetworkType network)
    {
      ParseDescriptor(descriptorString, derivePath, network);
    }

    public void ParseDescriptor(string descriptorString, string derivePath, CfdNetworkType network)
    {
      descriptor = descriptorString;
      throw new NotImplementedException();  // FIXME not implementss
      /*
          internal static extern CfdErrorCode CfdParseDescriptor(
              [In] IntPtr handle,
              [In] string descriptor,
              [In] int networkType,
              [In] string bip32DerivationPath,
              [Out] out IntPtr descriptorHandle,
              [Out] out uint maxIndex);

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
      */
    }

  }
}
