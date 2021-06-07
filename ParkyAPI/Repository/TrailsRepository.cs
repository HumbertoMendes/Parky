using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int id)
        {
            return _db.Trails
                .Include(trail => trail.NationalPark)
                .FirstOrDefault(park => park.Id == id);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails
                .Include(trail => trail.NationalPark)
                .OrderBy(trail => trail.Name)
                .ToList();
        }
        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _db.Trails
                .Include(trail => trail.NationalPark)
                .Where(trail => trail.NationalPark.Id == nationalParkId)
                .OrderBy(trail => trail.Name)
                .ToList();
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(park => park.Id == id);
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(park => park.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
