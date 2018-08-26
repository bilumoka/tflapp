using System;
using System.Collections.Generic;
using System.Text;
using TflApp.Console.Entity;

namespace TflApp.Console.Utils
{
    public interface IExitCodeUtil
    {
        void ExitWithCode(ExitCode exitCode);
    }
}
