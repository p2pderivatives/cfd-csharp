using System;
using System.Collections.Generic;

namespace Cfd
{
  /// <summary>
  /// binding option.
  /// </summary>
  public enum CfdBlindOption
  {
    MinimumRangeValue = 1,  //!< minRangeValue
    Exponent = 2,           //!< exponent
    MinimumBits = 3,        //!< minBits
  }

  /// <summary>
  /// Blind option data.
  /// </summary>
  public struct CfdBlindOptionData : IEquatable<CfdBlindOptionData>
  {
    public long MinimumRangeValue { get; }
    public int Exponent { get; }
    public int MinimumBits { get; }

    public CfdBlindOptionData(long minimumRangeValue, int exponent, int minimumBits)
    {
      this.MinimumRangeValue = minimumRangeValue;
      this.Exponent = exponent;
      this.MinimumBits = minimumBits;
    }

    public bool Equals(CfdBlindOptionData other)
    {
      return MinimumRangeValue.Equals(other.MinimumRangeValue) &&
        Exponent.Equals(other.Exponent) &&
        MinimumBits.Equals(other.MinimumBits);
    }

    public override bool Equals(object obj)
    {
      if (obj is CfdBlindOptionData)
      {
        return this.Equals((CfdBlindOptionData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return MinimumRangeValue.GetHashCode() + Exponent.GetHashCode() + MinimumBits.GetHashCode();
    }

    public static bool operator ==(CfdBlindOptionData left, CfdBlindOptionData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(CfdBlindOptionData left, CfdBlindOptionData right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// asset and amountValue data struct.
  /// </summary>
  public struct AssetValueData : IEquatable<AssetValueData>
  {
    public string Asset { get; }
    public long SatoshiValue { get; }
    public BlindFactor AssetBlindFactor { get; }
    public BlindFactor AmountBlindFactor { get; }

    /// <summary>
    /// Constructor. (use blinded utxo)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    /// <param name="assetBlindFactor">asset blinder</param>
    /// <param name="amountBlindFactor">amount blinder</param>
    public AssetValueData(string asset, long satoshiValue, BlindFactor assetBlindFactor, BlindFactor amountBlindFactor)
    {
      this.Asset = asset;
      this.SatoshiValue = satoshiValue;
      this.AssetBlindFactor = assetBlindFactor;
      this.AmountBlindFactor = amountBlindFactor;
    }

    /// <summary>
    /// Constructor. (use unblinded utxo)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    public AssetValueData(string asset, long satoshiValue)
    {
      this.Asset = asset;
      this.SatoshiValue = satoshiValue;
      this.AssetBlindFactor = new BlindFactor();
      this.AmountBlindFactor = new BlindFactor();
    }

    /// <summary>
    /// Constructor. (use issue/reissue response only)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    /// <param name="amountBlindFactor">amount blinder</param>
    public AssetValueData(string asset, long satoshiValue, BlindFactor amountBlindFactor)
    {
      this.Asset = asset;
      this.SatoshiValue = satoshiValue;
      this.AssetBlindFactor = new BlindFactor();
      this.AmountBlindFactor = amountBlindFactor;
    }

    public bool Equals(AssetValueData other)
    {
      return Asset.Equals(other.Asset, StringComparison.Ordinal) &&
        SatoshiValue.Equals(other.SatoshiValue);
    }

    public override bool Equals(object obj)
    {
      if (obj is AssetValueData)
      {
        return this.Equals((AssetValueData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Asset.GetHashCode(StringComparison.Ordinal) + SatoshiValue.GetHashCode();
    }

    public static bool operator ==(AssetValueData left, AssetValueData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(AssetValueData left, AssetValueData right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// unblinding issuance data. (asset and token)
  /// </summary>
  public struct UnblindIssuanceData : IEquatable<UnblindIssuanceData>
  {
    public AssetValueData AssetData { get; }
    public AssetValueData TokenData { get; }

    public UnblindIssuanceData(AssetValueData asset, AssetValueData token)
    {
      AssetData = asset;
      TokenData = token;
    }

    public bool Equals(UnblindIssuanceData other)
    {
      return AssetData.Equals(other.AssetData) &&
        TokenData.Equals(other.TokenData);
    }

    public override bool Equals(object obj)
    {
      if (obj is UnblindIssuanceData)
      {
        return this.Equals((UnblindIssuanceData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return AssetData.GetHashCode() + TokenData.GetHashCode();
    }

    public static bool operator ==(UnblindIssuanceData left, UnblindIssuanceData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(UnblindIssuanceData left, UnblindIssuanceData right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// issuance blinding key pairs.
  /// </summary>
  public struct IssuanceKeys : IEquatable<IssuanceKeys>
  {
    public Privkey AssetKey { get; }
    public Privkey TokenKey { get; }

    /// <summary>
    /// Constructor. (use issueasset)
    /// </summary>
    /// <param name="assetKey">asset blinding key</param>
    /// <param name="tokenKey">token blinding key</param>
    public IssuanceKeys(Privkey assetKey, Privkey tokenKey)
    {
      this.AssetKey = assetKey;
      this.TokenKey = tokenKey;
    }

    /// <summary>
    /// Constructor. (use reissueasset)
    /// </summary>
    /// <param name="assetKey">asset blinding key</param>
    public IssuanceKeys(Privkey assetKey)
    {
      this.AssetKey = assetKey;
      this.TokenKey = new Privkey();
    }

    public bool Equals(IssuanceKeys other)
    {
      return AssetKey.Equals(other.AssetKey) &&
        TokenKey.Equals(other.TokenKey);
    }

    public override bool Equals(object obj)
    {
      if (obj is IssuanceKeys)
      {
        return this.Equals((IssuanceKeys)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return AssetKey.GetHashCode() + TokenKey.GetHashCode();
    }

    public static bool operator ==(IssuanceKeys left, IssuanceKeys right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(IssuanceKeys left, IssuanceKeys right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// issuance data.
  /// </summary>
  public struct IssuanceData : IEquatable<IssuanceData>
  {
    public ByteData BlindingNonce { get; }
    public ByteData AssetEntropy { get; }
    public ConfidentialValue IssuanceAmount { get; }
    public ConfidentialValue InflationKeys { get; }
    public ByteData IssuanceAmountRangeproof { get; }
    public ByteData InflationKeysRangeproof { get; }

    public IssuanceData(byte[] blindingNonce, byte[] assetEntropy,
        ConfidentialValue issuanceAmount, ConfidentialValue tokenAmount,
        byte[] issuanceRangeproof, byte[] tokenRangeproof)
    {
      this.BlindingNonce = new ByteData(blindingNonce);
      this.AssetEntropy = new ByteData(assetEntropy);
      this.IssuanceAmount = issuanceAmount;
      this.InflationKeys = tokenAmount;
      this.IssuanceAmountRangeproof = new ByteData(issuanceRangeproof);
      this.InflationKeysRangeproof = new ByteData(tokenRangeproof);
    }

    public bool Equals(IssuanceData other)
    {
      return BlindingNonce.Equals(other.BlindingNonce) &&
        AssetEntropy.Equals(other.AssetEntropy);
    }

    public override bool Equals(object obj)
    {
      if (obj is IssuanceData)
      {
        return this.Equals((IssuanceData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return BlindingNonce.GetHashCode() + AssetEntropy.GetHashCode();
    }

    public static bool operator ==(IssuanceData left, IssuanceData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(IssuanceData left, IssuanceData right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// Transaction input struct.
  /// </summary>
  public struct ConfidentialTxIn : IEquatable<ConfidentialTxIn>
  {
    public OutPoint OutPoint { get; }
    public Script ScriptSig { get; }
    public UInt32 Sequence { get; }
    public ScriptWitness WitnessStack { get; }
    public ScriptWitness PeginWitness { get; }
    public IssuanceData Issuance { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    public ConfidentialTxIn(OutPoint outPoint)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = new Script();
      this.Sequence = CfdSequenceLockTime.Disable;
      this.WitnessStack = new ScriptWitness();
      this.PeginWitness = new ScriptWitness();
      this.Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, UInt32 sequence)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = new Script();
      this.Sequence = sequence;
      this.WitnessStack = new ScriptWitness();
      this.PeginWitness = new ScriptWitness();
      this.Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, ScriptWitness scriptWitness)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = new Script();
      this.Sequence = CfdSequenceLockTime.Disable;
      this.WitnessStack = scriptWitness;
      this.PeginWitness = new ScriptWitness();
      this.Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, UInt32 sequence, ScriptWitness scriptWitness)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = new Script();
      this.Sequence = sequence;
      this.WitnessStack = scriptWitness;
      this.PeginWitness = new ScriptWitness();
      this.Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, Script scriptSig, ScriptWitness witnessStack, ScriptWitness peginWitness, IssuanceData issuance)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = scriptSig;
      this.Sequence = CfdSequenceLockTime.Disable;
      this.WitnessStack = witnessStack;
      this.PeginWitness = peginWitness;
      this.Issuance = issuance;
    }
    public ConfidentialTxIn(OutPoint outPoint, UInt32 sequence, Script scriptSig, ScriptWitness witnessStack, ScriptWitness peginWitness, IssuanceData issuance)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = scriptSig;
      this.Sequence = sequence;
      this.WitnessStack = witnessStack;
      this.PeginWitness = peginWitness;
      this.Issuance = issuance;
    }

    public bool Equals(ConfidentialTxIn other)
    {
      return OutPoint.Equals(other.OutPoint);
    }

    public override bool Equals(object obj)
    {
      if (obj is ConfidentialTxIn)
      {
        return this.Equals((ConfidentialTxIn)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(OutPoint);
    }

    public static bool operator ==(ConfidentialTxIn left, ConfidentialTxIn right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ConfidentialTxIn left, ConfidentialTxIn right)
    {
      return !(left == right);
    }
  };

  /// <summary>
  /// Transaction output struct.
  /// </summary>
  public struct ConfidentialTxOut : IEquatable<ConfidentialTxOut>
  {
    public ConfidentialAsset Asset { get; }
    public ConfidentialValue Value { get; }
    public ByteData Nonce { get; }
    public Script ScriptPubkey { get; }
    public ByteData SurjectionProof { get; }
    public ByteData RangeProof { get; }

    public ConfidentialTxOut(ConfidentialAsset asset, long value)
    {
      this.Asset = asset;
      this.Value = new ConfidentialValue(value);
      this.ScriptPubkey = new Script();
      this.Nonce = new ByteData(Array.Empty<byte>());
      this.SurjectionProof = new ByteData(Array.Empty<byte>());
      this.RangeProof = new ByteData(Array.Empty<byte>());
    }

    public ConfidentialTxOut(ConfidentialAsset asset, ConfidentialValue value, Script scriptPubkey)
    {
      this.Asset = asset;
      this.Value = value;
      this.ScriptPubkey = scriptPubkey;
      this.Nonce = new ByteData(Array.Empty<byte>());
      this.SurjectionProof = new ByteData(Array.Empty<byte>());
      this.RangeProof = new ByteData(Array.Empty<byte>());
    }

    public ConfidentialTxOut(ConfidentialAsset asset, ConfidentialValue value,
        Script scriptPubkey, byte[] nonce, byte[] surjectionProof, byte[] rangeProof)
    {
      this.Asset = asset;
      this.Value = value;
      this.ScriptPubkey = scriptPubkey;
      this.Nonce = new ByteData(nonce);
      this.SurjectionProof = new ByteData(surjectionProof);
      this.RangeProof = new ByteData(rangeProof);
    }

    public bool Equals(ConfidentialTxOut other)
    {
      return ScriptPubkey.Equals(other.ScriptPubkey);
    }

    public override bool Equals(object obj)
    {
      if (obj is ConfidentialTxOut)
      {
        return this.Equals((ConfidentialTxOut)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(ScriptPubkey);
    }

    public static bool operator ==(ConfidentialTxOut left, ConfidentialTxOut right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ConfidentialTxOut left, ConfidentialTxOut right)
    {
      return !(left == right);
    }
  };

  /// <summary>
  /// Confidential transaction class.
  /// </summary>
  public class ConfidentialTransaction
  {
    private string tx;
    private string lastGetTx = "";
    private string txid = "";
    private string wtxid = "";
    private string witHash = "";
    private uint txSize;
    private uint txVsize;
    private uint txWeight;
    private uint txVersion;
    private uint txLocktime;

    /// <summary>
    /// Convert tx to decoderawtransaction json string.
    /// </summary>
    /// <param name="tx">transaction object.</param>
    /// <returns>json string</returns>
    public static string DecodeRawTransaction(
        ConfidentialTransaction tx)
    {
      return DecodeRawTransaction(tx, CfdNetworkType.Liquidv1, CfdNetworkType.Mainnet);
    }

    /// <summary>
    /// Convert tx to decoderawtransaction json string.
    /// </summary>
    /// <param name="tx">transaction object.</param>
    /// <param name="network">network type.</param>
    /// <returns>json string</returns>
    public static string DecodeRawTransaction(
        ConfidentialTransaction tx,
        CfdNetworkType network)
    {
      return DecodeRawTransaction(tx, network, CfdNetworkType.Mainnet);
    }

    /// <summary>
    /// Convert tx to decoderawtransaction json string.
    /// </summary>
    /// <param name="tx">transaction object.</param>
    /// <param name="network">network type.</param>
    /// <param name="mainchainNetwork">mainchain network type.</param>
    /// <returns>json string</returns>
    public static string DecodeRawTransaction(
        ConfidentialTransaction tx,
        CfdNetworkType network,
        CfdNetworkType mainchainNetwork)
    {
      if (tx is null)
      {
        throw new ArgumentNullException(nameof(tx));
      }
      string networkStr = "regtest";
      string mainchainNetworkStr = "regtest";
      if (network == CfdNetworkType.Liquidv1)
      {
        networkStr = "liquidv1";
      }
      if (mainchainNetwork == CfdNetworkType.Mainnet)
      {
        mainchainNetworkStr = "mainnet";
      }
#pragma warning disable CA1305 // IFormatProvider
      string request = string.Format(
          "{{\"hex\":\"{0}\",\"network\":\"{1}\",\"mainchainNetwork\":\"{2}\"}}",
          tx.ToHexString(), networkStr, mainchainNetworkStr);
#pragma warning restore CA1305 // IFormatProvider

      using (var handle = new ErrorHandle())
      {
        var ret = CCommon.CfdRequestExecuteJson(
            handle.GetHandle(),
            "ElementsDecodeRawTransaction",
            request,
            out IntPtr responseJsonString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return CCommon.ConvertToString(responseJsonString);
      }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="version">transaction version</param>
    /// <param name="locktime">transaction locktime</param>
    public ConfidentialTransaction(uint version, uint locktime)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeConfidentialTx(
          handle.GetHandle(),
          version,
          locktime,
          out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="txHex">transaction hex</param>
    public ConfidentialTransaction(string txHex)
    {
      tx = txHex;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">transaction byte array</param>
    public ConfidentialTransaction(byte[] bytes)
    {
      tx = StringUtil.FromBytes(bytes);
    }

    /// <summary>
    /// Add transction input.
    /// </summary>
    /// <param name="txid">utxo txid.</param>
    /// <param name="vout">utxo vout.</param>
    public void AddTxIn(Txid txid, uint vout)
    {
      AddTxIn(txid, vout, CfdSequenceLockTime.Disable);
    }

    /// <summary>
    /// Add transction input.
    /// </summary>
    /// <param name="txid">utxo txid.</param>
    /// <param name="vout">utxo vout.</param>
    /// <param name="sequence">sequence number. (default: 0xffffffff)</param>
    public void AddTxIn(Txid txid, uint vout, uint sequence)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxIn(
          handle.GetHandle(), tx, txid.ToHexString(), vout, sequence, out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    /// <summary>
    /// Add transction input.
    /// </summary>
    /// <param name="outpoint">outpoint.</param>
    public void AddTxIn(OutPoint outpoint)
    {
      AddTxIn(outpoint, CfdSequenceLockTime.Disable);
    }

    /// <summary>
    /// Add transction input.
    /// </summary>
    /// <param name="outpoint">outpoint.</param>
    /// <param name="sequence">sequence number. (default: 0xffffffff)</param>
    public void AddTxIn(OutPoint outpoint, uint sequence)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxIn(
          handle.GetHandle(), tx, outpoint.GetTxid().ToHexString(), outpoint.GetVout(),
          sequence, out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="satoshiValue">txout satoshi value.</param>
    /// <param name="address">txout address.</param>
    public void AddTxOut(string asset, long satoshiValue, Address address)
    {
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            satoshiValue,
            "",
            address.ToAddressString(),
            "",
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="valueCommitment">txout commitment value.</param>
    /// <param name="address">txout address.</param>
    public void AddTxOut(string asset, ConfidentialValue valueCommitment, Address address)
    {
      if (valueCommitment is null)
      {
        throw new ArgumentNullException(nameof(valueCommitment));
      }
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            (long)0,
            valueCommitment.ToHexString(),
            address.ToAddressString(),
            "",
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddFeeTxOut(long satoshiValue)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            "",
            satoshiValue,
            "",
            "",
            "",
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddDestroyAmountTxOut(string asset, long satoshiValue)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset,
            satoshiValue,
            "",
            "",
            "6a",  // OP_RETURN
            "",
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    /// <summary>
    /// Blind transction txout only.
    /// </summary>
    /// <param name="utxos">txin utxo list</param>
    /// <param name="confidentialKeys">txout confidential key list</param>
    public void BlindTxOut(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<uint, Pubkey> confidentialKeys)
    {
      BlindTransaction(utxos, new Dictionary<OutPoint, IssuanceKeys>(),
          confidentialKeys);
    }

    /// <summary>
    /// Blind transction.
    /// </summary>
    /// <param name="utxos">txin utxo list</param>
    /// <param name="issuanceKeys">issuance blinding key list</param>
    /// <param name="confidentialKeys">txout confidential key list</param>
    public void BlindTransaction(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<OutPoint, IssuanceKeys> issuanceKeys,
        IDictionary<uint, Pubkey> confidentialKeys)
    {
      if (issuanceKeys is null)
      {
        throw new ArgumentNullException(nameof(issuanceKeys));
      }
      if (confidentialKeys is null)
      {
        throw new ArgumentNullException(nameof(confidentialKeys));
      }
      if (utxos is null)
      {
        throw new ArgumentNullException(nameof(utxos));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeBlindTx(
          handle.GetHandle(), out IntPtr blindHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          foreach (var outpoint in utxos.Keys)
          {
            var data = utxos[outpoint];
            string assetKey = "";
            string tokenKey = "";
            if (issuanceKeys.ContainsKey(outpoint))
            {
              IssuanceKeys keys = issuanceKeys[outpoint];
              assetKey = keys.AssetKey.ToHexString();
              tokenKey = keys.TokenKey.ToHexString();
            }

            ret = NativeMethods.CfdAddBlindTxInData(
              handle.GetHandle(), blindHandle,
              outpoint.GetTxid().ToHexString(), outpoint.GetVout(),
              data.Asset,
              data.AssetBlindFactor.ToHexString(),
              data.AmountBlindFactor.ToHexString(),
              data.SatoshiValue,
              assetKey,
              tokenKey);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          foreach (var index in confidentialKeys.Keys)
          {
            ret = NativeMethods.CfdAddBlindTxOutData(
              handle.GetHandle(), blindHandle,
              index, confidentialKeys[index].ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdFinalizeBlindTx(
            handle.GetHandle(), blindHandle, tx,
            out IntPtr txHexString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          tx = CCommon.ConvertToString(txHexString);
        }
        finally
        {
          NativeMethods.CfdFreeBlindHandle(handle.GetHandle(), blindHandle);
        }
      }
    }

    public void BlindTransaction(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<OutPoint, IssuanceKeys> issuanceKeys,
        ConfidentialAddress[] confidentialAddresses)
    {
      BlindTransaction(utxos, issuanceKeys, confidentialAddresses,
        new CfdBlindOptionData(1, 0, 52));
    }

    public void BlindTransaction(IDictionary<OutPoint, AssetValueData> utxos,
        IDictionary<OutPoint, IssuanceKeys> issuanceKeys,
        ConfidentialAddress[] confidentialAddresses, CfdBlindOptionData option)
    {
      if (issuanceKeys is null)
      {
        throw new ArgumentNullException(nameof(issuanceKeys));
      }
      if (confidentialAddresses is null)
      {
        throw new ArgumentNullException(nameof(confidentialAddresses));
      }
      if (utxos is null)
      {
        throw new ArgumentNullException(nameof(utxos));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeBlindTx(
          handle.GetHandle(), out IntPtr blindHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          ret = NativeMethods.CfdSetBlindTxOption(
            handle.GetHandle(), blindHandle, (int)CfdBlindOption.MinimumRangeValue,
            option.MinimumRangeValue);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          ret = NativeMethods.CfdSetBlindTxOption(
            handle.GetHandle(), blindHandle, (int)CfdBlindOption.Exponent,
            option.Exponent);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          ret = NativeMethods.CfdSetBlindTxOption(
            handle.GetHandle(), blindHandle, (int)CfdBlindOption.MinimumBits,
            option.MinimumBits);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }

          foreach (var outpoint in utxos.Keys)
          {
            var data = utxos[outpoint];
            string assetKey = "";
            string tokenKey = "";
            if (issuanceKeys.ContainsKey(outpoint))
            {
              IssuanceKeys keys = issuanceKeys[outpoint];
              assetKey = keys.AssetKey.ToHexString();
              tokenKey = keys.TokenKey.ToHexString();
            }

            ret = NativeMethods.CfdAddBlindTxInData(
              handle.GetHandle(), blindHandle,
              outpoint.GetTxid().ToHexString(), outpoint.GetVout(),
              data.Asset,
              data.AssetBlindFactor.ToHexString(),
              data.AmountBlindFactor.ToHexString(),
              data.SatoshiValue,
              assetKey,
              tokenKey);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          foreach (var address in confidentialAddresses)
          {
            ret = NativeMethods.CfdAddBlindTxOutByAddress(
              handle.GetHandle(), blindHandle,
              address.ToAddressString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdFinalizeBlindTx(
            handle.GetHandle(), blindHandle, tx,
            out IntPtr txHexString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          tx = CCommon.ConvertToString(txHexString);
        }
        finally
        {
          NativeMethods.CfdFreeBlindHandle(handle.GetHandle(), blindHandle);
        }
      }
    }

    /// <summary>
    /// Unblind transction output.
    /// </summary>
    /// <param name="txoutIndex">txout index</param>
    /// <param name="blindingKey">blinding key</param>
    /// <returns>asset and amount data</returns>
    public AssetValueData UnblindTxOut(uint txoutIndex, Privkey blindingKey)
    {
      if (blindingKey is null)
      {
        throw new ArgumentNullException(nameof(blindingKey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdUnblindTxOut(
            handle.GetHandle(), tx,
            txoutIndex,
            blindingKey.ToHexString(),
            out IntPtr asset,
            out long value,
            out IntPtr assetBlindFactor,
            out IntPtr amountBlindFactor);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        var abf = CCommon.ConvertToString(assetBlindFactor);
        var vbf = CCommon.ConvertToString(amountBlindFactor);
        var assetId = CCommon.ConvertToString(asset);
        return new AssetValueData(assetId, value, new BlindFactor(abf), new BlindFactor(vbf));
      }
    }

    /// <summary>
    /// Unblind transction output.
    /// </summary>
    /// <param name="txinIndex">txin index</param>
    /// <param name="assetBlindingKey">asset blinding key(issue/reissue)</param>
    /// <param name="tokenBlindingKey">token blinding key(issue only)</param>
    /// <returns>issuance asset data</returns>
    public UnblindIssuanceData UnblindIssuance(uint txinIndex, Privkey assetBlindingKey, Privkey tokenBlindingKey)
    {
      if (assetBlindingKey is null)
      {
        throw new ArgumentNullException(nameof(assetBlindingKey));
      }
      if (tokenBlindingKey is null)
      {
        throw new ArgumentNullException(nameof(tokenBlindingKey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdUnblindIssuance(
            handle.GetHandle(), tx,
            txinIndex,
            assetBlindingKey.ToHexString(),
            tokenBlindingKey.ToHexString(),
            out IntPtr asset,
            out long assetValue,
            out IntPtr assetBlindFactor,
            out IntPtr assetAmountBlindFactor,
            out IntPtr token,
            out long tokenValue,
            out IntPtr tokenBlindFactor,
            out IntPtr tokenAmountBlindFactor);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        var assetAbf = CCommon.ConvertToString(assetBlindFactor);
        var assetVbf = CCommon.ConvertToString(assetAmountBlindFactor);
        var tokenAbf = CCommon.ConvertToString(tokenBlindFactor);
        var tokenVbf = CCommon.ConvertToString(tokenBlindFactor);
        var assetId = CCommon.ConvertToString(asset);
        var tokenId = CCommon.ConvertToString(token);

        return new UnblindIssuanceData(
            new AssetValueData(assetId, assetValue,
                new BlindFactor(assetAbf), new BlindFactor(assetVbf)),
            new AssetValueData(tokenId, tokenValue,
                new BlindFactor(tokenAbf), new BlindFactor(tokenVbf)));
      }
    }

    /// <summary>
    /// Get transaction input data.
    /// </summary>
    /// <param name="outpoint">utxo outpoint(txid and vout)</param>
    /// <returns>transaction input data.</returns>
    public ConfidentialTxIn GetTxIn(OutPoint outpoint)
    {
      uint index = GetTxInIndex(outpoint);
      using (var handle = new ErrorHandle())
      {
        return GetInputByIndex(handle, index);
      }
    }

    /// <summary>
    /// Get transaction input data.
    /// </summary>
    /// <param name="index">txin index.</param>
    /// <returns>transaction input data.</returns>
    public ConfidentialTxIn GetTxIn(uint index)
    {
      using (var handle = new ErrorHandle())
      {
        return GetInputByIndex(handle, index);
      }
    }

    /// <summary>
    /// Get transaction input list.
    /// </summary>
    /// <returns>transaction input list.</returns>
    public ConfidentialTxIn[] GetTxInList()
    {
      using (var handle = new ErrorHandle())
      {
        uint count = GetInputCount(handle);
        ConfidentialTxIn[] result = new ConfidentialTxIn[count];

        for (uint index = 0; index < count; ++index)
        {
          result[index] = GetInputByIndex(handle, index);
        }
        return result;
      }
    }

    /// <summary>
    /// Get transaction input count.
    /// </summary>
    /// <returns>transaction input count.</returns>
    public uint GetTxInCount()
    {
      using (var handle = new ErrorHandle())
      {
        return GetInputCount(handle);
      }
    }

    /// <summary>
    /// Get transaction output data.
    /// </summary>
    /// <param name="index">txout index.</param>
    /// <returns>transaction output data.</returns>
    public ConfidentialTxOut GetTxOut(uint index)
    {
      using (var handle = new ErrorHandle())
      {
        return GetOutputByIndex(handle, index);
      }
    }

    /// <summary>
    /// Get transaction output list.
    /// </summary>
    /// <returns>transaction output list.</returns>
    public ConfidentialTxOut[] GetTxOutList()
    {
      using (var handle = new ErrorHandle())
      {
        uint count = GetOutputCount(handle);
        ConfidentialTxOut[] result = new ConfidentialTxOut[count];

        for (uint index = 0; index < count; ++index)
        {
          result[index] = GetOutputByIndex(handle, index);
        }
        return result;
      }
    }

    /// <summary>
    /// Get transaction output count.
    /// </summary>
    /// <returns>transaction output count.</returns>
    public uint GetTxOutCount()
    {
      using (var handle = new ErrorHandle())
      {
        return GetOutputCount(handle);
      }
    }

    /// <summary>
    /// Get transaction input index.
    /// </summary>
    /// <param name="outpoint">txin outpoint(txid and vout).</param>
    /// <returns>transaction input index.</returns>
    public uint GetTxInIndex(OutPoint outpoint)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetConfidentialTxInIndex(
            handle.GetHandle(), tx,
            outpoint.GetTxid().ToHexString(),
            outpoint.GetVout(),
            out uint index);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return index;
      }
    }

    /// <summary>
    /// Get transaction output index.
    /// </summary>
    /// <param name="address">txout address.</param>
    /// <returns>transaction output index.</returns>
    public uint GetTxOutIndex(Address address)
    {
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetConfidentialTxOutIndex(
            handle.GetHandle(), tx,
            address.ToAddressString(),
            "",
            out uint index);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return index;
      }
    }

    /// <summary>
    /// Get transaction output index.
    /// </summary>
    /// <param name="address">txout confidential address.</param>
    /// <returns>transaction output index.</returns>
    public uint GetTxOutIndex(ConfidentialAddress address)
    {
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetConfidentialTxOutIndex(
            handle.GetHandle(), tx,
            address.ToAddressString(),
            "",
            out uint index);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return index;
      }
    }

    /// <summary>
    /// Get transaction output index.
    /// </summary>
    /// <param name="script">txout locking script. (fee is empty string)</param>
    /// <returns>transaction output index.</returns>
    public uint GetTxOutIndex(Script script)
    {
      if (script is null)
      {
        throw new ArgumentNullException(nameof(script));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetConfidentialTxOutIndex(
            handle.GetHandle(), tx,
            "",
            script.ToHexString(),
            out uint index);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return index;
      }
    }

    public ByteData GetSignatureHash(OutPoint outpoint, CfdHashType hashType,
        Pubkey pubkey, ConfidentialValue value, SignatureHashType sighashType)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return GetSignatureHash(outpoint.GetTxid(), outpoint.GetVout(), hashType, pubkey, value, sighashType);
    }

    public ByteData GetSignatureHash(Txid txid, uint vout, CfdHashType hashType,
        Pubkey pubkey, ConfidentialValue value, SignatureHashType sighashType)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateConfidentialSighash(
            handle.GetHandle(), tx, txid.ToHexString(), vout, (int)hashType,
            pubkey.ToHexString(), "",
            value.GetSatoshiValue(),
            (value.IsEmpty()) ? "" : value.ToHexString(),
            (int)sighashType.SighashType,
            sighashType.IsSighashAnyoneCanPay,
            out IntPtr sighash);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new ByteData(CCommon.ConvertToString(sighash));
      }
    }

    public ByteData GetSignatureHash(OutPoint outpoint, CfdHashType hashType,
        Script redeemScript, ConfidentialValue value, SignatureHashType sighashType)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return GetSignatureHash(outpoint.GetTxid(), outpoint.GetVout(), hashType, redeemScript, value, sighashType);
    }

    public ByteData GetSignatureHash(Txid txid, uint vout, CfdHashType hashType,
        Script redeemScript, ConfidentialValue value, SignatureHashType sighashType)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (redeemScript is null)
      {
        throw new ArgumentNullException(nameof(redeemScript));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateConfidentialSighash(
            handle.GetHandle(), tx, txid.ToHexString(), vout, (int)hashType,
            "", redeemScript.ToHexString(),
            value.GetSatoshiValue(),
            (value.IsEmpty()) ? "" : value.ToHexString(),
            (int)sighashType.SighashType,
            sighashType.IsSighashAnyoneCanPay,
            out IntPtr sighash);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new ByteData(CCommon.ConvertToString(sighash));
      }
    }

    public void AddSignWithPrivkeySimple(OutPoint outpoint, CfdHashType hashType,
        Privkey privkey, ConfidentialValue value, SignatureHashType sighashType)
    {
      AddSignWithPrivkeySimple(outpoint, hashType, privkey, value, sighashType, true);
    }

    public void AddSignWithPrivkeySimple(OutPoint outpoint, CfdHashType hashType,
        Privkey privkey, ConfidentialValue value, SignatureHashType sighashType, bool hasGrindR)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      AddSignWithPrivkeySimple(outpoint.GetTxid(), outpoint.GetVout(), hashType, privkey.GetPubkey(),
        privkey, value, sighashType, hasGrindR);
    }

    public void AddSignWithPrivkeySimple(Txid txid, uint vout, CfdHashType hashType,
    Privkey privkey, ConfidentialValue value, SignatureHashType sighashType)
    {
      AddSignWithPrivkeySimple(txid, vout, hashType, privkey, value, sighashType, true);
    }

    public void AddSignWithPrivkeySimple(Txid txid, uint vout, CfdHashType hashType,
        Privkey privkey, ConfidentialValue value, SignatureHashType sighashType, bool hasGrindR)
    {
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      AddSignWithPrivkeySimple(txid, vout, hashType, privkey.GetPubkey(), privkey, value, sighashType, hasGrindR);
    }

    public void AddSignWithPrivkeySimple(Txid txid, uint vout, CfdHashType hashType, Pubkey pubkey,
        Privkey privkey, ConfidentialValue value, SignatureHashType sighashType)
    {
      AddSignWithPrivkeySimple(txid, vout, hashType, pubkey, privkey, value, sighashType, true);
    }

    public void AddSignWithPrivkeySimple(Txid txid, uint vout, CfdHashType hashType, Pubkey pubkey,
        Privkey privkey, ConfidentialValue value, SignatureHashType sighashType, bool hasGrindR)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxSignWithPrivkeySimple(
            handle.GetHandle(), tx, txid.ToHexString(), vout, (int)hashType,
            pubkey.ToHexString(),
            (privkey.ToHexString().Length > 0) ? privkey.ToHexString() : privkey.GetWif(),
            value.GetSatoshiValue(),
            (value.IsEmpty()) ? "" : value.ToHexString(),
            (int)sighashType.SighashType,
            sighashType.IsSighashAnyoneCanPay,
            hasGrindR, out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddPubkeySign(OutPoint outpoint, CfdHashType hashType, Pubkey pubkey, SignParameter signature)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      AddPubkeySign(outpoint.GetTxid(), outpoint.GetVout(), hashType, pubkey, signature);
    }

    public void AddPubkeySign(Txid txid, uint vout, CfdHashType hashType, Pubkey pubkey, SignParameter signature)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddPubkeyHashSign(
            handle.GetHandle(), (int)CfdNetworkType.Liquidv1,
            tx, txid.ToHexString(), vout, (int)hashType,
            pubkey.ToHexString(), signature.ToHexString(),
            signature.IsDerEncode(),
            (int)signature.GetSignatureHashType().SighashType,
            signature.GetSignatureHashType().IsSighashAnyoneCanPay,
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddMultisigSign(OutPoint outpoint, CfdHashType hashType, SignParameter[] signList, Script redeemScript)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      AddMultisigSign(outpoint.GetTxid(), outpoint.GetVout(), hashType, signList, redeemScript);
    }

    public void AddMultisigSign(Txid txid, uint vout, CfdHashType hashType, SignParameter[] signList, Script redeemScript)
    {
      if (signList is null)
      {
        throw new ArgumentNullException(nameof(signList));
      }
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (redeemScript is null)
      {
        throw new ArgumentNullException(nameof(redeemScript));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeMultisigSign(
            handle.GetHandle(), out IntPtr multiSignHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          for (uint index = 0; index < signList.Length; ++index)
          {
            if (signList[index].IsDerEncode())
            {
              ret = NativeMethods.CfdAddMultisigSignDataToDer(
                handle.GetHandle(), multiSignHandle,
                signList[index].ToHexString(),
                (int)signList[index].GetSignatureHashType().SighashType,
                signList[index].GetSignatureHashType().IsSighashAnyoneCanPay,
                signList[index].GetRelatedPubkey().ToHexString());
            }
            else
            {
              ret = NativeMethods.CfdAddMultisigSignData(
                handle.GetHandle(), multiSignHandle,
                signList[index].ToHexString(),
                signList[index].GetRelatedPubkey().ToHexString());
            }
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdFinalizeMultisigSign(
              handle.GetHandle(), multiSignHandle, (int)CfdNetworkType.Liquidv1,
              tx, txid.ToHexString(), vout, (int)hashType,
              redeemScript.ToHexString(), out IntPtr txString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          tx = CCommon.ConvertToString(txString);
        }
        finally
        {
          NativeMethods.CfdFreeMultisigSignHandle(handle.GetHandle(), multiSignHandle);
        }
      }
    }

    public void AddScriptSign(OutPoint outpoint, CfdHashType hashType, SignParameter[] signList, Script redeemScript)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      AddScriptSign(outpoint.GetTxid(), outpoint.GetVout(), hashType, signList, redeemScript);
    }

    public void AddScriptSign(Txid txid, uint vout, CfdHashType hashType, SignParameter[] signList, Script redeemScript)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (signList is null)
      {
        throw new ArgumentNullException(nameof(signList));
      }
      if (redeemScript is null)
      {
        throw new ArgumentNullException(nameof(redeemScript));
      }
      using (var handle = new ErrorHandle())
      {
        CfdErrorCode ret;
        IntPtr txString;
        bool clearStack = true;
        string tempTx = tx;
        for (uint index = 0; index < signList.Length; ++index)
        {
          ret = NativeMethods.CfdAddTxSign(
              handle.GetHandle(), (int)CfdNetworkType.Liquidv1,
              tempTx, txid.ToHexString(), vout, (int)hashType,
              signList[index].ToHexString(),
              signList[index].IsDerEncode(),
              (int)signList[index].GetSignatureHashType().SighashType,
              signList[index].GetSignatureHashType().IsSighashAnyoneCanPay,
              clearStack, out txString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          tempTx = CCommon.ConvertToString(txString);
          clearStack = false;
        }

        ret = NativeMethods.CfdAddScriptHashSign(
            handle.GetHandle(), (int)CfdNetworkType.Liquidv1,
            tempTx, txid.ToHexString(), vout, (int)hashType,
            redeemScript.ToHexString(), false, out txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddSign(Txid txid, uint vout, CfdHashType hashType, SignParameter signData, bool clearStack)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (signData is null)
      {
        throw new ArgumentNullException(nameof(signData));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddTxSign(
            handle.GetHandle(), (int)CfdNetworkType.Liquidv1,
            tx, txid.ToHexString(), vout, (int)hashType,
            signData.ToHexString(), signData.IsDerEncode(),
            (int)signData.GetSignatureHashType().SighashType,
            signData.GetSignatureHashType().IsSighashAnyoneCanPay,
            clearStack, out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void VerifySign(OutPoint outpoint, Address address, CfdAddressType addressType, ConfidentialValue value)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      VerifySign(outpoint.GetTxid(), outpoint.GetVout(), address, addressType, value);
    }

    public bool VerifySign(Txid txid, uint vout, Address address, CfdAddressType addressType, ConfidentialValue value)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifyConfidentialTxSign(
            handle.GetHandle(), tx, txid.ToHexString(), vout,
            address.ToAddressString(), (int)addressType, "",
            value.GetSatoshiValue(),
            (value.IsEmpty()) ? "" : value.ToHexString());
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

    public bool VerifySignature(Txid txid, uint vout, CfdHashType hashType,
        ByteData signature, Pubkey pubkey, SignatureHashType sighashType, ConfidentialValue value)
    {
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifyConfidentialTxSignature(
            handle.GetHandle(), tx, signature.ToHexString(), pubkey.ToHexString(), "",
            txid.ToHexString(), vout,
            (int)sighashType.SighashType, sighashType.IsSighashAnyoneCanPay,
            value.GetSatoshiValue(),
            (value.IsEmpty()) ? "" : value.ToHexString(),
            (int)((hashType == CfdHashType.P2pkh) ? CfdWitnessVersion.VersionNone : CfdWitnessVersion.Version0));
        if (ret == CfdErrorCode.Success)
        {
          return true;
        }
        else if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
      }
      return false;
    }

    public bool VerifySignature(Txid txid, uint vout, CfdHashType hashType,
        ByteData signature, Script redeemScript, SignatureHashType sighashType, ConfidentialValue value)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      if (redeemScript is null)
      {
        throw new ArgumentNullException(nameof(redeemScript));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifyConfidentialTxSignature(
            handle.GetHandle(), tx, signature.ToHexString(), "", redeemScript.ToHexString(),
            txid.ToHexString(), vout,
            (int)sighashType.SighashType, sighashType.IsSighashAnyoneCanPay,
            value.GetSatoshiValue(),
            (value.IsEmpty()) ? "" : value.ToHexString(),
            (int)((hashType == CfdHashType.P2sh) ? CfdWitnessVersion.VersionNone : CfdWitnessVersion.Version0));
        if (ret == CfdErrorCode.Success)
        {
          return true;
        }
        else if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
      }
      return false;
    }

    public void EstimateFee()
    {
      // FIXME 実装する
      /*
      internal static extern CfdErrorCode CfdInitializeEstimateFee(
        [In] IntPtr handle,
        [Out] out IntPtr feeHandle,
        [In] bool isElements);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddTxInForEstimateFee(
        [In] IntPtr handle,
        [In] IntPtr feeHandle,
        [In] string txid,
        [In] uint vout,
        [In] string descriptor,
        [In] string asset,
        [In] bool isIssuance,
        [In] bool isBlindIssuance,
        [In] bool isPegin,
        [In] uint peginBtcTxSize,
        [In] string fedpegScript);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdFinalizeEstimateFee(
        [In] IntPtr handle,
        [In] IntPtr feeHandle,
        [In] string txHex,
        [In] string feeAsset,
        [Out] out long txFee,
        [Out] out long utxoFee,
        [In] bool isBlind,
        [In] double effectiveFeeRate);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeEstimateFeeHandle(
        [In] IntPtr handle,
        [In] IntPtr feeHandle);
    */
    }

    public void UpdateFee()
    {
      // FIXME
      // CfdUpdateConfidentialTxOut
    }

    public void SetRawReissueAsset()
    {
      // FIXME
      // CfdSetRawReissueAsset

    }

    public string ToHexString()
    {
      return tx;
    }

    public byte[] GetBytes()
    {
      return StringUtil.ToBytes(tx);
    }

    public Txid GetTxid()
    {
      UpdateTxInfoCache();
      return new Txid(txid);
    }

    public Txid GetWtxid()
    {
      UpdateTxInfoCache();
      return new Txid(wtxid);
    }

    public string GetWitHash()
    {
      UpdateTxInfoCache();
      return witHash;
    }

    public uint GetSize()
    {
      UpdateTxInfoCache();
      return txSize;
    }

    public uint GetVsize()
    {
      UpdateTxInfoCache();
      return txVsize;
    }

    public uint GetWeight()
    {
      UpdateTxInfoCache();
      return txWeight;
    }

    public uint GetVersion()
    {
      UpdateTxInfoCache();
      return txVersion;
    }

    public uint GetLockTime()
    {
      UpdateTxInfoCache();
      return txLocktime;
    }

    public static Privkey GetIssuanceBlindingKey(
        Privkey masterBlindingKey, Txid txid, uint vout)
    {
      if (masterBlindingKey is null)
      {
        throw new ArgumentNullException(nameof(masterBlindingKey));
      }
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetIssuanceBlindingKey(
            handle.GetHandle(),
            masterBlindingKey.ToHexString(),
            txid.ToHexString(),
            vout,
            out IntPtr blindingKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var key = CCommon.ConvertToString(blindingKey);
        return new Privkey(key);
      }
    }

    private void UpdateTxInfoCache()
    {
      if (tx == lastGetTx)
      {
        return;
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetConfidentialTxInfo(
            handle.GetHandle(),
            tx,
            out IntPtr outputTxid,
            out IntPtr outputWtxid,
            out IntPtr outputWitHash,
            out uint outputSize,
            out uint outputVsize,
            out uint outputWeight,
            out uint outputVersion,
            out uint outputLocktime);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        txid = CCommon.ConvertToString(outputTxid);
        wtxid = CCommon.ConvertToString(outputWtxid);
        witHash = CCommon.ConvertToString(outputWitHash);
        txSize = outputSize;
        txVsize = outputVsize;
        txWeight = outputWeight;
        txVersion = outputVersion;
        txLocktime = outputLocktime;
        lastGetTx = tx;
      }
    }

    private ConfidentialTxIn GetInputByIndex(ErrorHandle handle, uint index)
    {
      var ret = NativeMethods.CfdGetConfidentialTxIn(
          handle.GetHandle(), tx, index,
          out IntPtr outTxid,
          out uint vout,
          out uint sequence,
          out IntPtr outScriptSig);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      var utxoTxid = CCommon.ConvertToString(outTxid);
      var scriptSig = CCommon.ConvertToString(outScriptSig);

      ret = NativeMethods.CfdGetConfidentialTxInWitnessCount(
          handle.GetHandle(), tx, index,
          out uint witnessCount);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      string[] witnessArray = new string[witnessCount];

      for (uint witnessIndex = 0; witnessIndex < witnessCount; ++witnessIndex)
      {
#pragma warning disable IDE0059 // 値の不必要な代入
        IntPtr stackData = IntPtr.Zero;
#pragma warning restore IDE0059 // 値の不必要な代入
        ret = NativeMethods.CfdGetConfidentialTxInWitness(
            handle.GetHandle(), tx, index, witnessIndex,
            out stackData);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        witnessArray[witnessIndex] = CCommon.ConvertToString(stackData);
      }

      ret = NativeMethods.CfdGetTxInIssuanceInfo(
          handle.GetHandle(), tx, index,
          out IntPtr outEntropy,
          out IntPtr outNonce,
          out long assetAmount,
          out IntPtr outAssetValue,
          out long tokenAmount,
          out IntPtr outTokenValue,
          out IntPtr outAssetRangeproof,
          out IntPtr outTokenRangeproof);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      var entropy = CCommon.ConvertToString(outEntropy);
      var nonce = CCommon.ConvertToString(outNonce);
      var assetValue = CCommon.ConvertToString(outAssetValue);
      var tokenValue = CCommon.ConvertToString(outTokenValue);
      var assetRangeproof = CCommon.ConvertToString(outAssetRangeproof);
      var tokenRangeproof = CCommon.ConvertToString(outTokenRangeproof);

      var entropyBytes = StringUtil.ToBytes(entropy);
      entropyBytes = CfdCommon.ReverseBytes(entropyBytes);
      var nonceBytes = StringUtil.ToBytes(nonce);
      nonceBytes = CfdCommon.ReverseBytes(nonceBytes);

      ret = NativeMethods.CfdGetConfidentialTxInPeginWitnessCount(
          handle.GetHandle(), tx, index,
          out uint peginCount);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      string[] peginWitness = new string[peginCount];

      for (uint witnessIndex = 0; witnessIndex < peginCount; ++witnessIndex)
      {
#pragma warning disable IDE0059 // 値の不必要な代入
        IntPtr stackData = IntPtr.Zero;
#pragma warning restore IDE0059 // 値の不必要な代入
        ret = NativeMethods.CfdGetConfidentialTxInPeginWitness(
            handle.GetHandle(), tx, index, witnessIndex,
            out stackData);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        peginWitness[witnessIndex] = CCommon.ConvertToString(stackData);
      }

      return new ConfidentialTxIn(
          new OutPoint(utxoTxid, vout), sequence, new Script(scriptSig),
          new ScriptWitness(witnessArray), new ScriptWitness(peginWitness),
          new IssuanceData(nonceBytes, entropyBytes,
              new ConfidentialValue(assetValue, assetAmount),
              new ConfidentialValue(tokenValue, tokenAmount),
              StringUtil.ToBytes(assetRangeproof),
              StringUtil.ToBytes(tokenRangeproof)));
    }

    private uint GetInputCount(ErrorHandle handle)
    {
      var ret = NativeMethods.CfdGetConfidentialTxInCount(
          handle.GetHandle(), tx, out uint count);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      return count;
    }

    private ConfidentialTxOut GetOutputByIndex(ErrorHandle handle, uint index)
    {
      var ret = NativeMethods.CfdGetConfidentialTxOut(
          handle.GetHandle(), tx, index,
          out IntPtr assetString,
          out long satoshi,
          out IntPtr valueCommitment,
          out IntPtr outNonce,
          out IntPtr lockingScript,
          out IntPtr outSurjectionProof,
          out IntPtr outRangeproof);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      var asset = CCommon.ConvertToString(assetString);
      var nonce = CCommon.ConvertToString(outNonce);
      var value = CCommon.ConvertToString(valueCommitment);
      var scriptPubkey = CCommon.ConvertToString(lockingScript);
      var surjectionProof = CCommon.ConvertToString(outSurjectionProof);
      var rangeProof = CCommon.ConvertToString(outRangeproof);

      return new ConfidentialTxOut(
          new ConfidentialAsset(asset), new ConfidentialValue(value, satoshi),
          new Script(scriptPubkey),
          StringUtil.ToBytes(nonce),
          StringUtil.ToBytes(surjectionProof),
          StringUtil.ToBytes(rangeProof));
    }

    private uint GetOutputCount(ErrorHandle handle)
    {
      var ret = NativeMethods.CfdGetConfidentialTxOutCount(
          handle.GetHandle(), tx, out uint count);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      return count;
    }

  }
}
