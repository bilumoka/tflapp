using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TflApp.Console.Entity;
using TflApp.Console.Service;
using TflApp.Console.Utils;


namespace TflApp.Console
{
    public class App
    {
        private readonly IRoadService roadService;
        private readonly IConsoleWriterUtil consoleWriterUtil;
        private readonly ILogger<App> logger;
        private readonly IExitCodeUtil exitCodeUtil;

        public App(IRoadService roadService,
            IConsoleWriterUtil consoleWriterUtil,
            IExitCodeUtil exitCodeUtil,
            ILogger<App> logger)
        {
            this.roadService = roadService;
            this.consoleWriterUtil = consoleWriterUtil;
            this.exitCodeUtil = exitCodeUtil;
            this.logger = logger;
        }

        public async Task ShowRoadStatusAsync(string[] args)
        {
            if (args.Length == 0)
            {
                consoleWriterUtil.WriteOutput("Please enter a road name.");
                this.exitCodeUtil.ExitWithCode(ExitCode.NoInputProvided);
                return;
            }

            try
            {
                var roadName = args.GetValue(0).ToString();
                var roads = await roadService.GetRoadsStatusAsync(roadName);

                if (roads.Count >= 1)
                {
                    Action<Road> WriteStatusLine1 = (r) => consoleWriterUtil.WriteOutput(string.Format("The status of the {0} is as follows.", r.DisplayName));
                    Action<Road> WriteStatusLine2 = (r) => consoleWriterUtil.WriteOutput(string.Format("Road Status is {0}.", r.StatusSeverity));
                    Action<Road> WriteStatusLine3 = (r) => consoleWriterUtil.WriteOutput(string.Format("Road Status Description is {0}.", r.StatusSeverityDescription));

                    roads.ForEach(r =>
                    {
                        WriteStatusLine1(r);
                        WriteStatusLine2(r);
                        WriteStatusLine3(r);
                    });

                    this.exitCodeUtil.ExitWithCode(ExitCode.Success);
                }
                else
                {
                    consoleWriterUtil.WriteOutput(string.Format( "The following road id is not recognised: {0}.", roadName));

                    this.exitCodeUtil.ExitWithCode(ExitCode.Invalid);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred");
                consoleWriterUtil.WriteOutput("Sorry, an unexpected error occurred.");

                this.exitCodeUtil.ExitWithCode(ExitCode.UnknownError);
            }

        }
    }
}
