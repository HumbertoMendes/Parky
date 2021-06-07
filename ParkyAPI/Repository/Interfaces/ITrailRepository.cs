using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Repository.Interfaces
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);
        Trail GetTrail(int id);
        bool TrailExists(int id);
        bool TrailExists(string name);
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        bool Save();
    }
}
