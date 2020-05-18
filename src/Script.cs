using System;
using System.Text;

namespace Cfd
{
  public class Script : IEquatable<Script>
  {
    public static readonly uint MaxSize = 65535;
    private readonly string script;
    private readonly string[] scriptItems;

    public static Script CreateFromAsm(string asm)
    {
      if (asm is null)
      {
        throw new ArgumentNullException(nameof(asm));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdConvertScriptAsmToHex(handle.GetHandle(), asm, out IntPtr hexString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new Script(CCommon.ConvertToString(hexString));
      }
    }

    public static Script CreateFromAsm(string[] asmList)
    {
      if (asmList is null)
      {
        throw new ArgumentNullException(nameof(asmList));
      }
      using (var handle = new ErrorHandle())
      {
        StringBuilder builder = new StringBuilder();
        foreach (string asm in asmList)
        {
#pragma warning disable IDE0059 // Unnecessary value assignment
          IntPtr hexString = IntPtr.Zero;
#pragma warning restore IDE0059 // Unnecessary value assignment
          var ret = NativeMethods.CfdConvertScriptAsmToHex(handle.GetHandle(), asm, out hexString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          builder.Append(CCommon.ConvertToString(hexString));
        }
        return new Script(builder.ToString());
      }
    }

    public static Script CreateMultisigScript(uint requireNum, Pubkey[] pubkeys)
    {
      if (pubkeys is null)
      {
        throw new ArgumentNullException(nameof(pubkeys));
      }
      using (var handle = new ErrorHandle())
      {
        return new Script(CreateMultisig(handle, requireNum, pubkeys));
      }
    }

    public static Address[] GetMultisigAddresses(Script multisigScript,
      CfdNetworkType networkType, CfdAddressType addressType
      )
    {
      if (multisigScript is null)
      {
        throw new ArgumentNullException(nameof(multisigScript));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetAddressesFromMultisig(
          handle.GetHandle(), multisigScript.ToHexString(), (int)networkType,
          (int)addressType, out IntPtr multisigHandle, out uint maxKeyNum);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          Address[] addrList = new Address[maxKeyNum];
          for (uint index = 0; index < maxKeyNum; ++index)
          {
            IntPtr address = IntPtr.Zero;
            IntPtr pubkey = IntPtr.Zero;
            ret = NativeMethods.CfdGetAddressFromMultisigKey(
              handle.GetHandle(), multisigHandle, index,
              out address, out pubkey);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
            CCommon.ConvertToString(pubkey);
            string addr = CCommon.ConvertToString(address);
            addrList[index] = new Address(addr);
          }

          return addrList;
        }
        finally
        {
          NativeMethods.CfdFreeAddressesMultisigHandle(
            handle.GetHandle(), multisigHandle);
        }
      }
    }

    public Script()
    {
      script = "";
      scriptItems = Array.Empty<string>();
    }

    public Script(string scriptHex)
    {
      if (scriptHex is null)
      {
        throw new ArgumentNullException(nameof(scriptHex));
      }
      if (scriptHex.Length > MaxSize * 2)
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
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      if (bytes.Length > MaxSize)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to script size.");
      }
      script = StringUtil.FromBytes(bytes);
      using (var handle = new ErrorHandle())
      {
        scriptItems = ParseScript(handle, script);
      }
    }

    private static string CreateMultisig(ErrorHandle handle, uint requireNum, Pubkey[] pubkeys)
    {
      var ret = NativeMethods.CfdInitializeMultisigScript(
        handle.GetHandle(), (int)CfdNetworkType.Mainnet,
        (int)CfdHashType.P2sh, out IntPtr multisigHandle);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      try
      {
        foreach (Pubkey pubkey in pubkeys)
        {
          ret = NativeMethods.CfdAddMultisigScriptData(
            handle.GetHandle(), multisigHandle, pubkey.ToHexString());
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
        }

        ret = NativeMethods.CfdFinalizeMultisigScript(
            handle.GetHandle(), multisigHandle, requireNum, out IntPtr addr,
            out IntPtr redeemScript, out IntPtr witnessScript);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(addr);
        CCommon.ConvertToString(witnessScript);
        return CCommon.ConvertToString(redeemScript);
      }
      finally
      {
        NativeMethods.CfdFreeMultisigScriptHandle(
          handle.GetHandle(), multisigHandle);
      }
    }

    private static string[] ParseScript(ErrorHandle handle, string scriptHex)
    {
      var ret = NativeMethods.CfdParseScript(
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
          IntPtr scriptItem = IntPtr.Zero;
          ret = NativeMethods.CfdGetScriptItem(
            handle.GetHandle(), scriptItemHandle, index,
            out scriptItem);
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
        NativeMethods.CfdFreeScriptItemHandle(
          handle.GetHandle(),
          scriptItemHandle);
      }
    }

    public bool IsEmpty()
    {
      return string.IsNullOrEmpty(script);
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

    public bool Equals(Script other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }
      return script.Equals(other.script, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as Script) != null)
      {
        return Equals((Script)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return script.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(Script lhs, Script rhs)
    {
      if (lhs is null)
      {
        if (rhs is null)
        {
          return true;
        }
        return false;
      }
      return lhs.Equals(rhs);
    }

    public static bool operator !=(Script lhs, Script rhs)
    {
      return !(lhs == rhs);
    }
  }
}
