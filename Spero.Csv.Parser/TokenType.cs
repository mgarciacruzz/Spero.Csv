using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Parser
{
    public enum TokenType
    {
        Comma,
        EOF,
        NewLine,
        DoubleQuote,
        SingleQuote,
        Value,
        Default
    }
}
