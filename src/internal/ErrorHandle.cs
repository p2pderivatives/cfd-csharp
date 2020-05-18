using System;

namespace Cfd
{
  /// <summary>
  /// error handle wrapper class.
  /// </summary>
  internal class ErrorHandle : IDisposable
  {
    private readonly IntPtr handle;
    private bool disposed;

    /// <summary>
    /// constructor.
    /// </summary>
    public ErrorHandle()
    {
      var ret = CCommon.CfdCreateSimpleHandle(out IntPtr newHandle);
      if ((ret != CfdErrorCode.Success) || (newHandle == IntPtr.Zero))
      {
        throw new InvalidOperationException();
      }
      handle = newHandle;
    }

    public IntPtr GetHandle()
    {
      return handle;
    }

    ~ErrorHandle()
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
        CCommon.CfdFreeHandle(handle);
        disposed = true;
      }
    }
  }
}
