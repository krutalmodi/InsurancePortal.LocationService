using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InsurancePortal.LocationService.Domain.Entities.Lookups;

namespace InsurancePortal.LocationService.Infrastructure.Data.Configurations.Lookups
{
    public class CountryLookupConfiguration : IEntityTypeConfiguration<CountryLookup>
    {
        public void Configure(EntityTypeBuilder<CountryLookup> builder)
        {
            builder.ToTable("CountryLookups");
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

            builder.Property(x => x.IsoCode)
                .IsRequired()
                .HasMaxLength(2);
            builder.HasIndex(x => x.IsoCode).IsUnique();

            builder.HasMany(x => x.Provinces)
                .WithOne(p => p.Country)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeee01"), Code = "USA", Name = "United States", Description = (string?)null, IsActive = true, SortOrder = 1, CreatedAt = seedDate, UpdatedAt = seedDate, IsoCode = "US" },
                new { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeee02"), Code = "Canada", Name = "Canada", Description = (string?)null, IsActive = true, SortOrder = 2, CreatedAt = seedDate, UpdatedAt = seedDate, IsoCode = "CA" });
        }
    }
}
