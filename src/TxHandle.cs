using System;

namespace Cfd
{
  /// <summary>
  /// tx handle wrapper class.
  /// </summary>
  internal class TxHandle : IDisposable
  {
    private readonly IntPtr handle;
    private bool disposed;
    private readonly ErrorHandle errHandle;

    /// <summary>
    /// constructor.
    /// </summary>
    public TxHandle(ErrorHandle errorhandle, int networkType, string txHex)
    {
      var ret = NativeMethods.CfdInitializeTxDataHandle(
        errorhandle.GetHandle(), networkType, txHex, out IntPtr newHandle);
      if ((ret != CfdErrorCode.Success) || (newHandle == IntPtr.Zero))
      {
        throw new InvalidOperationException();
      }
      handle = newHandle;
      errHandle = errorhandle;
    }

    public IntPtr GetHandle()
    {
      return handle;
    }

    ~TxHandle()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);  // Dispose of unmanaged resources.
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Throw exception from error handle and error code.
    /// </summary>
    /// <param name="errorCode">error code</param>
    /// <exception cref="ArgumentOutOfRangeException">argument range exception</exception>
    /// <exception cref="ArgumentException">argument exception</exception>
    /// <exception cref="InsufficientMemoryException">memory full exception</exception>
    /// <exception cref="InvalidOperationException">illegal exception</exception>
    public void ThrowError(CfdErrorCode errorCode)
    {
      if ((errorCode == CfdErrorCode.Success) || disposed)
      {
        return;
      }

      string errorMessage;
      var ret = CCommon.CfdGetLastErrorMessage(handle, out IntPtr messageAddress);
      if (ret == CfdErrorCode.Success)
      {
        string message = CCommon.ConvertToString(messageAddress);
        errorMessage = $"CFD error[{errorCode}] message:{message}";
      }
      else
      {
        errorMessage = $"CFD error[{errorCode}]";
      }
      CfdCommon.ThrowError(errorCode, errorMessage);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        NativeMethods.CfdFreeTxDataHandle(errHandle.GetHandle(), handle);
        disposed = true;
      }
    }
  }
}
