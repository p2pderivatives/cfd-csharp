using System;
using System.Runtime.InteropServices;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  /// <summary>
  /// cfd error code.
  /// </summary>
  public enum CfdErrorCode
  {
    Success = 0,
    UnknownError = -1,
    InternalError = -2,
    MemoryFullError = -3,
    IllegalArgumentError = 1,
    IllegalStateError = 2,
    OutOfRangeError = 3,
    InvalidSettingError = 4,
    ConnectionError = 5,
    DiskAccessError = 6,
    SignVerificationError = 7
  };

  public static class CfdCommon
  {
    /// <summary>
    /// Get string from cfd's pointer address, and free address buffer.
    /// </summary>
    /// <param name="address">pointer address</param>
    /// <returns>string</returns>
    public static byte[] ReverseBytes(byte[] bytes)
    {
      if (bytes == null)
      {
        return new byte[0];
      }
      var temp_bytes = new byte[bytes.Length];
      Array.Copy(bytes, temp_bytes, bytes.Length);
      Array.Reverse(temp_bytes);
      return temp_bytes;
    }

    /// <summary>
    /// Throw exception from error code.
    /// </summary>
    /// <param name="error_code">error code</param>
    /// <param name="message">error message</param>
    /// <exception cref="System.ArgumentOutOfRangeException">argument range exception</exception>
    /// <exception cref="System.ArgumentException">argument exception</exception>
    /// <exception cref="System.InsufficientMemoryException">memory full exception</exception>
    /// <exception cref="System.InvalidOperationException">illegal exception</exception>
    public static void ThrowError(CfdErrorCode error_code, string message)
    {
      if (error_code == CfdErrorCode.Success)
      {
        return;
      }

      var err_message = message;
      if (String.IsNullOrEmpty(err_message))
      {
        err_message = String.Format("CFD error[{0}]", error_code);
      }
      switch (error_code)
      {
        case CfdErrorCode.MemoryFullError:
          throw new InsufficientMemoryException(err_message);
        case CfdErrorCode.OutOfRangeError:
          throw new ArgumentOutOfRangeException(err_message);
        case CfdErrorCode.SignVerificationError:
        case CfdErrorCode.InvalidSettingError:
        case CfdErrorCode.IllegalArgumentError:
          throw new ArgumentException(err_message);
        case CfdErrorCode.ConnectionError:
        case CfdErrorCode.DiskAccessError:
        case CfdErrorCode.IllegalStateError:
        case CfdErrorCode.InternalError:
        case CfdErrorCode.UnknownError:
        default:
          throw new InvalidOperationException(err_message);
      }
    }
  }
}
