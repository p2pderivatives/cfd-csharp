using System;

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

  public struct CfdKeyData : IEquatable<CfdKeyData>
  {
    public CfdDescriptorKeyType KeyType { get; }
    public Pubkey Pubkey { get; }
    public ExtPubkey ExtPubkey { get; }
    public ExtPrivkey ExtPrivkey { get; }


    public CfdKeyData(Pubkey pubkey)
    {
      KeyType = CfdDescriptorKeyType.Public;
      Pubkey = pubkey;
      ExtPubkey = new ExtPubkey();
      ExtPrivkey = new ExtPrivkey();
    }

    public CfdKeyData(ExtPubkey extPubkey)
    {
      KeyType = CfdDescriptorKeyType.Bip32;
      Pubkey = new Pubkey();
      ExtPubkey = extPubkey;
      ExtPrivkey = new ExtPrivkey();
    }

    public CfdKeyData(ExtPrivkey extPrivkey)
    {
      KeyType = CfdDescriptorKeyType.Bip32Priv;
      Pubkey = new Pubkey();
      ExtPubkey = new ExtPubkey();
      ExtPrivkey = extPrivkey;
    }

    public CfdKeyData(CfdDescriptorKeyType keyType, Pubkey pubkey,
        ExtPubkey extPubkey, ExtPrivkey extPrivkey)
    {
      KeyType = keyType;
      Pubkey = pubkey;
      ExtPubkey = extPubkey;
      ExtPrivkey = extPrivkey;
    }

    public bool Equals(CfdKeyData other)
    {
      if (KeyType != other.KeyType)
      {
        return false;
      }
      else if (KeyType == CfdDescriptorKeyType.Bip32)
      {
        return ExtPubkey.Equals(other.ExtPubkey);
      }
      else if (KeyType == CfdDescriptorKeyType.Bip32Priv)
      {
        return ExtPrivkey.Equals(other.ExtPrivkey);
      }
      else if (KeyType == CfdDescriptorKeyType.Public)
      {
        return Pubkey.Equals(other.Pubkey);
      }
      else
      {
        return false;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is CfdKeyData)
      {
        return Equals((CfdKeyData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      if (KeyType == CfdDescriptorKeyType.Bip32)
      {
        return KeyType.GetHashCode() + ExtPubkey.GetHashCode();
      }
      else if (KeyType == CfdDescriptorKeyType.Bip32Priv)
      {
        return KeyType.GetHashCode() + ExtPrivkey.GetHashCode();
      }
      else if (KeyType == CfdDescriptorKeyType.Public)
      {
        return KeyType.GetHashCode() + Pubkey.GetHashCode();
      }
      else
      {
        return KeyType.GetHashCode();
      }
    }

    public static bool operator ==(CfdKeyData left, CfdKeyData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(CfdKeyData left, CfdKeyData right)
    {
      return !(left == right);
    }
  }

  public struct CfdDescriptorScriptData : IEquatable<CfdDescriptorScriptData>
  {
    public CfdDescriptorScriptType ScriptType { get; }
    public uint Depth { get; }
    public CfdHashType HashType { get; }
    public Address Address { get; }
    public Script RedeemScript { get; }
    public CfdKeyData KeyData { get; }
    public ArraySegment<CfdKeyData> MultisigKeyList { get; }
    public uint MultisigRequireNum { get; }

    public CfdDescriptorScriptData(CfdDescriptorScriptType scriptType, uint depth,
        Script redeemScript)
    {
      ScriptType = scriptType;
      Depth = depth;
      HashType = CfdHashType.P2sh;
      Address = new Address();
      RedeemScript = redeemScript;
      KeyData = new CfdKeyData();
      MultisigKeyList = new ArraySegment<CfdKeyData>();
      MultisigRequireNum = 0;
    }

    public CfdDescriptorScriptData(CfdDescriptorScriptType scriptType, uint depth,
        CfdHashType hashType, Address address)
    {
      ScriptType = scriptType;
      Depth = depth;
      HashType = hashType;
      Address = address;
      RedeemScript = new Script();
      KeyData = new CfdKeyData();
      MultisigKeyList = new ArraySegment<CfdKeyData>();
      MultisigRequireNum = 0;
    }

    public CfdDescriptorScriptData(CfdDescriptorScriptType scriptType, uint depth,
        CfdHashType hashType, Address address, CfdKeyData keyData)
    {
      ScriptType = scriptType;
      Depth = depth;
      HashType = hashType;
      Address = address;
      RedeemScript = new Script();
      KeyData = keyData;
      MultisigKeyList = new ArraySegment<CfdKeyData>();
      MultisigRequireNum = 0;
    }

    public CfdDescriptorScriptData(CfdDescriptorScriptType scriptType, uint depth,
    CfdHashType hashType, Address address, Script redeemScript)
    {
      ScriptType = scriptType;
      Depth = depth;
      HashType = hashType;
      Address = address;
      RedeemScript = redeemScript;
      KeyData = new CfdKeyData();
      MultisigKeyList = new ArraySegment<CfdKeyData>();
      MultisigRequireNum = 0;
    }

    public CfdDescriptorScriptData(CfdDescriptorScriptType scriptType, uint depth,
        CfdHashType hashType, Address address, Script redeemScript,
        CfdKeyData[] multisigKeyList, uint multisigRequireNum)
    {
      ScriptType = scriptType;
      Depth = depth;
      HashType = hashType;
      Address = address;
      RedeemScript = redeemScript;
      KeyData = new CfdKeyData();
      MultisigKeyList = new ArraySegment<CfdKeyData>(multisigKeyList);
      MultisigRequireNum = multisigRequireNum;
    }

    public CfdDescriptorScriptData(CfdDescriptorScriptType scriptType, uint depth,
        CfdHashType hashType, Address address, Script redeemScript,
        CfdKeyData keyData, CfdKeyData[] multisigKeyList, uint multisigRequireNum)
    {
      ScriptType = scriptType;
      Depth = depth;
      HashType = hashType;
      Address = address;
      RedeemScript = redeemScript;
      KeyData = keyData;
      MultisigKeyList = new ArraySegment<CfdKeyData>(multisigKeyList);
      MultisigRequireNum = multisigRequireNum;
    }

    public bool Equals(CfdDescriptorScriptData other)
    {
      if (ScriptType != other.ScriptType)
      {
        return false;
      }
      switch (ScriptType)
      {
        case CfdDescriptorScriptType.Pk:
        case CfdDescriptorScriptType.Pkh:
        case CfdDescriptorScriptType.Wpkh:
        case CfdDescriptorScriptType.Combo:
          return KeyData.Equals(other.KeyData);
        case CfdDescriptorScriptType.Sh:
        case CfdDescriptorScriptType.Wsh:
        case CfdDescriptorScriptType.Multi:
        case CfdDescriptorScriptType.SortedMulti:
        case CfdDescriptorScriptType.Raw:
          return RedeemScript.Equals(other.RedeemScript);
        case CfdDescriptorScriptType.Addr:
          return Address.Equals(other.Address);
        default:
          return false;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is CfdDescriptorScriptData)
      {
        return Equals((CfdDescriptorScriptData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      switch (ScriptType)
      {
        case CfdDescriptorScriptType.Pk:
        case CfdDescriptorScriptType.Pkh:
        case CfdDescriptorScriptType.Wpkh:
        case CfdDescriptorScriptType.Combo:
          return ScriptType.GetHashCode() + KeyData.GetHashCode();
        case CfdDescriptorScriptType.Sh:
        case CfdDescriptorScriptType.Wsh:
        case CfdDescriptorScriptType.Multi:
        case CfdDescriptorScriptType.SortedMulti:
        case CfdDescriptorScriptType.Raw:
          return ScriptType.GetHashCode() + RedeemScript.GetHashCode();
        case CfdDescriptorScriptType.Addr:
          return ScriptType.GetHashCode() + Address.GetHashCode();
        default:
          return ScriptType.GetHashCode();
      }
    }

    public static bool operator ==(CfdDescriptorScriptData left, CfdDescriptorScriptData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(CfdDescriptorScriptData left, CfdDescriptorScriptData right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// Output descriptor class.
  /// </summary>
  public class Descriptor
  {
    private readonly string descriptor;
    private readonly CfdDescriptorScriptData[] scriptList;
    private readonly CfdDescriptorScriptData rootData;

    /// <summary>
    /// parse output descriptor.
    /// </summary>
    /// <param name="descriptorString">output descriptor</param>
    /// <param name="network">network type for address</param>
    public Descriptor(string descriptorString, CfdNetworkType network)
    {
      if (descriptorString is null)
      {
        throw new ArgumentNullException(nameof(descriptorString));
      }
      using (var handle = new ErrorHandle())
      {
        descriptor = GetDescriptorChecksum(handle, descriptorString, network);
        scriptList = ParseDescriptor(handle, descriptorString, "", network);
        rootData = AnalyzeScriptList(scriptList);
      }
    }

    /// <summary>
    /// parse output descriptor for derive path.
    /// </summary>
    /// <param name="descriptorString">output descriptor</param>
    /// <param name="derivePath">derive path</param>
    /// <param name="network">network type for address</param>
    public Descriptor(string descriptorString, string derivePath, CfdNetworkType network)
    {
      if (descriptorString is null)
      {
        throw new ArgumentNullException(nameof(descriptorString));
      }
      if (derivePath is null)
      {
        throw new ArgumentNullException(nameof(derivePath));
      }
      using (var handle = new ErrorHandle())
      {
        descriptor = GetDescriptorChecksum(handle, descriptorString, network);
        scriptList = ParseDescriptor(handle, descriptorString, derivePath, network);
        rootData = AnalyzeScriptList(scriptList);
      }
    }

    private static string GetDescriptorChecksum(ErrorHandle handle, string descriptorString,
        CfdNetworkType network)
    {
      var ret = NativeMethods.CfdGetDescriptorChecksum(handle.GetHandle(), (int)network,
          descriptorString, out IntPtr descriptorAddedChecksum);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      return CCommon.ConvertToString(descriptorAddedChecksum);
    }

    private static CfdDescriptorScriptData[] ParseDescriptor(
        ErrorHandle handle, string descriptorString,
        string derivePath, CfdNetworkType network)
    {
      var ret = NativeMethods.CfdParseDescriptor(
        handle.GetHandle(), descriptorString, (int)network, derivePath,
        out IntPtr descriptorHandle, out uint maxIndex);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      try
      {
        bool isMultisig = false;
        uint maxKeyNum = 0;
        uint requireNum = 0;
        CfdDescriptorScriptData[] list = new CfdDescriptorScriptData[maxIndex + 1];
        for (uint index = 0; index <= maxIndex; ++index)
        {
          // force initialized because guard illegal memory access.
          IntPtr lockingScript = IntPtr.Zero;
          IntPtr redeemScript = IntPtr.Zero;
          IntPtr address = IntPtr.Zero;
          IntPtr pubkey = IntPtr.Zero;
          IntPtr extPubkey = IntPtr.Zero;
          IntPtr extPrivkey = IntPtr.Zero;
          ret = NativeMethods.CfdGetDescriptorData(
            handle.GetHandle(), descriptorHandle, index, out _, out uint depth,
            out int scriptType, out lockingScript, out address,
            out int hashType, out redeemScript,
            out int keyType, out pubkey, out extPubkey, out extPrivkey,
            out isMultisig, out maxKeyNum, out requireNum);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          CCommon.ConvertToString(lockingScript);
          string tempAddress = CCommon.ConvertToString(address);
          string tempRedeemScript = CCommon.ConvertToString(redeemScript);
          string tempPubkey = CCommon.ConvertToString(pubkey);
          string tempExtPubkey = CCommon.ConvertToString(extPubkey);
          string tempExtPrivkey = CCommon.ConvertToString(extPrivkey);
          CfdDescriptorScriptData data;
          CfdKeyData keyData;
          CfdDescriptorKeyType type;
          switch ((CfdDescriptorScriptType)scriptType)
          {
            case CfdDescriptorScriptType.Combo:
            case CfdDescriptorScriptType.Pk:
            case CfdDescriptorScriptType.Pkh:
            case CfdDescriptorScriptType.Wpkh:
              type = (CfdDescriptorKeyType)keyType;
              if (type == CfdDescriptorKeyType.Bip32)
              {
                keyData = new CfdKeyData(new ExtPubkey(tempExtPubkey));
              }
              else if (type == CfdDescriptorKeyType.Bip32Priv)
              {
                keyData = new CfdKeyData(new ExtPrivkey(tempExtPrivkey));
              }
              else
              {
                keyData = new CfdKeyData(new Pubkey(tempPubkey));
              }
              data = new CfdDescriptorScriptData(
                (CfdDescriptorScriptType)scriptType,
                depth, (CfdHashType)hashType, new Address(tempAddress), keyData);
              break;
            case CfdDescriptorScriptType.Sh:
            case CfdDescriptorScriptType.Wsh:
            case CfdDescriptorScriptType.Multi:
            case CfdDescriptorScriptType.SortedMulti:
              if (isMultisig)
              {
                CfdKeyData[] keyList = new CfdKeyData[maxKeyNum];
                for (uint multisigIndex = 0; multisigIndex < maxKeyNum; ++multisigIndex)
                {
                  IntPtr multisigPubkey = IntPtr.Zero;
                  IntPtr multisigExtPubkey = IntPtr.Zero;
                  IntPtr multisigExtPrivkey = IntPtr.Zero;
                  ret = NativeMethods.CfdGetDescriptorMultisigKey(handle.GetHandle(), descriptorHandle,
                    multisigIndex, out int multisigKeyType, out multisigPubkey,
                    out multisigExtPubkey, out multisigExtPrivkey);
                  if (ret != CfdErrorCode.Success)
                  {
                    handle.ThrowError(ret);
                  }
                  tempPubkey = CCommon.ConvertToString(multisigPubkey);
                  tempExtPubkey = CCommon.ConvertToString(multisigExtPubkey);
                  tempExtPrivkey = CCommon.ConvertToString(multisigExtPrivkey);
                  type = (CfdDescriptorKeyType)multisigKeyType;
                  if (type == CfdDescriptorKeyType.Bip32)
                  {
                    keyList[multisigIndex] = new CfdKeyData(new ExtPubkey(tempExtPubkey));
                  }
                  else if (type == CfdDescriptorKeyType.Bip32Priv)
                  {
                    keyList[multisigIndex] = new CfdKeyData(new ExtPrivkey(tempExtPrivkey));
                  }
                  else
                  {
                    keyList[multisigIndex] = new CfdKeyData(new Pubkey(tempPubkey));
                  }
                }
                data = new CfdDescriptorScriptData(
                    (CfdDescriptorScriptType)scriptType,
                    depth, (CfdHashType)hashType, new Address(tempAddress),
                    new Script(tempRedeemScript), keyList, requireNum);
              }
              else
              {
                data = new CfdDescriptorScriptData(
                  (CfdDescriptorScriptType)scriptType,
                  depth, (CfdHashType)hashType, new Address(tempAddress),
                  new Script(tempRedeemScript));
              }
              break;
            case CfdDescriptorScriptType.Raw:
              data = new CfdDescriptorScriptData(
                (CfdDescriptorScriptType)scriptType,
                depth, new Script(tempRedeemScript));
              break;
            case CfdDescriptorScriptType.Addr:
              data = new CfdDescriptorScriptData(
                (CfdDescriptorScriptType)scriptType,
                depth, (CfdHashType)hashType, new Address(tempAddress));
              break;
            default:
              data = new CfdDescriptorScriptData();
              break;
          }
          list[index] = data;

          if (scriptType == (int)CfdDescriptorScriptType.Combo)
          {
            // TODO: combo data is top only.
            CfdDescriptorScriptData[] newList = { list[0] };
            list = newList;
            break;
          }
        }
        return list;
      }
      finally
      {
        NativeMethods.CfdFreeDescriptorHandle(
          handle.GetHandle(), descriptorHandle);
      }
    }

    private static CfdDescriptorScriptData AnalyzeScriptList(CfdDescriptorScriptData[] scriptList)
    {
      if ((scriptList[0].HashType == CfdHashType.P2wsh) ||
        (scriptList[0].HashType == CfdHashType.P2sh))
      {
        if ((scriptList.Length > 1) && (scriptList[1].HashType == CfdHashType.P2pkh))
        {
          CfdDescriptorScriptData data = new CfdDescriptorScriptData(
            scriptList[0].ScriptType,
            scriptList[0].Depth,
            scriptList[0].HashType,
            scriptList[0].Address,
            scriptList[0].RedeemScript,
            scriptList[1].KeyData, Array.Empty<CfdKeyData>(), 0);
          return data;
        }
      }
      if (scriptList[0].HashType == CfdHashType.P2shP2wsh)
      {
        if ((scriptList.Length > 2) && (scriptList[2].HashType == CfdHashType.P2pkh))
        {
          CfdDescriptorScriptData data = new CfdDescriptorScriptData(
            scriptList[0].ScriptType, scriptList[0].Depth,
            scriptList[0].HashType, scriptList[0].Address,
            scriptList[1].RedeemScript,
            scriptList[2].KeyData, Array.Empty<CfdKeyData>(), 0);
          return data;
        }
      }
      if (scriptList.Length == 1)
      {
        return scriptList[0];
      }

      if (scriptList[0].HashType == CfdHashType.P2shP2wsh)
      {
        if (scriptList[1].MultisigRequireNum > 0)
        {
          CfdDescriptorScriptData data = new CfdDescriptorScriptData(
            scriptList[0].ScriptType,
            scriptList[0].Depth,
            scriptList[0].HashType,
            scriptList[0].Address,
            scriptList[1].RedeemScript,
            scriptList[1].MultisigKeyList.ToArray(),
            scriptList[1].MultisigRequireNum);
          return data;
        }
        else
        {
          CfdDescriptorScriptData data = new CfdDescriptorScriptData(
            scriptList[0].ScriptType,
            scriptList[0].Depth,
            scriptList[0].HashType,
            scriptList[0].Address,
            scriptList[1].RedeemScript);
          return data;
        }
      }
      else if (scriptList[0].HashType == CfdHashType.P2shP2wpkh)
      {
        CfdDescriptorScriptData data = new CfdDescriptorScriptData(
          scriptList[0].ScriptType,
          scriptList[0].Depth,
          scriptList[0].HashType,
          scriptList[0].Address,
          scriptList[1].KeyData);
        return data;
      }
      return scriptList[0];
    }


    public override string ToString()
    {
      return descriptor;
    }

    public CfdDescriptorScriptData[] GetList()
    {
      CfdDescriptorScriptData[] newList = new CfdDescriptorScriptData[scriptList.Length];
      scriptList.CopyTo(newList, 0);
      return newList;
    }

    public CfdHashType GetHashType()
    {
      return rootData.HashType;
    }

    public Address GetAddress()
    {
      return rootData.Address;
    }

    public CfdAddressType GetAddressType()
    {
      return (CfdAddressType)rootData.HashType;
    }

    public bool HasScriptHash()
    {
      switch (scriptList[0].ScriptType)
      {
        case CfdDescriptorScriptType.Sh:
        case CfdDescriptorScriptType.Wsh:
          break;
        default:
          return false;
      }
      switch (rootData.HashType)
      {
        case CfdHashType.P2sh:
        case CfdHashType.P2wsh:
        case CfdHashType.P2shP2wsh:
          return true;
        default:
          return false;
      }
    }

    public Script GetRedeemScript()
    {
      switch (rootData.HashType)
      {
        case CfdHashType.P2sh:
        case CfdHashType.P2wsh:
        case CfdHashType.P2shP2wsh:
          return rootData.RedeemScript;
        default:
          CfdCommon.ThrowError(CfdErrorCode.IllegalStateError,
            "Failed to unused script descriptor.");
          return null;
      }
    }

    public bool HasKeyHash()
    {
      switch (scriptList[0].ScriptType)
      {
        case CfdDescriptorScriptType.Sh:
        case CfdDescriptorScriptType.Pkh:
        case CfdDescriptorScriptType.Wpkh:
        case CfdDescriptorScriptType.Combo:
          break;
        default:
          return false;
      }
      switch (rootData.HashType)
      {
        case CfdHashType.P2pkh:
        case CfdHashType.P2wpkh:
        case CfdHashType.P2shP2wpkh:
          return true;
        default:
          return false;
      }
    }

    public CfdKeyData GetKeyData()
    {
      if (rootData.KeyData.KeyType == CfdDescriptorKeyType.Null)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalStateError,
          "Failed to script hash or multisig key.");
      }
      return rootData.KeyData;
    }

    public bool HasMultisig()
    {
      return (rootData.MultisigRequireNum != 0);
    }

    public CfdKeyData[] GetMultisigKeyList()
    {
      if ((HasKeyHash()) || (rootData.MultisigRequireNum == 0))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalStateError,
          "Failed to script hash or multisig key.");
      }
      CfdKeyData[] keyList = new CfdKeyData[rootData.MultisigKeyList.Count];
      rootData.MultisigKeyList.CopyTo(keyList);
      return keyList;
    }
  }
}
