using System;

/// <summary>
/// cfd library namespace.
/// </summary>
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

    public static ByteData EncodeToDer(ByteData signature, SignatureHashType sighashType)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CKey.CfdEncodeSignatureByDer(
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

    public static ByteData DecodeFromDer(ByteData derSignature, out SignatureHashType sighashType)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = CKey.CfdDecodeSignatureFromDer(
            handle.GetHandle(), derSignature.ToHexString(),
            out IntPtr signature,
            out int signatureHashType,
            out bool sighashAnyoneCanPay);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string signatureStr = CCommon.ConvertToString(signature);
        sighashType = new SignatureHashType((CfdSighashType)signatureHashType, sighashAnyoneCanPay);
        return new ByteData(signatureStr);
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
      this.data = StringUtil.FromBytes(bytes);
      signatureHashType = new SignatureHashType(CfdSighashType.All, false);
      pubkey = new Pubkey();
      isSetDerEncode = false;
    }

    public void SetDerEncode(SignatureHashType signatureHashType)
    {
      this.signatureHashType = signatureHashType;
      isSetDerEncode = true;
    }

    public void SetRelatedPubkey(Pubkey relatedPubkey)
    {
      this.pubkey = relatedPubkey;
    }

    public string ToHexString()
    {
      return data;
    }

    public byte[] GetBytes()
    {
      return StringUtil.ToBytes(data);
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
