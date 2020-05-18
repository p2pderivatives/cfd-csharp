using Xunit;
using Xunit.Abstractions;

namespace Cfd.xTests
{
  public class ScriptTest
  {
    private readonly ITestOutputHelper output;

    public ScriptTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Fact]
    public void CreateMultisigTest()
    {
      Pubkey[] list = { new Pubkey("02522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0") };
      Script multisig1of1 = Script.CreateMultisigScript(1, list);
      Assert.Equal("512102522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a051ae", multisig1of1.ToHexString());

      Pubkey[] list2 = {
        new Pubkey("02522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0"),
        new Pubkey("0340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c"),
      };
      Script multisig1of2 = Script.CreateMultisigScript(1, list2);
      Assert.Equal("512102522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0210340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c52ae",
        multisig1of2.ToHexString());

      Pubkey[] list3 = {
        new Pubkey("02522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0"),
        new Pubkey("0340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c"),
        new Pubkey("024a3477bc8b933a320eb5667ee72c35a81aa155c8e20cc51c65fb666de3a43b82"),
      };
      Script multisig2of3 = Script.CreateMultisigScript(2, list3);
      Assert.Equal("522102522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0210340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c21024a3477bc8b933a320eb5667ee72c35a81aa155c8e20cc51c65fb666de3a43b8253ae",
        multisig2of3.ToHexString());
    }

    [Fact]
    public void CreateFromAsmTest()
    {
      Script script = Script.CreateFromAsm("OP_TRUE");
      Assert.Equal("OP_1", script.GetAsm());
      Assert.Equal("51", script.ToHexString());
    }

    [Fact]
    public void GetMultisigAddressesTest()
    {
      Script multisigScript = new Script("522102522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0210340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c21024a3477bc8b933a320eb5667ee72c35a81aa155c8e20cc51c65fb666de3a43b8253ae");
      Address[] list = Script.GetMultisigAddresses(multisigScript, CfdNetworkType.ElementsRegtest, CfdAddressType.P2wpkh);
      Assert.Equal(3, list.Length);
      output.WriteLine("addr1: " + list[0].ToAddressString());
      output.WriteLine("addr2: " + list[1].ToAddressString());
      output.WriteLine("addr3: " + list[2].ToAddressString());
      Assert.Equal("ert1qakhjg9r4zgumw2m986sqftwrzz34yt3h8r7gum", list[0].ToAddressString());
      Assert.Equal("ert1qfxspr7tm55sd4vrr7vym44va46esmcgp926ace", list[1].ToAddressString());
      Assert.Equal("ert1qlwmuy4kyap9u3p6cf4xxq49de82yv8j4qkt5pn", list[2].ToAddressString());
    }

    [Fact]
    public void ScriptHashTest()
    {
      Script multisigScript = new Script("522102522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0210340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c21024a3477bc8b933a320eb5667ee72c35a81aa155c8e20cc51c65fb666de3a43b8253ae");
      output.WriteLine("script: " + multisigScript.GetAsm());
      Assert.Equal("OP_2 02522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0 0340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c 024a3477bc8b933a320eb5667ee72c35a81aa155c8e20cc51c65fb666de3a43b82 OP_3 OP_CHECKMULTISIG", multisigScript.GetAsm());
      Assert.False(multisigScript.IsEmpty());
      string[] items = multisigScript.GetScriptItems();
      Assert.Equal(6, items.Length);
      if (6 == items.Length)
      {
        Assert.Equal("OP_2", items[0]);
        Assert.Equal("02522952c3fc2a53a8651b08ce10988b7506a3b40a5c26f9648a911be33e73e1a0", items[1]);
        Assert.Equal("0340b52ae45bc1be5de083f1730fe537374e219c4836400623741d2a874e60590c", items[2]);
        Assert.Equal("024a3477bc8b933a320eb5667ee72c35a81aa155c8e20cc51c65fb666de3a43b82", items[3]);
        Assert.Equal("OP_3", items[4]);
        Assert.Equal("OP_CHECKMULTISIG", items[5]);
      }
      Script rebuild = Script.CreateFromAsm(items);
      Assert.Equal(multisigScript.GetAsm(), rebuild.GetAsm());
      output.WriteLine("re-script: " + rebuild.GetAsm());

      Script byteData = new Script(multisigScript.ToBytes());
      Assert.Equal(byteData.ToHexString(), multisigScript.ToHexString());

      Script emptyScript = new Script();
      Assert.True(emptyScript.IsEmpty());
    }
  }
}
