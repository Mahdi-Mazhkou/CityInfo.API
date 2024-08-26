using CityInfo.API.DbContext;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories
{
    public class CityInfoRepository:ICityInfoRepository
    {
        private readonly CityInfoDbContext _context;

        public CityInfoRepository(CityInfoDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context)); 
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities
                .OrderBy(x=>x.Name)
                .ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return await _context.Cities
                    .Include(x=>x.PointOfInterests)
                    .Where(c => c.Id == cityId)
                    .FirstOrDefaultAsync();
            }

            return await _context.Cities
                .Where(c => c.Id == cityId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId)
        {
            return await
                _context
                    .PointOfInterests
                    .Where(p => p.CityId == cityId)
                    .ToListAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return await
                _context
                    .PointOfInterests
                    .Where(p => p.CityId == cityId && p.Id==pointOfInterestId)
                    .FirstOrDefaultAsync();
        }
    }
}
