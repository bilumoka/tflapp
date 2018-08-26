using System;
using System.Collections.Generic;
using System.Text;
using TflApp.Console.Entity;

namespace TflApp.Console.Utils
{
    public class ExitCodeUtil : IExitCodeUtil
    {
        public void ExitWithCode(ExitCode exitCode)
        {
            Environment.Exit((int)exitCode);
        }
    }
}
