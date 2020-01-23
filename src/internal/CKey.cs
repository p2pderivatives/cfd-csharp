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
          [In] bool isCompressed,
          [Out] out IntPtr pubkey);

    /*
        internal static extern CfdErrorCode CfdCalculateEcSignature(
            void* handle, const char* sighash, const char* privkey, const char* wif,
            int networkType, bool hasGrindR, char** signature);

        internal static extern CfdErrorCode CfdEncodeSignatureByDer(
            void* handle, const char* signature, int sighashType,
            bool sighashAnyoneCanPay, char** derSignature);

        internal static extern CfdErrorCode CfdNormalizeSignature(
            void* handle, const char* signature, char** normalizedSignature);

        internal static extern CfdErrorCode CfdCreateKeyPair(
            void* handle, bool isCompressed, int networkType, char** pubkey,
            char** privkey, char** wif);

        internal static extern CfdErrorCode CfdGetPrivkeyFromWif(
            void* handle, const char* wif, int networkType, char** privkey);


        internal static extern CfdErrorCode CfdCreateExtkeyFromSeed(
            void* handle, const char* seedHex, int networkType, int keyType,
            char** extkey);

        internal static extern CfdErrorCode CfdCreateExtkeyFromParentPath(
            void* handle, const char* extkey, const char* path, int networkType,
            int keyType, char** childExtkey);

        internal static extern CfdErrorCode CfdCreateExtPubkey(
            void* handle, const char* extkey, int networkType, char** extPubkey);

        internal static extern CfdErrorCode CfdGetPrivkeyFromExtkey(
            void* handle, const char* extkey, int networkType, char** privkey,
            char** wif);

        internal static extern CfdErrorCode CfdGetPubkeyFromExtkey(
            void* handle, const char* extkey, int networkType, char** pubkey);
    */
  }
}
