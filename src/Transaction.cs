using System;
using System.Runtime.InteropServices;

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
}
