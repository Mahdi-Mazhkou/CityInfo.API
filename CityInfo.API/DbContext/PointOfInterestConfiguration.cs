using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityInfo.API.DbContext
{
    public class PointOfInterestConfiguration:IEntityTypeConfiguration<PointOfInterest>
    {
        public void Configure(EntityTypeBuilder<PointOfInterest> builder)
        {
            
        }
    }
}
