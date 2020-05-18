using System;

namespace Cfd
{
  /// <summary>
  /// extend key type.
  /// </summary>
  public enum CfdExtKeyType
  {
    Privkey = 0,  //!< extended privkey
    Pubkey        //!< extended pubkey
  };

  public class ExtPubkey : IEquatable<ExtPubkey>
  {
    public static readonly string VersionMainnet = "0488b21e";
    public static readonly string VersionTestnet = "043587cf";

    private readonly string extkey;
    private readonly ByteData version;
    private readonly ByteData fingerprint;
    private readonly ByteData chainCode;
    private readonly uint depth;
    private readonly uint childNumber;
    private readonly CfdNetworkType networkType;
    private readonly Pubkey pubkey;

    public ExtPubkey()
    {
      extkey = "";

    }
    public ExtPubkey(string base58String)
    {
      if (base58String is null)
      {
        throw new ArgumentNullException(nameof(base58String));
      }
      extkey = base58String;
      using (var handle = new ErrorHandle())
      {
        GetExtkeyInformation(handle, extkey, out version, out fingerprint,
          out chainCode, out depth, out childNumber, out networkType);
        pubkey = GetPubkeyFromExtKey(handle, extkey, networkType);
      }
    }

    public ExtPubkey(CfdNetworkType networkType, Pubkey parentPubkey,
      Pubkey pubkey, ByteData chainCode, uint depth, uint childNumber)
    {
      if (parentPubkey is null)
      {
        throw new ArgumentNullException(nameof(parentPubkey));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      if (chainCode is null)
      {
        throw new ArgumentNullException(nameof(chainCode));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtkey(
          handle.GetHandle(), (int)networkType, (int)CfdExtKeyType.Pubkey,
          parentPubkey.ToHexString(), "", pubkey.ToHexString(),
          chainCode.ToHexString(), (byte)depth, childNumber, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        extkey = CCommon.ConvertToString(tempExtkey);
        this.networkType = networkType;
        this.pubkey = pubkey;
        this.chainCode = chainCode;
        this.depth = depth;
        this.childNumber = childNumber;
        GetExtkeyInformation(handle, extkey, out version, out fingerprint,
          out _, out _, out _, out _);
      }
    }

    public ExtPubkey(CfdNetworkType networkType, ByteData fingerprint,
      Pubkey pubkey, ByteData chainCode, uint depth, uint childNumber)
    {
      if (fingerprint is null)
      {
        throw new ArgumentNullException(nameof(fingerprint));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      if (chainCode is null)
      {
        throw new ArgumentNullException(nameof(chainCode));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtkey(
          handle.GetHandle(), (int)networkType, (int)CfdExtKeyType.Pubkey, "",
          fingerprint.ToHexString(), pubkey.ToHexString(), chainCode.ToHexString(),
          (byte)depth, childNumber, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        extkey = CCommon.ConvertToString(tempExtkey);
        this.networkType = networkType;
        this.fingerprint = fingerprint;
        this.pubkey = pubkey;
        this.chainCode = chainCode;
        this.depth = depth;
        this.childNumber = childNumber;
        GetExtkeyInformation(handle, extkey, out version, out _,
          out _, out _, out _, out _);
      }
    }

    public ExtPubkey DerivePubkey(uint childNumber)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtkeyFromParent(
          handle.GetHandle(), extkey, childNumber, false,
          (int)networkType,
          (int)CfdExtKeyType.Pubkey, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string childExtkey = CCommon.ConvertToString(tempExtkey);
        return new ExtPubkey(childExtkey);
      }
    }

    public ExtPubkey DerivePubkey(uint[] path)
    {
      if (path is null)
      {
        throw new ArgumentNullException(nameof(path));
      }
      using (var handle = new ErrorHandle())
      {
        string childExtkey = extkey;
        foreach (uint childNum in path)
        {
          IntPtr tempExtkey = IntPtr.Zero;
          var ret = NativeMethods.CfdCreateExtkeyFromParent(
            handle.GetHandle(), childExtkey, childNum, false,
            (int)networkType, (int)CfdExtKeyType.Pubkey,
            out tempExtkey);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          childExtkey = CCommon.ConvertToString(tempExtkey);
        }
        return new ExtPubkey(childExtkey);
      }
    }

    public ExtPubkey DerivePubkey(string path)
    {
      if (path is null)
      {
        throw new ArgumentNullException(nameof(path));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtkeyFromParentPath(
          handle.GetHandle(), extkey, path, (int)networkType,
          (int)CfdExtKeyType.Pubkey, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string childExtkey = CCommon.ConvertToString(tempExtkey);
        return new ExtPubkey(childExtkey);
      }
    }

    public override string ToString()
    {
      return extkey;
    }

    public Pubkey GetPubkey()
    {
      return pubkey;
    }
    public bool IsValid()
    {
      return extkey.Length != 0;
    }

    public ByteData GetVersion()
    {
      return version;
    }
    public ByteData GetFingerprint()
    {
      return fingerprint;
    }
    public ByteData GetChainCode()
    {
      return chainCode;
    }
    public uint GetDepth()
    {
      return depth;
    }
    public uint GetChildNumber()
    {
      return childNumber;
    }

    public CfdNetworkType GetNetworkType()
    {
      return networkType;
    }

    private static void GetExtkeyInformation(ErrorHandle handle, string extkey,
      out ByteData version, out ByteData fingerprint, out ByteData chainCode,
      out uint depth, out uint childNumber, out CfdNetworkType networkType)
    {
      var ret = NativeMethods.CfdGetExtkeyInformation(
          handle.GetHandle(), extkey,
          out IntPtr tempVersion,
          out IntPtr tempFingerprint,
          out IntPtr tempChainCode,
          out depth,
          out childNumber);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      string workVersion = CCommon.ConvertToString(tempVersion);
      string workFingerprint = CCommon.ConvertToString(tempFingerprint);
      string workChainCode = CCommon.ConvertToString(tempChainCode);
      version = new ByteData(workVersion);
      fingerprint = new ByteData(workFingerprint);
      chainCode = new ByteData(workChainCode);
      if (VersionMainnet == version.ToHexString())
      {
        networkType = CfdNetworkType.Mainnet;
      }
      else
      {
        networkType = CfdNetworkType.Testnet;
        if (VersionTestnet != version.ToHexString())
        {
          CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError,
          "Failed to version format.");
        }
      }
    }

    private static Pubkey GetPubkeyFromExtKey(ErrorHandle handle, string extPubkey,
      CfdNetworkType networkType)
    {
      var ret = NativeMethods.CfdGetPubkeyFromExtkey(
        handle.GetHandle(), extPubkey, (int)networkType,
        out IntPtr tempPubkey);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      string pubkeyHex = CCommon.ConvertToString(tempPubkey);
      return new Pubkey(pubkeyHex);
    }

    public bool Equals(ExtPubkey other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }
      return extkey.Equals(other.extkey, StringComparison.Ordinal);
    }
    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as ExtPubkey) != null)
      {
        return Equals((ExtPubkey)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return extkey.GetHashCode(StringComparison.Ordinal);
    }
  }
}
