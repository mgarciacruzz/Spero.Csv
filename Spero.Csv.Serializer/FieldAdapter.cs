using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer
{
    public class FieldAdapter : IFieldAdapter
    {
        public Type FieldType { get; private set; }
        private MethodInfo _getter, _setter;

        public FieldAdapter(PropertyInfo property)
        {
            FieldType = property.PropertyType;
            _getter = property.GetMethod;
            _setter = property.SetMethod;
        }
        public object GetValue(object targetObject)
        {
            try
            {
                return _getter.Invoke(targetObject, new object[0]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetValue(object targetObject, object value)
        {
            _setter.Invoke(targetObject, new[] { Convert.ChangeType(value, FieldType) });
        }
    }
}
