using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  public enum CfdExtKeyType
  {
    Privkey = 0,  //!< extended privkey
    Pubkey        //!< extended pubkey
  };

  public class Pubkey
  {
    private string pubkey;

    public Pubkey()
    {
      pubkey = "";
    }

    public Pubkey(byte[] bytes)
    {
      pubkey = StringUtil.FromBytes(bytes);
    }

    public Pubkey(string pubkey_hex)
    {
      pubkey = pubkey_hex;
      // FIXME check format
    }

    public string ToHexString()
    {
      return pubkey;
    }

    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(pubkey);
    }
  }

  public class Privkey
  {
    private string privkey;
    private string privkey_wif;
    private Pubkey pubkey;

    public Privkey()
    {
      privkey = "";
      pubkey = new Pubkey();
    }

    public Privkey(byte[] bytes)
    {
      privkey = StringUtil.FromBytes(bytes);
      pubkey = new Pubkey();
    }

    public Privkey(string privkey_hex)
    {
      privkey = privkey_hex;
      // FIXME check format
      // CfdGetPubkeyFromPrivkey(
      //  void* handle, const char* privkey, const char* wif, bool is_compressed,
      //  char** pubkey);
      pubkey = new Pubkey();
    }

    public Privkey(string wif, bool is_compressed)
    {
      privkey_wif = wif;
      // FIXME check format
      // CfdGetPubkeyFromPrivkey(
      //  void* handle, const char* privkey, const char* wif, bool is_compressed,
      //  char** pubkey);
      pubkey = new Pubkey();
    }

    public string ToHexString()
    {
      return privkey;
    }

    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(privkey);
    }

    public string GetWif()
    {
      return privkey_wif;
    }

    public Pubkey GetPubkey()
    {
      return pubkey;
    }
  }

  /*
    internal class CKey
    {
      CFDC_API int CfdCalculateEcSignature(
          void* handle, const char* sighash, const char* privkey, const char* wif,
          int network_type, bool has_grind_r, char** signature);

      CFDC_API int CfdEncodeSignatureByDer(
          void* handle, const char* signature, int sighash_type,
          bool sighash_anyone_can_pay, char** der_signature);

      CFDC_API int CfdNormalizeSignature(
          void* handle, const char* signature, char** normalized_signature);

      CFDC_API int CfdCreateKeyPair(
          void* handle, bool is_compressed, int network_type, char** pubkey,
          char** privkey, char** wif);

      CFDC_API int CfdGetPrivkeyFromWif(
          void* handle, const char* wif, int network_type, char** privkey);

      CFDC_API int CfdGetPubkeyFromPrivkey(
          void* handle, const char* privkey, const char* wif, bool is_compressed,
          char** pubkey);

      CFDC_API int CfdCreateExtkeyFromSeed(
          void* handle, const char* seed_hex, int network_type, int key_type,
          char** extkey);

      CFDC_API int CfdCreateExtkeyFromParentPath(
          void* handle, const char* extkey, const char* path, int network_type,
          int key_type, char** child_extkey);

      CFDC_API int CfdCreateExtPubkey(
          void* handle, const char* extkey, int network_type, char** ext_pubkey);

      CFDC_API int CfdGetPrivkeyFromExtkey(
          void* handle, const char* extkey, int network_type, char** privkey,
          char** wif);

      CFDC_API int CfdGetPubkeyFromExtkey(
          void* handle, const char* extkey, int network_type, char** pubkey);
    }
  */
}
