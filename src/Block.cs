using System;

namespace Cfd
{
  public struct BlockHeader : IEquatable<BlockHeader>
  {
    public uint Version { get; }
    public BlockHash PrevBlockHash { get; }
    public BlockHash MerkleRoot { get; }
    public uint Time { get; }
    public uint Bits { get; }
    public uint Nonce { get; }

    public BlockHeader(uint version, BlockHash prevBlockHash, BlockHash merkleRoot, uint time, uint bits, uint nonce)
    {
      Version = version;
      PrevBlockHash = prevBlockHash;
      MerkleRoot = merkleRoot;
      Time = time;
      Bits = bits;
      Nonce = nonce;
    }

    public bool Equals(BlockHeader other)
    {
      return (Version == other.Version) && (PrevBlockHash == other.PrevBlockHash)
        && (MerkleRoot == other.MerkleRoot) && (Time == other.Time);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if (obj is BlockHeader)
      {
        return Equals((BlockHeader)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Version.GetHashCode() + PrevBlockHash.GetHashCode()
        + MerkleRoot.GetHashCode() + Time.GetHashCode();
    }

    public static bool operator ==(BlockHeader left, BlockHeader right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(BlockHeader left, BlockHeader right)
    {
      return !(left == right);
    }
  };

  /// <summary>
  /// block class.
  /// </summary>
  public class Block
  {
    private readonly string hex;
    private readonly BlockHash hash;
    public static readonly int defaultNetType = (int)CfdNetworkType.Mainnet;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public Block()
    {
      hex = string.Empty;
      hash = new BlockHash();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="blockHex">block hex</param>
    public Block(string blockHex)
    {
      hex = blockHex;
      hash = GetHash(hex);
    }

    /// <summary>
    /// Constructor. (valid block)
    /// </summary>
    /// <param name="bytes">blinder</param>
    public Block(byte[] bytes)
    {
      hex = StringUtil.FromBytes(bytes);
      hash = GetHash(hex);
    }

    /// <summary>
    /// block hex string.
    /// </summary>
    /// <returns>block hex string</returns>
    public string ToHexString()
    {
      return hex;
    }

    /// <summary>
    /// byte array.
    /// </summary>
    /// <returns>byte array</returns>
    public byte[] GetBytes()
    {
      return StringUtil.ToBytes(hex);
    }

    /// <summary>
    /// byte data.
    /// </summary>
    /// <returns>byte data</returns>
    public ByteData GetData()
    {
      return new ByteData(hex);
    }

    /// <summary>
    /// check empty byte data.
    /// </summary>
    /// <returns>true is empty byte data.</returns>
    public bool IsEmpty()
    {
      return hex.Length == 0;
    }

    /// <summary>
    /// Get block hash.
    /// </summary>
    /// <returns>block hash</returns>
    public BlockHash GetBlockHash()
    {
      return hash;
    }

    /// <summary>
    /// Get block header.
    /// </summary>
    /// <returns>block header</returns>
    public BlockHeader GetHeader()
    {
      using (var handle = new ErrorHandle())
      using (var txHandle = new BlockHandle(handle, defaultNetType, hex))
      {
        var ret = NativeMethods.CfdGetBlockHeaderData(
            handle.GetHandle(), txHandle.GetHandle(),
            out uint version, out IntPtr prevBlockHash, out IntPtr merkleRoot,
            out uint time, out uint bits, out uint nonce);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var prevHash = CCommon.ConvertToString(prevBlockHash);
        var merkleRootHash = CCommon.ConvertToString(merkleRoot);
        return new BlockHeader(version, new BlockHash(prevHash),
          new BlockHash(merkleRootHash), time, bits, nonce);
      }
    }

    /// <summary>
    /// Get transaction count in block.
    /// </summary>
    /// <returns>transaction count</returns>
    public uint GetTxCount()
    {
      using (var handle = new ErrorHandle())
      using (var txHandle = new BlockHandle(handle, defaultNetType, hex))
      {
        var ret = NativeMethods.CfdGetTxCountInBlock(
            handle.GetHandle(), txHandle.GetHandle(), out uint txCount);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return txCount;
      }
    }

    /// <summary>
    /// Get txid list.
    /// </summary>
    /// <returns>txid list</returns>
    public Txid[] GetTxidList()
    {
      using (var handle = new ErrorHandle())
      using (var txHandle = new BlockHandle(handle, defaultNetType, hex))
      {
        var ret = NativeMethods.CfdGetTxCountInBlock(
            handle.GetHandle(), txHandle.GetHandle(), out uint txCount);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }

        var result = new Txid[txCount];
        for (uint index = 0; index < txCount; ++index)
        {
          ret = NativeMethods.CfdGetTxidFromBlock(
              handle.GetHandle(), txHandle.GetHandle(), index, out IntPtr txidHex);
          if (ret != CfdErrorCode.Success)
          {
            handle.ThrowError(ret);
          }
          var txid = CCommon.ConvertToString(txidHex);
          result[index] = new Txid(txid);
        }
        return result;
      }
    }

    /// <summary>
    /// Exist txid in block.
    /// </summary>
    /// <param name="txid">txid</param>
    /// <returns>true is exist txid.</returns>
    public bool ExistTxid(Txid txid)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      using (var handle = new ErrorHandle())
      using (var txHandle = new BlockHandle(handle, defaultNetType, hex))
      {
        var ret = NativeMethods.CfdExistTxidInBlock(
            handle.GetHandle(), txHandle.GetHandle(), txid.ToHexString());
        if (ret == CfdErrorCode.Success)
        {
          return true;
        }
        else if (ret != CfdErrorCode.NotFound)
        {
          handle.ThrowError(ret);
        }
        return false;
      }
    }

    /// <summary>
    /// Get transaction from block.
    /// </summary>
    /// <param name="txid">txid</param>
    /// <returns>transaction</returns>
    public Transaction GetTransaction(Txid txid)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      using (var handle = new ErrorHandle())
      using (var txHandle = new BlockHandle(handle, defaultNetType, hex))
      {
        var ret = NativeMethods.CfdGetTransactionFromBlock(
            handle.GetHandle(), txHandle.GetHandle(), txid.ToHexString(), out IntPtr tx);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var txHex = CCommon.ConvertToString(tx);
        return new Transaction(txHex);
      }
    }

    /// <summary>
    /// Get txout proof.
    /// </summary>
    /// <param name="txid">txid</param>
    /// <returns>txoutproof</returns>
    public ByteData GetTxOutProof(Txid txid)
    {
      if (txid is null)
      {
        throw new ArgumentNullException(nameof(txid));
      }
      using (var handle = new ErrorHandle())
      using (var txHandle = new BlockHandle(handle, defaultNetType, hex))
      {
        var ret = NativeMethods.CfdGetTxOutProof(
            handle.GetHandle(), txHandle.GetHandle(), txid.ToHexString(), out IntPtr txoutProof);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var proof = CCommon.ConvertToString(txoutProof);
        return new ByteData(proof);
      }
    }

    /// <summary>
    /// Get block hash (internal).
    /// </summary>
    /// <param name="hex">block hex</param>
    /// <returns>block hash</returns>
    private static BlockHash GetHash(string hex)
    {
      using (var handle = new ErrorHandle())
      using (var txHandle = new BlockHandle(handle, defaultNetType, hex))
      {
        var ret = NativeMethods.CfdGetBlockHash(
            handle.GetHandle(), txHandle.GetHandle(), out IntPtr hash);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        var blockHash = CCommon.ConvertToString(hash);
        return new BlockHash(blockHash);
      }
    }
  }


  /// <summary>
  /// block handle wrapper class.
  /// </summary>
  internal class BlockHandle : IDisposable
  {
    private readonly IntPtr handle;
    private bool disposed;
    private readonly ErrorHandle errHandle;

    /// <summary>
    /// constructor.
    /// </summary>
    public BlockHandle(ErrorHandle errorhandle, int networkType, string blockHex)
    {
      var ret = NativeMethods.CfdInitializeBlockHandle(
        errorhandle.GetHandle(), networkType, blockHex, out IntPtr newHandle);
      if ((ret != CfdErrorCode.Success) || (newHandle == IntPtr.Zero))
      {
        throw new InvalidOperationException();
      }
      handle = newHandle;
      errHandle = errorhandle;
    }

    public IntPtr GetHandle()
    {
      return handle;
    }

    ~BlockHandle()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);  // Dispose of unmanaged resources.
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Throw exception from error handle and error code.
    /// </summary>
    /// <param name="errorCode">error code</param>
    /// <exception cref="ArgumentOutOfRangeException">argument range exception</exception>
    /// <exception cref="ArgumentException">argument exception</exception>
    /// <exception cref="InsufficientMemoryException">memory full exception</exception>
    /// <exception cref="InvalidOperationException">illegal exception</exception>
    public void ThrowError(CfdErrorCode errorCode)
    {
      if ((errorCode == CfdErrorCode.Success) || disposed)
      {
        return;
      }

      string errorMessage;
      var ret = CCommon.CfdGetLastErrorMessage(handle, out IntPtr messageAddress);
      if (ret == CfdErrorCode.Success)
      {
        string message = CCommon.ConvertToString(messageAddress);
        errorMessage = $"CFD error[{errorCode}] message:{message}";
      }
      else
      {
        errorMessage = $"CFD error[{errorCode}]";
      }
      CfdCommon.ThrowError(errorCode, errorMessage);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        NativeMethods.CfdFreeBlockHandle(errHandle.GetHandle(), handle);
        disposed = true;
      }
    }
  }
}
