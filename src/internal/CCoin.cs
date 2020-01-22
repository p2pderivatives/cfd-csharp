// using System;
// using System.Runtime.InteropServices;

namespace Cfd
{
  /*
    internal static class CCoin
    {

  internal static extern CfdErrorCode CfdInitializeCoinSelection(
      void* handle, uint32_t utxo_count, uint32_t target_asset_count,
      const char* fee_asset, int64_t tx_fee_amount, double effective_fee_rate,
      double long_term_fee_rate, double dust_fee_rate,
      int64_t knapsack_min_change, void** coin_select_handle);

  internal static extern CfdErrorCode CfdAddCoinSelectionUtxo(
      void* handle, void* coin_select_handle, int32_t utxo_index,
      const char* txid, uint32_t vout, int64_t amount, const char* asset,
      const char* descriptor);

  internal static extern CfdErrorCode CfdAddCoinSelectionAmount(
      void* handle, void* coin_select_handle, uint32_t asset_index,
      int64_t amount, const char* asset);

  internal static extern CfdErrorCode CfdFinalizeCoinSelection(
      void* handle, void* coin_select_handle, int64_t* utxo_fee_amount);

  internal static extern CfdErrorCode CfdGetSelectedCoinIndex(
      void* handle, void* coin_select_handle, uint32_t index,
      int32_t* utxo_index);

  internal static extern CfdErrorCode CfdGetSelectedCoinAssetAmount(
      void* handle, void* coin_select_handle, uint32_t asset_index,
      int64_t* amount);

  internal static extern CfdErrorCode CfdFreeCoinSelectionHandle(
      void* handle, void* coin_select_handle);

  internal static extern CfdErrorCode CfdInitializeEstimateFee(
      void* handle, void** fee_handle, bool is_elements);

  internal static extern CfdErrorCode CfdAddTxInForEstimateFee(
      void* handle, void* fee_handle, const char* txid, uint32_t vout,
      const char* descriptor, const char* asset, bool is_issuance,
      bool is_blind_issuance, bool is_pegin, uint32_t pegin_btc_tx_size,
      const char* fedpeg_script);

  internal static extern CfdErrorCode CfdFinalizeEstimateFee(
      void* handle, void* fee_handle, const char* tx_hex, const char* fee_asset,
      int64_t* tx_fee, int64_t* utxo_fee, bool is_blind,
      double effective_fee_rate);

  internal static extern CfdErrorCode CfdFreeEstimateFeeHandle(void* handle, void* fee_handle);
    }
  */
}
