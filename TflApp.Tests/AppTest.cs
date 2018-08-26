using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TflApp.Console;
using TflApp.Console.Entity;
using TflApp.Console.Service;
using TflApp.Console.Utils;
using Moq;
using NUnit.Framework;

namespace TflApp.Tests
{
    [TestFixture]
    public class AppTest
    {
        private App app;
        private Mock<IRoadService> roadService;
        private Mock<IConsoleWriterUtil> consoleWriterUtil;
        private Mock<ILogger<App>> logger;
        private Mock<IExitCodeUtil> exitCodeUtil;

        [SetUp]
        public void SetUp()
        {
            roadService = new Mock<IRoadService>();
            consoleWriterUtil = new Mock<IConsoleWriterUtil>();
            logger = new Mock<ILogger<App>>();
            exitCodeUtil = new Mock<IExitCodeUtil>();
            app = new App(roadService.Object,
                consoleWriterUtil.Object, 
                exitCodeUtil.Object, 
                logger.Object);
        }

        [Test]
        public void ShowRoadStatusAync_WhenRoadIsFound_ShouldShowRoadStatus()
        {
            var roads = new List<Road>();
            roads.Add(new Road
            {
                Type = "Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities",
                Id = "a13",
                DisplayName = "A13",
                StatusSeverity = "Good",
                StatusSeverityDescription = "No Exceptional Delays",
                Url = "/Road/a13"
            });

            var roadName = "A13";
            string[] args = { roadName };

            roadService.Setup(x => x.GetRoadsStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(roads));

            var expectedLine1 = string.Format("The status of the {0} is as follows.", roads[0].DisplayName);
            var expectedLine2 = string.Format("Road Status is {0}.", roads[0].StatusSeverity);
            var expectedLine3 = string.Format("Road Status Description is {0}.", roads[0].StatusSeverityDescription);

            app.ShowRoadStatusAsync(args).GetAwaiter().GetResult();

            consoleWriterUtil.Verify(cw => cw.WriteOutput(It.Is<string>(s => s == expectedLine1)), Times.Once);
            consoleWriterUtil.Verify(cw => cw.WriteOutput(It.Is<string>(s => s == expectedLine2)), Times.Once);
            consoleWriterUtil.Verify(cw => cw.WriteOutput(It.Is<string>(s => s == expectedLine3)), Times.Once);
            exitCodeUtil.Verify(ec => ec.ExitWithCode(ExitCode.Success), Times.Once);
        }

        [Test]
        public void ShowRoadStatusAync_WhenRoadIsNotFound_ShouldShowRoadIsNotFound()
        {
            var roads = new List<Road>();
            roadService.Setup(x => x.GetRoadsStatusAsync(It.IsAny<string>())).Returns(Task.FromResult(roads));

            var roadName = "A5000";
            string[] args = { roadName };

            var expectedLine1 = string.Format("The following road id is not recognised: {0}.", roadName);

            app.ShowRoadStatusAsync(args).GetAwaiter().GetResult();

            consoleWriterUtil.Verify(cw => cw.WriteOutput(It.Is<string>(s => s == expectedLine1)), Times.Once);
            exitCodeUtil.Verify(ec => ec.ExitWithCode(ExitCode.Invalid), Times.Once);
        }

        [Test]
        public void ShowRoadStatusAync_WhenExceptionIsThrown_ShouldShowGenericErrorMessage()
        {
            roadService.Setup(x => x.GetRoadsStatusAsync(It.IsAny<string>())).Throws(new Exception("Application error."));

            var roadName = "A13";
            string[] args = { roadName };

            var expectedLine1 = string.Format("Sorry, an unexpected error occurred.");
            app.ShowRoadStatusAsync(args).GetAwaiter().GetResult();

            consoleWriterUtil.Verify(cw => cw.WriteOutput(It.Is<string>(s => s == expectedLine1)), Times.Once);
            exitCodeUtil.Verify(ec => ec.ExitWithCode(ExitCode.UnknownError), Times.Once);
        }

        [Test]
        public void ShowRoadStatusAync_WhenNoArgumentsArePassed()
        {
            var expectedLine1 = "Please enter a road name.";
            string[] args = { };
            app.ShowRoadStatusAsync(args).GetAwaiter().GetResult();

            consoleWriterUtil.Verify(cw => cw.WriteOutput(It.Is<string>(s => s == expectedLine1)), Times.Once);
            roadService.Verify(r => r.GetRoadsStatusAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
