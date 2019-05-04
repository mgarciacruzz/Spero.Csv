using Spero.Csv.Serializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer.Tests.Mocks
{
    [CsvSerializable(name: "TableName")]
    public class SerializableClassMock
    {
        [CsvSerializable]
        public string Column1 { get; set; }

        [CsvSerializable]
        public int Column2 { get; set; }
    }
}
