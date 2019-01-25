// Copyright (c) 2019 Jeremy Oursler All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rcp.Utilities.Tests
{
    [TestClass]
    public class Sha1Tests
    {
        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1Base64HashTest_1()
        {
            long taskId = 57689768;
            var  time   = DateTime.Parse("9/23/2016 11:17:14 AM");

            var val = taskId + time.ToString();


            var res = SHA1.Sha1Base64Hash(val);

            Assert.AreEqual("usDrD71C5NEDvqvQU6jui6Eaiy8=",
                            res);
        }

        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1Base64HashTest_hello()
        {
            var res = SHA1.Sha1Base64Hash("hello");

            Assert.AreEqual("qvTGHdzF6KLavt4PO0gs2a6pQ00=",
                            res);
        }

        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1Base64HashTest_Hello()
        {
            //ensure changing the cap causes a different hash
            var res = SHA1.Sha1Base64Hash("Hello");

            Assert.AreNotEqual("qvTGHdzF6KLavt4PO0gs2a6pQ00=",
                               res);
        }

        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1Base64HashTest_world()
        {
            var res = SHA1.Sha1Base64Hash("world");

            Assert.AreEqual("fCEUM/AgcVl3Qeb/Wo6jR4mrv0M=",
                            res);
        }

        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1HashTest_1()
        {
            long taskId = 57689768;
            var  time   = DateTime.Parse("9/23/2016 11:17:14 AM");

            var val = taskId + time.ToString();


            var res = SHA1.Sha1Hash(val);

            Assert.AreEqual("bac0eb0fbd42e4d103beabd053a8ee8ba11a8b2f",
                            res);
        }

        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1HashTest_hello()
        {
            var res = SHA1.Sha1Hash("hello");

            Assert.AreEqual("aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d",
                            res);
        }

        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1HashTest_Hello()
        {
            //ensure changing the cap causes a different hash
            var res = SHA1.Sha1Hash("Hello");

            Assert.AreNotEqual("aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d",
                               res);
        }

        [TestCategory("Common Unit Tests")]
        [TestMethod]
        public void Sha1HashTest_world()
        {
            var res = SHA1.Sha1Hash("world");

            Assert.AreEqual("7c211433f02071597741e6ff5a8ea34789abbf43",
                            res);
        }
    }
}
