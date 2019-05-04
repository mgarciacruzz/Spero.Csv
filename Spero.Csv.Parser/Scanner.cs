using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Parser
{
    public class Scanner
    {
        private int _position;
        private readonly StreamReader _reader;
        public Scanner(FileStream stream)
        {
            _position = 0;
            _reader =  new StreamReader(stream);
        }

        public Token Read()
        {
            var c = (char)_reader.Read();


            
            _position++;
            return new Token(null, TokenType.Comma, 0);
        }
    }
}
