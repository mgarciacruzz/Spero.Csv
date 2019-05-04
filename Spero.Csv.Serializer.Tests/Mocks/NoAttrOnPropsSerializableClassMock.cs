using Spero.Csv.Serializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer.Tests.Mocks
{
    [CsvSerializable(name: "TableName")]
    public class NoAttrOnPropsSerializableClassMock
    {
        public string Column1 { get; set; }

        [IgnoreCsvSerializable]
        public int Column2 { get; set; }
        public bool Column3 { get; set; }
    }
}
