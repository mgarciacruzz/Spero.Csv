using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Parser
{
    public class Parser
    {
        private Scanner _scanner;
        private int _rowNumber;
        
        public static Parser Parse(string filepath)
        {
            var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            return new Parser(new Scanner(stream));
        }

        public Parser(string filepath)
            :this(new FileStream(filepath, FileMode.Open, FileAccess.Read))
        {
        }

        public Parser(FileStream stream)
            :this(new Scanner(stream))
        {
        }

        public Parser(Scanner scanner)
        {
            _rowNumber = 0;
            _scanner = scanner;
        }

        public bool Read(Row row)
        {
            var token = _scanner.Read();

        start:
            switch (token.Type)
            {
                case TokenType.Comma:
                    row.Add(null, _rowNumber, token.Position); goto comma;
                case TokenType.EOF:
                    return false;
                case TokenType.NewLine:
                    goto newLine;
                case TokenType.DoubleQuote:
                    goto doubleQuote;
                case TokenType.SingleQuote:
                    goto singleQuote;
                case TokenType.Value:
                    row.Add(token.Value, _rowNumber, token.Position); goto value;
                default:
                    throw new CsvParseException();
            }

        comma:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.NewLine:
                    row.Add(null, _rowNumber, token.Position); goto newLine;
                case TokenType.Comma:
                    row.Add(null, _rowNumber, token.Position); goto comma;
                case TokenType.EOF:
                    row.Add(null, _rowNumber, token.Position); return true;
                case TokenType.DoubleQuote:
                    goto doubleQuote;
                case TokenType.SingleQuote:
                    goto singleQuote;
                case TokenType.Value:
                    row.Add(token.Value, _rowNumber, token.Position); goto value;
                default:
                    throw new CsvParseException();
            };

        doubleQuote:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.DoubleQuote:
                    row.Add(null, _rowNumber, token.Position); goto endDoubleQuote;
                case TokenType.Value:
                    row.Add(token.Value, _rowNumber, token.Position); goto valueDoubleQuote;
                default:
                    throw new CsvParseException();
            }

        endDoubleQuote:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.Comma:
                    goto comma;
                case TokenType.EOF:
                case TokenType.NewLine:
                    goto newLine;
                default:
                    throw new CsvParseException();
            }

        singleQuote:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.SingleQuote:
                    row.Add(null, _rowNumber, token.Position); goto endSingleQuote;
                case TokenType.Value:
                    row.Add(token.Value, _rowNumber, token.Position); goto valueSingleQuote;
                default:
                    throw new CsvParseException();
            }

        endSingleQuote:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.Comma:
                    goto comma;
                case TokenType.EOF:
                case TokenType.NewLine:
                    goto newLine;
                default:
                    throw new CsvParseException();
            }

        valueSingleQuote:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.SingleQuote:
                    goto endSingleQuote;
                default:
                    throw new CsvParseException();
            }

        valueDoubleQuote:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.DoubleQuote:
                    goto endSingleQuote;
                default:
                    throw new CsvParseException();
            }

        value:
            token = _scanner.Read();
            switch (token.Type)
            {
                case TokenType.Comma:
                    goto comma;
                case TokenType.EOF:
                case TokenType.NewLine:
                    goto newLine;
                default:
                    throw new CsvParseException();
            }

        newLine:
            _rowNumber++;
            return true;
        }


    }
}
