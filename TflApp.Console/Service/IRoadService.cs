using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TflApp.Console.Entity;

namespace TflApp.Console.Service
{
    public interface IRoadService
    {
        Task<List<Road>> GetRoadsStatusAsync(string roadName);
    }
}
