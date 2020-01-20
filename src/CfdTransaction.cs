using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  //! txin sequence locktime
  public enum CfdSequenceLockTime : uint {
    /// disable locktime
    Disable = 0xffffffff,
    /// enable locktime (maximum time)
    EnableMax = 0xfffffffe,
  };

/*
  internal class CTransaction
  {
    CFDC_API int CfdInitializeMultisigSign(void* handle, void** multisign_handle);

    CFDC_API int CfdAddMultisigSignData(
        void* handle, void* multisign_handle, const char* signature,
        const char* related_pubkey);

    CFDC_API int CfdAddMultisigSignDataToDer(
        void* handle, void* multisign_handle, const char* signature,
        int sighash_type, bool sighash_anyone_can_pay, const char* related_pubkey);

    CFDC_API int CfdFreeMultisigSignHandle(void* handle, void* multisign_handle);
  }
*/
}
