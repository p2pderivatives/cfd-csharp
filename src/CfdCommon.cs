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
  public enum CfdErrorCode {
    Success = 0,               //!< 正常終了
    UnknownError = -1,         //!< 不明なエラー
    InternalError = -2,        //!< 内部エラー
    MemoryFullError = -3,      //!< メモリ確保エラー
    IllegalArgumentError = 1,  //!< 引数不正
    IllegalStateError = 2,     //!< 状態不正
    OutOfRangeError = 3,       //!< 範囲外の値
    InvalidSettingError = 4,   //!< 設定不正
    ConnectionError = 5,       //!< 接続エラー
    DiskAccessError = 6,       //!< ディスクアクセスエラー
    SignVerificationError = 7  //!< Signature Verification 失敗時のエラー
  };

  /// <summary>
  /// error handle wrapper class.
  /// </summary>
  internal class ErrorHandle : IDisposable
  {
    private IntPtr handle = IntPtr.Zero;

    /// <summary>
    /// constructor.
    /// </summary>
    /// <param name="data">与えられたデータ列</param>
    /// <param name="mean">dataの平均値(出力)</param>
    /// <param name="variance">dataの分散(出力)</param>
    public ErrorHandle() {
      CfdErrorCode ret = CUtil.CfdCreateSimpleHandle(out IntPtr temp_handle);
      if (ret != CfdErrorCode.Success) {
        throw new System.InvalidOperationException();
      }
      handle = temp_handle;
    }

    public IntPtr GetHandle() {
      return handle;
    }

    ~ErrorHandle() {
      if (handle != IntPtr.Zero) {
        CUtil.CfdFreeHandle(handle);
        handle = IntPtr.Zero;
      }
    }

    public void Dispose() {
      if (handle != IntPtr.Zero) {
        CUtil.CfdFreeHandle(handle);
        handle = IntPtr.Zero;
      }
    }
  }

  internal class Handle
  {
    private IntPtr handle = IntPtr.Zero;

    public Handle() {
      CfdErrorCode ret = CUtil.CfdCreateSimpleHandle(out IntPtr temp_handle);
      if (ret != CfdErrorCode.Success) {
        throw new System.InvalidOperationException();
      }
      handle = temp_handle;
    }

    public IntPtr GetHandle() {
      return handle;
    }
  }

  /// <summary>
  /// cfd libraryへのアクセスおよびUtility関数を定義したクラス。
  /// </summary>
  internal class CUtil
  {
    /// <summary>
    /// cfdから取得したcharポインタから文字列を取得し、バッファを解放する。
    /// </summary>
    /// <param name="address">ポインタアドレス</param>
    /// <returns>文字列情報</returns>
    internal static string ConvertToString(IntPtr address) {
      string result = "";
      if (address != IntPtr.Zero) {
        result = Marshal.PtrToStringAnsi(address);
        CfdFreeBuffer(address);
      }
      return result;
    }

    /// <summary>
    /// cfdのハンドル情報とエラーコードから、例外を生成してスローする。
    /// </summary>
    /// <param name="handle">エラーハンドル</param>
    /// <param name="error_code">エラーコード</param>
    /// <exception cref="System.ArgumentOutOfRangeException">argument range exception</exception>
    /// <exception cref="System.ArgumentException">argument exception</exception>
    /// <exception cref="System.InsufficientMemoryException">memory full exception</exception>
    /// <exception cref="System.InvalidOperationException">illegal exception</exception>
    internal static void ThrowError(ErrorHandle handle, CfdErrorCode error_code) {
      if (error_code == CfdErrorCode.Success) {
        return;
      }

      string err_message = "";
      CfdErrorCode ret = CfdGetLastErrorMessage(handle.GetHandle(), out IntPtr message_address);
      if (ret == CfdErrorCode.Success) {
        string conv_message = ConvertToString(message_address);
        err_message = String.Format("CFD error[{0}] message:{1}", error_code, conv_message);
      } else {
        err_message = String.Format("CFD error[{0}]", error_code);
      }
      switch ((CfdErrorCode)error_code) {
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

    internal static void ThrowError(CfdErrorCode error_code) {
      if (error_code == CfdErrorCode.Success) {
        return;
      }

      string err_message = String.Format("CFD error[{0}]", error_code);
      switch ((CfdErrorCode)error_code) {
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
