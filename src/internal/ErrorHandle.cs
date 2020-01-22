using System;
using System.Runtime.InteropServices;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// error handle wrapper class.
  /// </summary>
  internal class ErrorHandle : IDisposable
  {
    private readonly IntPtr handle;
    private bool disposed = false;

    /// <summary>
    /// constructor.
    /// </summary>
    public ErrorHandle()
    {
      var ret = CCommon.CfdCreateSimpleHandle(out IntPtr temp_handle);
      if ((ret != CfdErrorCode.Success) || (temp_handle == IntPtr.Zero))
      {
        throw new InvalidOperationException();
      }
      handle = temp_handle;
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
    /// <param name="error_code">error code</param>
    /// <exception cref="System.ArgumentOutOfRangeException">argument range exception</exception>
    /// <exception cref="System.ArgumentException">argument exception</exception>
    /// <exception cref="System.InsufficientMemoryException">memory full exception</exception>
    /// <exception cref="System.InvalidOperationException">illegal exception</exception>
    public void ThrowError(CfdErrorCode error_code)
    {
      if ((error_code == CfdErrorCode.Success) || disposed)
      {
        return;
      }

      string err_message;
      var ret = CCommon.CfdGetLastErrorMessage(handle, out IntPtr message_address);
      if (ret == CfdErrorCode.Success)
      {
        string conv_message = CCommon.ConvertToString(message_address);
        err_message = $"CFD error[{error_code}] message:{conv_message}";
      }
      else
      {
        err_message = $"CFD error[{error_code}]";
      }
      CfdCommon.ThrowError(error_code, err_message);
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
