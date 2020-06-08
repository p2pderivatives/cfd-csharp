using System;

namespace Cfd
{
  /// <summary>
  /// txin sequence locktime
  /// </summary>
  public static class CfdSequenceLockTime
  {
    /// disable locktime
    public static readonly uint Disable = 0xffffffff;
    /// enable locktime (maximum time)
    public static readonly uint EnableMax = 0xfffffffe;
  };

  public static class CfdFundTxOption
  {
    /// use blind fee (bool)
    public static readonly int UseBlind = 1;
    /// dust fee rate (double)
    public static readonly int DustFeeRate = 2;
    /// longterm fee rate (double)
    public static readonly int LongTermFeeRate = 3;
    /// knapsack min change (int64)
    public static readonly int KnapsackMinChange = 4;
    /// blind exponent (elements only) (int64)
    public static readonly int Exponent = 5;
    /// blind minimum bits (elements only) (int64)
    public static readonly int MinimumBits = 6;
  };

  public static class CfdEstimateFeeOption
  {
    /// blind exponent (elements only) (int64)
    public static readonly int Exponent = 1;
    /// blind minimum bits (elements only) (int64)
    public static readonly int MinimumBits = 2;
  };

  public struct FeeData : IEquatable<FeeData>
  {
    public long TxFee { get; }
    public long InputFee { get; }

    public FeeData(long txFee, long inputFee)
    {
      TxFee = txFee;
      InputFee = inputFee;
    }

    public bool Equals(FeeData other)
    {
      return (TxFee == other.TxFee) && (InputFee == other.InputFee);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is FeeData)
      {
        return Equals((FeeData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return TxFee.GetHashCode() + InputFee.GetHashCode();
    }

    public static bool operator ==(FeeData left, FeeData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(FeeData left, FeeData right)
    {
      return !(left == right);
    }
  };

  /// <summary>
  /// Transaction signature hash type.
  /// </summary>
  public struct SignatureHashType : IEquatable<SignatureHashType>
  {
    /// <summary>
    /// signature hash type.
    /// </summary>
    public CfdSighashType SighashType { get; }
    /// <summary>
    /// use signature hash anyone can pay.
    /// </summary>
    public bool IsSighashAnyoneCanPay { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="sighashType">sighash type.</param>
    /// <param name="sighashAnyoneCanPay">sighash anyone can pay.</param>
    public SignatureHashType(CfdSighashType sighashType, bool sighashAnyoneCanPay)
    {
      SighashType = sighashType;
      IsSighashAnyoneCanPay = sighashAnyoneCanPay;
    }

    public bool Equals(SignatureHashType other)
    {
      return SighashType.Equals(other.SighashType) &&
          (IsSighashAnyoneCanPay == other.IsSighashAnyoneCanPay);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is SignatureHashType)
      {
        return Equals((SignatureHashType)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return SighashType.GetHashCode() + IsSighashAnyoneCanPay.GetHashCode();
    }

    public static bool operator ==(SignatureHashType left, SignatureHashType right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(SignatureHashType left, SignatureHashType right)
    {
      return !(left == right);
    }

  }

  /// <summary>
  /// Transaction input struct.
  /// </summary>
  public struct TxIn : IEquatable<TxIn>
  {
    /// <summary>
    /// outpoint.
    /// </summary>
    public OutPoint OutPoint { get; }
    /// <summary>
    /// scriptsig data.
    /// </summary>
    public Script ScriptSig { get; }
    /// <summary>
    /// sequence number.
    /// </summary>
    public uint Sequence { get; }
    /// <summary>
    /// witness stack.
    /// </summary>
    public ScriptWitness WitnessStack { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    public TxIn(OutPoint outPoint)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = CfdSequenceLockTime.Disable;
      WitnessStack = new ScriptWitness();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    /// <param name="sequence">sequence number</param>
    public TxIn(OutPoint outPoint, uint sequence)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = sequence;
      WitnessStack = new ScriptWitness();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    /// <param name="scriptWitness">witness stack</param>
    public TxIn(OutPoint outPoint, ScriptWitness scriptWitness)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = CfdSequenceLockTime.Disable;
      WitnessStack = scriptWitness;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    /// <param name="sequence">sequence number</param>
    /// <param name="scriptWitness">witness stack</param>
    public TxIn(OutPoint outPoint, uint sequence, ScriptWitness scriptWitness)
    {
      OutPoint = outPoint;
      ScriptSig = new Script();
      Sequence = sequence;
      WitnessStack = scriptWitness;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    /// <param name="scriptSig">scriptsig data</param>
    /// <param name="scriptWitness">witness stack</param>
    public TxIn(OutPoint outPoint, Script scriptSig, ScriptWitness scriptWitness)
    {
      OutPoint = outPoint;
      ScriptSig = scriptSig;
      Sequence = CfdSequenceLockTime.Disable;
      WitnessStack = scriptWitness;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    /// <param name="sequence">sequence number</param>
    /// <param name="scriptSig">scriptsig data</param>
    /// <param name="scriptWitness">witness stack</param>
    public TxIn(OutPoint outPoint, uint sequence, Script scriptSig, ScriptWitness scriptWitness)
    {
      OutPoint = outPoint;
      ScriptSig = scriptSig;
      Sequence = sequence;
      WitnessStack = scriptWitness;
    }

    public bool Equals(TxIn other)
    {
      return OutPoint.Equals(other.OutPoint);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is TxIn)
      {
        return Equals((TxIn)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(OutPoint);
    }

    public static bool operator ==(TxIn left, TxIn right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(TxIn left, TxIn right)
    {
      return !(left == right);
    }
  };

  /// <summary>
  /// Transaction output struct.
  /// </summary>
  public struct TxOut : IEquatable<TxOut>
  {
    /// <summary>
    /// satoshi value.
    /// </summary>
    public long SatoshiValue { get; }
    /// <summary>
    /// scriptPubkey (locking script).
    /// </summary>
    public Script ScriptPubkey { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="satoshiValue">satoshi value</param>
    /// <param name="scriptPubkey">locking script</param>
    public TxOut(long satoshiValue, Script scriptPubkey)
    {
      SatoshiValue = satoshiValue;
      ScriptPubkey = scriptPubkey;
    }

    public bool Equals(TxOut other)
    {
      return ScriptPubkey.Equals(other.ScriptPubkey);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is TxOut)
      {
        return Equals((TxOut)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(ScriptPubkey);
    }

    public static bool operator ==(TxOut left, TxOut right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(TxOut left, TxOut right)
    {
      return !(left == right);
    }
  };

  public class Transaction
  {
    public static readonly int defaultNetType = (int)CfdNetworkType.Mainnet;
    private string tx;
    private string lastGetTx = "";
    private string txid = "";
    private string wtxid = "";
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
        Transaction tx)
    {
      return DecodeRawTransaction(tx, CfdNetworkType.Mainnet);
    }

    /// <summary>
    /// Convert tx to decoderawtransaction json string.
    /// </summary>
    /// <param name="tx">transaction object.</param>
    /// <param name="network">network type.</param>
    /// <returns>json string</returns>
    public static string DecodeRawTransaction(
        Transaction tx,
        CfdNetworkType network)
    {
      if (tx is null)
      {
        throw new ArgumentNullException(nameof(tx));
      }
      string networkStr = "regtest";
      if (network == CfdNetworkType.Liquidv1)
      {
        networkStr = "mainnet";
      }
#pragma warning disable CA1305 // IFormatProvider
      string request = string.Format(
          "{{\"hex\":\"{0}\",\"network\":\"{1}\"}}",
          tx.ToHexString(), networkStr);
#pragma warning restore CA1305 // IFormatProvider

      using (var handle = new ErrorHandle())
      {
        var ret = CCommon.CfdRequestExecuteJson(
            handle.GetHandle(),
            "DecodeRawTransaction",
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
    public Transaction(uint version, uint locktime)
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
    public Transaction(uint version, uint locktime, TxIn[] txinList, TxOut[] txoutList)
    {
      tx = CreateTransaction(version, locktime, "", txinList, txoutList);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="txHex">transaction hex</param>
    public Transaction(string txHex)
    {
      tx = txHex;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="txBytes">transaction byte array</param>
    public Transaction(byte[] txBytes)
    {
      tx = StringUtil.FromBytes(txBytes);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="txHex">transaction hex</param>
    /// <param name="txinList">transaction input array</param>
    /// <param name="txoutList">transaction output array</param>
    public Transaction(string txHex, TxIn[] txinList, TxOut[] txoutList)
    {
      tx = CreateTransaction(0, 0, txHex, txinList, txoutList);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="txBytes">transaction byte array</param>
    /// <param name="txinList">transaction input array</param>
    /// <param name="txoutList">transaction output array</param>
    public Transaction(byte[] txBytes, TxIn[] txinList, TxOut[] txoutList)
    {
      tx = CreateTransaction(0, 0, StringUtil.FromBytes(txBytes), txinList, txoutList);
    }

    /// <summary>
    /// Create Transaction.
    /// </summary>
    private static string CreateTransaction(uint version, uint locktime, string txHex, TxIn[] txinList, TxOut[] txoutList)
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
                handle.GetHandle(), txHandle, txout.SatoshiValue,
                "", txout.ScriptPubkey.ToHexString(), "");
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
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      TxIn txin = new TxIn(new OutPoint(txid, vout), sequence);
      tx = CreateTransaction(0, 0, tx, new[] { txin }, null);
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
      AddTxIn(outpoint.GetTxid(), outpoint.GetVout(), sequence);
    }

    public void AddTxInList(TxIn[] txinList)
    {
      tx = CreateTransaction(0, 0, tx, txinList, null);
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="satoshiValue">txout satoshi value.</param>
    /// <param name="address">txout address.</param>
    public void AddTxOut(long satoshiValue, Address address)
    {
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeTransaction(
          handle.GetHandle(), defaultNetType,
          0, 0, tx,
          out IntPtr txHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          ret = NativeMethods.CfdAddTransactionOutput(
            handle.GetHandle(), txHandle, satoshiValue,
            address.ToAddressString(), "", "");
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }

          ret = NativeMethods.CfdFinalizeTransaction(
            handle.GetHandle(), txHandle, out IntPtr txString);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          tx = CCommon.ConvertToString(txString);
        }
        finally
        {
          NativeMethods.CfdFreeTransactionHandle(handle.GetHandle(), txHandle);
        }
      }
    }

    /// <summary>
    /// Add transction output.
    /// </summary>
    /// <param name="satoshiValue">txout satoshi value.</param>
    /// <param name="lockingScript">txout locking script.</param>
    public void AddTxOut(long satoshiValue, Script lockingScript)
    {
      if (lockingScript is null)
      {
        throw new ArgumentNullException(nameof(lockingScript));
      }
      TxOut txout = new TxOut(satoshiValue, lockingScript);
      tx = CreateTransaction(0, 0, tx, null, new[] { txout });
    }

    public void AddTxOutList(TxOut[] txoutList)
    {
      tx = CreateTransaction(0, 0, tx, null, txoutList);
    }

    /// <summary>
    /// Get transaction input data.
    /// </summary>
    /// <param name="outpoint">utxo outpoint(txid and vout)</param>
    /// <returns>transaction input data.</returns>
    public TxIn GetTxIn(OutPoint outpoint)
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
    public TxIn GetTxIn(uint index)
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
    public TxIn[] GetTxInList()
    {
      using (var handle = new ErrorHandle())
      {
        uint count = GetInputCount(handle);
        TxIn[] result = new TxIn[count];

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
    /// <param name="address">txout address.</param>
    /// <returns>transaction output data.</returns>
    public TxOut GetTxOut(Address address)
    {
      uint index = GetTxOutIndex(address);
      using (var handle = new ErrorHandle())
      {
        return GetOutputByIndex(handle, index);
      }
    }

    /// <summary>
    /// Get transaction output data.
    /// </summary>
    /// <param name="lockingScript">txout locking script.</param>
    /// <returns>transaction output data.</returns>
    public TxOut GetTxOut(Script lockingScript)
    {
      uint index = GetTxOutIndex(lockingScript);
      using (var handle = new ErrorHandle())
      {
        return GetOutputByIndex(handle, index);
      }
    }

    /// <summary>
    /// Get transaction output data.
    /// </summary>
    /// <param name="index">txout index.</param>
    /// <returns>transaction output data.</returns>
    public TxOut GetTxOut(uint index)
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
    public TxOut[] GetTxOutList()
    {
      using (var handle = new ErrorHandle())
      {
        uint count = GetOutputCount(handle);
        TxOut[] result = new TxOut[count];

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
        var ret = NativeMethods.CfdGetTxInIndex(
            handle.GetHandle(), defaultNetType, tx,
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
        var ret = NativeMethods.CfdGetTxOutIndex(
            handle.GetHandle(), defaultNetType, tx,
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
    /// <param name="lockingScript">txout locking script.</param>
    /// <returns>transaction output index.</returns>
    public uint GetTxOutIndex(Script lockingScript)
    {
      if (lockingScript is null)
      {
        throw new ArgumentNullException(nameof(lockingScript));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetTxOutIndex(
            handle.GetHandle(), defaultNetType, tx,
            "",
            lockingScript.ToHexString(),
            out uint index);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return index;
      }
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
        Pubkey pubkey, long value, SignatureHashType sighashType)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return GetSignatureHash(outpoint.GetTxid(), outpoint.GetVout(), hashType, pubkey, value, sighashType);
    }

    public ByteData GetSignatureHash(Txid txid, uint vout, CfdHashType hashType,
        Pubkey pubkey, long value, SignatureHashType sighashType)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdCreateSighash(
            handle.GetHandle(), defaultNetType, tx, txid.ToHexString(), vout, (int)hashType,
            pubkey.ToHexString(), "",
            value,
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
        Script redeemScript, long value, SignatureHashType sighashType)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return GetSignatureHash(outpoint.GetTxid(), outpoint.GetVout(), hashType, redeemScript, value, sighashType);
    }

    public ByteData GetSignatureHash(Txid txid, uint vout, CfdHashType hashType,
        Script redeemScript, long value, SignatureHashType sighashType)
    {
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
        var ret = NativeMethods.CfdCreateSighash(
            handle.GetHandle(), defaultNetType, tx, txid.ToHexString(), vout, (int)hashType,
            "", redeemScript.ToHexString(), value,
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
        Privkey privkey, long value, SignatureHashType sighashType)
    {
      AddSignWithPrivkeySimple(outpoint, hashType, privkey, value, sighashType, true);
    }

    public void AddSignWithPrivkeySimple(OutPoint outpoint, CfdHashType hashType,
        Privkey privkey, long value, SignatureHashType sighashType, bool hasGrindR)
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
    Privkey privkey, long value, SignatureHashType sighashType)
    {
      AddSignWithPrivkeySimple(txid, vout, hashType, privkey, value, sighashType, true);
    }

    public void AddSignWithPrivkeySimple(Txid txid, uint vout, CfdHashType hashType,
        Privkey privkey, long value, SignatureHashType sighashType, bool hasGrindR)
    {
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      AddSignWithPrivkeySimple(txid, vout, hashType, privkey.GetPubkey(), privkey, value, sighashType, hasGrindR);
    }

    public void AddSignWithPrivkeySimple(Txid txid, uint vout, CfdHashType hashType, Pubkey pubkey,
        Privkey privkey, long value, SignatureHashType sighashType)
    {
      AddSignWithPrivkeySimple(txid, vout, hashType, pubkey, privkey, value, sighashType, true);
    }

    public void AddSignWithPrivkeySimple(Txid txid, uint vout, CfdHashType hashType, Pubkey pubkey,
        Privkey privkey, long value, SignatureHashType sighashType, bool hasGrindR)
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
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdAddSignWithPrivkeySimple(
            handle.GetHandle(), defaultNetType, tx, txid.ToHexString(), vout, (int)hashType,
            pubkey.ToHexString(),
            (privkey.ToHexString().Length > 0) ? privkey.ToHexString() : privkey.GetWif(),
            value,
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

    public void AddSign(OutPoint outpoint, CfdHashType hashType, SignParameter signData, bool clearStack)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      AddSign(outpoint.GetTxid(), outpoint.GetVout(), hashType, signData, clearStack);
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

    public bool VerifySign(OutPoint outpoint, Address address, CfdAddressType addressType, long satoshiValue)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return VerifySign(outpoint.GetTxid(), outpoint.GetVout(), address, addressType, satoshiValue);
    }

    public bool VerifySign(Txid txid, uint vout, Address address, CfdAddressType addressType, long satoshiValue)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      if (address is null)
      {
        throw new ArgumentNullException(nameof(address));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifyTxSign(
            handle.GetHandle(), defaultNetType, tx, txid.ToHexString(), vout,
            address.ToAddressString(), (int)addressType, "",
            satoshiValue, "");
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

    public bool VerifySignature(OutPoint outpoint, CfdHashType hashType,
        SignParameter signature, Pubkey pubkey, SignatureHashType sighashType, long satoshiValue)
    {
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      return VerifySignature(outpoint, hashType, signature.GetData(), pubkey, sighashType, satoshiValue);
    }

    public bool VerifySignature(OutPoint outpoint, CfdHashType hashType,
        ByteData signature, Pubkey pubkey, SignatureHashType sighashType, long satoshiValue)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return VerifySignature(outpoint.GetTxid(), outpoint.GetVout(), hashType, signature, pubkey, sighashType, satoshiValue);
    }

    public bool VerifySignature(Txid txid, uint vout, CfdHashType hashType,
        ByteData signature, Pubkey pubkey, SignatureHashType sighashType, long satoshiValue)
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
      ByteData sig = signature;
      if (signature.GetSize() > 65)
      {
        // decode der
        sig = SignParameter.DecodeFromDer(sig).GetData();
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdVerifySignature(
            handle.GetHandle(), defaultNetType, tx, sig.ToHexString(),
            (int)hashType,
            pubkey.ToHexString(), "", txid.ToHexString(), vout,
            (int)sighashType.SighashType, sighashType.IsSighashAnyoneCanPay,
            satoshiValue, "");
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

    public bool VerifySignature(OutPoint outpoint, CfdHashType hashType,
        SignParameter signature, Pubkey pubkey, Script redeemScript, SignatureHashType sighashType, long satoshiValue)
    {
      if (signature is null)
      {
        throw new ArgumentNullException(nameof(signature));
      }
      return VerifySignature(outpoint, hashType, signature.GetData(), pubkey, redeemScript, sighashType, satoshiValue);
    }

    public bool VerifySignature(OutPoint outpoint, CfdHashType hashType,
        ByteData signature, Pubkey pubkey, Script redeemScript, SignatureHashType sighashType, long satoshiValue)
    {
      if (outpoint is null)
      {
        throw new ArgumentNullException(nameof(outpoint));
      }
      return VerifySignature(outpoint.GetTxid(), outpoint.GetVout(), hashType, signature, pubkey, redeemScript, sighashType, satoshiValue);
    }

    public bool VerifySignature(Txid txid, uint vout, CfdHashType hashType, ByteData signature,
      Pubkey pubkey, Script redeemScript, SignatureHashType sighashType, long satoshiValue)
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
        var ret = NativeMethods.CfdVerifySignature(
            handle.GetHandle(), default, tx, sig.ToHexString(), (int)hashType,
            pubkey.ToHexString(), redeemScript.ToHexString(),
            txid.ToHexString(), vout,
            (int)sighashType.SighashType, sighashType.IsSighashAnyoneCanPay,
            satoshiValue, "");
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

    public FeeData EstimateFee(UtxoData[] txinList, double feeRate)
    {
      if (txinList is null)
      {
        throw new ArgumentNullException(nameof(txinList));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeEstimateFee(
          handle.GetHandle(), out IntPtr feeHandle, false);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          foreach (UtxoData txin in txinList)
          {
            ret = NativeMethods.CfdAddTxInTemplateForEstimateFee(
              handle.GetHandle(), feeHandle, txin.GetOutPoint().GetTxid().ToHexString(),
              txin.GetOutPoint().GetVout(), txin.GetDescriptor().ToString(),
              "", false, false, false, 0, "",
              txin.GetScriptSigTemplate().ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdFinalizeEstimateFee(
              handle.GetHandle(), feeHandle, tx, "", out long txFee,
              out long utxoFee, false, feeRate);
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

    public string FundRawTransaction(UtxoData[] txinList, UtxoData[] utxoList,
      string reservedAddress, double effectiveFeeRate)
    {
      return FundRawTransaction(txinList, utxoList, 0, reservedAddress,
        effectiveFeeRate, effectiveFeeRate, -1, -1);
    }

    public string FundRawTransaction(UtxoData[] txinList, UtxoData[] utxoList,
      long targetAmount, string reservedAddress,
      double effectiveFeeRate, double longTermFeeRate, long dustFeeRate, long knapsackMinChange)
    {
      if (utxoList is null)
      {
        throw new ArgumentNullException(nameof(utxoList));
      }
      if (reservedAddress is null)
      {
        throw new ArgumentNullException(nameof(reservedAddress));
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeFundRawTx(
          handle.GetHandle(), defaultNetType, 1, "", out IntPtr fundHandle);
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
                "", false, false, false, 0, "",
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
              utxo.GetAmount(), utxo.GetDescriptor().ToString(), "",
              utxo.GetScriptSigTemplate().ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdAddTargetAmountForFundRawTx(
            handle.GetHandle(), fundHandle, 0, targetAmount, "", reservedAddress);
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

          ret = NativeMethods.CfdFinalizeFundRawTx(
            handle.GetHandle(), fundHandle, tx, effectiveFeeRate,
            out long txFee, out uint appendTxOutCount, out IntPtr outputTxHex);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          string fundTx = CCommon.ConvertToString(outputTxHex);
          if (appendTxOutCount > 1)
          {
            throw new InvalidProgramException("Invalid txout append count.");
          }

          string usedReserveAddress = "";
          for (uint index = 0; index < appendTxOutCount; ++index)
          {
            ret = NativeMethods.CfdGetAppendTxOutFundRawTx(
              handle.GetHandle(), fundHandle, index, out IntPtr appendAddress);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
            usedReserveAddress = CCommon.ConvertToString(appendAddress);
          }

          tx = fundTx;
          lastTxFee = txFee;
          return usedReserveAddress;
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

    private void UpdateTxInfoCache()
    {
      if (tx == lastGetTx)
      {
        return;
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdGetTxInfo(
            handle.GetHandle(),
            defaultNetType,
            tx,
            out IntPtr outputTxid,
            out IntPtr outputWtxid,
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
        txSize = outputSize;
        txVsize = outputVsize;
        txWeight = outputWeight;
        txVersion = outputVersion;
        txLocktime = outputLocktime;
        lastGetTx = tx;
      }
    }

    private TxIn GetInputByIndex(ErrorHandle handle, uint index)
    {
      var ret = NativeMethods.CfdGetTxIn(
          handle.GetHandle(), defaultNetType, tx, index,
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

      ret = NativeMethods.CfdGetTxInWitnessCount(
          handle.GetHandle(), defaultNetType, tx, index,
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
        ret = NativeMethods.CfdGetTxInWitness(
            handle.GetHandle(), defaultNetType, tx, index, witnessIndex,
            out stackData);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        witnessArray[witnessIndex] = CCommon.ConvertToString(stackData);
      }

      return new TxIn(
          new OutPoint(utxoTxid, vout), sequence, new Script(scriptSig),
          new ScriptWitness(witnessArray));
    }

    private uint GetInputCount(ErrorHandle handle)
    {
      var ret = NativeMethods.CfdGetTxInCount(
          handle.GetHandle(), defaultNetType, tx, out uint count);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      return count;
    }

    private TxOut GetOutputByIndex(ErrorHandle handle, uint index)
    {
      var ret = NativeMethods.CfdGetTxOut(
          handle.GetHandle(), defaultNetType, tx, index,
          out long satoshi,
          out IntPtr lockingScript);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      var scriptPubkey = CCommon.ConvertToString(lockingScript);

      return new TxOut(satoshi, new Script(scriptPubkey));
    }

    private uint GetOutputCount(ErrorHandle handle)
    {
      var ret = NativeMethods.CfdGetTxOutCount(
          handle.GetHandle(), defaultNetType, tx, out uint count);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      return count;
    }
  }
}
