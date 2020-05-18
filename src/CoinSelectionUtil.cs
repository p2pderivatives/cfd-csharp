using System;
using System.Collections.Generic;

namespace Cfd
{
  public class CoinSelectionUtil
  {
    /// option: blind exponent (int64)
    public static readonly int Exponent = 1;
    /// option: blind minimum bits (int64)
    public static readonly int MinimumBits = 2;

    private long lastSelectedUtxoFee;

    public static long GetTotalAmount(UtxoData[] utxoList)
    {
      if (utxoList is null)
      {
        return 0;
      }
      long amount = 0;
      foreach (var utxo in utxoList)
      {
        amount += utxo.GetAmount();
      }
      return amount;
    }

    public static long GetTotalAmount(ElementsUtxoData[] utxoList, ConfidentialAsset asset)
    {
      if ((utxoList is null) || (asset is null))
      {
        return 0;
      }
      long amount = 0;
      foreach (var utxo in utxoList)
      {
        if (utxo.GetAsset() == asset.ToHexString())
        {
          amount += utxo.GetAmount();
        }
      }
      return amount;
    }

    public UtxoData[] SelectCoins(UtxoData[] utxoList, long txFeeAmount, long targetAmount,
      double effectiveFeeRate)
    {
      return SelectCoins(utxoList, txFeeAmount, targetAmount, effectiveFeeRate,
        effectiveFeeRate, -1, -1);
    }

    public UtxoData[] SelectCoins(UtxoData[] utxoList, long txFeeAmount, long targetAmount,
      double effectiveFeeRate, double longTermFeeRate, long dustFeeRate, long knapsackMinChange)
    {
      if (utxoList is null)
      {
        throw new ArgumentNullException(nameof(utxoList));
      }
      if (utxoList.Length <= 0)
      {
        throw new InvalidOperationException("utxoList is empty.");
      }
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdInitializeCoinSelection(
          handle.GetHandle(), (uint)utxoList.Length, 1, "", txFeeAmount,
          effectiveFeeRate, longTermFeeRate, dustFeeRate, knapsackMinChange,
          out IntPtr coinSelectHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          for (uint index = 0; index < utxoList.Length; ++index)
          {
            string desc = (utxoList[index].GetDescriptor() is null) ?
              "" : utxoList[index].GetDescriptor().ToString();
            ret = NativeMethods.CfdAddCoinSelectionUtxoTemplate(
              handle.GetHandle(), coinSelectHandle, index,
              utxoList[index].GetOutPoint().GetTxid().ToHexString(),
              utxoList[index].GetOutPoint().GetVout(),
              utxoList[index].GetAmount(), "",
              desc, utxoList[index].GetScriptSigTemplate().ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }
          ret = NativeMethods.CfdAddCoinSelectionAmount(
            handle.GetHandle(), coinSelectHandle, 0, targetAmount, "");
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }

          ret = NativeMethods.CfdFinalizeCoinSelection(
            handle.GetHandle(), coinSelectHandle, out long utxoFeeAmount);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }

          uint[] collectIndexes = new uint[utxoList.Length];
          uint collectCount = 0;
          if ((utxoFeeAmount > 0) || (targetAmount > 0))
          {
            for (uint index = 0; index < utxoList.Length; ++index)
            {
              ret = NativeMethods.CfdGetSelectedCoinIndex(
                handle.GetHandle(), coinSelectHandle,
                index, out int utxoIndex);
              if (ret != CfdErrorCode.Success)
              {
                handle.ThrowError(ret);
              }
              if (utxoIndex < 0)
              {
                break;
              }
              if (utxoList.Length <= utxoIndex)
              {
                throw new InvalidProgramException("utxoIndex maximum over.");
              }
              ++collectCount;
              collectIndexes[index] = (uint)utxoIndex;
            }
          }

          /*
          ret = NativeMethods.CfdGetSelectedCoinAssetAmount(
            handle.GetHandle(), coinSelectHandle, 0, out long collectAmount);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          */

          UtxoData[] selectedUtxoList = new UtxoData[collectCount];
          for (uint index = 0; index < collectCount; ++index)
          {
            selectedUtxoList[index] = utxoList[collectIndexes[index]];
          }
          lastSelectedUtxoFee = utxoFeeAmount;
          return selectedUtxoList;
        }
        finally
        {
          NativeMethods.CfdFreeCoinSelectionHandle(handle.GetHandle(), coinSelectHandle);
        }
      }
    }

    public ElementsUtxoData[] SelectCoinsForElements(
      ElementsUtxoData[] utxoList,
      IDictionary<ConfidentialAsset, long> targetAssetAmountMap,
      ConfidentialAsset feeAsset, long txFeeAmount,
      double effectiveFeeRate)
    {
      return SelectCoinsForElements(utxoList, targetAssetAmountMap, feeAsset, txFeeAmount,
        effectiveFeeRate, 0, ConfidentialTransaction.defaultMinimumBits,
        effectiveFeeRate, -1, -1);
    }

    public ElementsUtxoData[] SelectCoinsForElements(
      ElementsUtxoData[] utxoList,
      IDictionary<ConfidentialAsset, long> targetAssetAmountMap,
      ConfidentialAsset feeAsset, long txFeeAmount,
      double effectiveFeeRate, int exponent, int minimumBits)
    {
      return SelectCoinsForElements(utxoList, targetAssetAmountMap, feeAsset, txFeeAmount,
        effectiveFeeRate, exponent, minimumBits, effectiveFeeRate, -1, -1);
    }

    public ElementsUtxoData[] SelectCoinsForElements(
      ElementsUtxoData[] utxoList,
      IDictionary<ConfidentialAsset, long> targetAssetAmountMap,
      ConfidentialAsset feeAsset, long txFeeAmount,
      double effectiveFeeRate, double longTermFeeRate,
      long dustFeeRate, long knapsackMinChange)
    {
      return SelectCoinsForElements(utxoList, targetAssetAmountMap, feeAsset, txFeeAmount,
        effectiveFeeRate, 0, ConfidentialTransaction.defaultMinimumBits,
        longTermFeeRate, dustFeeRate, knapsackMinChange);
    }

    public ElementsUtxoData[] SelectCoinsForElements(
      ElementsUtxoData[] utxoList,
      IDictionary<ConfidentialAsset, long> targetAssetAmountMap,
      ConfidentialAsset feeAsset, long txFeeAmount,
      double effectiveFeeRate, int exponent, int minimumBits,
      double longTermFeeRate,
      long dustFeeRate, long knapsackMinChange)
    {
      if (utxoList is null)
      {
        throw new ArgumentNullException(nameof(utxoList));
      }
      if (utxoList.Length <= 0)
      {
        throw new InvalidOperationException("utxoList is empty.");
      }
      if (targetAssetAmountMap is null)
      {
        throw new ArgumentNullException(nameof(targetAssetAmountMap));
      }
      if (targetAssetAmountMap.Count <= 0)
      {
        throw new InvalidOperationException("targetAssetAmountMap is empty.");
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
        var ret = NativeMethods.CfdInitializeCoinSelection(
          handle.GetHandle(), (uint)utxoList.Length, (uint)targetAssetAmountMap.Count,
          feeAsset.ToHexString(), txFeeAmount,
          effectiveFeeRate, longTermFeeRate, dustFeeRate, knapsackMinChange,
          out IntPtr coinSelectHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        try
        {
          for (uint index = 0; index < utxoList.Length; ++index)
          {
            string desc = (utxoList[index].GetDescriptor() is null) ?
              "" : utxoList[index].GetDescriptor().ToString();
            ret = NativeMethods.CfdAddCoinSelectionUtxoTemplate(
              handle.GetHandle(), coinSelectHandle, index,
              utxoList[index].GetOutPoint().GetTxid().ToHexString(),
              utxoList[index].GetOutPoint().GetVout(),
              utxoList[index].GetAmount(),
              utxoList[index].GetAsset(),
              desc, utxoList[index].GetScriptSigTemplate().ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }
          long targetAmountAll = 0;
          uint assetIndex = 0;
          foreach (var key in targetAssetAmountMap.Keys)
          {
            ret = NativeMethods.CfdAddCoinSelectionAmount(
              handle.GetHandle(), coinSelectHandle, assetIndex,
              targetAssetAmountMap[key], key.ToHexString());
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
            ++assetIndex;
            targetAmountAll += targetAssetAmountMap[key];
          }

          if (exponent >= -1)
          {
            ret = NativeMethods.CfdSetOptionCoinSelection(handle.GetHandle(), coinSelectHandle,
              Exponent, exponent, 0, false);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }
          if (minimumBits >= 0)
          {
            ret = NativeMethods.CfdSetOptionCoinSelection(handle.GetHandle(), coinSelectHandle,
                MinimumBits, minimumBits, 0, false);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
          }

          ret = NativeMethods.CfdFinalizeCoinSelection(
            handle.GetHandle(), coinSelectHandle, out long utxoFeeAmount);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }

          uint[] collectIndexes = new uint[utxoList.Length];
          uint collectCount = 0;
          if ((utxoFeeAmount > 0) || (targetAmountAll > 0))
          {
            for (uint index = 0; index < utxoList.Length; ++index)
            {
              ret = NativeMethods.CfdGetSelectedCoinIndex(
                handle.GetHandle(), coinSelectHandle,
                index, out int utxoIndex);
              if (ret != CfdErrorCode.Success)
              {
                handle.ThrowError(ret);
              }
              if (utxoIndex < 0)
              {
                break;
              }
              if (utxoList.Length <= utxoIndex)
              {
                throw new InvalidProgramException("utxoIndex maximum over.");
              }
              ++collectCount;
              collectIndexes[index] = (uint)utxoIndex;
            }
          }
          /*
          assetIndex = 0;
          collectAmountList = new Dictionary<ConfidentialAsset, long>();
          foreach (var key in targetAssetAmountMap.Keys)
          {
            ret = NativeMethods.CfdGetSelectedCoinAssetAmount(
              handle.GetHandle(), coinSelectHandle, assetIndex,
              out long collectAmount);
            if (ret != CfdErrorCode.Success)
            {
              handle.ThrowError(ret);
            }
            ++assetIndex;
            collectAmountList.Add(key, collectAmount);
          }
          */

          ElementsUtxoData[] selectedUtxoList = new ElementsUtxoData[collectCount];
          for (uint index = 0; index < collectCount; ++index)
          {
            selectedUtxoList[index] = utxoList[collectIndexes[index]];
          }
          lastSelectedUtxoFee = utxoFeeAmount;
          return selectedUtxoList;
        }
        finally
        {
          NativeMethods.CfdFreeCoinSelectionHandle(handle.GetHandle(), coinSelectHandle);
        }
      }
    }

    public long GetLastSelectedUtxoFee()
    {
      return lastSelectedUtxoFee;
    }
  }
}
