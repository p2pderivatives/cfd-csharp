using System;

namespace Cfd
{
  /// <summary>
  /// byte data class.
  /// </summary>
  public class ByteData : IEquatable<ByteData>
  {
    private readonly string data;
    private readonly bool hasReverse;

    /// <summary>
    /// Constructor. (empty)
    /// </summary>
    public ByteData()
    {
      data = "";
      hasReverse = false;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bytes">byte array</param>
    public ByteData(byte[] bytes)
    {
      if (bytes is null)
      {
        throw new ArgumentNullException(nameof(bytes));
      }
      data = StringUtil.FromBytes(bytes);
      hasReverse = false;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hex">hex string</param>
    public ByteData(string hex) : this(hex, false)
    {
      // do nothing
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="hex">hex string</param>
    /// <param name="hasReverse">reversed data</param>
    public ByteData(string hex, bool hasReverse)
    {
      if (hex is null)
      {
        throw new ArgumentNullException(nameof(hex));
      }
      data = hex;
      this.hasReverse = hasReverse;
    }

    /// <summary>
    /// Serialize byte data.
    /// </summary>
    /// <returns>serialized byte data.</returns>
    public ByteData Serialize()
    {
      using (var handle = new ErrorHandle())
      {
        var ret = NativeMethods.CfdSerializeByteData(
          handle.GetHandle(), data, out IntPtr output);
        if (ret != CfdErrorCode.Success)
        {
          handle.ThrowError(ret);
        }
        return new ByteData(CCommon.ConvertToString(output));
      }
    }

    /// <summary>
    /// hex string.
    /// </summary>
    /// <returns>hex string</returns>
    public string ToHexString()
    {
      return data;
    }

    /// <summary>
    /// byte array.
    /// </summary>
    /// <returns>byte array</returns>
    public byte[] ToBytes()
    {
      if (hasReverse)
      {
        return CfdCommon.ReverseBytes(StringUtil.ToBytes(data));
      }
      else
      {
        return StringUtil.ToBytes(data);
      }
    }

    public uint GetSize()
    {
      return (uint)data.Length / 2;
    }

    public uint GetLength()
    {
      return GetSize();
    }

    public bool IsEmpty()
    {
      return data.Length == 0;
    }
    public bool Equals(ByteData other)
    {
      if (other is null)
      {
        return false;
      }
      if (ReferenceEquals(this, other))
      {
        return true;
      }
      return data.Equals(other.data, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
      if (obj is null)
      {
        return false;
      }
      if ((obj as ByteData) != null)
      {
        return Equals((ByteData)obj);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return data.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(ByteData lhs, ByteData rhs)
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

    public static bool operator !=(ByteData lhs, ByteData rhs)
    {
      return !(lhs == rhs);
    }
  }
}
