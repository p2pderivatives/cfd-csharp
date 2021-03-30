using System;

namespace Cfd
{
  /// <summary>
  /// tapbranch class.
  /// </summary>
  public class TapBranch
  {
    /// <summary>
    /// tapscript leaf version.
    /// </summary>
    public static readonly byte tapscriptLeafVersion = 0xc0;
    private Script tapscript;
    private byte leafVersion;
    private TapBranch[] branches;
    private ByteData256 topHash;
    private string treeString;
    private ByteData256[] targetNodes;

    /// <summary>
    /// Constructor. (default)
    /// </summary>
    public TapBranch()
    {
      tapscript = new Script();
      leafVersion = 0;
      branches = Array.Empty<TapBranch>();
      topHash = new ByteData256();
      treeString = "";
      targetNodes = Array.Empty<ByteData256>();
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="tapscript">tapscript</param>
    public TapBranch(Script tapscript) : this(tapscript, tapscriptLeafVersion)
    {
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="tapscript">tapscript</param>
    /// <param name="leafVersion">leaf version</param>
    public TapBranch(Script tapscript, byte leafVersion)
    {
      if (tapscript is null)
      {
        throw new ArgumentNullException(nameof(tapscript));
      }
      this.tapscript = tapscript;
      this.leafVersion = leafVersion;
      treeString = ConvertTapscriptToString(tapscript, leafVersion);
      targetNodes = Array.Empty<ByteData256>();
      branches = Array.Empty<TapBranch>();
      topHash = new ByteData256();

      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        Load(handle, treeHandle);
        UpdateBranchData(handle, treeHandle);
      }
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="hash">branch hash</param>
    public TapBranch(ByteData256 hash)
    {
      if (hash is null)
      {
        throw new ArgumentNullException(nameof(hash));
      }
      if (hash.GetSize() != 32)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Invalid branch hash size.");
      }
      treeString = hash.ToHexString();
      targetNodes = Array.Empty<ByteData256>();
      branches = Array.Empty<TapBranch>();
      topHash = hash;
      tapscript = new Script();
      leafVersion = 0;
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="hash">branch hash</param>
    public TapBranch(string treeStr)
    {
      if (treeStr is null)
      {
        throw new ArgumentNullException(nameof(treeStr));
      }
      treeString = treeStr;
      targetNodes = Array.Empty<ByteData256>();
      branches = Array.Empty<TapBranch>();
      topHash = new ByteData256();
      tapscript = new Script();
      leafVersion = 0;

      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        Load(handle, treeHandle);
        GetAllData(handle, treeHandle);
      }
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="treeStr">tree string</param>
    /// <param name="tapscript">tapscript</param>
    /// <param name="targetNodes">target branch hash list</param>
    public TapBranch(string treeStr, Script tapscript) : this(treeStr, tapscript, null)
    {
    }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="treeStr">tree string</param>
    /// <param name="tapscript">tapscript</param>
    /// <param name="targetNodes">target node list</param>
    public TapBranch(string treeStr, Script tapscript, ByteData256[] targetNodes)
    {
      if (treeStr is null)
      {
        throw new ArgumentNullException(nameof(treeStr));
      }
      if (tapscript is null)
      {
        throw new ArgumentNullException(nameof(tapscript));
      }
      ByteData256[] nodes = Array.Empty<ByteData256>();
      if (!(targetNodes is null))
      {
        foreach (var node in targetNodes)
        {
          if (node.GetSize() != 32)
          {
            CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Invalid branch hash size.");
          }
        }
        nodes = targetNodes;
      }
      treeString = treeStr;
      this.targetNodes = nodes;
      branches = Array.Empty<TapBranch>();
      topHash = new ByteData256();
      this.tapscript = tapscript;
      leafVersion = tapscriptLeafVersion;

      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        Load(handle, treeHandle);
        GetAllData(handle, treeHandle);
      }
    }

    internal void Initialize(string treeString, Script tapscript, ByteData256[] targetNodes)
    {
      this.treeString = treeString;
      this.targetNodes = targetNodes;
      branches = Array.Empty<TapBranch>();
      topHash = new ByteData256();
      this.tapscript = tapscript;
      leafVersion = tapscriptLeafVersion;
    }

    /// <summary>
    /// Add branch.
    /// </summary>
    /// <param name="branch">tapbranch</param>
    public void AddBranch(TapBranch branch)
    {
      if (branch is null)
      {
        throw new ArgumentNullException(nameof(branch));
      }
      AddBranch(branch.treeString);
    }

    /// <summary>
    /// Add branch.
    /// </summary>
    /// <param name="hash">branch hash.</param>
    public void AddBranch(ByteData hash)
    {
      if (hash is null)
      {
        throw new ArgumentNullException(nameof(hash));
      }
      if (hash.GetSize() != 32)
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalArgumentError, "Invalid branch hash size.");
      }
      AddBranch(hash.ToHexString());
    }

    /// <summary>
    /// Add branch. (adding tapleaf)
    /// </summary>
    /// <param name="tapscript">tapscript.</param>
    public void AddBranch(Script tapscript)
    {
      AddBranch(tapscript, tapscriptLeafVersion);
    }

    /// <summary>
    /// Add branch. (adding tapleaf)
    /// </summary>
    /// <param name="tapscript">tapscript.</param>
    /// <param name="leafVersion">leaf version</param>
    public void AddBranch(Script tapscript, byte leafVersion)
    {
      if (tapscript is null)
      {
        throw new ArgumentNullException(nameof(tapscript));
      }
      AddBranch(ConvertTapscriptToString(tapscript, leafVersion));
    }

    /// <summary>
    /// Add branch.
    /// </summary>
    /// <param name="treeString">tree string.</param>
    public void AddBranch(string treeString)
    {
      if (treeString is null)
      {
        throw new ArgumentNullException(nameof(treeString));
      }
      AddBranches(new string[] { treeString });
    }

    /// <summary>
    /// Add branches.
    /// </summary>
    /// <param name="branches">branch list</param>
    public void AddBranches(TapBranch[] branches)
    {
      if (branches is null)
      {
        throw new ArgumentNullException(nameof(branches));
      }
      string[] strList = new string[branches.Length];
      for (var index = 0; index < branches.Length; ++index)
      {
        strList[index] = branches[index].treeString;
      }
      AddBranches(strList);
    }

    /// <summary>
    /// Add branches. (for simple tapscript tree)
    /// </summary>
    /// <param name="tapscriptList">tapscript list</param>
    public void AddBranches(Script[] tapscriptList)
    {
      if (tapscriptList is null)
      {
        throw new ArgumentNullException(nameof(tapscriptList));
      }
      string[] strList = new string[tapscriptList.Length];
      for (var index = 0; index < tapscriptList.Length; ++index)
      {
        strList[index] = ConvertTapscriptToString(tapscriptList[index], tapscriptLeafVersion);
      }
      AddBranches(strList);
    }

    /// <summary>
    /// Add branches.
    /// </summary>
    /// <param name="treeStringList">tree string list</param>
    public void AddBranches(string[] treeStringList)
    {
      if (treeStringList is null)
      {
        throw new ArgumentNullException(nameof(treeStringList));
      }
      using (var handle = new ErrorHandle())
      using (var treeHandle = new TreeHandle(handle))
      {
        Load(handle, treeHandle);
        foreach (var treeStr in treeStringList)
        {
          var ret = NativeMethods.CfdAddTapBranchByScriptTreeString(
            handle.GetHandle(), treeHandle.GetHandle(), treeStr);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
        }
        UpdateAllData(handle, treeHandle);
      }
    }

    /// <summary>
    /// Check exist tapscript on this branch root.
    /// </summary>
    /// <returns>true or false</returns>
    public bool HasTapScript()
    {
      return !tapscript.IsEmpty();
    }

    /// <summary>
    /// Get tapscript if existing tapscript on branch root.
    /// </summary>
    /// <returns></returns>
    public Script GetTapScript()
    {
      if (tapscript.IsEmpty())
      {
        CfdCommon.ThrowError(CfdErrorCode.IllegalStateError, "tapscript not found.");
      }
      return tapscript;
    }

    /// <summary>
    /// Get current top branch hash.
    /// </summary>
    /// <returns>branch hash</returns>
    public ByteData256 GetCurrentHash()
    {
      return topHash;
    }

    /// <summary>
    /// Get tree string.
    /// </summary>
    /// <returns>tree string</returns>
    public override string ToString()
    {
      return treeString;
    }

    /// <summary>
    /// Get leaf version
    /// </summary>
    /// <returns>leaf version</returns>
    public byte GetLeafVersion()
    {
      return leafVersion;
    }

    /// <summary>
    /// Get branch by index.
    /// </summary>
    /// <param name="index">branch index</param>
    /// <returns>tapbranch</returns>
    public TapBranch GetBranch(uint index)
    {
      return branches[index];
    }

    /// <summary>
    /// Get branch count on this branch.
    /// </summary>
    /// <returns>branch count</returns>
    public uint GetBranchCount()
    {
      return (uint)branches.Length;
    }

    /// <summary>
    /// Get branch hash list for this tapleaf.
    /// </summary>
    /// <returns>branch hash list</returns>
    public ByteData256[] GetTargetNodes()
    {
      return targetNodes;
    }

    protected static string ConvertTapscriptToString(Script tapscript, byte leafVersion)
    {
      if (tapscript is null)
      {
        throw new ArgumentNullException(nameof(tapscript));
      }
      string treeStr = "tl(" + tapscript.ToHexString();
      if (leafVersion != tapscriptLeafVersion)
      {
        ByteData data = new ByteData(new byte[] { leafVersion });
        treeStr += "," + data.ToHexString();
      }
      treeStr += ")";
      return treeStr;
    }

    internal void Load(ErrorHandle handle, TreeHandle treeHandle)
    {
      if (handle is null)
      {
        throw new ArgumentNullException(nameof(handle));
      }
      if (treeHandle is null)
      {
        throw new ArgumentNullException(nameof(treeHandle));
      }

      string nodes = "";
      foreach (var node in targetNodes)
      {
        nodes += node.ToHexString();
      }
      var ret = NativeMethods.CfdSetScriptTreeFromString(handle.GetHandle(),
          treeHandle.GetHandle(), treeString, tapscript.ToHexString(), leafVersion, nodes);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
    }

    void UpdateBranchData(ErrorHandle handle, TreeHandle treeHandle)
    {
      if (handle is null)
      {
        throw new ArgumentNullException(nameof(handle));
      }
      if (treeHandle is null)
      {
        throw new ArgumentNullException(nameof(treeHandle));
      }

      var ret = NativeMethods.CfdGetTaprootScriptTreeSrting(handle.GetHandle(),
         treeHandle.GetHandle(), out IntPtr treeString);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      this.treeString = CCommon.ConvertToString(treeString);
      if (branches.Length != 0)
      {
        var index = branches.Length - 1;
        ret = NativeMethods.CfdGetTapBranchData(handle.GetHandle(),
            treeHandle.GetHandle(), (byte)index, true, out IntPtr branchHash,
            out _, out IntPtr tempScript, out byte _);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(tempScript);
        topHash = new ByteData256(CCommon.ConvertToString(branchHash));
      }
      else if (topHash.IsEmpty())
      {
        ret = NativeMethods.CfdGetBaseTapLeaf(handle.GetHandle(),
          treeHandle.GetHandle(), out byte _,
          out IntPtr tapscript, out IntPtr tapLeafHash);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(tapscript);
        string tapLeafHashStr = CCommon.ConvertToString(tapLeafHash);
        topHash = new ByteData256(tapLeafHashStr);
      }
    }

    static TapBranch GetBranchData(ErrorHandle handle, TreeHandle treeHandle)
    {
      TapBranch branch = new TapBranch();
      var ret = NativeMethods.CfdGetTapBranchCount(
        handle.GetHandle(), treeHandle.GetHandle(), out uint count);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      TapBranch[] branchList = new TapBranch[count];
      ret = NativeMethods.CfdGetBaseTapLeaf(handle.GetHandle(),
          treeHandle.GetHandle(), out byte leafVersion,
          out IntPtr tapscript, out IntPtr tapLeafHash);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      string tapscriptStr = CCommon.ConvertToString(tapscript);
      string tapLeafHashStr = CCommon.ConvertToString(tapLeafHash);
      if (count == 0)
      {
        branch.topHash = new ByteData256(tapLeafHashStr);
      }
      branch.branches = branchList;
      if (tapscriptStr.Length != 0)
      {
        branch.tapscript = new Script(tapscriptStr);
        branch.leafVersion = leafVersion;
        branch.targetNodes = new ByteData256[count];
      }
      branch.UpdateBranchData(handle, treeHandle);
      return branch;
    }

    internal void GetAllData(ErrorHandle handle, TreeHandle treeHandle)
    {
      var tree = GetBranchData(handle, treeHandle);
      var count = tree.branches.Length;
      TapBranch[] branchList = new TapBranch[count];
      ByteData256[] nodeList = new ByteData256[count];
      for (var index = 0; index < count; ++index)
      {
        var ret = NativeMethods.CfdGetTapBranchHandle(
          handle.GetHandle(), treeHandle.GetHandle(), (byte)index,
          out IntPtr branchHash, out IntPtr branchTreeHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(branchHash);
        using (var branchHandle = new TreeHandle(handle, branchTreeHandle))
        {
          TapBranch branch = new TapBranch();
          branch.GetAllData(handle, branchHandle);
          branchList[index] = branch;
        }
      }
      if (count != 0)
      {
        branches = branchList;
      }
      topHash = tree.topHash;
      treeString = tree.treeString;
      if (!tree.tapscript.IsEmpty())
      {
        tapscript = tree.tapscript;
        leafVersion = tree.leafVersion;
        for (var index = 0; index < count; ++index)
        {
          nodeList[index] = branchList[index].topHash;
        }
        targetNodes = nodeList;
      }
    }

    void UpdateAllData(ErrorHandle handle, TreeHandle treeHandle)
    {
      var ret = NativeMethods.CfdGetTapBranchCount(
        handle.GetHandle(), treeHandle.GetHandle(), out uint count);
      if (ret != CfdErrorCode.Success)
      {
        handle.ThrowError(ret);
      }
      TapBranch[] branchList = new TapBranch[count];
      var offset = branches.Length;
      for (var index = offset; index < count; ++index)
      {
        ret = NativeMethods.CfdGetTapBranchHandle(
          handle.GetHandle(), treeHandle.GetHandle(), (byte)index,
          out IntPtr branchHash, out IntPtr branchTreeHandle);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        CCommon.ConvertToString(branchHash);
        using (var branchHandle = new TreeHandle(handle, branchTreeHandle))
        {
          TapBranch branch = new TapBranch();
          branch.GetAllData(handle, branchHandle);
          branchList[index] = branch;
        }
      }
      if (count != 0)
      {
        var newTargetNodes = new ByteData256[count];
        for (var index = 0; index < offset; ++index)
        {
          branchList[index] = branches[index];
          newTargetNodes[index] = targetNodes[index];
        }
        for (var index = offset; index < count; ++index)
        {
          newTargetNodes[index] = branchList[index].topHash;
        }
        branches = branchList;
        targetNodes = newTargetNodes;
      }
      UpdateBranchData(handle, treeHandle);
    }
  }
}
