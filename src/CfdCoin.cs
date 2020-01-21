using System;
using System.Runtime.InteropServices;

namespace Cfd
{
  /*
    internal class CCoin
    {

  CFDC_API int CfdInitializeCoinSelection(
      void* handle, uint32_t utxo_count, uint32_t target_asset_count,
      const char* fee_asset, int64_t tx_fee_amount, double effective_fee_rate,
      double long_term_fee_rate, double dust_fee_rate,
      int64_t knapsack_min_change, void** coin_select_handle);

  CFDC_API int CfdAddCoinSelectionUtxo(
      void* handle, void* coin_select_handle, int32_t utxo_index,
      const char* txid, uint32_t vout, int64_t amount, const char* asset,
      const char* descriptor);

  CFDC_API int CfdAddCoinSelectionAmount(
      void* handle, void* coin_select_handle, uint32_t asset_index,
      int64_t amount, const char* asset);

  CFDC_API int CfdFinalizeCoinSelection(
      void* handle, void* coin_select_handle, int64_t* utxo_fee_amount);

  CFDC_API int CfdGetSelectedCoinIndex(
      void* handle, void* coin_select_handle, uint32_t index,
      int32_t* utxo_index);

  CFDC_API int CfdGetSelectedCoinAssetAmount(
      void* handle, void* coin_select_handle, uint32_t asset_index,
      int64_t* amount);

  CFDC_API int CfdFreeCoinSelectionHandle(
      void* handle, void* coin_select_handle);

  CFDC_API int CfdInitializeEstimateFee(
      void* handle, void** fee_handle, bool is_elements);

  CFDC_API int CfdAddTxInForEstimateFee(
      void* handle, void* fee_handle, const char* txid, uint32_t vout,
      const char* descriptor, const char* asset, bool is_issuance,
      bool is_blind_issuance, bool is_pegin, uint32_t pegin_btc_tx_size,
      const char* fedpeg_script);

  CFDC_API int CfdFinalizeEstimateFee(
      void* handle, void* fee_handle, const char* tx_hex, const char* fee_asset,
      int64_t* tx_fee, int64_t* utxo_fee, bool is_blind,
      double effective_fee_rate);

  CFDC_API int CfdFreeEstimateFeeHandle(void* handle, void* fee_handle);
    }
  */
}
