using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TflApp.Console.Entity;


namespace TflApp.Console.Repository
{
    public class RoadRepository : IRoadRepository
    {
        private readonly HttpClient httpClient;

        public RoadRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Road>> GetAllAsync(string path)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<Road>));

            HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as List<Road>;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {

                return new List<Road>();
            }

            return new List<Road>();
        }
    }
}
