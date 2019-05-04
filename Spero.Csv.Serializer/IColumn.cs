using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer
{
    public interface IColumn
    {
        string Name { get; }
        IFieldAdapter Adapter { get; }
    }
}
