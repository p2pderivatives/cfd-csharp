using System;
using System.Text;

namespace Cfd
{
  /// <summary>
  /// HD Wallet class.
  /// </summary>
  public class HDWallet : IEquatable<HDWallet>
  {
    private readonly ByteData seed;

    /// <summary>
    /// get mnemonic word list.
    /// </summary>
    /// <returns>mnemonic word list.</returns>
    public static string[] GetMnemonicWordlist()
    {
      return GetMnemonicWordlist("en");
    }

    /// <summary>
    /// get mnemonic word list.
    /// </summary>
    /// <param name="language">language/country</param>
    /// <returns>mnemonic word list.</returns>
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

    /// <summary>
    /// convert entropy to mnemonic.
    /// </summary>
    /// <param name="entropy">entropy</param>
    /// <param name="language">language</param>
    /// <returns>mnemonic words</returns>
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

    /// <summary>
    /// convert mnemonic to entropy
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="language">language</param>
    /// <returns>entropy</returns>
    public static ByteData ConvertMnemonicToEntropy(
      string mnemonic, string language)
    {
      if (mnemonic is null)
      {
        throw new ArgumentNullException(nameof(mnemonic));
      }
      string[] words = mnemonic.Split(new[] { ' ', '　' });
      return ConvertMnemonicToEntropy(words, language);
    }

    /// <summary>
    /// convert mnemonic to entropy
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="language">language</param>
    /// <returns>entropy</returns>
    public static ByteData ConvertMnemonicToEntropy(
      string[] mnemonic, string language)
    {
      if (mnemonic is null)
      {
        throw new ArgumentNullException(nameof(mnemonic));
      }
      if (language is null)
      {
        throw new ArgumentNullException(nameof(language));
      }
      string mnemonicJoinWord = string.Join(" ", mnemonic);
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdConvertMnemonicToSeed(
          handle.GetHandle(),
          Encoding.UTF8.GetBytes(mnemonicJoinWord.ToCharArray()),
          "", true, language, false,
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

    /// <summary>
    /// convert mnemonic to seed.
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="passphrase">passphrase</param>
    /// <param name="language">language</param>
    /// <returns>seed bytes</returns>
    public static HDWallet ConvertMnemonicToSeed(
      string mnemonic, string passphrase, string language)
    {
      if (mnemonic is null)
      {
        throw new ArgumentNullException(nameof(mnemonic));
      }
      string[] words = mnemonic.Split(new[] { ' ', '　' });
      return ConvertMnemonicToSeed(words, passphrase, language, false);
    }

    /// <summary>
    /// convert mnemonic to seed.
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="passphrase">passphrase</param>
    /// <param name="language">language</param>
    /// <returns>seed bytes</returns>
    public static HDWallet ConvertMnemonicToSeed(
      string[] mnemonic, string passphrase, string language)
    {
      return ConvertMnemonicToSeed(mnemonic, passphrase, language, false);
    }

    /// <summary>
    /// convert mnemonic to seed.
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="passphrase">passphrase</param>
    /// <param name="language">language</param>
    /// <param name="useIdeographicSpace">use ideographics space</param>
    /// <returns>seed bytes</returns>
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
      string mnemonicJoinWord = string.Join(' ', mnemonic);
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdConvertMnemonicToSeed(
          handle.GetHandle(),
          Encoding.UTF8.GetBytes(mnemonicJoinWord.ToCharArray()),
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

    /// <summary>
    /// constructor from seed.
    /// </summary>
    /// <param name="seed">seed bytes</param>
    public HDWallet(ByteData seed)
    {
      if (seed is null)
      {
        throw new ArgumentNullException(nameof(seed));
      }
      this.seed = seed;
    }

    /// <summary>
    /// constructor from english mnemonic.
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="passphrase">passphrase</param>
    public HDWallet(string mnemonic, string passphrase) : this(mnemonic, passphrase, "en")
    {
      // do nothing
    }

    /// <summary>
    /// constructor from mnemonic.
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="passphrase">passphrase</param>
    /// <param name="language">language</param>
    public HDWallet(string mnemonic, string passphrase, string language)
      : this((mnemonic is null) ? Array.Empty<string>() : mnemonic.Split(new[] { ' ', '　' }),
          passphrase, language)
    {
      // do nothing
    }

    /// <summary>
    /// constructor from mnemonic.
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="passphrase">passphrase</param>
    public HDWallet(string[] mnemonic, string passphrase) : this(mnemonic, passphrase, "en")
    {
      // do nothing
    }

    /// <summary>
    /// constructor from mnemonic.
    /// </summary>
    /// <param name="mnemonic">mnemonic words</param>
    /// <param name="passphrase">passphrase</param>
    /// <param name="language">language</param>
    public HDWallet(string[] mnemonic, string passphrase, string language)
    {
      if (mnemonic is null)
      {
        throw new ArgumentNullException(nameof(mnemonic));
      }
      if (passphrase is null)
      {
        throw new ArgumentNullException(nameof(passphrase));
      }
      HDWallet seedData = ConvertMnemonicToSeed(mnemonic, passphrase, language);
      seed = seedData.GetSeed();
    }

    /// <summary>
    /// constructor from entropy.
    /// </summary>
    /// <param name="entropy">entropy</param>
    /// <param name="passphrase">passphrase</param>
    /// <param name="language">language</param>
    public HDWallet(ByteData entropy, string passphrase, string language)
    {
      if (entropy is null)
      {
        throw new ArgumentNullException(nameof(entropy));
      }
      if (passphrase is null)
      {
        throw new ArgumentNullException(nameof(passphrase));
      }

      string[] mnemonic = ConvertEntropyToMnemonic(entropy, language);
      HDWallet seedData = ConvertMnemonicToSeed(mnemonic, passphrase, language);
      seed = seedData.GetSeed();
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
      return GeneratePrivkey(networkType).DerivePubkey(bip32Path);
    }

    public ExtPubkey GeneratePubkey(CfdNetworkType networkType, uint childNumber)
    {
      return GeneratePrivkey(networkType).DerivePubkey(childNumber);
    }

    public ExtPubkey GeneratePubkey(CfdNetworkType networkType, uint[] bip32Path)
    {
      return GeneratePrivkey(networkType).DerivePubkey(bip32Path);
    }
    public bool Equals(HDWallet other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
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
        return Equals((HDWallet)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(seed);
    }
  }
}
