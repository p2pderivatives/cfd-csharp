/// <summary>
/// cfd library namespace.
/// </summary>
namespace Cfd
{
  //! txin sequence locktime
  public enum CfdSequenceLockTime : uint
  {
    /// disable locktime
    Disable = 0xffffffff,
    /// enable locktime (maximum time)
    EnableMax = 0xfffffffe,
  };

  /// <summary>
  /// Transaction input class.
  /// </summary>
  public struct TxIn
  {
    public OutPoint OutPoint { get; }
    public Script ScriptSig { get; }
    public ScriptWitness WitnessStack { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    public TxIn(OutPoint outPoint)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = new Script();
      this.WitnessStack = new ScriptWitness();
    }

    public TxIn(OutPoint outPoint, ScriptWitness scriptWitness)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = new Script();
      this.WitnessStack = scriptWitness;
    }

    public TxIn(OutPoint outPoint, Script scriptSig, ScriptWitness scriptWitness)
    {
      this.OutPoint = outPoint;
      this.ScriptSig = scriptSig;
      this.WitnessStack = scriptWitness;
    }
  };

  /// <summary>
  /// Transaction output class.
  /// </summary>
  public struct TxOut
  {
    public long SatoshiValue { get; }
    public Script ScriptPubkey { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="outPoint">outpoint</param>
    public TxOut(long satoshiValue, Script scriptPubkey)
    {
      this.SatoshiValue = satoshiValue;
      this.ScriptPubkey = scriptPubkey;
    }
  };
}
