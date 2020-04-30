// using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
// using System.Runtime.InteropServices;
using Cfd;
using Xunit;

namespace Cfd.xTests
{
  public class CfdAddressTest
  {
    [Fact]
    public void AddressTest()
    {
      string addrStr = "bcrt1q576jgpgewxwu205cpjq4s4j5tprxlq38l7kd85";
      Cfd.Address addr = new Cfd.Address(addrStr);
      Assert.Equal(addrStr, addr.ToAddressString());
      Assert.Equal("0014a7b5240519719dc53e980c8158565458466f8227", addr.GetLockingScript().ToHexString());
      Assert.Equal("a7b5240519719dc53e980c8158565458466f8227", addr.GetHash());

      Cfd.Pubkey pubkey = new Cfd.Pubkey("031d7463018f867de51a27db866f869ceaf52abab71827a6051bab8a0fd020f4c1");
      addr = new Cfd.Address(pubkey, Cfd.CfdAddressType.P2wpkh, Cfd.CfdNetworkType.ElementsRegtest);
      Assert.Equal("ert1q7jm5vw5cunpy3lkvwdl3sr3qfm794xd4zr6z3k", addr.ToAddressString());
      Assert.Equal("0014f4b7463a98e4c248fecc737f180e204efc5a99b5", addr.GetLockingScript().ToHexString());
    }

    [Fact]
    public void ConfidentialAddressTest()
    {
      string ctAddr = "VTpvKKc1SNmLG4H8CnR1fGJdHdyWGEQEvdP9gfeneJR7n81S5kiwNtgF7vrZjC8mp63HvwxM81nEbTxU";
      string address = "Q7wegLt2qMGhm28vch6VTzvpzs8KXvs4X7";
      string ctKey = "025476c2e83188368da1ff3e292e7acafcdb3566bb0ad253f62fc70f07aeee6357";
      Cfd.ConfidentialAddress caddr = new Cfd.ConfidentialAddress(ctAddr);
      Assert.Equal(ctAddr, caddr.ToAddressString());
      Assert.Equal(address, caddr.GetAddress().ToAddressString());
      Assert.Equal("76a914751e76e8199196d454941c45d1b3a323f1433bd688ac", caddr.GetAddress().GetLockingScript().ToHexString());
      Assert.Equal(ctKey, caddr.GetConfidentialKey().ToHexString());

      Cfd.Address addr = new Cfd.Address(address);
      Cfd.Pubkey pubkey = new Cfd.Pubkey(ctKey);
      caddr = new Cfd.ConfidentialAddress(addr, pubkey);
      Assert.Equal(ctAddr, caddr.ToAddressString());
      Assert.Equal(address, caddr.GetAddress().ToAddressString());
      Assert.Equal("76a914751e76e8199196d454941c45d1b3a323f1433bd688ac", caddr.GetAddress().GetLockingScript().ToHexString());
      Assert.Equal(ctKey, caddr.GetConfidentialKey().ToHexString());
    }
  }
}
