using System;

/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public class Script
  {
    public const uint MaxSize = 65535;
    private readonly string script;
    private readonly string[] scriptItems;

    public Script()
    {
      script = "";
      scriptItems = new string[0];
    }

    public Script(string scriptHex)
    {
      if ((scriptHex == null) || (scriptHex.Length > MaxSize * 2))
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to script size.");
      }
      script = scriptHex;
      using (var handle = new ErrorHandle())
      {
        scriptItems = ParseScript(handle, scriptHex);
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

    public Script(uint requireNum, Pubkey[] pubkeys)
    {
      throw new NotImplementedException();  // FIXME not implements
    }

    private static string[] ParseScript(ErrorHandle handle, string scriptHex)
    {
      var ret = CScript.CfdParseScript(
        handle.GetHandle(), scriptHex, out IntPtr scriptItemHandle,
        out uint scriptItemNum);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      try
      {
        var items = new string[scriptItemNum];
        for (uint index = 0; index < scriptItemNum; ++index)
        {
          ret = CScript.CfdGetScriptItem(
            handle.GetHandle(), scriptItemHandle, index,
            out IntPtr scriptItem);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          items[index] = CCommon.ConvertToString(scriptItem);
        }
        return items;
      }
      finally
      {
        CScript.CfdFreeScriptItemHandle(
          handle.GetHandle(),
          scriptItemHandle);
      }
    }

    public bool IsEmpty()
    {
      return String.IsNullOrEmpty(script);
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

    public string GetAsm()
    {
      return string.Join(" ", scriptItems);
    }
  }
}
