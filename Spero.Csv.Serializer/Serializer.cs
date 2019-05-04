using Spero.Csv.Serializer.Attributes;
using Spero.Csv.Serializer.Extensions;
using Spero.Csv.Serializer.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer
{
    public class Serializer<T> : ICsvSerializer<T> where T : new()
    {
        private HashSet<int> _fixedIndexes = new HashSet<int>();
        private Encoding _encoding = Encoding.ASCII;

        #region Properties
        public char Delimiter { get; private set; }
        public IColumn[] Columns { get; private set; }
        public string TableName { get; private set; }

        public string[] Headers
        {
            get => Columns.Select(x => x.Name).ToArray();
            private set
            {
                Headers = value;
            }
        }

        #endregion

        public Serializer(char delimiter = ',')
        {
            Delimiter = delimiter;

            // TODO: refactor redundant code
            GetStructure();
        }

        private void GetStructure()
        {

            if (!(typeof(T).GetCustomAttributes(true).Where(x => (x as CsvSerializableAttribute) != null).FirstOrDefault() is CsvSerializableAttribute classAttribute))
                throw new Exception("Class must contain the CsvSerializableAttribute attribute");

            TableName = classAttribute.Name;
            var columns = new IColumn[0];
            // Getting the fields witht he csvSerializableAttribute
            var properties = typeof(T).GetProperties();

            // Keeps track of the next column position (in case that they are out of order)
            var nextItemIndex = 0;

            foreach (var propInfo in properties)
            {
                var attributes = propInfo.GetCustomAttributes(true);
                var bothAttributesAbsent = true;

                foreach (var attr in attributes)
                {
                    if (attr is CsvSerializableAttribute csvAttribute)
                    {
                        bothAttributesAbsent = false;
                        if (csvAttribute.OrderSpecified)
                        {
                            var order = csvAttribute.Order;

                            // Verify that the order is not repeated
                            if (_fixedIndexes.Contains(order))
                                throw new InvalidDataException(string.Format("Order {0} repeated. Order must be unique", order));

                            var temp = ArrayUtilities.TryGet(ref columns, order);
                            var col = new Column(csvAttribute.Name, new FieldAdapter(propInfo));

                            if (temp == null)
                            {
                                ArrayUtilities.TryAdd(ref columns, order, col);

                                if (order == nextItemIndex)
                                {
                                    // Advance next item index until an available spot is found
                                    FindNextAvailableColumn(ref columns, ref nextItemIndex);
                                }
                            }
                            else
                            {
                                ArrayUtilities.TryAdd(ref columns, order, col);
                                while (temp != null)
                                {
                                    var value = ArrayUtilities.TryGet(ref columns, ++order);

                                    if (_fixedIndexes.Contains(order))
                                        continue;
                                    ArrayUtilities.TryAdd(ref columns, order, temp);
                                    temp = value;
                                }

                                nextItemIndex = order;
                                FindNextAvailableColumn(ref columns, ref nextItemIndex);
                            }
                        }
                        else
                        {
                            var col = new Column(csvAttribute.Name, new FieldAdapter(propInfo));
                            DefaultAttributeBehaviour(ref columns, ref nextItemIndex, col);
                        }
                    }
                    else if (attr is IgnoreCsvSerializable)
                    {
                        bothAttributesAbsent = false;
                        // ignore this property and move to the next one
                        break;
                    }
                }

                // Csv Serializable can be ommited in the properties
                if (bothAttributesAbsent)
                {
                    var col = new Column(propInfo.Name, new FieldAdapter(propInfo));
                    DefaultAttributeBehaviour(ref columns, ref nextItemIndex, col);
                }

            }
            // Correct next index pointing to the last element
            Array.Resize(ref columns, columns.Length - 1);
            Columns = columns;
        }
        public IEnumerable<T> ReadFrom(Stream stream)
        {
            var result = new List<T>();

            var firstLine = stream.ReadLine(_encoding);
            if (!string.Equals(firstLine, TableName, StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException("Table name in the csv file does not match the class defined table name");

            // Consume header line
            stream.ReadLine(_encoding);

            string line;
            while (!string.IsNullOrEmpty(line = stream.ReadLine(_encoding)))
            // outer loop reading the lines
            {
                var temp = new T();

                var tokens = line.Split(Delimiter);

                if (tokens.Length != Columns.Length)
                    throw new FormatException("Number of Columns in the file do not match the fields in the object");

                var index = 0;
                foreach (var col in Columns)
                {
                    col.Adapter.SetValue(temp, tokens[index]);
                    index++;
                }

                result.Add(temp);
            }

            return result.AsEnumerable();
        }

        public void WriteTo(IEnumerable<T> value, Stream stream)
        {

            // Writing name of the table
            stream.WriteLine(TableName);

            // Writing headers
            stream.WriteLine(string.Join(Delimiter.ToString(), Headers));

            foreach (var item in value)
            {
                var values = new List<string>();
                var index = 0;
                foreach (var col in Columns)
                {
                    var adapter = col.Adapter;
                    if (adapter != null)
                    {
                        values.Add(adapter.GetValue(item).ToString());
                        index++;
                    }
                }

                stream.WriteLine(string.Join(Delimiter.ToString(), values), _encoding);
            }
        }

        public void Serialize(IEnumerable<T> values, Stream stream)
        {
            WriteTo(values, stream);
        }

        public IEnumerable<T> DeSerialize(Stream stream)
        {
            return ReadFrom(stream);
        }

        #region Helper Functions
        private void DefaultAttributeBehaviour(ref IColumn[] columns, ref int currentIndex, IColumn col)
        {
            ArrayUtilities.TryAdd(ref columns, currentIndex, col);

            // Advance next item index until an available spot is found
            FindNextAvailableColumn(ref columns, ref currentIndex);
        }

        private void FindNextAvailableColumn(ref IColumn[] columns, ref int index)
        {
            // Advance next item index until an available spot is found
            while (ArrayUtilities.TryGet(ref columns, ++index) != null)
            {
            }
        }
        #endregion
    }
}
