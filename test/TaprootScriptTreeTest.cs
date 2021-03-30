using Xunit;
using Xunit.Abstractions;

namespace Cfd.xTests
{
  public class TaprootScriptTreeTest
  {
    private readonly ITestOutputHelper output;

    public TaprootScriptTreeTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Fact]
    public void CreateScriptTreeTest()
    {
      var sk = new Privkey("305e293b010d29bf3c888b617763a438fee9054c8cab66eb12ad078f819d9f27");
      var spk = SchnorrPubkey.GetPubkeyFromPrivkey(sk, out bool _);
      Assert.Equal("1777701648fa4dd93c74edd9d58cfcc7bdc2fa30a2f6fa908b6fd70c92833cfb", spk.ToHexString());

      var scriptCheckSig = Script.CreateFromAsm(new string[] { spk.ToHexString(), "OP_CHECKSIG" });
      var scriptOpTrue = Script.CreateFromAsm("OP_TRUE");
      var scriptCheckSig2 = Script.CreateFromAsm(new string[] {
        "ac52f50b28cdd4d3bcb7f0d5cb533f232e4c4ef12fbf3e718420b84d4e3c3440",
        "OP_CHECKSIG",
      });

      var tree = new TaprootScriptTree(scriptCheckSig);
      tree.AddBranch(scriptOpTrue);
      tree.AddBranch(scriptCheckSig2);
      output.WriteLine("tree1: " + tree.ToString());
      Assert.Equal(
        "{tl(20ac52f50b28cdd4d3bcb7f0d5cb533f232e4c4ef12fbf3e718420b84d4e3c3440ac),{tl(51),tl(201777701648fa4dd93c74edd9d58cfcc7bdc2fa30a2f6fa908b6fd70c92833cfbac)}}",
        tree.ToString());
      var count = tree.GetBranchCount();
      Assert.Equal((uint)2, count);
    }

    [Fact]
    public void DeserializeScriptTreeTest()
    {
      var scriptOpTrue = Script.CreateFromAsm("OP_TRUE");

      var treeStr = "{{{tl(51),{tl(204a7af8660f2b0bdb92d2ce8b88ab30feb916343228d2e7bd15da02e1f6a31d47ac),tl(2000d134c42fd51c90fa82c6cfdaabd895474d979118525362c0cd236c857e29d9ac)}},{{tl(20ac52f50b28cdd4d3bcb7f0d5cb533f232e4c4ef12fbf3e718420b84d4e3c3440ac),{tl(2057bf643684f6c5c75e1cdf45990036502a0d897394013210858cdabcbb95a05aac),tl(51)}},tl(2057bf643684f6c5c75e1cdf45990036502a0d897394013210858cdabcbb95a05aad205bec1a08fa3443176edd0a08e2a64642f45e57543b62bffe43ec350edc33dc22ac)}},tl(2008f8280d68e02e807ccffee141c4a6b7ac31d3c283ae0921892d95f691742c44ad20b0f8ce3e1df406514a773414b5d9e5779d8e68ce816e9db39b8e53255ac3b406ac)}";
      var controlNodes = new ByteData256[]{
        new ByteData256("06b46c960d6824f0da5af71d9ecc55714de5b2d2da51be60bd12c77df20a20df"),
		    new ByteData256("4691fbb1196f4675241c8958a7ab6378a63aa0cc008ed03d216fd038357f52fd"),
		    new ByteData256("e47f58011f27e9046b8195d0ab6a2acbc68ce281437a8d5132dadf389b2a5ebb"),
		    new ByteData256("32a0a039ec1412be2803fd7b5f5444c03d498e5e8e107ee431a9597c7b5b3a7c"),
		    new ByteData256("d7b0b8d070638ff4f0b7e7d2aa930c58ec2d39853fd04c29c4c6688fdcb2ae75"),
	    };

      var tree = new TaprootScriptTree(treeStr, scriptOpTrue, controlNodes);
      var count = tree.GetBranchCount();
      Assert.Equal((uint)5, count);
      var nodeList = tree.GetTargetNodes();
      Assert.Equal(5, nodeList.Length);
      if (nodeList.Length == 5) {
        for (var index = 0; index < nodeList.Length; ++index)
        {
          Assert.Equal(controlNodes[index].ToHexString(), nodeList[index].ToHexString());
        }
      }

      var branch = tree.GetBranch(3);
      Assert.Equal(
        "{tl(51),{tl(204a7af8660f2b0bdb92d2ce8b88ab30feb916343228d2e7bd15da02e1f6a31d47ac),tl(2000d134c42fd51c90fa82c6cfdaabd895474d979118525362c0cd236c857e29d9ac)}}",
        branch.ToString());

      var tree2 = new TapBranch(
        "{{tl(20ac52f50b28cdd4d3bcb7f0d5cb533f232e4c4ef12fbf3e718420b84d4e3c3440ac),{tl(2057bf643684f6c5c75e1cdf45990036502a0d897394013210858cdabcbb95a05aac),tl(51)}},tl(2057bf643684f6c5c75e1cdf45990036502a0d897394013210858cdabcbb95a05aad205bec1a08fa3443176edd0a08e2a64642f45e57543b62bffe43ec350edc33dc22ac)}",
        scriptOpTrue);
      tree2.AddBranch(branch);
      tree2.AddBranch("tl(2008f8280d68e02e807ccffee141c4a6b7ac31d3c283ae0921892d95f691742c44ad20b0f8ce3e1df406514a773414b5d9e5779d8e68ce816e9db39b8e53255ac3b406ac)");
      Assert.Equal(tree.ToString(), tree2.ToString());
    }
  }
}
