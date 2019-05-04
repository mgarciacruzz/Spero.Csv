using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spero.Csv.Serializer.Tests.Mocks;

namespace Spero.Csv.Serializer.Tests
{
    [TestClass]
    public class SerializerTests
    {
        private const string _filename = "testfile.csv";

        [TestCleanup]
        public void TestCleanup()
        {

            // Delete file after test
            if (File.Exists(_filename))
            {
                File.Delete(_filename);
            }
        }

        [TestMethod]
        public void TableDesignTest()
        {
            var csvSerializer = new Serializer<SerializableClassMock>();

            Assert.AreEqual("TableName", csvSerializer.TableName);
            Assert.AreEqual(2, csvSerializer.Headers.Length);
            Assert.AreEqual(nameof(SerializableClassMock.Column1), csvSerializer.Headers[0]);
            Assert.AreEqual(nameof(SerializableClassMock.Column2), csvSerializer.Headers[1]);
        }

        [TestMethod]
        public void OrderedSerializerTest()
        {

            var values = new List<SerializableClassMock>()
            {
                new SerializableClassMock{Column1 = "a1", Column2 = 1},
                new SerializableClassMock{Column1 = "a2", Column2 = 2}
            };
            var csvSerializer = new Serializer<SerializableClassMock>();

            using (var stream = new FileStream(_filename, FileMode.OpenOrCreate))
            {
                csvSerializer.Serialize(values, stream);
            }

            var expectedData = new[]
            {
                "TableName",
                "Column1,Column2",
                "a1,1",
                "a2,2"
            };

            // Checking created file
            using (var lines = File.ReadLines(_filename).GetEnumerator())
            {
                CheckFile(expectedData, lines);
            }
        }

        [TestMethod]
        public void OutOfOrderSerializerTest()
        {
            var values = new List<RandomOrderedSerializableClassMock>()
            {
                new RandomOrderedSerializableClassMock{Column1 = "a1", Column2 = 1, Column3=true, Column4=1.1F},
                new RandomOrderedSerializableClassMock{Column1 = "a2", Column2 = 2, Column3=false, Column4=2.2F}
            };
            var csvSerializer = new Serializer<RandomOrderedSerializableClassMock>();

            using (var stream = new FileStream(_filename, FileMode.OpenOrCreate))
            {
                csvSerializer.Serialize(values, stream);
            }

            var expectedData = new[]
            {
                "TableName",
                "Column2,Column1,Column4,Column3",
                "1,a1,1.1,True",
                "2,a2,2.2,False"
            };

            // Checking created file
            using (var lines = File.ReadLines(_filename).GetEnumerator())
            {
                CheckFile(expectedData, lines);
            }
        }

        [TestMethod]
        public void DefaultAndIgnoreAttributeTest()
        {
            var values = new List<NoAttrOnPropsSerializableClassMock>()
            {
                new NoAttrOnPropsSerializableClassMock{Column1 = "a1", Column2 = 1, Column3 = true},
                new NoAttrOnPropsSerializableClassMock{Column1 = "a2", Column2 = 2, Column3 = false}
            };
            var csvSerializer = new Serializer<NoAttrOnPropsSerializableClassMock>();

            using (var stream = new FileStream(_filename, FileMode.OpenOrCreate))
            {
                csvSerializer.Serialize(values, stream);
            }

            var expectedData = new[]
            {
                "TableName",
                "Column1,Column3",
                "a1,True",
                "a2,False"
            };

            // Checking created file
            using (var lines = File.ReadLines(_filename).GetEnumerator())
            {
                CheckFile(expectedData, lines);
            }
        }

        [TestMethod]
        public void OverridePropColNamesTest()
        {
            var values = new List<OverridenColNamesSerializableClassMock>()
            {
                new OverridenColNamesSerializableClassMock{Column1 = "a1", Column2 = 1},
                new OverridenColNamesSerializableClassMock{Column1 = "a2", Column2 = 2}
            };
            var csvSerializer = new Serializer<OverridenColNamesSerializableClassMock>();

            using (var stream = new FileStream(_filename, FileMode.OpenOrCreate))
            {
                csvSerializer.Serialize(values, stream);
            }

            var expectedData = new[]
            {
                "TableName",
                "NewCol1,Column2",
                "a1,1",
                "a2,2"
            };


            // Checking created file
            using (var lines = File.ReadLines(_filename).GetEnumerator())
            {
                CheckFile(expectedData, lines);
            }
        }

        private void CheckFile(string[] expectedData, IEnumerator<string> lines)
        {
            var length = expectedData.Length;

            for (int i = 0; i < length; i++)
            {
                var data = expectedData[i];
                lines.MoveNext();
                Assert.AreEqual(data, lines.Current.ToString());
            }
        }
    }
}
