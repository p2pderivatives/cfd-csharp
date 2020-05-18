using System;

namespace Cfd
{
  public class ExtPrivkey : IEquatable<ExtPrivkey>
  {
    public static readonly uint Hardened = 0x80000000;
    public static readonly string VersionMainnet = "0488ade4";
    public static readonly string VersionTestnet = "04358394";

    private readonly string extkey;
    private readonly ByteData version;
    private readonly ByteData fingerprint;
    private readonly ByteData chainCode;
    private readonly uint depth;
    private readonly uint childNumber;
    private readonly CfdNetworkType networkType;
    private readonly Privkey privkey;

    public ExtPrivkey()
    {
      extkey = "";
    }
    public ExtPrivkey(ByteData seed, CfdNetworkType networkType)
    {
      if (seed is null)
      {
        throw new ArgumentNullException(nameof(seed));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtkeyFromSeed(
          handle.GetHandle(), seed.ToHexString(), (int)networkType,
          (int)CfdExtKeyType.Privkey, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        extkey = CCommon.ConvertToString(tempExtkey);
        GetExtkeyInformation(handle, extkey,
          out version, out fingerprint,
          out chainCode, out depth, out childNumber, out _);
        privkey = GetPrivkeyFromExtKey(handle, extkey, networkType);
        this.networkType = networkType;
      }
    }

    public ExtPrivkey(string base58String)
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
        privkey = GetPrivkeyFromExtKey(handle, extkey, networkType);
      }
    }

    public ExtPrivkey DerivePrivkey(uint childNumber)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtkeyFromParent(
          handle.GetHandle(), extkey, childNumber, false,
          (int)networkType,
          (int)CfdExtKeyType.Privkey, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string childExtkey = CCommon.ConvertToString(tempExtkey);
        return new ExtPrivkey(childExtkey);
      }
    }

    public ExtPrivkey DerivePrivkey(uint[] path)
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
            (int)networkType, (int)CfdExtKeyType.Privkey,
            out tempExtkey);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          childExtkey = CCommon.ConvertToString(tempExtkey);
        }
        return new ExtPrivkey(childExtkey);
      }
    }

    public ExtPrivkey DerivePrivkey(string path)
    {
      if (path is null)
      {
        throw new ArgumentNullException(nameof(path));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtkeyFromParentPath(
          handle.GetHandle(), extkey, path, (int)networkType,
          (int)CfdExtKeyType.Privkey, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string childExtkey = CCommon.ConvertToString(tempExtkey);
        return new ExtPrivkey(childExtkey);
      }
    }

    public ExtPubkey GetExtPubkey()
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateExtPubkey(
          handle.GetHandle(), extkey, (int)networkType, out IntPtr tempExtkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string childExtkey = CCommon.ConvertToString(tempExtkey);
        return new ExtPubkey(childExtkey);
      }
    }

    public ExtPubkey DerivePubkey(uint childNumber)
    {
      return DerivePrivkey(childNumber).GetExtPubkey();
    }

    public ExtPubkey DerivePubkey(uint[] path)
    {
      if (path is null)
      {
        throw new ArgumentNullException(nameof(path));
      }
      return DerivePrivkey(path).GetExtPubkey();
    }

    public ExtPubkey DerivePubkey(string path)
    {
      if (path is null)
      {
        throw new ArgumentNullException(nameof(path));
      }
      return DerivePrivkey(path).GetExtPubkey();
    }

    public override string ToString()
    {
      return extkey;
    }

    public Privkey GetPrivkey()
    {
      return privkey;
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

    private static Privkey GetPrivkeyFromExtKey(ErrorHandle handle, string extPrivkey,
      CfdNetworkType networkType)
    {
      var ret = NativeMethods.CfdGetPrivkeyFromExtkey(
        handle.GetHandle(), extPrivkey, (int)networkType,
        out IntPtr privkeyHex, out IntPtr wif);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      CCommon.ConvertToString(privkeyHex);
      string privkeyWif = CCommon.ConvertToString(wif);
      return new Privkey(privkeyWif);
    }

    public bool Equals(ExtPrivkey other)
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
      if ((obj as ExtPrivkey) != null)
      {
        return Equals((ExtPrivkey)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return extkey.GetHashCode(StringComparison.Ordinal);
    }
  }
}
