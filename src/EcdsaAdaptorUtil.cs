using System;

namespace Cfd
{
  /// <summary>
  /// ECDSA-adaptor pair.
  /// </summary>
  public struct AdaptorPair : IEquatable<AdaptorPair>
  {
    public ByteData Signature { get; }
    public ByteData Proof { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="signature">adaptor signature</param>
    /// <param name="proof">adaptor proof</param>
    public AdaptorPair(ByteData signature, ByteData proof)
    {
      Signature = signature;
      Proof = proof;
    }

    public bool Equals(AdaptorPair other)
    {
      return Signature.Equals(other.Signature) &&
        Proof.Equals(other.Proof);
    }

    public override bool Equals(object obj)
    {
      if (obj is CfdBlindOptionData)
      {
        return Equals((CfdBlindOptionData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Signature.GetHashCode() + Proof.GetHashCode();
    }

    public static bool operator ==(AdaptorPair left, AdaptorPair right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(AdaptorPair left, AdaptorPair right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// ECDSA-adaptor function utility class.
  /// </summary>
  public static class EcdsaAdaptorUtil
  {
    /// <summary>
    /// Sign ECDSA-adaptor.
    /// </summary>
    /// <param name="msg">32-byte msg</param>
    /// <param name="secretKey">secret key</param>
    /// <param name="adaptor">adaptor pubkey</param>
    /// <returns>ECDSA-adaptor pair</returns>
    public static AdaptorPair Sign(ByteData msg, Privkey secretKey, Pubkey adaptor)
    {
      if (msg is null)
      {
        throw new ArgumentNullException(nameof(msg));
      }
      if (secretKey is null)
      {
        throw new ArgumentNullException(nameof(secretKey));
      }
      if (adaptor is null)
      {
        throw new ArgumentNullException(nameof(adaptor));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdSignEcdsaAdaptor(
            handle.GetHandle(), msg.ToHexString(),
            secretKey.ToHexString(),
            adaptor.ToHexString(),
            out IntPtr signature,
            out IntPtr proof);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string tempSig = CCommon.ConvertToString(signature);
        string tempProof = CCommon.ConvertToString(proof);
        return new AdaptorPair(new ByteData(tempSig), new ByteData(tempProof));
      }
    }

    /// <summary>
    /// Adapt ECDSA-adaptor signature.
    /// </summary>
    /// <param name="adaptorSignature">adaptor signature</param>
    /// <param name="adaptorSecret">adaptor secret key</param>
    /// <returns>ecdsa signature in compact format</returns>
    public static ByteData Adapt(ByteData adaptorSignature, Privkey adaptorSecret)
    {
      if (adaptorSignature is null)
      {
        throw new ArgumentNullException(nameof(adaptorSignature));
      }
      if (adaptorSecret is null)
      {
        throw new ArgumentNullException(nameof(adaptorSecret));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAdaptEcdsaAdaptor(
            handle.GetHandle(), adaptorSignature.ToHexString(),
            adaptorSecret.ToHexString(),
            out IntPtr signature);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string sig = CCommon.ConvertToString(signature);
        return new ByteData(sig);
      }
    }

    /// <summary>
    /// Extract ECDSA-adaptor secret key.
    /// </summary>
    /// <param name="adaptorSignature">adaptor signature</param>
    /// <param name="signature">signature</param>
    /// <param name="adaptor">adaptor pubkey</param>
    /// <returns>secret key</returns>
    public static Privkey ExtractSecret(ByteData adaptorSignature, ByteData signature, Pubkey adaptor)
    {
      if (adaptorSignature is null)
      {
        throw new ArgumentNullException(nameof(adaptorSignature));
      }
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      if (adaptor is null)
      {
        throw new ArgumentNullException(nameof(adaptor));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdExtractEcdsaAdaptorSecret(
            handle.GetHandle(), adaptorSignature.ToHexString(),
            signature.ToHexString(),
            adaptor.ToHexString(),
            out IntPtr secret);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        string sk = CCommon.ConvertToString(secret);
        return new Privkey(sk);
      }
    }

    /// <summary>
    /// Verify ECDSA-adaptor signature.
    /// </summary>
    /// <param name="adaptorSignature">adaptor signature</param>
    /// <param name="adaptorProof">adaptor proof</param>
    /// <param name="adaptor">adaptor pubkey</param>
    /// <param name="msg">32-byte msg</param>
    /// <param name="pubkey">pubkey</param>
    /// <returns>verify result</returns>
    public static bool Verify(ByteData adaptorSignature, ByteData adaptorProof, Pubkey adaptor,
      ByteData msg, Pubkey pubkey)
    {
      if (adaptorSignature is null)
      {
        throw new ArgumentNullException(nameof(adaptorSignature));
      }
      if (adaptorProof is null)
      {
        throw new ArgumentNullException(nameof(adaptorProof));
      }
      if (adaptor is null)
      {
        throw new ArgumentNullException(nameof(adaptor));
      }
      if (msg is null)
      {
        throw new ArgumentNullException(nameof(msg));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifyEcdsaAdaptor(
            handle.GetHandle(), adaptorSignature.ToHexString(),
            adaptorProof.ToHexString(),
            adaptor.ToHexString(),
            msg.ToHexString(),
            pubkey.ToHexString());
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
