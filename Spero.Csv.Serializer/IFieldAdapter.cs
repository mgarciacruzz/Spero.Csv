using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer
{
    public interface IFieldAdapter
    {
        Type FieldType { get; }
        object GetValue(object targetObject);
        void SetValue(object targetObject, object value);
    }
}
