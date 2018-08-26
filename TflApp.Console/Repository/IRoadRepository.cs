using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TflApp.Console.Entity;

namespace TflApp.Console.Repository
{
    public interface IRoadRepository
    {
        Task<List<Road>> GetAllAsync(string path);
    }
}
