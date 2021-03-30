using System;

namespace Cfd
{
  /// <summary>
  /// taproot script data.
  /// </summary>
  public struct TaprootScriptData : IEquatable<TaprootScriptData>
  {
    /// <summary>
    /// tweaked schnorr public key. (taproot witness program)
    /// </summary>
    public SchnorrPubkey Pubkey { get; }
    /// <summary>
    /// Tapscript control block.
    /// (tapleaf version + parity + internal pubkey + branch hash)
    /// </summary>
    public ByteData ControlBlock { get; }
    /// <summary>
    /// TapLeaf hash. (for sighash)
    /// </summary>
    public ByteData256 TapLeafHash { get; }
    /// <summary>
    /// TapScript
    /// </summary>
    public Script TapScript { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="pubkey">schnorr pubkey</param>
    /// <param name="controlBlock">control block</param>
    /// <param name="tapLeafHash">tapleaf hash</param>
    /// <param name="tapScript">tapscript</param>
    public TaprootScriptData(SchnorrPubkey pubkey, ByteData controlBlock,
        ByteData256 tapLeafHash, Script tapScript)
    {
      Pubkey = pubkey;
      ControlBlock = controlBlock;
      TapLeafHash = tapLeafHash;
      TapScript = tapScript;
    }

    public bool Equals(TaprootScriptData other)
    {
      return Pubkey.Equals(other.Pubkey) &&
        ControlBlock.Equals(other.ControlBlock) &&
        TapScript.Equals(other.TapScript);
    }

    public override bool Equals(object obj)
    {
      if (obj is TaprootScriptData)
      {
        return Equals((TaprootScriptData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Pubkey.GetHashCode() + ControlBlock.GetHashCode() + TapScript.GetHashCode();
    }

    public static bool operator ==(TaprootScriptData left, TaprootScriptData right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(TaprootScriptData left, TaprootScriptData right)
    {
      return !(left == right);
    }
  }

  /// <summary>
  /// taproot script tree class.
  /// </summary>
  public class TaprootScriptTree : TapBranch
  {
    /// <summary>
    /// internal pubkey.
    /// </summary>
    private SchnorrPubkey internalPubkey;

    /// <summary>
    /// Get tree from control block.
    /// </summary>
    /// <param name="controlBlock">control block</param>
    /// <param name="tapscript">tapscript</param>
    /// <returns>Taproot script tree</returns>
    public static TaprootScriptTree GetFromControlBlock(
      ByteData controlBlock, Script tapscript)
    {
      return new TaprootScriptTree(controlBlock, tapscript);
    }

    /// <summary>
    /// Constructor. (default)
    /// </summary>
    public TaprootScriptTree()
    {
      internalPubkey = new SchnorrPubkey();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="tapscript">tapscript</param>
    public TaprootScriptTree(Script tapscript) : base(tapscript) {
      internalPubkey = new SchnorrPubkey();
    }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="tapscript">tapscript</param>
    /// <param name="leafVersion">leaf version</param>
    public TaprootScriptTree(Script tapscript, byte leafVersion) : base(tapscript, leafVersion) {
      internalPubkey = new SchnorrPubkey();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="treeStr">script tree string.</param>
    /// <param name="tapscript">tapscript</param>
    public TaprootScriptTree(string treeStr, Script tapscript) : base(treeStr, tapscript)
    {
      internalPubkey = new SchnorrPubkey();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="treeStr">script tree string.</param>
    /// <param name="tapscript">tapscript</param>
    /// <param name="targetNodes">branch hash list.</param>
    public TaprootScriptTree(string treeStr, Script tapscript, ByteData256[] targetNodes) : base(treeStr, tapscript, targetNodes)
    {
      internalPubkey = new SchnorrPubkey();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="origin">source branch.</param>
    public TaprootScriptTree(TapBranch origin)
    {
      if (origin is null)
      {
        throw new ArgumentNullException(nameof(origin));
      }
      var script = new Script();
      if (origin.HasTapScript()) script = origin.GetTapScript();
      Initialize(origin.ToString(), script, origin.GetTargetNodes());
      internalPubkey = new SchnorrPubkey();

      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        Load(handle, treeHandle);
        GetAllData(handle, treeHandle);
      }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="controlBlock">control block</param>
    /// <param name="tapscript">tapscript</param>
    public TaprootScriptTree(ByteData controlBlock, Script tapscript)
    {
      if (controlBlock is null)
      {
        throw new ArgumentNullException(nameof(controlBlock));
      }
      if (tapscript is null)
      {
        throw new ArgumentNullException(nameof(tapscript));
      }

      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        var ret = NativeMethods.CfdSetTapScriptByWitnessStack(handle.GetHandle(),
            treeHandle.GetHandle(), controlBlock.ToHexString(), tapscript.ToHexString(),
            out IntPtr internalPubkeyPtr);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        internalPubkey = new SchnorrPubkey(CCommon.ConvertToString(internalPubkeyPtr));
        GetAllData(handle, treeHandle);
      }
    }

    /// <summary>
    /// Set default internal pubkey.
    /// </summary>
    /// <param name="pubkey">internal pubkey</param>
    public void SetInternalPubkey(SchnorrPubkey pubkey)
    {
      if (pubkey is null)
      {
        throw new ArgumentNullException(nameof(pubkey));
      }
      else if (pubkey.IsValid())
      {
        internalPubkey = pubkey;
      }
    }

    /// <summary>
    /// Get taproot data.
    /// </summary>
    /// <returns>taproot data</returns>
    public TaprootScriptData GetTaprootData()
    {
      return GetTaprootData(internalPubkey);
    }

    /// <summary>
    /// Get taproot data.
    /// </summary>
    /// <param name="internalPubkey">internal pubkey</param>
    /// <returns>taproot data</returns>
    public TaprootScriptData GetTaprootData(SchnorrPubkey internalPubkey)
    {
      if (internalPubkey is null)
      {
        throw new ArgumentNullException(nameof(internalPubkey));
      }
      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        Load(handle, treeHandle);
        var ret = NativeMethods.CfdGetTaprootScriptTreeHash(
          handle.GetHandle(), treeHandle.GetHandle(), internalPubkey.ToHexString(),
          out IntPtr witnessProgram, out IntPtr tapLeafHashPtr, out IntPtr controlBlockStr);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var witnessProgramStr = CCommon.ConvertToString(witnessProgram);
        var tapLeafHash = CCommon.ConvertToString(tapLeafHashPtr);
        var controlBlock = CCommon.ConvertToString(controlBlockStr);

        return new TaprootScriptData(
          new SchnorrPubkey(witnessProgramStr), new ByteData(controlBlock),
          new ByteData256(tapLeafHash), GetTapScript());
      }
    }

    /// <summary>
    /// Get tweaked privkey from internal privkey.
    /// </summary>
    /// <param name="privkey">internal privkey.</param>
    /// <returns>tweaked privkey.</returns>
    public Privkey GetTweakedPrivkey(Privkey privkey)
    {
      if (privkey is null)
      {
        throw new ArgumentNullException(nameof(privkey));
      }
      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        Load(handle, treeHandle);
        var ret = NativeMethods.CfdGetTaprootTweakedPrivkey(
          handle.GetHandle(), treeHandle.GetHandle(), privkey.ToHexString(),
          out IntPtr tweakedPrivkey);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var tweakedPrivkeyStr = CCommon.ConvertToString(tweakedPrivkey);

        return new Privkey(tweakedPrivkeyStr);
      }
    }
  }
}
