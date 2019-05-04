using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Parser
{
    public class Scanner:IDisposable
    {
        private int _position;
        private readonly StreamReader _reader;
        private TokenType _lastToken;
        public Scanner(FileStream stream)
        {
            _position = -1;
            _reader =  new StreamReader(stream);
            _lastToken = TokenType.Default;
        }

        public Token Read()
        {
            if (_reader.EndOfStream)
            {
                _lastToken = TokenType.EOF;
                return new Token("\0", TokenType.EOF, _position + 1);
            }
            _position++;
            var c = (char)_reader.Read();

            switch (c)
            {
                case '\"':
                    _lastToken = TokenType.DoubleQuote;
                    return new Token("\"", TokenType.DoubleQuote, _position);
                case '\'':
                    _lastToken = TokenType.SingleQuote;
                    return new Token("\'", TokenType.SingleQuote, _position);
                case ',':
                    _lastToken = TokenType.Comma;
                    return new Token(",", TokenType.Comma, _position);
                case '\r':
                    _position++;
                    _reader.Read();
                    return new Token("\n", TokenType.NewLine, _position);
                case '\n':
                    _lastToken = TokenType.NewLine;
                    return new Token("\n", TokenType.NewLine, _position);
            }

            var stopQuote = '\n';

            if (_lastToken == TokenType.DoubleQuote || _lastToken == TokenType.SingleQuote)
            {
                stopQuote = _lastToken == TokenType.DoubleQuote ? '\"' : '\'';
                
            }

            var builder = new StringBuilder();
            builder.Append(c);
            var count = 0;
            c = (char)_reader.Peek();
            while (!_reader.EndOfStream && c != stopQuote  && c != ',' && c != '\n' && c != '\r')
            {
                builder.Append(c);
                _reader.Read();
                c = (char)_reader.Peek();
                count++;
            }

            var token = new Token(builder.ToString(), TokenType.Value, _position);
            _position += count;
            return token;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
