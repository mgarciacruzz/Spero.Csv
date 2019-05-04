using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spero.Csv.Serializer.Extensions
{
    public static class StreamExt
    {
        public static void Write(this Stream stream, string value)
        {
            var buffer = value.ToByteArray();

            stream.Write(buffer, 0, buffer.Length);
        }

        public static void WriteLine(this Stream stream, string value)
        {
            stream.WriteLine(value, Encoding.UTF8);
        }

        public static void WriteLine(this Stream stream, string value, Encoding encoding)
        {
            var lineEnding = "\r\n";

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    lineEnding = "\n";
                    break;
                default:
                    break;
            }
            var buffer = string.Format("{0}{1}", value, lineEnding).ToByteArray(encoding);

            stream.Write(buffer, 0, buffer.Length);
        }

        public static char ReadChar(this Stream stream)
        {
            byte[] buffer = new byte[1];

            var bytesRead = stream.Read(buffer, 0, 1);

            if (bytesRead == 0)
                return '\0'; // null character

            return (char)buffer[0];
        }

        public static string ReadLine(this Stream stream, Encoding encoding)
        {
            var buffer = new byte[1];
            var result = new List<byte>();
            var index = 0;
            while (stream.Read(buffer, 0, 1) != 0)
            {
                var c = (char)buffer[0];
                if (c == '\r' && stream.ReadChar() == '\n') // Windows line ending
                    break;
                else if (c == '\n')
                    break; // linux line ending

                result.Add(buffer[0]);
                index++;
            }

            return encoding.GetString(result.ToArray());
        }
    }
}
