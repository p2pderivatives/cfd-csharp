using System;
using System.Globalization;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  // FIXME Implementation is incomplete
  public class HDWallet : IEquatable<HDWallet>
  {
    private readonly ByteData seed;

    public static string[] GetMnemonicWordlist(string language)
    {
      if (language is null)
      {
        throw new ArgumentNullException(nameof(language));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeMnemonicWordList(handle.GetHandle(), language,
          out IntPtr mnemonicHandle, out uint maxIndex);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          string[] mnemonicList = new string[maxIndex];
          for (uint index = 0; index < maxIndex; ++index)
          {
            IntPtr word = IntPtr.Zero;
            ret = NativeMethods.CfdGetMnemonicWord(
              handle.GetHandle(), mnemonicHandle, index, out word);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
            mnemonicList[index] = CCommon.ConvertToString(word);
          }

          return mnemonicList;
        }
        finally
        {
          NativeMethods.CfdFreeMnemonicWordList(
            handle.GetHandle(), mnemonicHandle);
        }
      }
    }

    public static string[] ConvertEntropyToMnemonic(
      ByteData entropy, string language)
    {
      if (entropy is null)
      {
        throw new ArgumentNullException(nameof(entropy));
      }
      if (language is null)
      {
        throw new ArgumentNullException(nameof(language));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdConvertEntropyToMnemonic(handle.GetHandle(),
          entropy.ToHexString(),
          language, out IntPtr tempMnemonic);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string mnemonic = CCommon.ConvertToString(tempMnemonic);
        return mnemonic.Split(' ');
      }
    }

    public static ByteData ConvertMnemonicToEntropy(
      string[] mnemonic, string passphrase, string language)
    {
      if (mnemonic is null)
      {
        throw new ArgumentNullException(nameof(mnemonic));
      }
      if (passphrase is null)
      {
        throw new ArgumentNullException(nameof(passphrase));
      }
      if (language is null)
      {
        throw new ArgumentNullException(nameof(language));
      }
      String mnemonicJoinWord = String.Join(" ", mnemonic);
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdConvertMnemonicToSeed(
          handle.GetHandle(), mnemonicJoinWord.ToString(CultureInfo.InvariantCulture),
          passphrase, true, language, false,
          out IntPtr tempSeed, out IntPtr tempEntropy);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(tempSeed);
        string entropy = CCommon.ConvertToString(tempEntropy);
        return new ByteData(entropy);
      }
    }

    public static HDWallet ConvertMnemonicToSeed(
      string[] mnemonic, string passphrase, string language)
    {
      return ConvertMnemonicToSeed(mnemonic, passphrase, language, false);
    }

    public static HDWallet ConvertMnemonicToSeed(
      string[] mnemonic, string passphrase, string language, bool useIdeographicSpace)
    {
      if (mnemonic is null)
      {
        throw new ArgumentNullException(nameof(mnemonic));
      }
      if (passphrase is null)
      {
        throw new ArgumentNullException(nameof(passphrase));
      }
      if (language is null)
      {
        throw new ArgumentNullException(nameof(language));
      }
      String mnemonicJoinWord = String.Join(" ", mnemonic);
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdConvertMnemonicToSeed(
          handle.GetHandle(), mnemonicJoinWord.ToString(CultureInfo.InvariantCulture),
          passphrase, true, language, useIdeographicSpace,
          out IntPtr tempSeed, out IntPtr tempEntropy);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(tempEntropy);
        string seedString = CCommon.ConvertToString(tempSeed);
        return new HDWallet(new ByteData(seedString));
      }
    }

    public HDWallet(ByteData seed)
    {
      if (seed is null)
      {
        throw new ArgumentNullException(nameof(seed));
      }
      this.seed = seed;
    }

    public HDWallet(string[] mnemonic, string passphrase)
    {
      if (mnemonic is null)
      {
        throw new ArgumentNullException(nameof(mnemonic));
      }
      if (passphrase is null)
      {
        throw new ArgumentNullException(nameof(passphrase));
      }
      HDWallet seedData = ConvertMnemonicToSeed(mnemonic, passphrase, "en");
      this.seed = seedData.GetSeed();
    }

    public ByteData GetSeed()
    {
      return seed;
    }

    public ExtPrivkey GeneratePrivkey(CfdNetworkType networkType)
    {
      return new ExtPrivkey(seed, networkType);
    }

    public ExtPrivkey GeneratePrivkey(CfdNetworkType networkType, string bip32Path)
    {
      return GeneratePrivkey(networkType).DerivePrivkey(bip32Path);
    }

    public ExtPrivkey GeneratePrivkey(CfdNetworkType networkType, uint childNumber)
    {
      return GeneratePrivkey(networkType).DerivePrivkey(childNumber);
    }

    public ExtPrivkey GeneratePrivkey(CfdNetworkType networkType, uint[] bip32Path)
    {
      return GeneratePrivkey(networkType).DerivePrivkey(bip32Path);
    }

    public ExtPubkey GeneratePubkey(CfdNetworkType networkType)
    {
      return new ExtPrivkey(seed, networkType).GetExtPubkey();
    }

    public ExtPubkey GeneratePubkey(CfdNetworkType networkType, string bip32Path)
    {
      return GeneratePubkey(networkType).DerivePubkey(bip32Path);
    }

    public ExtPubkey GeneratePubkey(CfdNetworkType networkType, uint childNumber)
    {
      return GeneratePubkey(networkType).DerivePubkey(childNumber);
    }

    public ExtPubkey GeneratePubkey(CfdNetworkType networkType, uint[] bip32Path)
    {
      return GeneratePubkey(networkType).DerivePubkey(bip32Path);
    }
    public bool Equals(HDWallet other)
    {
      if (other is null)
      {
        return false;
      }
      if (Object.ReferenceEquals(this, other))
      {
        return true;
      }
      return seed.Equals(other.seed);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as HDWallet) != null)
      {
        return this.Equals((HDWallet)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(seed);
    }
  }
}
