using Xunit;
using Xunit.Abstractions;

namespace Cfd.xTests
{
  public class DescriptorTest
  {
    private readonly ITestOutputHelper output;

    public DescriptorTest(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Fact]
    public void DescriptorPkhTest()
    {
      string desc = "pkh(02c6047f9441ed7d6d3045406e95c07cd85c778e4b8cef3ca7abac09b95c709ee5)";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      Assert.Equal("pkh(02c6047f9441ed7d6d3045406e95c07cd85c778e4b8cef3ca7abac09b95c709ee5)#8fhd9pwu", descriptor.ToString());
      Assert.Equal(CfdHashType.P2pkh, descriptor.GetHashType());
      Assert.Equal("1cMh228HTCiwS8ZsaakH8A8wze1JR5ZsP", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_DUP OP_HASH160 06afd46bcdfd22ef94ac122aa11f241244a37ecc OP_EQUALVERIFY OP_CHECKSIG",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.Equal(CfdDescriptorKeyType.Public,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("02c6047f9441ed7d6d3045406e95c07cd85c778e4b8cef3ca7abac09b95c709ee5",
        descriptor.GetKeyData().Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorWpkhTest()
    {
      string desc = "wpkh(02f9308a019258c31049344f85f89d5229b531c845836f99b08601f113bce036f9)";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      Assert.Equal("wpkh(02f9308a019258c31049344f85f89d5229b531c845836f99b08601f113bce036f9)#8zl0zxma", descriptor.ToString());
      Assert.Equal(CfdHashType.P2wpkh, descriptor.GetHashType());
      Assert.Equal("bc1q0ht9tyks4vh7p5p904t340cr9nvahy7u3re7zg", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_0 7dd65592d0ab2fe0d0257d571abf032cd9db93dc",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.Equal(CfdDescriptorKeyType.Public,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("02f9308a019258c31049344f85f89d5229b531c845836f99b08601f113bce036f9",
        descriptor.GetKeyData().Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorShWpkhTest()
    {
      string desc = "sh(wpkh(03fff97bd5755eeea420453a14355235d382f6472f8568a18b2f057a1460297556))";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("sh(wpkh(03fff97bd5755eeea420453a14355235d382f6472f8568a18b2f057a1460297556))#qkrrc7je", descriptor.ToString());
      Assert.Equal(CfdHashType.P2shP2wpkh, descriptor.GetHashType());
      Assert.Equal("3LKyvRN6SmYXGBNn8fcQvYxW9MGKtwcinN", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_HASH160 cc6ffbc0bf31af759451068f90ba7a0272b6b332 OP_EQUAL",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(2,
        descriptor.GetList().Length);
      Assert.Equal(CfdDescriptorKeyType.Public,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("03fff97bd5755eeea420453a14355235d382f6472f8568a18b2f057a1460297556",
        descriptor.GetKeyData().Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorShMultiTest()
    {
      string desc = "sh(multi(2,022f01e5e15cca351daff3843fb70f3c2f0a1bdd05e5af888a67784ef3e10a2a01,03acd484e2f0c7f65309ad178a9f559abde09796974c57e714c35f110dfc27ccbe))";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("script: " + descriptor.GetRedeemScript().ToHexString());
      output.WriteLine("s-asm: " + descriptor.GetRedeemScript().GetAsm());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#y9zthqta", descriptor.ToString());
      Assert.Equal(CfdHashType.P2sh, descriptor.GetHashType());
      Assert.Equal("3GtEB3yg3r5de2cDJG48SkQwxfxJumKQdN", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_HASH160 a6a8b030a38762f4c1f5cbe387b61a3c5da5cd26 OP_EQUAL",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("5221022f01e5e15cca351daff3843fb70f3c2f0a1bdd05e5af888a67784ef3e10a2a012103acd484e2f0c7f65309ad178a9f559abde09796974c57e714c35f110dfc27ccbe52ae",
        descriptor.GetRedeemScript().ToHexString());
      Assert.Equal("OP_2 022f01e5e15cca351daff3843fb70f3c2f0a1bdd05e5af888a67784ef3e10a2a01 03acd484e2f0c7f65309ad178a9f559abde09796974c57e714c35f110dfc27ccbe OP_2 OP_CHECKMULTISIG",
        descriptor.GetRedeemScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.True(descriptor.HasMultisig());
      Assert.Equal(2,
        descriptor.GetMultisigKeyList().Length);
      Assert.Equal("022f01e5e15cca351daff3843fb70f3c2f0a1bdd05e5af888a67784ef3e10a2a01",
        descriptor.GetMultisigKeyList()[0].Pubkey.ToHexString());
      Assert.Equal("03acd484e2f0c7f65309ad178a9f559abde09796974c57e714c35f110dfc27ccbe",
        descriptor.GetMultisigKeyList()[1].Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorWshMultiTest()
    {
      string desc = "wsh(multi(2,03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7,03774ae7f858a9411e5ef4246b70c65aac5649980be5c17891bbec17895da008cb,03d01115d548e7561b15c38f004d734633687cf4419620095bc5b0f47070afe85a))";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("script: " + descriptor.GetRedeemScript().ToHexString());
      output.WriteLine("s-asm: " + descriptor.GetRedeemScript().GetAsm());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#en3tu306", descriptor.ToString());
      Assert.Equal(CfdHashType.P2wsh, descriptor.GetHashType());
      Assert.Equal("bc1qwu7hp9vckakyuw6htsy244qxtztrlyez4l7qlrpg68v6drgvj39qn4zazc", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_0 773d709598b76c4e3b575c08aad40658963f9322affc0f8c28d1d9a68d0c944a",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("522103a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c72103774ae7f858a9411e5ef4246b70c65aac5649980be5c17891bbec17895da008cb2103d01115d548e7561b15c38f004d734633687cf4419620095bc5b0f47070afe85a53ae",
        descriptor.GetRedeemScript().ToHexString());
      Assert.Equal("OP_2 03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7 03774ae7f858a9411e5ef4246b70c65aac5649980be5c17891bbec17895da008cb 03d01115d548e7561b15c38f004d734633687cf4419620095bc5b0f47070afe85a OP_3 OP_CHECKMULTISIG",
        descriptor.GetRedeemScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.True(descriptor.HasMultisig());
      Assert.Equal(3,
        descriptor.GetMultisigKeyList().Length);
      Assert.Equal("03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7",
        descriptor.GetMultisigKeyList()[0].Pubkey.ToHexString());
      Assert.Equal("03774ae7f858a9411e5ef4246b70c65aac5649980be5c17891bbec17895da008cb",
        descriptor.GetMultisigKeyList()[1].Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorShWshMultiTest()
    {
      string desc = "sh(wsh(multi(1,03f28773c2d975288bc7d1d205c3748651b075fbc6610e58cddeeddf8f19405aa8,03499fdf9e895e719cfd64e67f07d38e3226aa7b63678949e6e49b241a60e823e4,02d7924d4f7d43ea965a465ae3095ff41131e5946f3c85f79e44adbcf8e27e080e)))";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("script: " + descriptor.GetRedeemScript().ToHexString());
      output.WriteLine("s-asm: " + descriptor.GetRedeemScript().GetAsm());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#ks05yr6p", descriptor.ToString());
      Assert.Equal(CfdHashType.P2shP2wsh, descriptor.GetHashType());
      Assert.Equal("3Hd7YQStg9gYpEt6hgK14ZHUABxSURzeuQ", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_HASH160 aec509e284f909f769bb7dda299a717c87cc97ac OP_EQUAL",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("512103f28773c2d975288bc7d1d205c3748651b075fbc6610e58cddeeddf8f19405aa82103499fdf9e895e719cfd64e67f07d38e3226aa7b63678949e6e49b241a60e823e42102d7924d4f7d43ea965a465ae3095ff41131e5946f3c85f79e44adbcf8e27e080e53ae",
        descriptor.GetRedeemScript().ToHexString());
      Assert.Equal("OP_1 03f28773c2d975288bc7d1d205c3748651b075fbc6610e58cddeeddf8f19405aa8 03499fdf9e895e719cfd64e67f07d38e3226aa7b63678949e6e49b241a60e823e4 02d7924d4f7d43ea965a465ae3095ff41131e5946f3c85f79e44adbcf8e27e080e OP_3 OP_CHECKMULTISIG",
        descriptor.GetRedeemScript().GetAsm());
      Assert.Equal(2,
        descriptor.GetList().Length);
      Assert.True(descriptor.HasMultisig());
      Assert.Equal(3,
        descriptor.GetMultisigKeyList().Length);
      Assert.Equal("03f28773c2d975288bc7d1d205c3748651b075fbc6610e58cddeeddf8f19405aa8",
        descriptor.GetMultisigKeyList()[0].Pubkey.ToHexString());
      Assert.Equal("03499fdf9e895e719cfd64e67f07d38e3226aa7b63678949e6e49b241a60e823e4",
        descriptor.GetMultisigKeyList()[1].Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorAddrTest()
    {
      string desc = "addr(bc1qc7slrfxkknqcq2jevvvkdgvrt8080852dfjewde450xdlk4ugp7szw5tk9)";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("script: " + descriptor.GetRedeemScript().ToHexString());
      output.WriteLine("s-asm: " + descriptor.GetRedeemScript().GetAsm());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#6rmdcqux", descriptor.ToString());
      Assert.Equal(CfdHashType.P2wsh, descriptor.GetHashType());
      Assert.Equal("bc1qc7slrfxkknqcq2jevvvkdgvrt8080852dfjewde450xdlk4ugp7szw5tk9", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_0 c7a1f1a4d6b4c1802a59631966a18359de779e8a6a65973735a3ccdfdabc407d",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.False(descriptor.HasScriptHash());
      Assert.False(descriptor.HasKeyHash());
      Assert.False(descriptor.HasMultisig());
      Assert.True(descriptor.GetAddress().IsValid());
    }

    [Fact]
    public void DescriptorRawTest()
    {
      string desc = "raw(6a4c4f54686973204f505f52455455524e207472616e73616374696f6e206f7574707574207761732063726561746564206279206d6f646966696564206372656174657261777472616e73616374696f6e2e)#zf2avljj";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("script: " + descriptor.GetRedeemScript().ToHexString());
      output.WriteLine("s-asm: " + descriptor.GetRedeemScript().GetAsm());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      Assert.Equal(desc, descriptor.ToString());
      Assert.Equal("", descriptor.GetRedeemScript().ToHexString());
      Assert.Equal("", descriptor.GetRedeemScript().GetAsm());
      Assert.Equal("", descriptor.GetAddress().ToAddressString());
      Assert.Single(descriptor.GetList());
      Assert.False(descriptor.HasScriptHash());
      Assert.False(descriptor.HasKeyHash());
      Assert.False(descriptor.HasMultisig());
      Assert.False(descriptor.GetAddress().IsValid());
    }

    [Fact]
    public void DescriptorComboTest()
    {
      string desc = "combo(0279be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798)";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("combo(0279be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798)#lq9sf04s", descriptor.ToString());
      Assert.Equal(CfdHashType.P2wpkh, descriptor.GetHashType());
      Assert.Equal("bc1qw508d6qejxtdg4y5r3zarvary0c5xw7kv8f3t4", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_0 751e76e8199196d454941c45d1b3a323f1433bd6",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.Equal(CfdDescriptorKeyType.Public,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("0279be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798",
        descriptor.GetKeyData().Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorShWshPkhTest()
    {
      string desc = "sh(wsh(pkh(02e493dbf1c10d80f3581e4904930b1404cc6c13900ee0758474fa94abe8c4cd13)))";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("script: " + descriptor.GetRedeemScript().ToHexString());
      output.WriteLine("s-asm: " + descriptor.GetRedeemScript().GetAsm());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#2wtr0ej5", descriptor.ToString());
      Assert.Equal(CfdHashType.P2shP2wsh, descriptor.GetHashType());
      Assert.Equal("39XGHYpYmJV9sGFoGHZeU2rLkY6r1MJ6C1", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_HASH160 55e8d5e8ee4f3604aba23c71c2684fa0a56a3a12 OP_EQUAL",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("76a914c42e7ef92fdb603af844d064faad95db9bcdfd3d88ac",
        descriptor.GetRedeemScript().ToHexString());
      Assert.Equal("OP_DUP OP_HASH160 c42e7ef92fdb603af844d064faad95db9bcdfd3d OP_EQUALVERIFY OP_CHECKSIG",
        descriptor.GetRedeemScript().GetAsm());
      Assert.Equal(3,
        descriptor.GetList().Length);
      Assert.False(descriptor.HasMultisig());
      Assert.True(descriptor.HasScriptHash());
      Assert.False(descriptor.HasKeyHash());
      Assert.Equal(CfdDescriptorKeyType.Public,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("02e493dbf1c10d80f3581e4904930b1404cc6c13900ee0758474fa94abe8c4cd13",
        descriptor.GetKeyData().Pubkey.ToHexString());
    }

    [Fact]
    public void DescriptorPkhExtPubkeyTest()
    {
      string desc = "pkh(xpub68Gmy5EdvgibQVfPdqkBBCHxA5htiqg55crXYuXoQRKfDBFA1WEjWgP6LHhwBZeNK1VTsfTFUHCdrfp1bgwQ9xv5ski8PX9rL2dZXvgGDnw/1/2)";
      Descriptor descriptor = new Descriptor(desc, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      output.WriteLine("key : " + descriptor.GetKeyData().ExtPubkey.ToString());
      Assert.Equal(desc + "#kczqajcv", descriptor.ToString());
      Assert.Equal(CfdHashType.P2pkh, descriptor.GetHashType());
      Assert.Equal("1PdNaNxbyQvHW5QHuAZenMGVHrrRaJuZDJ", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_DUP OP_HASH160 f833c08f02389c451ae35ec797fccf7f396616bf OP_EQUALVERIFY OP_CHECKSIG",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.Equal(CfdDescriptorKeyType.Bip32,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("xpub6D4BDPcEgbv6wqbZ5Vfp1MUpa5tieyHKAoJCFjcUJpzSc9BV92TpCM85m3jfth6jfKA7LWFiip8zp8RuARjoLjkD13Z8cb9VdyMm3MMdTcA",
        descriptor.GetKeyData().ExtPubkey.ToString());
    }

    [Fact]
    public void DescriptorPkhExtPubkeyDeriveTest()
    {
      string desc = "pkh(xpub68Gmy5EdvgibQVfPdqkBBCHxA5htiqg55crXYuXoQRKfDBFA1WEjWgP6LHhwBZeNK1VTsfTFUHCdrfp1bgwQ9xv5ski8PX9rL2dZXvgGDnw/1/*)";
      string derivePath = "2/3";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      output.WriteLine("key : " + descriptor.GetKeyData().ExtPubkey.ToString());
      Assert.Equal(desc + "#8nhtvxel", descriptor.ToString());
      Assert.Equal(CfdHashType.P2pkh, descriptor.GetHashType());
      Assert.Equal("1Jh92Cjae6Kt8JXnkonohX36EWK7Du5ZMP", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_DUP OP_HASH160 c21178dfb721039b6936b167657cd31ab60b1bbd OP_EQUALVERIFY OP_CHECKSIG",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Single(descriptor.GetList());
      Assert.Equal(CfdDescriptorKeyType.Bip32,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("xpub6FMiTLEY5GgpKy1f9Vr2x5w25cs9eBtCMq6xJYPo8bWaFD11MrPxmBPoxqWTL2wninua6fwXuRyc5nAcg7RU3DebapJhaW8xXJWoFNpRN6s",
        descriptor.GetKeyData().ExtPubkey.ToString());
    }

    [Fact]
    public void DescriptorShMiniscriptTest()
    {
      string desc = "sh(or_d(sha256(38df1c1f64a24a77b23393bca50dff872e31edc4f3b5aa3b90ad0b82f4f089b6),and_n(un:after(499999999),older(4194305))))";
      string derivePath = "";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#ueuxphxk", descriptor.ToString());
      Assert.Equal(CfdHashType.P2sh, descriptor.GetHashType());
      Assert.Equal("38WFPv9fne2UeFxVkGMhLkamMadH8j6s1c", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_HASH160 4abf8cfc94ae837bf59965e0c74d02a611ec1329 OP_EQUAL",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("OP_SIZE 32 OP_EQUALVERIFY OP_SHA256 38df1c1f64a24a77b23393bca50dff872e31edc4f3b5aa3b90ad0b82f4f089b6 OP_EQUAL OP_IFDUP OP_NOTIF OP_IF 499999999 OP_CHECKLOCKTIMEVERIFY OP_0NOTEQUAL OP_ELSE OP_0 OP_ENDIF OP_NOTIF OP_0 OP_ELSE 4194305 OP_CHECKSEQUENCEVERIFY OP_ENDIF OP_ENDIF",
        descriptor.GetRedeemScript().GetAsm());
      Assert.Single(descriptor.GetList());
    }

    [Fact]
    public void DescriptorWshMiniscriptTest()
    {
      string desc = "wsh(thresh(2,multi(2,03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7,036d2b085e9e382ed10b69fc311a03f8641ccfff21574de0927513a49d9a688a00),a:multi(1,036d2b085e9e382ed10b69fc311a03f8641ccfff21574de0927513a49d9a688a00),ac:pk_k(022f01e5e15cca351daff3843fb70f3c2f0a1bdd05e5af888a67784ef3e10a2a01)))";
      string derivePath = "";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#pv8ptztg", descriptor.ToString());
      Assert.Equal(CfdHashType.P2wsh, descriptor.GetHashType());
      Assert.Equal("bc1qdfky9a3dh8atpy0l47fsuz5ywergnrfztcddjnl5xgnwyqvqh8gschn2ch", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_0 6a6c42f62db9fab091ffaf930e0a847646898d225e1ad94ff43226e20180b9d1",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("OP_2 03a0434d9e47f3c86235477c7b1ae6ae5d3442d49b1943c2b752a68e2a47e247c7 036d2b085e9e382ed10b69fc311a03f8641ccfff21574de0927513a49d9a688a00 OP_2 OP_CHECKMULTISIG OP_TOALTSTACK OP_1 036d2b085e9e382ed10b69fc311a03f8641ccfff21574de0927513a49d9a688a00 OP_1 OP_CHECKMULTISIG OP_FROMALTSTACK OP_ADD OP_TOALTSTACK 022f01e5e15cca351daff3843fb70f3c2f0a1bdd05e5af888a67784ef3e10a2a01 OP_CHECKSIG OP_FROMALTSTACK OP_ADD OP_2 OP_EQUAL",
        descriptor.GetRedeemScript().GetAsm());
      Assert.Single(descriptor.GetList());
    }

    [Fact]
    public void DescriptorShWshMiniscriptDeriveTest()
    {
      string desc = "sh(wsh(c:or_i(andor(c:pk_h(xpub661MyMwAqRbcFW31YEwpkMuc5THy2PSt5bDMsktWQcFF8syAmRUapSCGu8ED9W6oDMSgv6Zz8idoc4a6mr8BDzTJY47LJhkJ8UB7WEGuduB/1/0/*),pk_h(xpub69H7F5d8KSRgmmdJg2KhpAK8SR3DjMwAdkxj3ZuxV27CprR9LgpeyGmXUbC6wb7ERfvrnKZjXoUmmDznezpbZb7ap6r1D3tgFxHmwMkQTPH/0/0/*),pk_h(02c6047f9441ed7d6d3045406e95c07cd85c778e4b8cef3ca7abac09b95c709ee5)),pk_k(02d7924d4f7d43ea965a465ae3095ff41131e5946f3c85f79e44adbcf8e27e080e))))";
      string derivePath = "44";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#cpx6as23", descriptor.ToString());
      Assert.Equal(CfdHashType.P2shP2wsh, descriptor.GetHashType());
      Assert.Equal("3GyYN9WnJBoMn8M5tuqVcFJq1BvbAcdPAt", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_HASH160 a7a9f411001e3e3db96d7f02fc9ab1d0dc6aa691 OP_EQUAL",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal("OP_IF OP_DUP OP_HASH160 520e6e72bcd5b616bc744092139bd759c31d6bbe OP_EQUALVERIFY OP_CHECKSIG OP_NOTIF OP_DUP OP_HASH160 06afd46bcdfd22ef94ac122aa11f241244a37ecc OP_EQUALVERIFY OP_ELSE OP_DUP OP_HASH160 5ab62f0be26fe9d6205a155403f33e2ad2d31efe OP_EQUALVERIFY OP_ENDIF OP_ELSE 02d7924d4f7d43ea965a465ae3095ff41131e5946f3c85f79e44adbcf8e27e080e OP_ENDIF OP_CHECKSIG",
        descriptor.GetRedeemScript().GetAsm());
      Assert.Equal(2, descriptor.GetList().Length);
    }

    [Fact]
    public void DescriptorTaprootSchnorrTest()
    {
      string desc = "tr(ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a)";
      string derivePath = "";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Regtest);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#mavrnmjy", descriptor.ToString());
      Assert.Equal(CfdHashType.Taproot, descriptor.GetHashType());
      Assert.Equal("bcrt1pvv8jm84ye0xr7p9h8l2k58rm287nryk73cnw0nvfxyjfqqpn60gssz7u5f", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_1 630f2d9ea4cbcc3f04b73fd56a1c7b51fd3192de8e26e7cd893124900033d3d1",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.False(descriptor.HasScriptHash());
      Assert.Single(descriptor.GetList());
      Assert.False(descriptor.HasTapScript());
      Assert.Equal(CfdDescriptorKeyType.SchnorrPubkey,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a",
        descriptor.GetKeyData().SchnorrPubkey.ToHexString());
    }

    [Fact]
    public void DescriptorTaprootXpubDeriveTest()
    {
      string desc = "tr([bd16bee5/0]xpub69H7F5d8KSRgmmdJg2KhpAK8SR3DjMwAdkxj3ZuxV27CprR9LgpeyGmXUbC6wb7ERfvrnKZjXoUmmDznezpbZb7ap6r1D3tgFxHmwMkQTPH/0/0/*)";
      string derivePath = "1";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Mainnet);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#aa0v9ye4", descriptor.ToString());
      Assert.Equal(CfdHashType.Taproot, descriptor.GetHashType());
      Assert.Equal("bc1p4jueea9m897g4me0ef8eyqg9x5n02jzpwnl0yydvdtrl459r3fyqg8wvnj", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_1 acb99cf4bb397c8aef2fca4f9201053526f5484174fef211ac6ac7fad0a38a48",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.False(descriptor.HasScriptHash());
      Assert.Single(descriptor.GetList());
      Assert.False(descriptor.HasTapScript());
      Assert.Equal(CfdDescriptorKeyType.Bip32,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("xpub6EKMC2gSMfKgSwn7V9VZn7x1MvoeeVzSmmtSJ4z2L2d6R4WxvdQMouokypZHVp4fgKycrrQnGr6WJ5ED5jG9Q9FiA1q5gKYUc8u6JHJhdo8",
        descriptor.GetKeyData().ExtPubkey.ToString());
    }

    [Fact]
    public void DescriptorTaprootTapscriptSingleTest()
    {
      string desc = "tr(ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a,c:pk_k(8c6f5956c3cc7251d483fc683fa06b22d4e2ddc7496a2590acee36c4a313f816))";
      string derivePath = "";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Regtest);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#agrnj9m2", descriptor.ToString());
      Assert.Equal(CfdHashType.Taproot, descriptor.GetHashType());
      Assert.Equal("bcrt1p2druqmxfa49j9ph0ea8d9y4gzrhy2x7u2zj0p2622d9r7k28v02s6x9jx3", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_1 5347c06cc9ed4b2286efcf4ed292a810ee451bdc50a4f0ab4a534a3f594763d5",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.False(descriptor.HasScriptHash());
      Assert.Single(descriptor.GetList());
      Assert.True(descriptor.HasTapScript());
      Assert.Equal("tl(208c6f5956c3cc7251d483fc683fa06b22d4e2ddc7496a2590acee36c4a313f816ac)",
        descriptor.GetScriptTree().ToString());
      Assert.Equal(CfdDescriptorKeyType.SchnorrPubkey,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a",
        descriptor.GetKeyData().SchnorrPubkey.ToHexString());
    }

    [Fact]
    public void DescriptorTaprootTapscriptBranchTest()
    {
      string desc = "tr(ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a,{c:pk_k(8c6f5956c3cc7251d483fc683fa06b22d4e2ddc7496a2590acee36c4a313f816),{c:pk_k([bd16bee5/0]xpub69H7F5d8KSRgmmdJg2KhpAK8SR3DjMwAdkxj3ZuxV27CprR9LgpeyGmXUbC6wb7ERfvrnKZjXoUmmDznezpbZb7ap6r1D3tgFxHmwMkQTPH/0/0/*),thresh(2,c:pk_k(5cbdf0646e5db4eaa398f365f2ea7a0e3d419b7e0330e39ce92bddedcac4f9bc),s:sha256(e38990d0c7fc009880a9c07c23842e886c6bbdc964ce6bdd5817ad357335ee6f),a:hash160(dd69735817e0e3f6f826a9238dc2e291184f0131))}})";
      string derivePath = "1";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Regtest);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#7ezsl729", descriptor.ToString());
      Assert.Equal(CfdHashType.Taproot, descriptor.GetHashType());
      Assert.Equal("bcrt1pfuqf4j7ceyzmu3rsmude93ctu948r565hf2ucrn9z7zn7a7hjegskj3rsv", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_1 4f009acbd8c905be4470df1b92c70be16a71d354ba55cc0e6517853f77d79651",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.False(descriptor.HasScriptHash());
      Assert.Single(descriptor.GetList());
      Assert.True(descriptor.HasTapScript());
      Assert.Equal("{tl(208c6f5956c3cc7251d483fc683fa06b22d4e2ddc7496a2590acee36c4a313f816ac),{tl(208c6f5956c3cc7251d483fc683fa06b22d4e2ddc7496a2590acee36c4a313f816ac),tl(205cbdf0646e5db4eaa398f365f2ea7a0e3d419b7e0330e39ce92bddedcac4f9bcac7c82012088a820e38990d0c7fc009880a9c07c23842e886c6bbdc964ce6bdd5817ad357335ee6f87936b82012088a914dd69735817e0e3f6f826a9238dc2e291184f0131876c935287)}}",
        descriptor.GetScriptTree().ToString());
      Assert.Equal(CfdDescriptorKeyType.SchnorrPubkey,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a",
        descriptor.GetKeyData().SchnorrPubkey.ToHexString());
    }

    [Fact]
    public void DescriptorTaprootHashOnlyTest()
    {
      string desc = "tr(ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a,{1717a480c2e3a474eed8dba83f684731243cff8ef384521936cf3a730dd0a286,{1717a480c2e3a474eed8dba83f684731243cff8ef384521936cf3a730dd0a286,80039cda864c4f2f1c87f161b0038e57fb7a4a59ff37517048696b85cdaaf911}})";
      string derivePath = "1";
      Descriptor descriptor = new Descriptor(desc, derivePath, CfdNetworkType.Regtest);
      output.WriteLine("desc: " + descriptor.ToString());
      output.WriteLine("addr: " + descriptor.GetAddress().ToAddressString());
      output.WriteLine("asm: " + descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.Equal(desc + "#xffhk3u4", descriptor.ToString());
      Assert.Equal(CfdHashType.Taproot, descriptor.GetHashType());
      Assert.Equal("bcrt1pfuqf4j7ceyzmu3rsmude93ctu948r565hf2ucrn9z7zn7a7hjegskj3rsv", descriptor.GetAddress().ToAddressString());
      Assert.Equal("OP_1 4f009acbd8c905be4470df1b92c70be16a71d354ba55cc0e6517853f77d79651",
        descriptor.GetAddress().GetLockingScript().GetAsm());
      Assert.False(descriptor.HasScriptHash());
      Assert.Single(descriptor.GetList());
      Assert.True(descriptor.HasTapScript());
      Assert.Equal("{1717a480c2e3a474eed8dba83f684731243cff8ef384521936cf3a730dd0a286,{1717a480c2e3a474eed8dba83f684731243cff8ef384521936cf3a730dd0a286,80039cda864c4f2f1c87f161b0038e57fb7a4a59ff37517048696b85cdaaf911}}",
        descriptor.GetScriptTree().ToString());
      Assert.Equal(CfdDescriptorKeyType.SchnorrPubkey,
        descriptor.GetKeyData().KeyType);
      Assert.Equal("ef514f1aeb14baa6cc57ab3268fb329ca540c48454f7f46771ed731e34ba521a",
        descriptor.GetKeyData().SchnorrPubkey.ToHexString());
    }
  }
}
