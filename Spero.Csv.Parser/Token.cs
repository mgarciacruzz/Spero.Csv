using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Parser
{
    public class Token
    {
        #region Properties
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public int Position { get; private set; }
        #endregion

        public Token(string value, TokenType type, int pos)
        {
            Value = value;
            Type = type;
            Position = pos;
        }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}
