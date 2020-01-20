using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  public class Script
  {
    private string script;
    string[] script_items;

    public Script(string script_hex)
    {
      script = script_hex;
      using (ErrorHandle handle = new ErrorHandle())
      {
        ParseScript(handle, script_hex);
      }
    }

    public Script(byte[] bytes)
    {
      script = StringUtil.FromBytes(bytes);
      using (ErrorHandle handle = new ErrorHandle())
      {
        ParseScript(handle, script);
      }
    }

    public Script(uint require_num, Pubkey[] pubkeys)
    {
      // FIXME not implements
    }

    private void ParseScript(ErrorHandle handle, string script_hex)
    {
      CfdErrorCode ret = CScript.CfdParseScript(
        handle.GetHandle(), script_hex, out IntPtr script_item_handle,
        out uint script_item_num);
      if (ret != CfdErrorCode.Success)
      {
        CUtil.ThrowError(handle, ret);
      }
      try
      {
        script_items = new string[script_item_num];
        for (uint index = 0; index < script_item_num; ++index)
        {
          ret = CScript.CfdGetScriptItem(
            handle.GetHandle(), script_item_handle, index,
            out IntPtr script_item);
          if (ret != CfdErrorCode.Success)
          {
            CUtil.ThrowError(handle, ret);
          }
          script_items[index] = CUtil.ConvertToString(script_item);
        }
      }
      finally
      {
        CScript.CfdFreeScriptItemHandle(
          handle.GetHandle(),
          script_item_handle);
      }
    }

    public string ToHexString()
    {
      return script;
    }

    public byte[] ToBytes()
    {
      return StringUtil.ToBytes(script);
    }

    public string[] GetScriptItems()
    {
      return script_items;
    }
  }

  internal class CScript
  {
    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdParseScript(
        [In] IntPtr handle,
        [In] string script,
        [Out] out IntPtr script_item_handle,
        [Out] out uint script_item_num);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetScriptItem(
        [In] IntPtr handle,
        [In] IntPtr script_item_handle,
        [In] uint index,
        [Out] out IntPtr script_item);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeScriptItemHandle(
        [In] IntPtr handle, [In] IntPtr script_item_handle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdConvertScriptAsmToHex(
        [In] IntPtr handle,
        [In] string script_asm,
        [Out] out IntPtr script_hex);
  }
}
