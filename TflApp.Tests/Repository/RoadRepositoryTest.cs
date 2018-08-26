using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TflApp.Console.Entity;
using TflApp.Console.Repository;
using Moq;
using NUnit;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace TflApp.Tests.Repository
{
    [TestFixture]
    public class RoadRepositoryTest
    {
        private RoadRepository roadRepository;
        private Mock<HttpClient> httpClient;

        [SetUp]
        public void SetUp()
        {
            httpClient = new Mock<HttpClient>();
        }

        [Test]
        public void GetRoadsStatusAsync_WhenHttpStatusIs200_ReturnsNonEmptyListOfRoads()
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

            var expectedCount = roads.Count;
            var urlPath = "roads/A13";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://appapi/roads/A13")
                .Respond("application/json", GetRoadStatusJson(roads));
               
            var apiClient = mockHttp.ToHttpClient();
            apiClient.BaseAddress = new Uri("https://appapi/");

            roadRepository = new RoadRepository(apiClient);

            var result = roadRepository.GetAllAsync(urlPath).GetAwaiter().GetResult();

            Assert.That(result, Is.TypeOf<List<Road>>());
            Assert.That(result, Has.Count.EqualTo(expectedCount));
            Assert.That(result[0].Id, Is.EquivalentTo(roads[0].Id));
            Assert.That(result[0].Type, Is.EquivalentTo(roads[0].Type));
            Assert.That(result[0].DisplayName, Is.EquivalentTo(roads[0].DisplayName));
            Assert.That(result[0].StatusSeverity, Is.EquivalentTo(roads[0].StatusSeverity));
            Assert.That(result[0].StatusSeverityDescription, Is.EquivalentTo(roads[0].StatusSeverityDescription));
            Assert.That(result[0].Url, Is.EquivalentTo(roads[0].Url));
        }

        [Test]
        public void GetRoadsStatusAsync_WhenHttpStatusIs404_ReturnsEmptyListOfRoads()
        {
            var expectedCount = 0;
            var urlPath = "roads/A13";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://appapi/roads/A13")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.NotFound));

            var apiClient = mockHttp.ToHttpClient();
            apiClient.BaseAddress = new Uri("https://appapi/");

            roadRepository = new RoadRepository(apiClient);

            var result = roadRepository.GetAllAsync(urlPath).GetAwaiter().GetResult();

            Assert.That(result, Is.TypeOf<List<Road>>());
            Assert.That(result, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public void GetRoadsStatusAsync_WhenHttpStatusIsNot200Or404_ReturnsEmptyListOfRoads()
        {
            var expectedCount = 0;
            var urlPath = "roads/A13";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://appapi/roads/A13")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest));

            var apiClient = mockHttp.ToHttpClient();
            apiClient.BaseAddress = new Uri("https://appapi/");

            roadRepository = new RoadRepository(apiClient);

            var result = roadRepository.GetAllAsync(urlPath).GetAwaiter().GetResult();

            Assert.That(result, Is.TypeOf<List<Road>>());
            Assert.That(result, Has.Count.EqualTo(expectedCount));
        }

        private string GetRoadStatusJson(List<Road> roads)
        {
            var result = "";

            MemoryStream ms = new MemoryStream();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Road>));
            ser.WriteObject(ms, roads);
            byte[] json = ms.ToArray();
            ms.Close();
            result = Encoding.UTF8.GetString(json, 0, json.Length);

            return result;
        }
    }
}
