using System;

namespace Cfd
{
  /// <summary>
  /// block hash class.
  /// </summary>
  public class BlockHash : Txid
  {
    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public BlockHash() : base()
    {
    }

    public BlockHash(string hash) : base(hash)
    {
    }

    public BlockHash(byte[] bytes) : base(bytes)
    {
    }
  }
}
