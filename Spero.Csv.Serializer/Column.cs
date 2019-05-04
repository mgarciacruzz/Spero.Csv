using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer
{
    public class Column : IColumn
    {
        public string Name { get; }
        public IFieldAdapter Adapter { get; }

        public Column(string name, IFieldAdapter adapter)
        {
            Name = name;
            Adapter = adapter;
        }
    }
}
