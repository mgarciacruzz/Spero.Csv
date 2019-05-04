using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Parser
{
    public class CsvParseException: Exception
    {
        public CsvParseException()
            : base()
        {
        }

        public CsvParseException(string message)
          : base(message)
        {
        }

        public CsvParseException(string message, Exception innerException)
            :base(message, innerException)
        {
        }

       
    }
}
