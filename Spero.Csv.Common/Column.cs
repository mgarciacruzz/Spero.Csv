using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv
{ 
    public class Column
    {
        #region Properties
        public string Value { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        #endregion

        public Column(string value, int row, int col)
        {
            Row = row;
            Col = col;
            Value = value;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Value);
        }

        public override string ToString()
        {
            return $"({Row},{Col}) - {Value}";
        }
    }
}
