using System;

namespace Cfd
{
  /**
   * @brief network type
   */
  public enum CfdNetworkType : int
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
  public enum CfdAddressType : int
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
  public enum CfdHashType : int
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
  public enum CfdWitnessVersion : int
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
#pragma warning disable CA1720 // Identifier contains type name
    Single = 0x03  //!< SIGHASH_SINGLE
#pragma warning restore CA1720 // Identifier contains type name
  };

  public class Address : IEquatable<Address>
  {
    private readonly string address;
    private readonly string lockingScript;
    private readonly string p2shSegwitLockingScript;
    private readonly string hash;
    private readonly CfdNetworkType network;
    private CfdAddressType addressType;
    private readonly CfdWitnessVersion witnessVersion;

    public static Address GetAddressByLockingScript(Script inputLockingScript, CfdNetworkType network)
    {
      if (inputLockingScript is null)
      {
        throw new ArgumentNullException(nameof(inputLockingScript));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetAddressFromLockingScript(
          handle.GetHandle(),
          inputLockingScript.ToHexString(),
          (int)network,
          out IntPtr outputAddress);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string tempAddress = CCommon.ConvertToString(outputAddress);
        return new Address(tempAddress);
      }
    }

    public Address()
    {
      address = "";
    }

    public Address(string addressString)
    {
      using (var handle = new ErrorHandle())
      {
        Initialize(handle, addressString, out CfdNetworkType outputNetwork,
            out CfdWitnessVersion outputWitnessVersion, out string outputLockingScript,
            out string outputHash);
        witnessVersion = outputWitnessVersion;
        hash = outputHash;
        network = outputNetwork;
        lockingScript = outputLockingScript;
        address = addressString;
      }
    }

    public Address(Pubkey pubkey, CfdAddressType type, CfdNetworkType network)
    {
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateAddress(
          handle.GetHandle(),
          (int)type,
          pubkey.ToHexString(),
          "",
          (int)network,
          out IntPtr outputAddress,
          out IntPtr outputLockingScript,
          out IntPtr outputP2shSegwitLockingScript);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        address = CCommon.ConvertToString(outputAddress);
        lockingScript = CCommon.ConvertToString(outputLockingScript);
        p2shSegwitLockingScript = CCommon.ConvertToString(outputP2shSegwitLockingScript);

        Initialize(handle, address, out network,
            out witnessVersion, out string tempLockingScript,
            out hash);

        addressType = type;
      }
    }

    public Address(Script script, CfdAddressType type, CfdNetworkType networkType)
    {
      if (script is null)
      {
        throw new ArgumentNullException(nameof(script));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateAddress(
          handle.GetHandle(),
          (int)type,
          "",
          script.ToHexString(),
          (int)networkType,
          out IntPtr outputAddress,
          out IntPtr outputLockingScript,
          out IntPtr outputP2shSegwitLockingScript);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        address = CCommon.ConvertToString(outputAddress);
        lockingScript = CCommon.ConvertToString(outputLockingScript);
        p2shSegwitLockingScript = CCommon.ConvertToString(outputP2shSegwitLockingScript);
        Initialize(handle, address, out _, out CfdWitnessVersion outputWitnessVersion, out _,
            out string outputHash);
        witnessVersion = outputWitnessVersion;
        hash = outputHash;

        network = networkType;
        addressType = type;
      }
    }

    public string ToAddressString()
    {
      return address;
    }

    public Script GetLockingScript()
    {
      return new Script(lockingScript);
    }

    public string GetHash()
    {
      return hash;
    }

    public string GetP2shLockingScript()
    {
      return p2shSegwitLockingScript;
    }

    public CfdNetworkType GetNetwork()
    {
      return network;
    }

    public CfdAddressType GetAddressType()
    {
      return addressType;
    }

    public CfdWitnessVersion GetWitnessVersion()
    {
      return witnessVersion;
    }

    public void SetAddressType(CfdAddressType addrType)
    {
      addressType = addrType;
    }

    private void Initialize(ErrorHandle handle, string addressString, out CfdNetworkType outputNetwork,
        out CfdWitnessVersion outputWitnessVersion, out string outputLockingScript, out string outputHash)
    {
      var ret = NativeMethods.CfdGetAddressInfo(
        handle.GetHandle(),
        addressString,
        out int networkType,
        out int hashType,
        out int segwitVersion,
        out IntPtr lockingScript,
        out IntPtr hashString);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      outputNetwork = (CfdNetworkType)networkType;
      addressType = (CfdAddressType)hashType;
      outputWitnessVersion = (CfdWitnessVersion)segwitVersion;
      outputLockingScript = CCommon.ConvertToString(lockingScript);
      outputHash = CCommon.ConvertToString(hashString);
    }

    public bool IsValid()
    {
      return address.Length != 0;
    }

    public bool Equals(Address other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }
      return address.Equals(other.address, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as Address) != null)
      {
        return Equals((Address)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return address.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(Address lhs, Address rhs)
    {
      if (lhs is null)
      {
        if (rhs is null)
        {
          return true;
        }
        return false;
      }
      return lhs.Equals(rhs);
    }

    public static bool operator !=(Address lhs, Address rhs)
    {
      return !(lhs == rhs);
    }
  }
}
