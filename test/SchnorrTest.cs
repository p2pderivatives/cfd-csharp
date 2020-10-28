using Xunit;

namespace Cfd.xTests
{
  public class SchnorrTest
  {
    [Fact]
    public void AdaptorUtilTest()
    {
      var msg = new ByteData("024bdd11f2144e825db05759bdd9041367a420fad14b665fd08af5b42056e5e2");
      var adaptor = new Pubkey("038d48057fc4ce150482114d43201b333bf3706f3cd527e8767ceb4b443ab5d349");
      var sk = new Privkey("90ac0d5dc0a1a9ab352afb02005a5cc6c4df0da61d8149d729ff50db9b5a5215");
      var pubkey = new Pubkey("03490cec9a53cd8f2f664aea61922f26ee920c42d2489778bb7c9d9ece44d149a7");
      var adaptorSig = new ByteData("00cbe0859638c3600ea1872ed7a55b8182a251969f59d7d2da6bd4afedf25f5021a49956234cbbbbede8ca72e0113319c84921bf1224897a6abd89dc96b9c5b208");
      var adaptorProof = new ByteData("00b02472be1ba09f5675488e841a10878b38c798ca63eff3650c8e311e3e2ebe2e3b6fee5654580a91cc5149a71bf25bcbeae63dea3ac5ad157a0ab7373c3011d0fc2592a07f719c5fc1323f935569ecd010db62f045e965cc1d564eb42cce8d6d");
      var adaptorSig2 = new ByteData("01099c91aa1fe7f25c41085c1d3c9e73fe04a9d24dac3f9c2172d6198628e57f47bb90e2ad6630900b69f55674c8ad74a419e6ce113c10a21a79345a6e47bc74c1");
      // var sigDer = new ByteData("30440220099c91aa1fe7f25c41085c1d3c9e73fe04a9d24dac3f9c2172d6198628e57f4702204d13456e98d8989043fd4674302ce90c432e2f8bb0269f02c72aafec60b72de101");
      var sig = new ByteData("099c91aa1fe7f25c41085c1d3c9e73fe04a9d24dac3f9c2172d6198628e57f474d13456e98d8989043fd4674302ce90c432e2f8bb0269f02c72aafec60b72de1");
      var secret = new Privkey("475697a71a74ff3f2a8f150534e9b67d4b0b6561fab86fcaa51f8c9d6c9db8c6");

      var pair = EcdsaAdaptorUtil.Sign(msg, sk, adaptor);
      Assert.Equal(adaptorSig.ToHexString(), pair.Signature.ToHexString());
      Assert.Equal(adaptorProof.ToHexString(), pair.Proof.ToHexString());

      var isVerify = EcdsaAdaptorUtil.Verify(pair.Signature, pair.Proof, adaptor, msg, pubkey);
      Assert.True(isVerify);

      var adaptSig = EcdsaAdaptorUtil.Adapt(adaptorSig2, secret);
      Assert.Equal(sig.ToHexString(), adaptSig.ToHexString());

      var adaptorSecret = EcdsaAdaptorUtil.ExtractSecret(adaptorSig2, adaptSig, adaptor);
      Assert.Equal(secret.ToHexString(), adaptorSecret.ToHexString());
    }

    [Fact]
    public void SchnorrUtilTest()
    {
      var msg = new ByteData("e48441762fb75010b2aa31a512b62b4148aa3fb08eb0765d76b252559064a614");
      var sk = new Privkey("688c77bc2d5aaff5491cf309d4753b732135470d05b7b2cd21add0744fe97bef");
      var pubkey = new SchnorrPubkey("b33cc9edc096d0a83416964bd3c6247b8fecd256e4efa7870d2c854bdeb33390");
      var auxRand = new ByteData("02cce08e913f22a36c5648d6405a2c7c50106e7aa2f1649e381c7f09d16b80ab");
      var nonce = new ByteData("8c8ca771d3c25eb38de7401818eeda281ac5446f5c1396148f8d9d67592440fe");
      var schnorrNonce = new SchnorrPubkey("f14d7e54ff58c5d019ce9986be4a0e8b7d643bd08ef2cdf1099e1a457865b547");
      var signature = new SchnorrSignature("6470fd1303dda4fda717b9837153c24a6eab377183fc438f939e0ed2b620e9ee5077c4a8b8dca28963d772a94f5f0ddf598e1c47c137f91933274c7c3edadce8");

      var sig = SchnorrUtil.Sign(msg, sk, auxRand);
      Assert.Equal(signature.ToHexString(), sig.ToHexString());

      var expectedSig =
          "5da618c1936ec728e5ccff29207f1680dcf4146370bdcfab0039951b91e3637a958e91d68537d1f6f19687cec1fd5db1d83da56ef3ade1f3c611babd7d08af42";
      var sig2 = SchnorrUtil.SignWithNonce(msg, sk, nonce);
      Assert.Equal(expectedSig, sig2.ToHexString());

      string expectedSigPoint =
          "03735acf82eef9da1540efb07a68251d5476dabb11ac77054924eccbb4121885e8";
      var sigPoint = SchnorrUtil.ComputeSigPoint(msg, schnorrNonce, pubkey);
      Assert.Equal(expectedSigPoint, sigPoint.ToHexString());

      var isVerify = SchnorrUtil.Verify(signature, msg, pubkey);
      Assert.True(isVerify);

      var expectedNonce =
          "6470fd1303dda4fda717b9837153c24a6eab377183fc438f939e0ed2b620e9ee";
      var expectedPrivkey =
          "5077c4a8b8dca28963d772a94f5f0ddf598e1c47c137f91933274c7c3edadce8";
      Assert.Equal(expectedNonce, sig.GetNonce().ToHexString());
      Assert.Equal(expectedPrivkey, sig.GetKey().ToHexString());
    }

    [Fact]
    public void SchnorrKeyTest()
    {
      var tweak = new ByteData("e48441762fb75010b2aa31a512b62b4148aa3fb08eb0765d76b252559064a614");
      var sk = new Privkey("688c77bc2d5aaff5491cf309d4753b732135470d05b7b2cd21add0744fe97bef");
      var pk = new Pubkey("03b33cc9edc096d0a83416964bd3c6247b8fecd256e4efa7870d2c854bdeb33390");
      var pubkey = new SchnorrPubkey("b33cc9edc096d0a83416964bd3c6247b8fecd256e4efa7870d2c854bdeb33390");
      var expTweakedPk = new SchnorrPubkey("1fc8e882e34cc7942a15f39ffaebcbdf58a19239bcb17b7f5aa88e0eb808f906");
      // bool expTweakedParity = true;
      var expTweakedSk = new Privkey("7bf7c9ba025ca01b698d3e9b3e40efce2774f8a388f8c390550481e1407b2a25");

      var schnorrPubkey = SchnorrPubkey.GetPubkeyFromPrivkey(sk, out bool parity);
      Assert.Equal(pubkey.ToHexString(), schnorrPubkey.ToHexString());
      Assert.True(parity);

      var spk2 = SchnorrPubkey.GetPubkeyFromPubkey(pk, out parity);
      Assert.Equal(pubkey.ToHexString(), spk2.ToHexString());
      Assert.True(parity);

      var tweakedPubkey = schnorrPubkey.TweakAdd(tweak, out parity);
      Assert.Equal(expTweakedPk.ToHexString(), tweakedPubkey.ToHexString());
      Assert.True(parity);

      SchnorrPubkey.GetTweakAddKeyPair(sk, tweak, out SchnorrPubkey tweakedPubkey2, out parity, out Privkey tweakedPrivkey);
      Assert.Equal(expTweakedPk.ToHexString(), tweakedPubkey2.ToHexString());
      Assert.True(parity);
      Assert.Equal(expTweakedSk.ToHexString(), tweakedPrivkey.ToHexString());

      var isValid = tweakedPubkey.IsTweaked(parity, pubkey, tweak);
      Assert.True(isValid);
      isValid = tweakedPubkey.IsTweaked(!parity, pubkey, tweak);
      Assert.False(isValid);
    }
  }
}
