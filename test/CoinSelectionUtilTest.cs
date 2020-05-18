using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cfd.xTests
{
  public class CoinSelectionUtilTest
  {
    private readonly ITestOutputHelper output;

    public CoinSelectionUtilTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    static UtxoData[] GetBitcoinUtxoList()
    {
      Descriptor desc = new Descriptor("wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1)", CfdNetworkType.Mainnet);
      UtxoData[] utxos = new[] {
        new UtxoData(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0), 312500000, desc),
        new UtxoData(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0), 78125000, desc),
        new UtxoData(new OutPoint("9e1ead91c432889cb478237da974dd1e9009c9e22694fd1e3999c40a1ef59b0a", 0), 1250000000, desc),
        new UtxoData(new OutPoint("8f4af7ee42e62a3d32f25ca56f618fb2f5df3d4c3a9c59e2c3646c5535a3d40a", 0), 39062500, desc),
        new UtxoData(new OutPoint("4d97d0119b90421818bff4ec9033e5199199b53358f56390cb20f8148e76f40a", 0), 156250000, desc),
        new UtxoData(new OutPoint("b9720ed2265a4ced42425bffdb4ef90a473b4106811a802fce53f7c57487fa0b", 0), 2500000000, desc),
        new UtxoData(new OutPoint("0f093988839178ea5895431241cb4400fb31dd7b665a1a93cbd372336c717e0c", 0), 5000000000, desc),
      };
      return utxos;
    }

    static UtxoData[] GetBitcoinBnbUtxoList(CfdNetworkType netType)
    {
      string desc = "sh(wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1))";
      UtxoData[] utxos = new[] {
        new UtxoData(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0), 155062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0), 85062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("9e1ead91c432889cb478237da974dd1e9009c9e22694fd1e3999c40a1ef59b0a", 0), 39062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("8f4af7ee42e62a3d32f25ca56f618fb2f5df3d4c3a9c59e2c3646c5535a3d40a", 0), 61062500,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("4d97d0119b90421818bff4ec9033e5199199b53358f56390cb20f8148e76f40a", 0), 15675000,
          new Descriptor(desc, netType)),
        new UtxoData(new OutPoint("b9720ed2265a4ced42425bffdb4ef90a473b4106811a802fce53f7c57487fa0b", 0), 14938590,
          new Descriptor(desc, netType)),
      };
      return utxos;
    }

    [Fact]
    public void SelectCoinsTest01()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      UtxoData[] selectedList = util.SelectCoins(utxos, 0, 0, 20.0);
      Assert.Empty(selectedList);
      Assert.Equal(0, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest02()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 39059180, 20.0);
      Assert.Single(selectedList);
      Assert.Equal(1360, util.GetLastSelectedUtxoFee());
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(39062500, totalAmount);
      if (selectedList.Length == 1)
      {
        Assert.Equal(39062500, selectedList[0].GetAmount());
      }
    }

    [Fact]
    public void SelectCoinsTest03()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 119154360, 20.0);
      Assert.Single(selectedList);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(156250000, totalAmount);
      if (selectedList.Length == 1)
      {
        Assert.Equal(156250000, selectedList[0].GetAmount());
      }
      Assert.Equal(1360, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest04()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 120000000, 20.0);
      Assert.Single(selectedList);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(156250000, totalAmount);
      if (selectedList.Length == 1)
      {
        Assert.Equal(156250000, selectedList[0].GetAmount());
      }
      Assert.Equal(1360, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest05()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 220000000, 20.0);
      Assert.Equal(2, selectedList.Length);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(234375000, totalAmount);
      if (selectedList.Length == 2)
      {
        Assert.Equal(156250000, selectedList[0].GetAmount());
        Assert.Equal(78125000, selectedList[1].GetAmount());
      }
      Assert.Equal(2720, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest06()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 460000000, 20.0);
      Assert.Equal(2, selectedList.Length);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(468750000, totalAmount);
      if (selectedList.Length == 2)
      {
        Assert.Equal(312500000, selectedList[0].GetAmount());
        Assert.Equal(156250000, selectedList[1].GetAmount());
      }
      Assert.Equal(2720, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest07()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 468700000, 20.0);
      Assert.Single(selectedList);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(1250000000, totalAmount);
      if (selectedList.Length == 1)
      {
        Assert.Equal(1250000000, selectedList[0].GetAmount());
      }
      Assert.Equal(1360, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest08()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      long knapsackMinChange = 0;
      UtxoData[] selectedList = util.SelectCoins(utxos, 1000, 468700000, 20.0, -1, -1, knapsackMinChange);
      Assert.Equal(2, selectedList.Length);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(468750000, totalAmount);
      if (selectedList.Length == 2)
      {
        Assert.Equal(312500000, selectedList[0].GetAmount());
        Assert.Equal(156250000, selectedList[1].GetAmount());
      }
      Assert.Equal(2720, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest11()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinBnbUtxoList(CfdNetworkType.Mainnet);
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 99998500, 2.0, -1, -1, -1);
      Assert.Equal(2, selectedList.Length);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(100001090, totalAmount);
      if (selectedList.Length == 2)
      {
        Assert.Equal(85062500, selectedList[0].GetAmount());
        Assert.Equal(14938590, selectedList[1].GetAmount());
      }
      Assert.Equal(360, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest12()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinBnbUtxoList(CfdNetworkType.Mainnet);
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 155060800, 2.0, -1, -1, -1);
      Assert.Single(selectedList);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(155062500, totalAmount);
      if (selectedList.Length == 1)
      {
        Assert.Equal(155062500, selectedList[0].GetAmount());
      }
      Assert.Equal(180, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsTest13()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinBnbUtxoList(CfdNetworkType.Mainnet);
      UtxoData[] selectedList = util.SelectCoins(utxos, 1500, 114040000, 1.0, -1, -1, -1);
      Assert.Equal(3, selectedList.Length);
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList);
      Assert.Equal(115063590, totalAmount);
      if (selectedList.Length == 3)
      {
        Assert.Equal(61062500, selectedList[0].GetAmount());
        Assert.Equal(39062500, selectedList[1].GetAmount());
        Assert.Equal(14938590, selectedList[2].GetAmount());
      }
      Assert.Equal(270, util.GetLastSelectedUtxoFee());
    }

    [Fact]
    public void SelectCoinsErrorTest01()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = new UtxoData[0];
      try
      {
        util.SelectCoins(utxos, 1500, 100000000, 20.0);
        Assert.True(false);
      }
      catch (InvalidOperationException e)
      {
        output.WriteLine(e.Message);
        Assert.Equal("utxoList is empty.", e.Message);
      }
      catch (Exception except)
      {
        output.WriteLine(except.Message);
        throw except;
      }
    }

    [Fact]
    public void SelectCoinsErrorTest02()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      UtxoData[] utxos = GetBitcoinUtxoList();
      try
      {
        util.SelectCoins(utxos, 1500, 9500000000, 20.0);
        Assert.True(false);
      }
      catch (InvalidOperationException e)
      {
        output.WriteLine(e.Message);
        Assert.Equal("CFD error[IllegalStateError] message:Failed to select coin. Not enough utxos.", e.Message);
      }
      catch (Exception except)
      {
        output.WriteLine(except.Message);
        throw except;
      }
    }

    private static readonly string assetA = "aa00000000000000000000000000000000000000000000000000000000000000";
    private static readonly string assetB = "bb00000000000000000000000000000000000000000000000000000000000000";
    private static readonly string assetC = "cc00000000000000000000000000000000000000000000000000000000000000";

    static ElementsUtxoData[] GetElementsUtxoList()
    {
      Descriptor desc = new Descriptor("sh(wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1))", CfdNetworkType.ElementsRegtest);
      ElementsUtxoData[] utxos = new[] {
        new ElementsUtxoData(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0),
          new ConfidentialAsset(assetA), 312500000),
        new ElementsUtxoData(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0),
          new ConfidentialAsset(assetA), 78125000),
        new ElementsUtxoData(new OutPoint("9e1ead91c432889cb478237da974dd1e9009c9e22694fd1e3999c40a1ef59b0a", 0),
          new ConfidentialAsset(assetA), 1250000000),
        new ElementsUtxoData(new OutPoint("8f4af7ee42e62a3d32f25ca56f618fb2f5df3d4c3a9c59e2c3646c5535a3d40a", 0),
          new ConfidentialAsset(assetA), 39062500),
        new ElementsUtxoData(new OutPoint("4d97d0119b90421818bff4ec9033e5199199b53358f56390cb20f8148e76f40a", 0),
          new ConfidentialAsset(assetA), 156250000),
        new ElementsUtxoData(new OutPoint("b9720ed2265a4ced42425bffdb4ef90a473b4106811a802fce53f7c57487fa0b", 0),
          new ConfidentialAsset(assetA), 2500000000),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b01", 0),
          new ConfidentialAsset(assetB), 26918400),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b02", 0),
          new ConfidentialAsset(assetB), 750000),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b03", 0),
          new ConfidentialAsset(assetB), 346430050),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b04", 0),
          new ConfidentialAsset(assetB), 18476350),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000c01", 0),
          new ConfidentialAsset(assetC), 5000000000),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000c02", 0),
          new ConfidentialAsset(assetC), 127030000),
      };
      return utxos;
    }
    static ElementsUtxoData[] GetElementsBnbUtxoList(CfdNetworkType netType)
    {
      string desc = "sh(wpkh([ef735203/0'/0'/7']022c2409fbf657ba25d97bb3dab5426d20677b774d4fc7bd3bfac27ff96ada3dd1))";
      ElementsUtxoData[] utxos = new[] {
        new ElementsUtxoData(new OutPoint("7ca81dd22c934747f4f5ab7844178445fe931fb248e0704c062b8f4fbd3d500a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(155062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("30f71f39d210f7ee291b0969c6935debf11395b0935dca84d30c810a75339a0a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(85062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("9e1ead91c432889cb478237da974dd1e9009c9e22694fd1e3999c40a1ef59b0a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(39062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("8f4af7ee42e62a3d32f25ca56f618fb2f5df3d4c3a9c59e2c3646c5535a3d40a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(61062500),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("4d97d0119b90421818bff4ec9033e5199199b53358f56390cb20f8148e76f40a", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(15675000),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("b9720ed2265a4ced42425bffdb4ef90a473b4106811a802fce53f7c57487fa0b", 0),
          new ConfidentialAsset(assetA), new ConfidentialValue(14938590),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b01", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(26918400),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b02", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(750000),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b03", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(346430050),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000b04", 0),
          new ConfidentialAsset(assetB), new ConfidentialValue(18476350),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000c01", 0),
          new ConfidentialAsset(assetC), new ConfidentialValue(37654200),
          new Descriptor(desc, netType)),
        new ElementsUtxoData(new OutPoint("0000000000000000000000000000000000000000000000000000000000000c02", 0),
          new ConfidentialAsset(assetC), new ConfidentialValue(127030000),
          new Descriptor(desc, netType)),
      };
      return utxos;
    }

    [Fact]
    public void ElementsSelectCoinsTest01()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsUtxoList();
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 0 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 0);
      Assert.Empty(selectedList);
      Assert.Equal(0, util.GetLastSelectedUtxoFee());
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      Assert.Equal(0, totalAmount);
    }

    [Fact]
    public void ElementsSelectCoinsTest02()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsUtxoList();
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 39060180 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 20.0);
      Assert.Single(selectedList);
      Assert.Equal(1380, util.GetLastSelectedUtxoFee());
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      Assert.Equal(78125000, totalAmount);
      if (selectedList.Length == 1)
      {
        Assert.Equal(78125000, selectedList[0].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest03()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsUtxoList();
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 220000000 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 20.0);
      Assert.Equal(2, selectedList.Length);
      Assert.Equal(2760, util.GetLastSelectedUtxoFee());
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      Assert.Equal(234375000, totalAmount);
      if (selectedList.Length == 2)
      {
        Assert.Equal(156250000, selectedList[0].GetAmount());
        Assert.Equal(78125000, selectedList[1].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest11()
    {
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 99998500 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 2.0);
      Assert.Equal(2, selectedList.Length);
      Assert.Equal(364, util.GetLastSelectedUtxoFee());
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      Assert.Equal(100001090, totalAmount);
      if (selectedList.Length == 2)
      {
        Assert.Equal(85062500, selectedList[0].GetAmount());
        Assert.Equal(14938590, selectedList[1].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest12()
    {
      // SelectCoins_SelectCoinsBnB_single_with_asset
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 155060800 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 2.0);
      Assert.Single(selectedList);
      Assert.Equal(182, util.GetLastSelectedUtxoFee());
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      Assert.Equal(155062500, totalAmount);
      if (selectedList.Length == 1)
      {
        Assert.Equal(155062500, selectedList[0].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest13()
    {
      // SelectCoins_SelectCoinsBnB_empty_with_asset
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 114040000 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 1.0);
      Assert.Equal(3, selectedList.Length);
      Assert.Equal(273, util.GetLastSelectedUtxoFee());
      long totalAmount = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      Assert.Equal(115063590, totalAmount);
      if (selectedList.Length == 3)
      {
        Assert.Equal(61062500, selectedList[0].GetAmount());
        Assert.Equal(39062500, selectedList[1].GetAmount());
        Assert.Equal(14938590, selectedList[2].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest21()
    {
      // SelectCoins_KnapsackSolver_with_multiple_asset
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsUtxoList();
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 39060180 },
        { new ConfidentialAsset(assetB), 25000000 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 20.0);
      Assert.Equal(2, selectedList.Length);
      Assert.Equal(2760, util.GetLastSelectedUtxoFee());
      long totalAmountA = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      long totalAmountB = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetB));
      long totalAmountC = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetC));
      Assert.Equal(78125000, totalAmountA);
      Assert.Equal(26918400, totalAmountB);
      Assert.Equal(0, totalAmountC);
      if (selectedList.Length == 2)
      {
        Assert.Equal(26918400, selectedList[0].GetAmount());
        Assert.Equal(78125000, selectedList[1].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest22()
    {
      // SelectCoins_CoinSelectBnB_with_multiple_asset
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetA);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 99997900 },
        { new ConfidentialAsset(assetB), 346495050 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 2.0);
      Assert.Equal(4, selectedList.Length);
      Assert.Equal(728, util.GetLastSelectedUtxoFee());
      long totalAmountA = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      long totalAmountB = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetB));
      long totalAmountC = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetC));
      Assert.Equal(100001090, totalAmountA);
      Assert.Equal(347180050, totalAmountB);
      Assert.Equal(0, totalAmountC);
      if (selectedList.Length == 4)
      {
        Assert.Equal(346430050, selectedList[0].GetAmount());
        Assert.Equal(750000, selectedList[1].GetAmount());
        Assert.Equal(85062500, selectedList[2].GetAmount());
        Assert.Equal(14938590, selectedList[3].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest23()
    {
      // SelectCoins_with_multiple_asset_fee_only_target
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetC);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 99997900 },
        { new ConfidentialAsset(assetB), 346495050 },
        { new ConfidentialAsset(assetC), 0 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 2.0);
      Assert.Equal(4, selectedList.Length);
      Assert.Equal(728, util.GetLastSelectedUtxoFee());
      long totalAmountA = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      long totalAmountB = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetB));
      long totalAmountC = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetC));
      Assert.Equal(155062500, totalAmountA);
      Assert.Equal(347180050, totalAmountB);
      Assert.Equal(37654200, totalAmountC);
      if (selectedList.Length == 4)
      {
        Assert.Equal(155062500, selectedList[0].GetAmount());
        Assert.Equal(346430050, selectedList[1].GetAmount());
        Assert.Equal(750000, selectedList[2].GetAmount());
        Assert.Equal(37654200, selectedList[3].GetAmount());
      }
    }

    [Fact]
    public void ElementsSelectCoinsTest24()
    {
      // SelectCoins_with_multiple_asset_not_consider_fee
      CoinSelectionUtil util = new CoinSelectionUtil();
      ElementsUtxoData[] utxos = GetElementsBnbUtxoList(CfdNetworkType.Liquidv1);
      ConfidentialAsset feeAsset = new ConfidentialAsset(assetC);
      var targetAssetAmountMap = new Dictionary<ConfidentialAsset, long> {
        { new ConfidentialAsset(assetA), 115800000 },
        { new ConfidentialAsset(assetB), 19226350 },
        { new ConfidentialAsset(assetC), 99060000 },
      };
      ElementsUtxoData[] selectedList = util.SelectCoinsForElements(
        utxos, targetAssetAmountMap, feeAsset, 1500, 0);
      Assert.Equal(6, selectedList.Length);
      Assert.Equal(0, util.GetLastSelectedUtxoFee());
      long totalAmountA = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetA));
      long totalAmountB = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetB));
      long totalAmountC = CoinSelectionUtil.GetTotalAmount(selectedList, new ConfidentialAsset(assetC));
      Assert.Equal(115800000, totalAmountA);
      Assert.Equal(19226350, totalAmountB);
      Assert.Equal(127030000, totalAmountC);
      if (selectedList.Length == 6)
      {
        Assert.Equal(61062500, selectedList[0].GetAmount());
        Assert.Equal(39062500, selectedList[1].GetAmount());
        Assert.Equal(15675000, selectedList[2].GetAmount());
        Assert.Equal(18476350, selectedList[3].GetAmount());
        Assert.Equal(750000, selectedList[4].GetAmount());
        Assert.Equal(127030000, selectedList[5].GetAmount());
      }
    }
  }
}
