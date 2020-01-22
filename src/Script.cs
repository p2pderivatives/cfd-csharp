using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  public class Script
  {
    public const uint MaxSize = 65535;
    private readonly string script;
    private readonly string[] scriptItems;

    public Script(string script_hex)
    {
      if ((script_hex == null) || (script_hex.Length > MaxSize * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to script size.");
      }
      script = script_hex;
      using (var handle = new ErrorHandle())
      {
        scriptItems = ParseScript(handle, script_hex);
      }
    }

    public Script(byte[] bytes)
    {
      if ((bytes == null) || (bytes.Length > MaxSize))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to script size.");
      }
      script = StringUtil.FromBytes(bytes);
      using (var handle = new ErrorHandle())
      {
        scriptItems = ParseScript(handle, script);
      }
    }

    public Script(uint require_num, Pubkey[] pubkeys)
    {
      throw new NotImplementedException();  // FIXME not implements
    }

    private static string[] ParseScript(ErrorHandle handle, string script_hex)
    {
      var ret = CScript.CfdParseScript(
        handle.GetHandle(), script_hex, out IntPtr script_item_handle,
        out uint script_item_num);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      try
      {
        var items = new string[script_item_num];
        for (uint index = 0; index < script_item_num; ++index)
        {
          ret = CScript.CfdGetScriptItem(
            handle.GetHandle(), script_item_handle, index,
            out IntPtr script_item);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          items[index] = CCommon.ConvertToString(script_item);
        }
        return items;
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
      return scriptItems;
    }
  }
}
