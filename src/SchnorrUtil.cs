using System;

namespace Cfd
{
  /// <summary>
  /// Schnorr function utility class.
  /// </summary>
  public static class SchnorrUtil
  {
    /// <summary>
    /// Sign schnorr.
    /// </summary>
    /// <param name="msg">32-byte msg</param>
    /// <param name="secretKey">secret key</param>
    /// <param name="auxRand">32-byte random bytes</param>
    /// <returns>schnorr signature</returns>
    public static SchnorrSignature Sign(ByteData msg, Privkey secretKey, ByteData auxRand)
    {
      if (msg is null)
      {
        throw new ArgumentNullException(nameof(msg));
      }
      if (secretKey is null)
      {
        throw new ArgumentNullException(nameof(secretKey));
      }
      if (auxRand is null)
      {
        throw new ArgumentNullException(nameof(auxRand));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdSignSchnorr(
            handle.GetHandle(), msg.ToHexString(),
            secretKey.ToHexString(),
            auxRand.ToHexString(),
            out IntPtr signature);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string tempSig = CCommon.ConvertToString(signature);
        return new SchnorrSignature(tempSig);
      }
    }

    /// <summary>
    /// Sign schnorr with nonce.
    /// </summary>
    /// <param name="msg">32-byte msg</param>
    /// <param name="secretKey">secret key</param>
    /// <param name="nonce">32-byte nonce</param>
    /// <returns>schnorr signature</returns>
    public static SchnorrSignature SignWithNonce(ByteData msg, Privkey secretKey, ByteData nonce)
    {
      if (msg is null)
      {
        throw new ArgumentNullException(nameof(msg));
      }
      if (secretKey is null)
      {
        throw new ArgumentNullException(nameof(secretKey));
      }
      if (nonce is null)
      {
        throw new ArgumentNullException(nameof(nonce));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdSignSchnorrWithNonce(
            handle.GetHandle(), msg.ToHexString(),
            secretKey.ToHexString(),
            nonce.ToHexString(),
            out IntPtr signature);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string tempSig = CCommon.ConvertToString(signature);
        return new SchnorrSignature(tempSig);
      }
    }

    /// <summary>
    /// Compute signature point.
    /// </summary>
    /// <param name="msg">32-byte msg</param>
    /// <param name="nonce">schnorr nonce</param>
    /// <param name="schnorrPubkey">pubkey</param>
    /// <returns>signature point</returns>
    public static Pubkey ComputeSigPoint(ByteData msg, SchnorrPubkey nonce, SchnorrPubkey schnorrPubkey)
    {
      if (msg is null)
      {
        throw new ArgumentNullException(nameof(msg));
      }
      if (nonce is null)
      {
        throw new ArgumentNullException(nameof(nonce));
      }
      if (schnorrPubkey is null)
      {
        throw new ArgumentNullException(nameof(schnorrPubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdComputeSchnorrSigPoint(
            handle.GetHandle(), msg.ToHexString(),
            nonce.ToHexString(), schnorrPubkey.ToHexString(),
            out IntPtr sigPoint);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string point = CCommon.ConvertToString(sigPoint);
        return new Pubkey(point);
      }
    }

    /// <summary>
    /// Verify schnorr signature.
    /// </summary>
    /// <param name="signature">schnorr signature</param>
    /// <param name="msg">32-byte msg</param>
    /// <param name="schnorrPubkey">pubkey</param>
    /// <returns>verify result</returns>
    public static bool Verify(SchnorrSignature signature, ByteData msg, SchnorrPubkey schnorrPubkey)
    {
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      if (msg is null)
      {
        throw new ArgumentNullException(nameof(msg));
      }
      if (schnorrPubkey is null)
      {
        throw new ArgumentNullException(nameof(schnorrPubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifySchnorr(
            handle.GetHandle(), signature.ToHexString(),
            msg.ToHexString(),
            schnorrPubkey.ToHexString());
        if (ret == CfdErrorCode.Success)
        {
          return true;
        }
        else if (ret != CfdErrorCode.SignVerificationError)
        {
          handle.ThrowError(ret);
        }
      }
      return false;
    }
  }
}
