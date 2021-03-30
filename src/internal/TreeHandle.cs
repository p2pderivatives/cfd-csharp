using System;

namespace Cfd
{
  /// <summary>
  /// tree handle wrapper class.
  /// </summary>
  internal class TreeHandle : IDisposable
  {
    private readonly IntPtr handle;
    private bool disposed;
    private readonly ErrorHandle errHandle;

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="errorhandle">error handle</param>
    public TreeHandle(ErrorHandle errorhandle)
    {
      var ret = NativeMethods.CfdInitializeTaprootScriptTree(
        errorhandle.GetHandle(), out IntPtr newHandle);
      if ((ret != CfdErrorCode.Success) || (newHandle == IntPtr.Zero))
      {
        throw new InvalidOperationException();
      }
      handle = newHandle;
      errHandle = errorhandle;
    }

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="errorhandle">error handle</param>
    /// <param name="treeHandle">tree handle</param>
    public TreeHandle(ErrorHandle errorhandle, IntPtr treeHandle)
    {
      handle = treeHandle;
      errHandle = errorhandle;
    }

    public IntPtr GetHandle()
    {
      return handle;
    }

    ~TreeHandle()
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
        NativeMethods.CfdFreeTaprootScriptTreeHandle(errHandle.GetHandle(), handle);
        disposed = true;
      }
    }
  }
}
