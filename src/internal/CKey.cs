using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  internal static class CKey
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetPubkeyFromPrivkey(
          [In] IntPtr handle,
          [In] string privkey,
          [In] string wif,
          [In] bool is_compressed,
          [Out] out IntPtr pubkey);

    /*
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
    */
  }
}
