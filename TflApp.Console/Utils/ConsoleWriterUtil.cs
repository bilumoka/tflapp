using System;
using System.Collections.Generic;
using System.Text;

namespace TflApp.Console.Utils
{
    public class ConsoleWriterUtil : IConsoleWriterUtil
    {
        public void WriteOutput(string output)
        {
            System.Console.WriteLine(output);
        }
    }
}
