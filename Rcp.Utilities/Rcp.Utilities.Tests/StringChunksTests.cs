using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rcp.Utilities.Tests
{
    [TestClass]
    public class StringChunksTests
    {
        [TestCategory("String Unit Tests")]
        [TestMethod]
        public void StringShorterThanChunk()
        {
            var obj = new StringChunks();

            var val = obj.ChunkString("short",
                                      99)
                         .ToArray();

            Assert.AreEqual(1,
                            val.Count());
            Assert.AreEqual("short",
                            val[0]);
        }
        
        [TestCategory("String Unit Tests")]
        [TestMethod]
        public void StringLongerThanChunk()
        {
            var obj = new StringChunks();

            var val = obj.ChunkString("short",
                                      2)
                         .ToArray();

            Assert.AreEqual(3,
                            val.Count());
            Assert.AreEqual("or",
                            val[1]);
        }
    }
}
