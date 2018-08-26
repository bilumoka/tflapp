using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TflApp.Console.Entity;
using TflApp.Console.Service;
using TflApp.Console.Repository;
using Moq;
using NUnit;
using NUnit.Framework;

namespace TflApp.Tests.Service
{
    [TestFixture]
    public class RoadServiceTest
    {
        private RoadService roadService;
        private Mock<IRoadRepository> roadRepository;
        private Mock<IOptions<AppSettings>> appSettings;

        [SetUp]
        public void SetUp()
        {
            roadRepository = new Mock<IRoadRepository>();
            appSettings = new Mock<IOptions<AppSettings>>();
           
        }

        [Test]
        public void GetRoadsStatusAsync_ReturnsARoadObject()
        {        
            var listRoads = new List<Road>();
            var settings = new AppSettings { ApplicationID = "appID", ApplicationKey = "appKey", BaseApiUrl = "http://host" };
            var roadName = "A2000";
            appSettings.SetupGet(x => x.Value).Returns(settings);
            roadRepository.Setup(x => x.GetAllAsync(It.IsAny<String>())).Returns(Task.FromResult(listRoads));
            roadService = new RoadService(appSettings.Object, roadRepository.Object);

            var result = roadService.GetRoadsStatusAsync(roadName);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetAwaiter().GetResult(), Is.TypeOf<List<Road>>());
        }
    }
}
                                                                                                                                                                                                                                                                                