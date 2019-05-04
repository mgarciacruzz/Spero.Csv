using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spero.Csv.Serializer.Tests.Mocks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Spero.Csv.Serializer.Tests
{
    [TestClass]
    public class DeserializerTests
    {
        public static string _filename = "DesTestFile.csv";

        [TestMethod]
        public void BasicDeserialization()
        {
            var csvSerializer = new Serializer<SerializableClassMock>();

            IEnumerable<SerializableClassMock> result;
            using (var stream = new FileStream(_filename, FileMode.Open))
            {
                result = csvSerializer.DeSerialize(stream);
            }

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("a1", result.ElementAt(0).Column1);
            Assert.AreEqual(1, result.ElementAt(0).Column2);
            Assert.AreEqual("a2", result.ElementAt(1).Column1);
            Assert.AreEqual(2, result.ElementAt(1).Column2);
        }
    }
}
