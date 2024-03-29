using System;

namespace Cfd
{
  /// <summary>
  /// utxo data for elements class.
  /// </summary>
  public class ElementsUtxoData : UtxoData, IEquatable<ElementsUtxoData>
  {
    private readonly ConfidentialValue value;
    // private readonly ConfidentialAsset asset;
    private readonly string unblindedAsset;
    private readonly bool isIssuance;
    private readonly bool isBlindIssuance;
    private readonly bool isPegin;
    private readonly uint peginBtcTxSize;
    private readonly uint peginTxoutproofSize;
    private readonly Script claimScript;
    private readonly BlindFactor assetBlindFactor;
    private readonly BlindFactor amountBlindFactor;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public ElementsUtxoData()
    {
      // do nothing
    }

    /// <summary>
    /// constructor on outpoint.
    /// </summary>
    /// <param name="outpoint"></param>
    public ElementsUtxoData(OutPoint outpoint) : base(outpoint)
    {
      assetBlindFactor = new BlindFactor();
      amountBlindFactor = new BlindFactor();
    }

    public ElementsUtxoData(OutPoint outpoint, Descriptor descriptor) : base(outpoint, descriptor)
    {
      assetBlindFactor = new BlindFactor();
      amountBlindFactor = new BlindFactor();
    }

    public ElementsUtxoData(OutPoint outpoint, ConfidentialAsset asset, long amount) : base(outpoint, amount)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if (asset.HasBlinding())
      {
        throw new InvalidOperationException("asset is blinded.");
      }
      unblindedAsset = asset.ToHexString();
      value = new ConfidentialValue(amount);
      assetBlindFactor = new BlindFactor();
      amountBlindFactor = new BlindFactor();
    }

    public ElementsUtxoData(OutPoint outpoint, ConfidentialAsset asset, ConfidentialValue value)
         : base(outpoint, ((value is null) ? 0 : value.GetSatoshiValue()))
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      if (asset.HasBlinding())
      {
        throw new InvalidOperationException("asset is blinded.");
      }
      this.value = value;
      unblindedAsset = asset.ToHexString();
      assetBlindFactor = new BlindFactor();
      amountBlindFactor = new BlindFactor();
    }

    public ElementsUtxoData(OutPoint outpoint, ConfidentialAsset asset, ConfidentialValue value, Descriptor descriptor)
         : this(outpoint, asset, value, descriptor, null)
    {
      // do nothing
    }

    public ElementsUtxoData(OutPoint outpoint, ConfidentialAsset asset, ConfidentialValue value, Descriptor descriptor, Script scriptsigTemplte)
         : base(outpoint, ((value is null) ? 0 : value.GetSatoshiValue()), descriptor, scriptsigTemplte)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if (value is null)
      {
        throw new ArgumentNullException(nameof(value));
      }
      if (asset.HasBlinding())
      {
        throw new InvalidOperationException("asset is blinded.");
      }
      this.value = value;
      unblindedAsset = asset.ToHexString();
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment)
          : this(outpoint, asset, amount, assetCommitment, valueCommitment, null, null)
    {
      // do nothing
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        BlindFactor assetBlinder, BlindFactor amountBlinder)
          : base(outpoint, amount)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if (assetCommitment is null)
      {
        throw new ArgumentNullException(nameof(assetCommitment));
      }
      if (valueCommitment is null)
      {
        throw new ArgumentNullException(nameof(valueCommitment));
      }
      unblindedAsset = asset;
      value = valueCommitment;
      assetBlindFactor = (assetBlinder is null) ? new BlindFactor() : assetBlinder;
      amountBlindFactor = (amountBlinder is null) ? new BlindFactor() : amountBlinder;
      if (!assetBlindFactor.IsEmpty() && assetCommitment.HasBlinding())
      {
        ConfidentialAsset generateAsset = GetAssetCommitment();
        if (generateAsset.Equals(assetCommitment))
        {
          throw new InvalidOperationException("unmatch asset commitment.");
        }
      }
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        Descriptor descriptor)
          : this(outpoint, asset, amount, assetCommitment, valueCommitment, descriptor, null, null)
    {
      // do nothing
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        Descriptor descriptor, BlindFactor assetBlinder, BlindFactor amountBlinder)
          : this(outpoint, asset, amount, assetCommitment, valueCommitment, descriptor, assetBlinder, amountBlinder, null)
    {
      // do nothing
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        Descriptor descriptor, BlindFactor assetBlinder, BlindFactor amountBlinder, Script scriptsigTemplate)
          : base(outpoint, amount, descriptor, scriptsigTemplate)
    {
      if (asset is null)
      {
        throw new ArgumentNullException(nameof(asset));
      }
      if (assetCommitment is null)
      {
        throw new ArgumentNullException(nameof(assetCommitment));
      }
      if (valueCommitment is null)
      {
        throw new ArgumentNullException(nameof(valueCommitment));
      }
      unblindedAsset = asset;
      value = valueCommitment;
      assetBlindFactor = (assetBlinder is null) ? new BlindFactor() : assetBlinder;
      amountBlindFactor = (amountBlinder is null) ? new BlindFactor() : amountBlinder;
      if (!assetBlindFactor.IsEmpty() && assetCommitment.HasBlinding())
      {
        ConfidentialAsset generateAsset = GetAssetCommitment();
        if (generateAsset.Equals(assetCommitment))
        {
          throw new InvalidOperationException("unmatch asset commitment.");
        }
      }
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        Descriptor descriptor, bool isBlindIssuance)
          : this(outpoint, asset, amount, assetCommitment, valueCommitment, descriptor, isBlindIssuance, null, null)
    {
      // do nothing
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        Descriptor descriptor, bool isBlindIssuance, BlindFactor assetBlinder, BlindFactor amountBlinder)
          : this(outpoint, asset, amount, assetCommitment, valueCommitment, descriptor, assetBlinder, amountBlinder)
    {
      isIssuance = true;
      this.isBlindIssuance = isBlindIssuance;
    }

    public ElementsUtxoData(OutPoint outpoint, string asset, long amount,
        Descriptor descriptor, uint peginBtcTxSize, uint peginTxoutproofSize, Script claimScript)
          : this(outpoint, asset, amount, new ConfidentialAsset(asset), new ConfidentialValue(amount), descriptor, peginBtcTxSize, peginTxoutproofSize, claimScript, null, null)
    {
      // do nothing
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        Descriptor descriptor, uint peginBtcTxSize, uint peginTxoutproofSize, Script claimScript)
          : this(outpoint, asset, amount, assetCommitment, valueCommitment, descriptor, peginBtcTxSize, peginTxoutproofSize, claimScript, null, null)
    {
      // do nothing
    }

    public ElementsUtxoData(OutPoint outpoint, string asset,
        long amount, ConfidentialAsset assetCommitment, ConfidentialValue valueCommitment,
        Descriptor descriptor, uint peginBtcTxSize, uint peginTxoutproofSize, Script claimScript, BlindFactor assetBlinder, BlindFactor amountBlinder)
          : this(outpoint, asset, amount, assetCommitment, valueCommitment, descriptor, assetBlinder, amountBlinder)
    {
      if (claimScript is null)
      {
        throw new ArgumentNullException(nameof(claimScript));
      }
      isPegin = true;
      this.peginBtcTxSize = peginBtcTxSize;
      this.peginTxoutproofSize = peginTxoutproofSize;
      this.claimScript = claimScript;
    }

    public string GetAsset()
    {
      return unblindedAsset;
    }

    public ConfidentialAsset GetAssetCommitment()
    {
      ConfidentialAsset unblindAsset = new ConfidentialAsset(this.unblindedAsset);
      return ConfidentialTransaction.GetAssetCommitment(unblindAsset, this.assetBlindFactor);
    }

    public ConfidentialValue GetValueCommitment()
    {
      return value;
    }

    public bool IsIssuance()
    {
      return isIssuance;
    }

    public bool IsBlindIssuance()
    {
      return isBlindIssuance;
    }

    public bool IsPegin()
    {
      return isPegin;
    }

    public uint GetPeginBtcTxSize()
    {
      return peginBtcTxSize;
    }

    public uint GetPeginTxoutproofSize()
    {
      return peginTxoutproofSize;
    }

    public Script GetClaimScript()
    {
      return claimScript;
    }

    public BlindFactor GetAssetBlindFactor()
    {
      return assetBlindFactor;
    }

    public BlindFactor GetAmountBlindFactor()
    {
      return amountBlindFactor;
    }

    public bool Equals(ElementsUtxoData other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return (GetOutPoint() == other.GetOutPoint());
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as ElementsUtxoData) != null)
      {
        return Equals((ElementsUtxoData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return GetOutPoint().GetHashCode();
    }

    public static bool operator ==(ElementsUtxoData lhs, ElementsUtxoData rhs)
    {
      if (lhs is null)
      {
        if (rhs is null)
        {
          return true;
        }
        return false;
      }
      return lhs.Equals(rhs);
    }

    public static bool operator !=(ElementsUtxoData lhs, ElementsUtxoData rhs)
    {
      return !(lhs == rhs);
    }
  }
}
