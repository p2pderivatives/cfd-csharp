using System;

namespace Cfd
{
  /// <summary>
  /// network type
  /// </summary>
  public enum CfdNetworkType : int
  {
    /// <summary>
    /// btc mainnet
    /// </summary>
    Mainnet = 0,
    /// <summary>
    /// btc testnet
    /// </summary>
    Testnet,
    /// <summary>
    /// btc regtest
    /// </summary>
    Regtest,
    /// <summary>
    /// liquidv1
    /// </summary>
    Liquidv1 = 10,
    /// <summary>
    /// elements regtest
    /// </summary>
    ElementsRegtest,
    /// <summary>
    /// elements custom chain
    /// </summary>
    CustomChain,
  };

  /// <summary>
  /// address type
  /// </summary>
  public enum CfdAddressType : int
  {
    /// <summary>
    /// Legacy address (Script Hash)
    /// </summary>
    P2sh = 1,
    /// <summary>
    /// Legacy address (PublicKey Hash)
    /// </summary>
    P2pkh,
    /// <summary>
    /// Native segwit address (Script Hash)
    /// </summary>
    P2wsh,
    /// <summary>
    /// Native segwit address (PublicKey Hash)
    /// </summary>
    P2wpkh,
    /// <summary>
    /// P2sh wrapped address (Script Hash)
    /// </summary>
    P2shP2wsh,
    /// <summary>
    /// P2sh wrapped address (Pubkey Hash)
    /// </summary>
    P2shP2wpkh,
    /// <summary>
    /// taproot address
    /// </summary>
    Taproot
  };

  /// <summary>
  /// hash type
  /// </summary>
  public enum CfdHashType : int
  {
    /// <summary>
    /// Script Hash
    /// </summary>
    P2sh = 1,
    /// <summary>
    /// PublicKey Hash
    /// </summary>
    P2pkh,
    /// <summary>
    /// Native segwit Script Hash
    /// </summary>
    P2wsh,
    /// <summary>
    /// Native segwit PublicKey Hash
    /// </summary>
    P2wpkh,
    /// <summary>
    /// P2sh wrapped segwit Script Hash
    /// </summary>
    P2shP2wsh,
    /// <summary>
    /// P2sh wrapped segwit Pubkey Hash
    /// </summary>
    P2shP2wpkh,
    /// <summary>
    /// Taproot
    /// </summary>
    Taproot
  };

  /**
   * @brief witness version
   */
  public enum CfdWitnessVersion : int
  {
    /// <summary>
    /// Missing WitnessVersion
    /// </summary>
    VersionNone = -1,
    /// <summary>
    /// Witness Version 0
    /// </summary>
    Version0 = 0,
    /// <summary>
    /// Witness Version 1
    /// </summary>
    Version1,
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

  /// <summary>
  /// sighash type
  /// </summary>
  public enum CfdSighashType
  {
    /// <summary>
    /// SIGHASH_DEFAULT
    /// </summary>
    Default = 0,
    /// <summary>
    /// SIGHASH_ALL
    /// </summary>
    All = 0x01,
    /// <summary>
    /// SIGHASH_NONE
    /// </summary>
    None = 0x02,
#pragma warning disable CA1720 // Identifier contains type name
    /// <summary>
    /// SIGHASH_SINGLE
    /// </summary>
    Single = 0x03
#pragma warning restore CA1720 // Identifier contains type name
  };

  /// <summary>
  /// Elements pegin data struct.
  /// </summary>
  public struct PeginData : IEquatable<PeginData>
  {
    public Address Address { get; }
    public Script ClaimScript { get; }
    public Script TweakedFedpegScript { get; }

    public PeginData(Address peginAddress, Script claimScript, Script tweakedFedpegScript)
    {
      Address = peginAddress;
      ClaimScript = claimScript;
      TweakedFedpegScript = tweakedFedpegScript;
    }

    public bool Equals(PeginData other)
    {
      return Address == other.Address;
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is PeginData)
      {
        return Equals((PeginData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Address.GetHashCode();
    }

    public static bool operator ==(PeginData left, PeginData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(PeginData left, PeginData right)
    {
      return !(left == right);
    }
  };

  /// <summary>
  /// Elements pegout data struct.
  /// </summary>
  public struct PegoutData : IEquatable<PegoutData>
  {
    public Address Address { get; }
    public string BaseDescriptor { get; }

    public PegoutData(Address pegoutAddress, string baseDescriptor)
    {
      Address = pegoutAddress;
      BaseDescriptor = baseDescriptor;
    }

    public bool Equals(PegoutData other)
    {
      return Address == other.Address;
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is PegoutData)
      {
        return Equals((PegoutData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Address.GetHashCode();
    }

    public static bool operator ==(PegoutData left, PegoutData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(PegoutData left, PegoutData right)
    {
      return !(left == right);
    }
  };

  /// <summary>
  /// Bitcoin Address class.
  /// </summary>
  public class Address : IEquatable<Address>
  {
    private readonly string address;
    private readonly string lockingScript;
    private readonly string p2shSegwitLockingScript;
    private readonly string hash;
    private readonly CfdNetworkType network;
    private CfdAddressType addressType;
    private readonly CfdWitnessVersion witnessVersion;

    /// <summary>
    /// Get address from locking script.
    /// </summary>
    /// <param name="inputLockingScript">locking script</param>
    /// <param name="network">network type</param>
    /// <returns>address object</returns>
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

    /// <summary>
    /// Get pegin address.
    /// </summary>
    /// <param name="fedpegScript">fedpeg script</param>
    /// <param name="pubkey">pubkey</param>
    /// <param name="hashType">hash type</param>
    /// <param name="network">network type</param>
    /// <returns>pegin address data</returns>
    public static PeginData GetPeginAddress(Script fedpegScript, Pubkey pubkey, CfdHashType hashType, CfdNetworkType network)
    {
      if (fedpegScript is null)
      {
        throw new ArgumentNullException(nameof(fedpegScript));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetPeginAddress(
          handle.GetHandle(), (int)network, fedpegScript.ToHexString(), (int)hashType,
          pubkey.ToHexString(), "",
          out IntPtr outputPeginAddress, out IntPtr outputClaimScript, out IntPtr outputFedpegScript);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string peginAddress = CCommon.ConvertToString(outputPeginAddress);
        string claimScript = CCommon.ConvertToString(outputClaimScript);
        string tweakedFedpegScript = CCommon.ConvertToString(outputFedpegScript);
        return new PeginData(new Address(peginAddress), new Script(claimScript),
          new Script(tweakedFedpegScript));
      }
    }

    /// <summary>
    /// Get pegin address.
    /// </summary>
    /// <param name="fedpegScript">fedpeg script</param>
    /// <param name="redeemScript">redeem script</param>
    /// <param name="hashType">hash type</param>
    /// <param name="network">network type</param>
    /// <returns>pegin address data</returns>
    public static PeginData GetPeginAddress(Script fedpegScript, Script redeemScript, CfdHashType hashType, CfdNetworkType network)
    {
      if (fedpegScript is null)
      {
        throw new ArgumentNullException(nameof(fedpegScript));
      }
      if (redeemScript is null)
      {
        throw new ArgumentNullException(nameof(redeemScript));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetPeginAddress(
          handle.GetHandle(), (int)network, fedpegScript.ToHexString(), (int)hashType,
          "", redeemScript.ToHexString(),
          out IntPtr outputPeginAddress, out IntPtr outputClaimScript, out IntPtr outputFedpegScript);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string peginAddress = CCommon.ConvertToString(outputPeginAddress);
        string claimScript = CCommon.ConvertToString(outputClaimScript);
        string tweakedFedpegScript = CCommon.ConvertToString(outputFedpegScript);
        return new PeginData(new Address(peginAddress), new Script(claimScript),
          new Script(tweakedFedpegScript));
      }
    }

    /// <summary>
    /// Get pegout address.
    /// </summary>
    /// <param name="descriptor">descriptor</param>
    /// <param name="bip32Counter">bip32 counter</param>
    /// <param name="addressType">address type</param>
    /// <param name="network">network type</param>
    /// <returns>pegout address data</returns>
    public static PegoutData GetPegoutAddress(string descriptor, uint bip32Counter, CfdAddressType addressType, CfdNetworkType network)
    {
      if (descriptor is null)
      {
        throw new ArgumentNullException(nameof(descriptor));
      }
      using (var handle = new ErrorHandle())
      {
        CfdNetworkType elementsNetwork = CfdNetworkType.Liquidv1;
        if (network != CfdNetworkType.Mainnet)
        {
          elementsNetwork = CfdNetworkType.ElementsRegtest;
        }
        var ret = NativeMethods.CfdGetPegoutAddress(
          handle.GetHandle(), (int)network, (int)elementsNetwork, descriptor, bip32Counter,
          (int)addressType,
          out IntPtr outputAddress, out IntPtr outputDescriptor);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string mainchainAddress = CCommon.ConvertToString(outputAddress);
        string baseDescriptor = CCommon.ConvertToString(outputDescriptor);
        return new PegoutData(new Address(mainchainAddress), baseDescriptor);
      }
    }

    /// <summary>
    /// Get pegout address.
    /// </summary>
    /// <param name="xpub">xpubkey</param>
    /// <param name="bip32Counter">bip32 counter</param>
    /// <param name="addressType">address type</param>
    /// <param name="network">network type</param>
    /// <returns>pegout address data</returns>
    public static PegoutData GetPegoutAddress(ExtPubkey xpub, uint bip32Counter, CfdAddressType addressType, CfdNetworkType network)
    {
      if (xpub is null)
      {
        throw new ArgumentNullException(nameof(xpub));
      }
      return GetPegoutAddress(xpub.ToString(), bip32Counter, addressType, network);
    }

    /// <summary>
    /// Get pegout address.
    /// </summary>
    /// <param name="descriptor">descriptor</param>
    /// <param name="bip32Counter">bip32 counter</param>
    /// <param name="addressType">address type</param>
    /// <param name="network">network type</param>
    /// <returns>pegout address data</returns>
    public static PegoutData GetPegoutAddress(Descriptor descriptor, uint bip32Counter, CfdAddressType addressType, CfdNetworkType network)
    {
      if (descriptor is null)
      {
        throw new ArgumentNullException(nameof(descriptor));
      }
      return GetPegoutAddress(descriptor.ToString(), bip32Counter, addressType, network);
    }

    /// <summary>
    /// empty constructor.
    /// </summary>
    public Address()
    {
      address = "";
    }

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="addressString">address string.</param>
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

    /// <summary>
    /// constructor for pubkey.
    /// </summary>
    /// <param name="pubkey">public key</param>
    /// <param name="type">address type</param>
    /// <param name="network">network type</param>
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

    /// <summary>
    /// constructor for schnorr pubkey.
    /// </summary>
    /// <param name="pubkey">schnorr public key</param>
    /// <param name="type">address type</param>
    /// <param name="network">network type</param>
    public Address(SchnorrPubkey pubkey, CfdAddressType type, CfdNetworkType network)
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

        this.network = network;
        addressType = type;
      }
    }


    /// <summary>
    /// constructor for redeem script.
    /// </summary>
    /// <param name="script">redeem script</param>
    /// <param name="type">address type</param>
    /// <param name="networkType">network type</param>
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

    /// <summary>
    /// get address string
    /// </summary>
    /// <returns>address string</returns>
    public string ToAddressString()
    {
      return address;
    }

    /// <summary>
    /// get locking script.
    /// </summary>
    /// <returns>locking script</returns>
    public Script GetLockingScript()
    {
      return new Script(lockingScript);
    }

    public string GetHash()
    {
      return hash;
    }

    /// <summary>
    /// get p2sh-segwit's locking script.
    /// </summary>
    /// <returns>p2sh-segwit's locking script.</returns>
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

    /// <summary>
    /// set address type for p2sh-segwit.
    /// </summary>
    /// <param name="addrType">address type</param>
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
