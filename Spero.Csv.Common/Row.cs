using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv
{
    public class Row
    {
        private int _index;
        private Column[] _columns;

        #region Properties
        public Column[] Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _index = Math.Max(value.Length - 1, 0);
                _columns = value;
            }
        }

        public int Count => Columns.Length;
        #endregion

        public Row()
        {
            _index = 0;
            _columns = new Column[0];
        }

        internal void Add(string value, int row, int col)
        {
            var column = new Column(value, row, col);
            Array.Resize(ref _columns, Count + 1);
            _columns[_index++] = column;
        }

        public Column this[int index]
        {
            get
            {
                if (index > Count - 1)
                    throw new IndexOutOfRangeException();

                return Columns[index];
            }
        }

        public override string ToString()
        {
            return string.Join(",", Columns.Select(x => string.IsNullOrEmpty(x.Value) ? "" : x.Value).ToArray());
        }
    }
}
