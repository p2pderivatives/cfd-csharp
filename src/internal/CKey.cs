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
        internal static extern CfdErrorCode CfdCalculateEcSignature(
            void* handle, const char* sighash, const char* privkey, const char* wif,
            int network_type, bool has_grind_r, char** signature);

        internal static extern CfdErrorCode CfdEncodeSignatureByDer(
            void* handle, const char* signature, int sighash_type,
            bool sighash_anyone_can_pay, char** der_signature);

        internal static extern CfdErrorCode CfdNormalizeSignature(
            void* handle, const char* signature, char** normalized_signature);

        internal static extern CfdErrorCode CfdCreateKeyPair(
            void* handle, bool is_compressed, int network_type, char** pubkey,
            char** privkey, char** wif);

        internal static extern CfdErrorCode CfdGetPrivkeyFromWif(
            void* handle, const char* wif, int network_type, char** privkey);


        internal static extern CfdErrorCode CfdCreateExtkeyFromSeed(
            void* handle, const char* seed_hex, int network_type, int key_type,
            char** extkey);

        internal static extern CfdErrorCode CfdCreateExtkeyFromParentPath(
            void* handle, const char* extkey, const char* path, int network_type,
            int key_type, char** child_extkey);

        internal static extern CfdErrorCode CfdCreateExtPubkey(
            void* handle, const char* extkey, int network_type, char** ext_pubkey);

        internal static extern CfdErrorCode CfdGetPrivkeyFromExtkey(
            void* handle, const char* extkey, int network_type, char** privkey,
            char** wif);

        internal static extern CfdErrorCode CfdGetPubkeyFromExtkey(
            void* handle, const char* extkey, int network_type, char** pubkey);
    */
  }
}
