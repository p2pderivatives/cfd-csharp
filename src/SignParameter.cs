using System;

namespace Cfd
{
  /// <summary>
  /// txid data class.
  /// </summary>
  public class SignParameter
  {
    private readonly string data;
    private bool isSetDerEncode;
    private SignatureHashType signatureHashType;
    private Pubkey pubkey;

    public static ByteData NormalizeSignature(ByteData signature)
    {
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdNormalizeSignature(
            handle.GetHandle(), signature.ToHexString(),
            out IntPtr normalizedSignature);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new ByteData(CCommon.ConvertToString(normalizedSignature));
      }
    }

    public static ByteData EncodeToDer(ByteData signature, SignatureHashType sighashType)
    {
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdEncodeSignatureByDer(
            handle.GetHandle(), signature.ToHexString(),
            (int)sighashType.SighashType,
            sighashType.IsSighashAnyoneCanPay,
            out IntPtr derSignature);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new ByteData(CCommon.ConvertToString(derSignature));
      }
    }

    public static SignParameter DecodeFromDer(ByteData derSignature)
    {
      if (derSignature is null)
      {
        throw new ArgumentNullException(nameof(derSignature));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdDecodeSignatureFromDer(
            handle.GetHandle(), derSignature.ToHexString(),
            out IntPtr signature,
            out int signatureHashType,
            out bool sighashAnyoneCanPay);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string signatureStr = CCommon.ConvertToString(signature);
        SignatureHashType sighashType = new SignatureHashType((CfdSighashType)signatureHashType, sighashAnyoneCanPay);
        SignParameter signParam = new SignParameter(signatureStr);
        signParam.SetDerEncode(sighashType);
        return signParam;
      }
    }

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public SignParameter()
    {
      data = "";
      signatureHashType = new SignatureHashType(CfdSighashType.All, false);
      pubkey = new Pubkey();
      isSetDerEncode = false;
    }

    public SignParameter(string data)
    {
      if (data == null)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to txid size.");
      }
      this.data = data;
      signatureHashType = new SignatureHashType(CfdSighashType.All, false);
      pubkey = new Pubkey();
      isSetDerEncode = false;
    }

    public SignParameter(byte[] bytes)
    {
      if (bytes == null)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Failed to txid size.");
      }
      data = StringUtil.FromBytes(bytes);
      signatureHashType = new SignatureHashType(CfdSighashType.All, false);
      pubkey = new Pubkey();
      isSetDerEncode = false;
    }

    public ByteData ToDerEncode()
    {
      if (!isSetDerEncode)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalStateError, "Failed to unset der encode flag.");
      }
      return ToDerEncode(signatureHashType);
    }

    public ByteData ToDerEncode(SignatureHashType sighashType)
    {
      return EncodeToDer(new ByteData(data), sighashType);
    }

    public void SetDerEncode(SignatureHashType signatureHashType)
    {
      SetSignatureHashType(signatureHashType);
      isSetDerEncode = true;
    }

    public void SetSignatureHashType(SignatureHashType signatureHashType)
    {
      this.signatureHashType = signatureHashType;
    }

    public void SetRelatedPubkey(Pubkey relatedPubkey)
    {
      pubkey = relatedPubkey;
    }

    public string ToHexString()
    {
      return data;
    }

    public byte[] GetBytes()
    {
      return StringUtil.ToBytes(data);
    }

    public ByteData GetData()
    {
      return new ByteData(data);
    }

    public bool IsDerEncode()
    {
      return isSetDerEncode;
    }

    public SignatureHashType GetSignatureHashType()
    {
      return signatureHashType;
    }

    public Pubkey GetRelatedPubkey()
    {
      return pubkey;
    }
  }
}
