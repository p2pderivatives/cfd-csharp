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
      MinimumRangeValue = minimumRangeValue;
      Exponent = exponent;
      MinimumBits = minimumBits;
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
        return Equals((CfdBlindOptionData)obj);
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
      Asset = asset;
      SatoshiValue = satoshiValue;
      AssetBlindFactor = assetBlindFactor;
      AmountBlindFactor = amountBlindFactor;
    }

    /// <summary>
    /// Constructor. (use unblinded utxo)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    public AssetValueData(string asset, long satoshiValue)
    {
      Asset = asset;
      SatoshiValue = satoshiValue;
      AssetBlindFactor = new BlindFactor();
      AmountBlindFactor = new BlindFactor();
    }

    /// <summary>
    /// Constructor. (use issue/reissue response only)
    /// </summary>
    /// <param name="asset">asset</param>
    /// <param name="satoshiValue">satoshi amount</param>
    /// <param name="amountBlindFactor">amount blinder</param>
    public AssetValueData(string asset, long satoshiValue, BlindFactor amountBlindFactor)
    {
      Asset = asset;
      SatoshiValue = satoshiValue;
      AssetBlindFactor = new BlindFactor();
      AmountBlindFactor = amountBlindFactor;
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
        return Equals((AssetValueData)obj);
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
        return Equals((UnblindIssuanceData)obj);
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
      AssetKey = assetKey;
      TokenKey = tokenKey;
    }

    /// <summary>
    /// Constructor. (use reissueasset)
    /// </summary>
    /// <param name="assetKey">asset blinding key</param>
    public IssuanceKeys(Privkey assetKey)
    {
      AssetKey = assetKey;
      TokenKey = new Privkey();
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
        return Equals((IssuanceKeys)obj);
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
      BlindingNonce = new ByteData(blindingNonce);
      AssetEntropy = new ByteData(assetEntropy);
      IssuanceAmount = issuanceAmount;
      InflationKeys = tokenAmount;
      IssuanceAmountRangeproof = new ByteData(issuanceRangeproof);
      InflationKeysRangeproof = new ByteData(tokenRangeproof);
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
        return Equals((IssuanceData)obj);
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
    public uint Sequence { get; }
    public ScriptWitness WitnessStack { get; }
    public ScriptWitness PeginWitness { get; }
    public IssuanceData Issuance { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    public ConfidentialTxIn(OutPoint outPoint)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = CfdSequenceLockTime.Disable;
      WitnessStack = new ScriptWitness();
      PeginWitness = new ScriptWitness();
      Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, uint sequence)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = sequence;
      WitnessStack = new ScriptWitness();
      PeginWitness = new ScriptWitness();
      Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, ScriptWitness scriptWitness)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = CfdSequenceLockTime.Disable;
      WitnessStack = scriptWitness;
      PeginWitness = new ScriptWitness();
      Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, uint sequence, ScriptWitness scriptWitness)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = sequence;
      WitnessStack = scriptWitness;
      PeginWitness = new ScriptWitness();
      Issuance = new IssuanceData(Array.Empty<byte>(), Array.Empty<byte>(),
          new ConfidentialValue(), new ConfidentialValue(),
          Array.Empty<byte>(), Array.Empty<byte>());
    }

    public ConfidentialTxIn(OutPoint outPoint, Script scriptSig, ScriptWitness witnessStack, ScriptWitness peginWitness, IssuanceData issuance)
    {
      OutPoint = outPoint;
      ScriptSig = scriptSig;
      Sequence = CfdSequenceLockTime.Disable;
      WitnessStack = witnessStack;
      PeginWitness = peginWitness;
      Issuance = issuance;
    }
    public ConfidentialTxIn(OutPoint outPoint, uint sequence, Script scriptSig, ScriptWitness witnessStack, ScriptWitness peginWitness, IssuanceData issuance)
    {
      OutPoint = outPoint;
      ScriptSig = scriptSig;
      Sequence = sequence;
      WitnessStack = witnessStack;
      PeginWitness = peginWitness;
      Issuance = issuance;
    }

    public bool Equals(ConfidentialTxIn other)
    {
      return OutPoint.Equals(other.OutPoint);
    }

    public override bool Equals(object obj)
    {
      if (obj is ConfidentialTxIn)
      {
        return Equals((ConfidentialTxIn)obj);
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
      Asset = asset;
      Value = new ConfidentialValue(value);
      ScriptPubkey = new Script();
      Nonce = new ByteData(Array.Empty<byte>());
      SurjectionProof = new ByteData(Array.Empty<byte>());
      RangeProof = new ByteData(Array.Empty<byte>());
    }

    public ConfidentialTxOut(ConfidentialAsset asset, ConfidentialValue value, Script scriptPubkey)
    {
      Asset = asset;
      Value = value;
      ScriptPubkey = scriptPubkey;
      Nonce = new ByteData(Array.Empty<byte>());
      SurjectionProof = new ByteData(Array.Empty<byte>());
      RangeProof = new ByteData(Array.Empty<byte>());
    }

    public ConfidentialTxOut(ConfidentialAsset asset, ConfidentialValue value,
        Script scriptPubkey, byte[] nonce, byte[] surjectionProof, byte[] rangeProof)
    {
      Asset = asset;
      Value = value;
      ScriptPubkey = scriptPubkey;
      Nonce = new ByteData(nonce);
      SurjectionProof = new ByteData(surjectionProof);
      RangeProof = new ByteData(rangeProof);
    }

    public bool Equals(ConfidentialTxOut other)
    {
      return ScriptPubkey.Equals(other.ScriptPubkey);
    }

    public override bool Equals(object obj)
    {
      if (obj is ConfidentialTxOut)
      {
        return Equals((ConfidentialTxOut)obj);
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
    public static readonly int defaultMinimumBits = 52;
    public static readonly double defaultFeeRate = 0.15;
    public static readonly int defaultNetType = (int)CfdNetworkType.Liquidv1;
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
    private long lastTxFee;

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
      tx = CreateTransaction(version, locktime, "", null, null);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="version">transaction version</param>
    /// <param name="locktime">transaction locktime</param>
    /// <param name="txinList">transaction input array</param>
    /// <param name="txoutList">transaction output array</param>
    public ConfidentialTransaction(uint version, uint locktime, ConfidentialTxIn[] txinList, ConfidentialTxOut[] txoutList)
    {
      tx = CreateTransaction(version, locktime, "", txinList, txoutList);
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
    /// Constructor.
    /// </summary>
    /// <param name="txHex">transaction hex</param>
    /// <param name="txinList">transaction input array</param>
    /// <param name="txoutList">transaction output array</param>
    public ConfidentialTransaction(string txHex, ConfidentialTxIn[] txinList, ConfidentialTxOut[] txoutList)
    {
      tx = CreateTransaction(0, 0, txHex, txinList, txoutList);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="txBytes">transaction byte array</param>
    /// <param name="txinList">transaction input array</param>
    /// <param name="txoutList">transaction output array</param>
    public ConfidentialTransaction(byte[] txBytes, ConfidentialTxIn[] txinList, ConfidentialTxOut[] txoutList)
    {
      tx = CreateTransaction(0, 0, StringUtil.FromBytes(txBytes), txinList, txoutList);
    }

    /// <summary>
    /// Create Transaction.
    /// </summary>
    private static string CreateTransaction(uint version, uint locktime, string txHex,
      ConfidentialTxIn[] txinList, ConfidentialTxOut[] txoutList)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeTransaction(
          handle.GetHandle(), defaultNetType,
          version, locktime, txHex, out IntPtr txHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          if ((!(txinList is null)) && (txinList.Length > 0))
          {
            foreach (var txin in txinList)
            {
              ret = NativeMethods.CfdAddTransactionInput(
                handle.GetHandle(), txHandle, txin.OutPoint.GetTxid().ToHexString(),
                txin.OutPoint.GetVout(), txin.Sequence);
              if (ret != CfdErrorCode.Success)
              {
                handle.ThrowError(ret);
              }
            }
          }
          if ((!(txoutList is null)) && (txoutList.Length > 0))
          {
            foreach (var txout in txoutList)
            {
              ret = NativeMethods.CfdAddTransactionOutput(
                handle.GetHandle(), txHandle, txout.Value.GetSatoshiValue(),
                "", txout.ScriptPubkey.ToHexString(), txout.Asset.ToHexString());
              if (ret != CfdErrorCode.Success)
              {
                handle.ThrowError(ret);
              }
            }
          }
          ret = NativeMethods.CfdFinalizeTransaction(
            handle.GetHandle(), txHandle, out IntPtr txString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          return CCommon.ConvertToString(txString);
        }
        finally
        {
          NativeMethods.CfdFreeTransactionHandle(handle.GetHandle(), txHandle);
        }
      }
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
      AddTxIn(new OutPoint(txid, vout), sequence);
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
      AddTxInList(new[] {
        new ConfidentialTxIn(outpoint, sequence),
      });
    }

    public void AddTxInList(ConfidentialTxIn[] txinList)
    {
      tx = CreateTransaction(0, 0, tx, txinList, null);
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="satoshiValue">txout satoshi value.</param>
    /// <param name="address">txout address.</param>
    public void AddTxOut(string asset, long satoshiValue, Address address)
    {
      AddTxOut(new ConfidentialAsset(asset), new ConfidentialValue(satoshiValue), address, null);
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="satoshiValue">txout satoshi value.</param>
    /// <param name="address">txout confidential address.</param>
    public void AddTxOut(string asset, long satoshiValue, ConfidentialAddress address)
    {
      AddTxOut(new ConfidentialAsset(asset), new ConfidentialValue(satoshiValue), address);
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="satoshiValue">txout satoshi value.</param>
    /// <param name="address">txout address.</param>
    public void AddTxOut(ConfidentialAsset asset, long satoshiValue, Address address)
    {
      AddTxOut(asset, new ConfidentialValue(satoshiValue), address, null);
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="valueCommitment">txout commitment value.</param>
    /// <param name="address">txout address.</param>
    public void AddTxOut(string asset, ConfidentialValue valueCommitment, Address address)
    {
      AddTxOut(new ConfidentialAsset(asset), valueCommitment, address, null);
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="valueCommitment">txout commitment value.</param>
    /// <param name="address">txout address.</param>
    public void AddTxOut(ConfidentialAsset asset, ConfidentialValue valueCommitment, Address address)
    {
      AddTxOut(asset, valueCommitment, address, null);
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="valueCommitment">txout commitment value.</param>
    /// <param name="address">txout confidential address.</param>
    public void AddTxOut(ConfidentialAsset asset, ConfidentialValue valueCommitment, ConfidentialAddress address)
    {
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      AddTxOut(asset, valueCommitment, address.GetAddress(), address.GetConfidentialKey().GetData());
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="asset">txout asset.</param>
    /// <param name="valueCommitment">txout commitment value.</param>
    /// <param name="address">txout address.</param>
    /// <param name="nonce">txout nonce.</param>
    public void AddTxOut(ConfidentialAsset asset, ConfidentialValue valueCommitment, Address address, ByteData nonce)
    {
      if (valueCommitment is null)
      {
        throw new ArgumentNullException(nameof(valueCommitment));
      }
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddConfidentialTxOut(
            handle.GetHandle(), tx,
            asset.ToHexString(),
            (valueCommitment.HasBlinding()) ? 0 : valueCommitment.GetSatoshiValue(),
            (valueCommitment.HasBlinding()) ? valueCommitment.ToHexString() : "",
            address.ToAddressString(),
            "",
            (nonce is null) ? "" : nonce.ToHexString(),
            out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public void AddFeeTxOut(string asset, long satoshiValue)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      AddTxOutList(new[] { new ConfidentialTxOut(new ConfidentialAsset(asset), satoshiValue) });
    }

    public void AddDestroyAmountTxOut(string asset, long satoshiValue)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      AddTxOutList(new[] { new ConfidentialTxOut(new ConfidentialAsset(asset),
        new ConfidentialValue(satoshiValue), new Script("6a")) });
    }

    public void AddTxOutList(ConfidentialTxOut[] txoutList)
    {
      tx = CreateTransaction(0, 0, tx, null, txoutList);
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

          ret = NativeMethods.CfdSetBlindTxOption(
            handle.GetHandle(), blindHandle, (int)CfdBlindOption.MinimumBits,
            defaultMinimumBits);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
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

    public void BlindTransaction(ElementsUtxoData[] utxos,
        IDictionary<OutPoint, IssuanceKeys> issuanceKeys,
        ConfidentialAddress[] confidentialAddresses)
    {
      BlindTransaction(utxos, issuanceKeys, confidentialAddresses,
        new CfdBlindOptionData(1, 0, defaultMinimumBits));
    }

    public void BlindTransaction(ElementsUtxoData[] utxos,
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

          foreach (var utxo in utxos)
          {
            var outpoint = utxo.GetOutPoint();
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
              utxo.GetAsset(),
              utxo.GetAssetBlindFactor().ToHexString(),
              utxo.GetAmountBlindFactor().ToHexString(),
              utxo.GetAmount(),
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

    /// <summary>
    /// Get transaction output fee index.
    /// </summary>
    /// <returns>transaction output index.</returns>
    public uint GetTxOutFeeIndex()
    {
      return GetTxOutIndex(new Script(""));
    }

    public void UpdateTxOutAmount(uint index, long value)
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdUpdateTxOutAmount(
            handle.GetHandle(), defaultNetType, tx, index, value,
            out IntPtr txHex);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txHex);
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
            (value.HasBlinding()) ? value.ToHexString() : "",
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
            (value.HasBlinding()) ? value.ToHexString() : "",
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
            (value.HasBlinding()) ? value.ToHexString() : "",
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
            handle.GetHandle(), defaultNetType,
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
              handle.GetHandle(), multiSignHandle, defaultNetType,
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
              handle.GetHandle(), defaultNetType,
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
            handle.GetHandle(), defaultNetType,
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
            handle.GetHandle(), defaultNetType,
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

    public bool VerifySign(OutPoint outpoint, Address address, CfdAddressType addressType, ConfidentialValue value)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return VerifySign(outpoint.GetTxid(), outpoint.GetVout(), address, addressType, value);
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
            (value.HasBlinding()) ? value.ToHexString() : "");
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
      ByteData sig = signature;
      if (signature.GetSize() > 65)
      {
        // decode der
        sig = SignParameter.DecodeFromDer(sig).GetData();
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifyConfidentialTxSignature(
            handle.GetHandle(), tx, sig.ToHexString(), pubkey.ToHexString(), "",
            txid.ToHexString(), vout,
            (int)sighashType.SighashType, sighashType.IsSighashAnyoneCanPay,
            value.GetSatoshiValue(),
            (value.HasBlinding()) ? value.ToHexString() : "",
            (int)((hashType == CfdHashType.P2pkh) ? CfdWitnessVersion.VersionNone : CfdWitnessVersion.Version0));
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

    public bool VerifySignature(Txid txid, uint vout, CfdHashType hashType, ByteData signature,
      Pubkey pubkey, Script redeemScript, SignatureHashType sighashType, ConfidentialValue value)
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
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      if (redeemScript is null)
      {
        throw new ArgumentNullException(nameof(redeemScript));
      }
      ByteData sig = signature;
      if (signature.GetSize() > 65)
      {
        // decode der
        sig = SignParameter.DecodeFromDer(sig).GetData();
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifyConfidentialTxSignature(
            handle.GetHandle(), tx, sig.ToHexString(),
            pubkey.ToHexString(), redeemScript.ToHexString(),
            txid.ToHexString(), vout,
            (int)sighashType.SighashType, sighashType.IsSighashAnyoneCanPay,
            value.GetSatoshiValue(),
            (value.HasBlinding()) ? value.ToHexString() : "",
            (int)((hashType == CfdHashType.P2sh) ? CfdWitnessVersion.VersionNone : CfdWitnessVersion.Version0));
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

    public FeeData EstimateFee(ElementsUtxoData[] txinList,
      ConfidentialAsset feeAsset)
    {
      return EstimateFee(txinList, defaultFeeRate, feeAsset, true);
    }

    public FeeData EstimateFee(ElementsUtxoData[] txinList, double feeRate,
      ConfidentialAsset feeAsset)
    {
      return EstimateFee(txinList, feeRate, feeAsset, true);
    }

    public FeeData EstimateFee(ElementsUtxoData[] txinList, double feeRate,
      ConfidentialAsset feeAsset, bool isBlind)
    {
      return EstimateFee(txinList, feeRate, feeAsset, isBlind, 0, defaultMinimumBits);
    }

    public FeeData EstimateFee(ElementsUtxoData[] txinList, double feeRate,
      ConfidentialAsset feeAsset, bool isBlind, int exponent, int minimumBits)
    {
      if (txinList is null)
      {
        throw new ArgumentNullException(nameof(txinList));
      }
      if (feeAsset is null)
      {
        throw new ArgumentNullException(nameof(feeAsset));
      }
      if (feeAsset.HasBlinding())
      {
        throw new InvalidOperationException(
          "fee asset has blinding. fee asset is unblind only.");
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeEstimateFee(
          handle.GetHandle(), out IntPtr feeHandle, true);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          foreach (ElementsUtxoData txin in txinList)
          {
            ret = NativeMethods.CfdAddTxInTemplateForEstimateFee(
              handle.GetHandle(), feeHandle, txin.GetOutPoint().GetTxid().ToHexString(),
              txin.GetOutPoint().GetVout(), txin.GetDescriptor().ToString(),
              txin.GetAsset(), txin.IsIssuance(), txin.IsBlindIssuance(),
              txin.IsPegin(), txin.GetPeginBtcTxSize(),
              (txin.GetFedpegScript() is null) ? "" : txin.GetFedpegScript().ToHexString(),
              txin.GetScriptSigTemplate().ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          if (exponent >= -1)
          {
            ret = NativeMethods.CfdSetOptionEstimateFee(
              handle.GetHandle(), feeHandle,
              CfdEstimateFeeOption.Exponent, exponent, 0, false);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }
          if (minimumBits >= 0)
          {
            ret = NativeMethods.CfdSetOptionEstimateFee(
              handle.GetHandle(), feeHandle,
              CfdEstimateFeeOption.MinimumBits, minimumBits, 0, false);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }
          ret = NativeMethods.CfdFinalizeEstimateFee(
              handle.GetHandle(), feeHandle, tx, feeAsset.ToHexString(),
              out long txFee, out long utxoFee, isBlind, feeRate);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          return new FeeData(txFee, utxoFee);
        }
        finally
        {
          NativeMethods.CfdFreeEstimateFeeHandle(handle.GetHandle(), feeHandle);
        }
      }
    }

    public void UpdateFee(long feeAmount, ConfidentialAsset feeAsset)
    {
      if (feeAsset is null)
      {
        throw new ArgumentNullException(nameof(feeAsset));
      }
      if (feeAsset.HasBlinding())
      {
        throw new InvalidOperationException(
          "fee asset has blinding. fee asset is unblind only.");
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetConfidentialTxOutIndex(
          handle.GetHandle(), tx, "", "", out uint index);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        ret = NativeMethods.CfdUpdateConfidentialTxOut(
          handle.GetHandle(), tx, index, feeAsset.ToHexString(),
          feeAmount, "", "", "", "", out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        tx = CCommon.ConvertToString(txString);
      }
    }

    public ConfidentialAsset SetRawReissueAsset(OutPoint outpoint, long assetAmount,
      ByteData blindingNonce, ByteData entropy, Address address)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return SetRawReissueAsset(outpoint.GetTxid(), outpoint.GetVout(),
        assetAmount, blindingNonce, entropy, address);
    }

    public ConfidentialAsset SetRawReissueAsset(Txid txid, uint vout, long assetAmount,
      ByteData blindingNonce, ByteData entropy, Address address)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (blindingNonce is null)
      {
        throw new ArgumentNullException(nameof(blindingNonce));
      }
      if (entropy is null)
      {
        throw new ArgumentNullException(nameof(entropy));
      }
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdSetRawReissueAsset(
          handle.GetHandle(), tx, txid.ToHexString(), vout, assetAmount,
          blindingNonce.ToHexString(), entropy.ToHexString(), address.ToAddressString(),
          "", out IntPtr assetString, out IntPtr txString);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var asset = CCommon.ConvertToString(assetString);
        tx = CCommon.ConvertToString(txString);
        return new ConfidentialAsset(asset);
      }
    }

    public string[] FundRawTransaction(
      ElementsUtxoData[] txinList, ElementsUtxoData[] utxoList,
      IDictionary<ConfidentialAsset, long> targetAssetAmountMap,
      IDictionary<ConfidentialAsset, string> reservedAddressMap,
      ConfidentialAsset feeAsset, double effectiveFeeRate)
    {
      return FundRawTransaction(txinList, utxoList, targetAssetAmountMap,
        reservedAddressMap, feeAsset, true, effectiveFeeRate,
        0, defaultMinimumBits, effectiveFeeRate, -1, -1);
    }

    public string[] FundRawTransaction(
      ElementsUtxoData[] txinList, ElementsUtxoData[] utxoList,
      IDictionary<ConfidentialAsset, long> targetAssetAmountMap,
      IDictionary<ConfidentialAsset, string> reservedAddressMap,
      ConfidentialAsset feeAsset, double effectiveFeeRate, int exponent, int minimumBits)
    {
      return FundRawTransaction(txinList, utxoList, targetAssetAmountMap,
        reservedAddressMap, feeAsset, true, effectiveFeeRate,
        exponent, minimumBits, effectiveFeeRate, -1, -1);
    }

    public string[] FundRawTransaction(
      ElementsUtxoData[] txinList, ElementsUtxoData[] utxoList,
      IDictionary<ConfidentialAsset, long> targetAssetAmountMap,
      IDictionary<ConfidentialAsset, string> reservedAddressMap,
      ConfidentialAsset feeAsset,
      bool isBlind, double effectiveFeeRate, int exponent, int minimumBits,
      double longTermFeeRate, long dustFeeRate, long knapsackMinChange)
    {
      if (utxoList is null)
      {
        throw new ArgumentNullException(nameof(utxoList));
      }
      if (targetAssetAmountMap is null)
      {
        throw new ArgumentNullException(nameof(targetAssetAmountMap));
      }
      if (reservedAddressMap is null)
      {
        throw new ArgumentNullException(nameof(reservedAddressMap));
      }
      if (feeAsset is null)
      {
        throw new ArgumentNullException(nameof(feeAsset));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeFundRawTx(
          handle.GetHandle(), defaultNetType, (uint)targetAssetAmountMap.Keys.Count,
          feeAsset.ToHexString(), out IntPtr fundHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          if (txinList != null)
          {
            foreach (var txin in txinList)
            {
              ret = NativeMethods.CfdAddTxInTemplateForFundRawTx(
                handle.GetHandle(), fundHandle,
                txin.GetOutPoint().GetTxid().ToHexString(),
                txin.GetOutPoint().GetVout(),
                txin.GetAmount(), txin.GetDescriptor().ToString(),
                txin.GetAsset(), txin.IsIssuance(), txin.IsBlindIssuance(),
                txin.IsPegin(), txin.GetPeginBtcTxSize(),
                (txin.GetFedpegScript() is null) ? "" : txin.GetFedpegScript().ToHexString(),
                txin.GetScriptSigTemplate().ToHexString());
              if (ret != CfdErrorCode.Success)
              {
                handle.ThrowError(ret);
              }
            }
          }

          foreach (var utxo in utxoList)
          {
            ret = NativeMethods.CfdAddUtxoTemplateForFundRawTx(
              handle.GetHandle(), fundHandle,
              utxo.GetOutPoint().GetTxid().ToHexString(),
              utxo.GetOutPoint().GetVout(),
              utxo.GetAmount(), utxo.GetDescriptor().ToString(),
              utxo.GetAsset(),
              utxo.GetScriptSigTemplate().ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          uint assetIndex = 0;
          foreach (var key in targetAssetAmountMap.Keys)
          {
            string reservedAddress = (reservedAddressMap.ContainsKey(key)) ?
              reservedAddressMap[key] : "";
            ret = NativeMethods.CfdAddTargetAmountForFundRawTx(
              handle.GetHandle(), fundHandle, assetIndex,
              targetAssetAmountMap[key], key.ToHexString(),
              reservedAddress);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
            ++assetIndex;
          }

          ret = NativeMethods.CfdSetOptionFundRawTx(
            handle.GetHandle(), fundHandle,
            CfdFundTxOption.UseBlind, 0, 0, isBlind);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          ret = NativeMethods.CfdSetOptionFundRawTx(
            handle.GetHandle(), fundHandle,
            CfdFundTxOption.DustFeeRate, 0, dustFeeRate, false);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          ret = NativeMethods.CfdSetOptionFundRawTx(
            handle.GetHandle(), fundHandle,
            CfdFundTxOption.LongTermFeeRate, 0, longTermFeeRate, false);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          ret = NativeMethods.CfdSetOptionFundRawTx(
            handle.GetHandle(), fundHandle,
            CfdFundTxOption.KnapsackMinChange, knapsackMinChange, 0, false);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          if (exponent >= -1)
          {
            ret = NativeMethods.CfdSetOptionFundRawTx(
              handle.GetHandle(), fundHandle,
              CfdFundTxOption.Exponent, exponent, 0, false);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }
          if (minimumBits >= 0)
          {
            ret = NativeMethods.CfdSetOptionFundRawTx(
              handle.GetHandle(), fundHandle,
              CfdFundTxOption.MinimumBits, minimumBits, 0, false);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdFinalizeFundRawTx(
            handle.GetHandle(), fundHandle, tx, effectiveFeeRate,
            out long txFee, out uint appendTxOutCount, out IntPtr outputTxHex);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          string fundTx = CCommon.ConvertToString(outputTxHex);
          string[] usedReserveAddressList = new string[appendTxOutCount];
          for (uint index = 0; index < appendTxOutCount; ++index)
          {
            ret = NativeMethods.CfdGetAppendTxOutFundRawTx(
              handle.GetHandle(), fundHandle, index, out IntPtr appendAddress);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
            usedReserveAddressList[index] = CCommon.ConvertToString(appendAddress);
          }

          tx = fundTx;
          lastTxFee = txFee;
          return usedReserveAddressList;
        }
        finally
        {
          NativeMethods.CfdFreeFundRawTxHandle(handle.GetHandle(), fundHandle);
        }
      }
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

    public long GetLastTxFee()
    {
      return lastTxFee;
    }

    public static Privkey GetIssuanceBlindingKey(
        Privkey masterBlindingKey, OutPoint outpoint)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return GetIssuanceBlindingKey(masterBlindingKey, outpoint.GetTxid(), outpoint.GetVout());
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

    public static Privkey GetDefaultBlindingKey(
        Privkey masterBlindingKey, Address address)
    {
      if (masterBlindingKey is null)
      {
        throw new ArgumentNullException(nameof(masterBlindingKey));
      }
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetAddressInfo(
           handle.GetHandle(), address.ToAddressString(), out _, out _, out _,
           out IntPtr lockingScript, out IntPtr hash);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(hash);
        var lockingScriptString = CCommon.ConvertToString(lockingScript);

        ret = NativeMethods.CfdGetDefaultBlindingKey(
            handle.GetHandle(),
            masterBlindingKey.ToHexString(),
            lockingScriptString,
            out IntPtr blindingKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var key = CCommon.ConvertToString(blindingKey);
        return new Privkey(key);
      }
    }

    public static Privkey GetDefaultBlindingKey(
        Privkey masterBlindingKey, Script lockingScript)
    {
      if (masterBlindingKey is null)
      {
        throw new ArgumentNullException(nameof(masterBlindingKey));
      }
      if (lockingScript is null)
      {
        throw new ArgumentNullException(nameof(lockingScript));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetDefaultBlindingKey(
            handle.GetHandle(),
            masterBlindingKey.ToHexString(),
            lockingScript.ToHexString(),
            out IntPtr blindingKey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var key = CCommon.ConvertToString(blindingKey);
        return new Privkey(key);
      }
    }

    public static ConfidentialAsset GetAssetCommitment(
        ConfidentialAsset asset, BlindFactor assetBlindFactor)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if (assetBlindFactor is null)
      {
        throw new ArgumentNullException(nameof(assetBlindFactor));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetAssetCommitment(
            handle.GetHandle(),
            asset.ToHexString(),
            assetBlindFactor.ToHexString(),
            out IntPtr assetCommitment);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var commitment = CCommon.ConvertToString(assetCommitment);
        return new ConfidentialAsset(commitment);
      }
    }

    public static ConfidentialValue GetValueCommitment(
        long amount, ConfidentialAsset assetCommitment,
        BlindFactor valuetBlindFactor)
    {
      if (assetCommitment is null)
      {
        throw new ArgumentNullException(nameof(assetCommitment));
      }
      if (valuetBlindFactor is null)
      {
        throw new ArgumentNullException(nameof(valuetBlindFactor));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetValueCommitment(
            handle.GetHandle(),
            amount,
            assetCommitment.ToHexString(),
            valuetBlindFactor.ToHexString(),
            out IntPtr valueCommitment);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var commitment = CCommon.ConvertToString(valueCommitment);
        return new ConfidentialValue(commitment);
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
#pragma warning disable IDE0059 // Unnecessary value assignment
        IntPtr stackData = IntPtr.Zero;
#pragma warning restore IDE0059 // Unnecessary value assignment
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
#pragma warning disable IDE0059 // Unnecessary value assignment
        IntPtr stackData = IntPtr.Zero;
#pragma warning restore IDE0059 // Unnecessary value assignment
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
