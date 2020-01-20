// using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
// using System.Runtime.InteropServices;
using Cfd;

namespace Cfd.Tests
{
  // [TestClass()]
  public class CfdTests
  {
    // [TestMethod()]
    public void TestAddress1() {
      Console.WriteLine("Address from text");
      Cfd.Address addr = new Cfd.Address("bcrt1q576jgpgewxwu205cpjq4s4j5tprxlq38l7kd85");
      Console.WriteLine("- Address = " + addr.ToAddressString());
      Console.WriteLine("- LockingScript = " + addr.GetLockingScript());
      Console.WriteLine("- Hash = " + addr.GetHash());
    }

    public static void Main() {
      CfdTests test_obj = new CfdTests();
      test_obj.TestAddress1();
      Console.WriteLine();

      Console.WriteLine("Address from Pubkey");
      Cfd.Pubkey pubkey = new Cfd.Pubkey("031d7463018f867de51a27db866f869ceaf52abab71827a6051bab8a0fd020f4c1");
      Cfd.Address addr = new Cfd.Address(pubkey, Cfd.CfdAddressType.P2wpkh, Cfd.CfdNetworkType.ElementsRegtest);
      Console.WriteLine("- Address = " + addr.ToAddressString());
      Console.WriteLine("- LockingScript = " + addr.GetLockingScript());
      Console.WriteLine("- Hash = " + addr.GetHash());
      Console.WriteLine();

      Console.WriteLine("ConfidentialAddress from text");
      Cfd.ConfidentialAddress caddr = new Cfd.ConfidentialAddress("VTpvKKc1SNmLG4H8CnR1fGJdHdyWGEQEvdP9gfeneJR7n81S5kiwNtgF7vrZjC8mp63HvwxM81nEbTxU");
      Console.WriteLine("- ConfidentialAddress = " + caddr.ToAddressString());
      Console.WriteLine("- Address = " + caddr.GetAddress().ToAddressString());
      Console.WriteLine("- ConfidentialKey = " + caddr.GetConfidentialKey().ToHexString());
      Console.WriteLine();

      Console.WriteLine("ConfidentialAddress from address");
      addr = new Cfd.Address("Q7wegLt2qMGhm28vch6VTzvpzs8KXvs4X7");
      pubkey = new Cfd.Pubkey("025476c2e83188368da1ff3e292e7acafcdb3566bb0ad253f62fc70f07aeee6357");      caddr = new Cfd.ConfidentialAddress(addr, pubkey);
      Console.WriteLine("- ConfidentialAddress = " + caddr.ToAddressString());
      Console.WriteLine("- Address = " + caddr.GetAddress().ToAddressString());
      Console.WriteLine("- ConfidentialKey = " + caddr.GetConfidentialKey().ToHexString());
      Console.WriteLine();

      Console.WriteLine("Call Finish!");
    }
  }
}
