using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InsurancePortal.LocationService.Domain.Entities.Lookups;

namespace InsurancePortal.LocationService.Infrastructure.Data.Configurations.Lookups
{
    public class LocationTypeLookupConfiguration : IEntityTypeConfiguration<LocationTypeLookup>
    {
        public void Configure(EntityTypeBuilder<LocationTypeLookup> builder)
        {
            builder.ToTable("LocationTypeLookups");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(x => x.Code).IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.SortOrder)
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetime2");

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("datetime2");

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new { Id = Guid.Parse("55555555-5555-5555-5555-555555555501"), Code = "Residential", Name = "Residential", Description = (string?)null, IsActive = true, SortOrder = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = Guid.Parse("55555555-5555-5555-5555-555555555502"), Code = "Commercial", Name = "Commercial", Description = (string?)null, IsActive = true, SortOrder = 2, CreatedAt = seedDate, UpdatedAt = seedDate });
        }
    }
}
