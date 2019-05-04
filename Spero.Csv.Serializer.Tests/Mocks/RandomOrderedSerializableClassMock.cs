using Spero.Csv.Serializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer.Tests.Mocks
{
    [CsvSerializable(name: "TableName")]
    public class RandomOrderedSerializableClassMock
    {
        [CsvSerializable(order: 1)]
        public string Column1 { get; set; }

        [CsvSerializable(order: 0)]
        public int Column2 { get; set; }

        [CsvSerializable]
        public bool Column3 { get; set; }

        [CsvSerializable(order: 2)]
        public float Column4 { get; set; }
    }
}
