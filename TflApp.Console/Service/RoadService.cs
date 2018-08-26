using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TflApp.Console.Entity;
using TflApp.Console.Repository;

namespace TflApp.Console.Service
{
    public class RoadService : IRoadService
    {
        private readonly IRoadRepository roadRepository;
        private readonly AppSettings appSettings;

        public RoadService(IOptions<AppSettings> appSettings, IRoadRepository roadRepository)
        {
            this.roadRepository = roadRepository;
            this.appSettings = appSettings.Value;
        }

        public async Task<List<Road>> GetRoadsStatusAsync(string roadName)
        {
            try
            {
                var apiPath = string.Format("{0}/{1}?app_id={2}&app_key={3}", "Road", roadName, appSettings.ApplicationID, appSettings.ApplicationKey);
                return await this.roadRepository.GetAllAsync(apiPath);
            }
            catch (Exception ex)
            {
                throw new Exception("A critical error has occured", ex);
            }

        }
    }
}
