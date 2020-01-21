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

  public class StringUtil
  {
    public static byte[] ToBytes(string hex)
    {
      var buffer = new byte[hex.Length / 2];
      char[] clist = hex.ToCharArray();
      for (int i = 0; i < hex.Length / 2; ++i)
      {
        buffer[i] = (byte)((Convert.ToByte(clist[i * 2]) << 4) | Convert.ToByte(clist[(i * 2) + 1]));
        // buffer[i] = Convert.ToByte(str.Substring(i*2, 2), 16));
      }
      return buffer;

      //return hex.Select(x => Convert.ToByte(new string(x.ToArray()), 16)).ToArray();
    }

    public static string FromBytes(byte[] bytes)
    {
      return BitConverter.ToString(bytes).Replace("-", "");
    }
  }

  /// <summary>
  /// error handle wrapper class.
  /// </summary>
  internal class ErrorHandle : IDisposable
  {
    private IntPtr handle = IntPtr.Zero;

    /// <summary>
    /// constructor.
    /// </summary>
    public ErrorHandle()
    {
      CfdErrorCode ret = CUtil.CfdCreateSimpleHandle(out IntPtr temp_handle);
      if (ret != CfdErrorCode.Success)
      {
        throw new System.InvalidOperationException();
      }
      handle = temp_handle;
    }

    public IntPtr GetHandle()
    {
      return handle;
    }

    ~ErrorHandle()
    {
      if (handle != IntPtr.Zero)
      {
        CUtil.CfdFreeHandle(handle);
        handle = IntPtr.Zero;
      }
    }

    public void Dispose()
    {
      if (handle != IntPtr.Zero)
      {
        CUtil.CfdFreeHandle(handle);
        handle = IntPtr.Zero;
      }
    }
  }

  internal class Handle
  {
    private IntPtr handle = IntPtr.Zero;

    public Handle()
    {
      CfdErrorCode ret = CUtil.CfdCreateSimpleHandle(out IntPtr temp_handle);
      if (ret != CfdErrorCode.Success)
      {
        throw new System.InvalidOperationException();
      }
      handle = temp_handle;
    }

    public IntPtr GetHandle()
    {
      return handle;
    }
  }

  /// <summary>
  /// cfd library access utility.
  /// </summary>
  internal class CUtil
  {
    /// <summary>
    /// Get string from cfd's pointer address, and free address buffer.
    /// </summary>
    /// <param name="address">pointer address</param>
    /// <returns>string</returns>
    internal static byte[] ReverseBytes(byte[] bytes)
    {
      byte[] temp_bytes = bytes;
      Array.Reverse(temp_bytes);
      return temp_bytes;
    }

    /// <summary>
    /// Get string from cfd's pointer address, and free address buffer.
    /// </summary>
    /// <param name="address">pointer address</param>
    /// <returns>string</returns>
    internal static string ConvertToString(IntPtr address)
    {
      string result = "";
      if (address != IntPtr.Zero)
      {
        result = Marshal.PtrToStringAnsi(address);
        CfdFreeBuffer(address);
      }
      return result;
    }

    /// <summary>
    /// Throw exception from error handle and error code.
    /// </summary>
    /// <param name="handle">error handle</param>
    /// <param name="error_code">error code</param>
    /// <exception cref="System.ArgumentOutOfRangeException">argument range exception</exception>
    /// <exception cref="System.ArgumentException">argument exception</exception>
    /// <exception cref="System.InsufficientMemoryException">memory full exception</exception>
    /// <exception cref="System.InvalidOperationException">illegal exception</exception>
    internal static void ThrowError(ErrorHandle handle, CfdErrorCode error_code)
    {
      if (error_code == CfdErrorCode.Success)
      {
        return;
      }

      string err_message = "";
      CfdErrorCode ret = CfdGetLastErrorMessage(handle.GetHandle(), out IntPtr message_address);
      if (ret == CfdErrorCode.Success)
      {
        string conv_message = ConvertToString(message_address);
        err_message = String.Format("CFD error[{0}] message:{1}", error_code, conv_message);
      }
      else
      {
        err_message = String.Format("CFD error[{0}]", error_code);
      }
      switch ((CfdErrorCode)error_code)
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

    internal static void ThrowError(CfdErrorCode error_code)
    {
      if (error_code == CfdErrorCode.Success)
      {
        return;
      }

      string err_message = String.Format("CFD error[{0}]", error_code);
      switch ((CfdErrorCode)error_code)
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

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdGetSupportedFunction([Out] ulong support_flag);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    public static extern CfdErrorCode CfdInitialize();

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdFinalize([In] bool is_finish_process);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // internal static extern CfdErrorCode CfdCreateHandle([Out] out IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdCreateSimpleHandle([Out] out IntPtr handle);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdCloneHandle([In] IntPtr source, [Out] IntPtr handle);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdCopyErrorState([In] IntPtr source, [In] IntPtr destination);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeHandle([In] IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    static extern CfdErrorCode CfdFreeBuffer([In] IntPtr address);

    // [DllImport("cfd", CallingConvention = CallingConvention.Cdecl)]
    // static extern int CfdGetLastErrorCode([In] IntPtr handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetLastErrorMessage(
        [In] IntPtr handle,
        [Out] out IntPtr message);
  }
}
