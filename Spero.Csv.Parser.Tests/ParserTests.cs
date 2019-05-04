using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spero.Csv;

namespace Spero.Csv.Parser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var parser = Parser.Parse("testfile.csv"))
            {
                var row = new Row();
                while (parser.Read(row))
                {
                    Console.WriteLine(row);
                }
            }

        }
    }
}
