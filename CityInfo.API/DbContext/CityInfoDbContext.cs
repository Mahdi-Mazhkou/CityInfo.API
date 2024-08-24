
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContext
{
    public class CityInfoDbContext : Microsoft.EntityFrameworkCore.DbContext

    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) : base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<City>(new CityConfiguration());
            modelBuilder.ApplyConfiguration<PointOfInterest>(new PointOfInterestConfiguration());
        }
    }
}
