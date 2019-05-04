using CSharpTest.Net.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer
{
    public interface ICsvSerializer<T> : ISerializer<IEnumerable<T>> where T : new()
    {
        void Serialize(IEnumerable<T> values, Stream stream);
        IEnumerable<T> DeSerialize(Stream stream);
    }
}
