using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityInfo.API.DbContext
{
    public class PointOfInterestConfiguration:IEntityTypeConfiguration<PointOfInterest>
    {
        public void Configure(EntityTypeBuilder<PointOfInterest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(50);

        }
    }
}
