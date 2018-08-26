using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TflApp.Console.Utils;
using NUnit.Framework;

namespace TflApp.Tests.Utils
{
    [TestFixture]
    public class ConsoleWriterUtilTest
    {
        ConsoleWriterUtil consoleWriterUtil;

        [SetUp]
        public void SetUp()
        {
            consoleWriterUtil = new ConsoleWriterUtil();
        }

        [Test]
        public void WriteOutput_WhenMethodIsCalled_StringIsWrittenToConsole()
        {
            var contentToWriteToConsole = "This is to be written to console";

            TextWriter originalConsoleOutput = System.Console.Out;

            using (StringWriter sw = new StringWriter())
            {
                System.Console.SetOut(sw);

                consoleWriterUtil.WriteOutput(contentToWriteToConsole);

                var contentWrittenToConsole = sw.ToString().Trim();

                System.Console.SetOut(originalConsoleOutput);

                Assert.AreEqual(contentWrittenToConsole, contentToWriteToConsole);
            }


        }
    }
}
