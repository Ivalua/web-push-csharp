﻿using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebPush.Util;

namespace WebPush.Test
{
    [TestClass]
    public class ECKeyHelperTest
    {
        private const string TestPublicKey =
            @"BCvKwB2lbVUYMFAaBUygooKheqcEU-GDrVRnu8k33yJCZkNBNqjZj0VdxQ2QIZa4kV5kpX9aAqyBKZHURm6eG1A";

        private const string TestPrivateKey = @"on6X5KmLEFIVvPP3cNX9kE0OF6PV9TJQXVbnKU2xEHI";

        [TestMethod]
        public void TestGenerateKeys()
        {
            var keys = ECKeyHelper.GenerateKeys();
            var publicKey = keys.PublicKey;
            var privateKey = keys.PrivateKey;

            var publicKeyLength = publicKey.Length;
            var privateKeyLength = privateKey.Length;

            Assert.AreEqual(65, publicKeyLength);
            Assert.AreEqual(32, privateKeyLength);
        }

        [TestMethod]
        public void TestGenerateKeysNoCache()
        {
            var keys = ECKeyHelper.GenerateKeys();
            var publicKey1 = keys.PublicKey;
            var privateKey1 = keys.PrivateKey;

            var keys2 = ECKeyHelper.GenerateKeys();
            var publicKey2 = keys2.PublicKey;
            var privateKey2 = keys2.PrivateKey;


            Assert.IsFalse(publicKey1.SequenceEqual(publicKey2));
            Assert.IsFalse(privateKey1.SequenceEqual(privateKey2));
        }

        [TestMethod]
        public void TestGetPrivateKey()
        {
#if NET48
            var privateKey = UrlBase64.Decode(TestPrivateKey);
            var privateKeyParams = ECKeyHelper.GetPrivateKey(privateKey);

            var importedPrivateKey = UrlBase64.Encode((privateKeyParams as ECDsaCng).ExportParameters(true).D);

            Assert.AreEqual(TestPrivateKey, importedPrivateKey);
#endif
        }

        [TestMethod]
        public void TestGetPublicKey()
        {
            var publicKey = UrlBase64.Decode(TestPublicKey);
            var publicKeyParams = ECKeyHelper.GetPublicKey(publicKey);

            var importedPublicKey = UrlBase64.Encode(publicKeyParams.GetECPublicKey());

            Assert.AreEqual(TestPublicKey, importedPublicKey);
        }
    }
}