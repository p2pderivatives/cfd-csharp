/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  public static class CoinUtil
  {
    public static void CoinSelection()
    {
      // FIXME
      /*

      internal static extern CfdErrorCode CfdInitializeCoinSelection(
        [In] IntPtr handle,
        [In] uint utxoCount,
        [In] uint targetAssetCount,
        [In] string feeAsset,
        [In] long tx_feeAmount,
        [In] double effectiveFeeRate,
        [In] double longTermFeeRate,
        [In] double dustFeeRate,
        [In] long knapsackMinChange,
        [Out] out IntPtr coinSelectHandle);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddCoinSelectionUtxo(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint utxoIndex,
        [In] string txid,
        [In] uint vout,
        [In] long amount,
        [In] string asset,
        [In] string descriptor);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal static extern CfdErrorCode CfdAddCoinSelectionAmount(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint assetIndex,
        [In] long amount,
        [In] string asset);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFinalizeCoinSelection(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [Out] out long utxoFeeAmount);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetSelectedCoinIndex(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint index,
        [Out] out uint utxoIndex);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdGetSelectedCoinAssetAmount(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle,
        [In] uint assetIndex,
        [Out] out long amount);

    [DllImport("cfd", CallingConvention = CallingConvention.StdCall)]
    internal static extern CfdErrorCode CfdFreeCoinSelectionHandle(
        [In] IntPtr handle,
        [In] IntPtr coinSelectHandle);
      */
    }
  }
}
