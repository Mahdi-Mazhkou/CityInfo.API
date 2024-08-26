
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

            modelBuilder.Entity<City>()
                .HasData(
                    new City("Tehran")
                    {
                        Id = 1,
                        Description = "This Is Tehran"

                    },
                    new City("Shiraz")
                    {
                        Id = 2,
                        Description = "This Is Shiraz"
                    },
                    new City("Tabriz")
                    {
                        Id = 3,
                        Description = "This Is Tabriz"
                    }
                );

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest("Tajrish")
                    {
                        Id = 1,
                        CityId = 1,
                        Description = "This Is A Tajrish"

                    },
                    new PointOfInterest("Shemiram")
                    {
                        Id = 2,
                        CityId = 1,
                        Description = "This Is A Shemiran"

                    },
                    new PointOfInterest("Jordan")
                    {
                        Id = 3,
                        CityId = 1,
                        Description = "This Is A Jordan"

                    }

                    );


            modelBuilder.ApplyConfiguration<City>(new CityConfiguration());
            modelBuilder.ApplyConfiguration<PointOfInterest>(new PointOfInterestConfiguration());
        }
    }
}
