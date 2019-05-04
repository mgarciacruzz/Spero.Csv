using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer.Utilities
{
    public static class ArrayUtilities
    {
        public static void TryAdd<T>(ref T[] array, int index, T value)
        {
            CheckArray(ref array, index);

            array[index] = value;
        }

        public static T TryGet<T>(ref T[] array, int index)
        {
            CheckArray(ref array, index);

            return array[index];
        }

        private static void CheckArray<T>(ref T[] array, int index)
        {
            if (array == null)
                throw new NullReferenceException("array");

            var arrayLength = array.Length;

            if (index > arrayLength - 1)
                Array.Resize(ref array, ++index);
        }
    }
}
