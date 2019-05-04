using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Parser
{
    public class Row
    {
        #region Properties
        public Column[] Columns => _cols.ToArray();

        public int Count => _cols.Count;
        #endregion

        private List<Column> _cols;
        
        public Row()
        {
            _cols = new List<Column>();
        }

        internal void Add(string value, int row, int col)
        {
            var column = new Column(value, row, col);
            _cols.Add(column);
        }

        public Column this[int index]
        {
            get
            {
                if (index > _cols.Count - 1)
                    throw new IndexOutOfRangeException();

                return _cols.ElementAt(index);
            }
        }
    }
}
