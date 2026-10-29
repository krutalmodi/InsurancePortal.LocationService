using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InsurancePortal.LocationService.Domain.Entities.Lookups;

namespace InsurancePortal.LocationService.Infrastructure.Data.Configurations.Lookups
{
    public class ProvinceLookupConfiguration : IEntityTypeConfiguration<ProvinceLookup>
    {
        public void Configure(EntityTypeBuilder<ProvinceLookup> builder)
        {
            builder.ToTable("ProvinceLookups");
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

            builder.Property(x => x.CountryId)
                .IsRequired();

            builder.Property(x => x.ProvinceCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.HasIndex(x => new { x.CountryId, x.ProvinceCode })
                .IsUnique();

            builder.HasOne(x => x.Country)
                .WithMany(c => c.Provinces)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var usa = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeee01");
            var canada = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeee02");
            builder.HasData(
                Province("ffffffff-ffff-ffff-ffff-ffffffffff01", usa, "CA", "California", 1, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff02", usa, "TX", "Texas", 2, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff03", usa, "NY", "New York", 3, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff04", usa, "FL", "Florida", 4, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff05", canada, "ON", "Ontario", 5, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff06", canada, "QC", "Quebec", 6, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff07", canada, "BC", "British Columbia", 7, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff08", canada, "AB", "Alberta", 8, seedDate),
                Province("ffffffff-ffff-ffff-ffff-ffffffffff09", canada, "MB", "Manitoba", 9, seedDate));
        }

        private static object Province(string id, Guid countryId, string code, string name, int sortOrder, DateTime seedDate) => new
        {
            Id = Guid.Parse(id),
            CountryId = countryId,
            Code = code,
            ProvinceCode = code,
            Name = name,
            Description = (string?)null,
            IsActive = true,
            SortOrder = sortOrder,
            CreatedAt = seedDate,
            UpdatedAt = seedDate
        };
    }
}
