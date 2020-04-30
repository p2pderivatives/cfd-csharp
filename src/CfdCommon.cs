using System;
using System.Globalization;


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
    /// Get reversed buffer.
    /// </summary>
    /// <param name="bytes">byte data</param>
    /// <returns>string</returns>
    public static byte[] ReverseBytes(byte[] bytes)
    {
      if (bytes == null)
      {
        return Array.Empty<byte>();
      }
      var byteArray = new byte[bytes.Length];
      Array.Copy(bytes, byteArray, bytes.Length);
      Array.Reverse(byteArray);
      return byteArray;
    }

    /// <summary>
    /// Throw exception from error code.
    /// </summary>
    /// <param name="errorCode">error code</param>
    /// <param name="message">error message</param>
    /// <exception cref="System.ArgumentOutOfRangeException">argument range exception</exception>
    /// <exception cref="System.ArgumentException">argument exception</exception>
    /// <exception cref="System.InsufficientMemoryException">memory full exception</exception>
    /// <exception cref="System.InvalidOperationException">illegal exception</exception>
    public static void ThrowError(CfdErrorCode errorCode, string message)
    {
      if (errorCode == CfdErrorCode.Success)
      {
        return;
      }

      var errorMessage = message;
      if (String.IsNullOrEmpty(errorMessage))
      {
        errorMessage = String.Format(CultureInfo.InvariantCulture, "CFD error[{0}]", errorCode);
      }
      switch (errorCode)
      {
        case CfdErrorCode.MemoryFullError:
          throw new InsufficientMemoryException(errorMessage);
        case CfdErrorCode.OutOfRangeError:
          throw new ArgumentOutOfRangeException(errorMessage);
        case CfdErrorCode.SignVerificationError:
        case CfdErrorCode.InvalidSettingError:
        case CfdErrorCode.IllegalArgumentError:
          throw new ArgumentException(errorMessage);
        case CfdErrorCode.ConnectionError:
        case CfdErrorCode.DiskAccessError:
        case CfdErrorCode.IllegalStateError:
        case CfdErrorCode.InternalError:
        case CfdErrorCode.UnknownError:
        default:
          throw new InvalidOperationException(errorMessage);
      }
    }
  }
}
